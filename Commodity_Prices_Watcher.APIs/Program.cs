using Commodity_Prices_Watcher.BL;
using Commodity_Prices_Watcher.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Default

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region Database

builder.Services.AddDbContext<CommodityPricesWatcherContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CommodityPricesWatcherTestServer")));

#endregion

#region Identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    //options.User.RequireUniqueEmail = true;
    //options.SignIn.RequireConfirmedEmail = true;
    
    options.SignIn.RequireConfirmedPhoneNumber = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.ChangePhoneNumberTokenProvider = TokenOptions.DefaultPhoneProvider;
    options.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
})
    .AddEntityFrameworkStores<CommodityPricesWatcherContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
options.TokenLifespan = TimeSpan.FromDays(7));

#endregion

#region JWT Authentication

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = false;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:SecretKey"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

#endregion

#region Vodafone Configuration

builder.Services.Configure<VodafoneConfiguration>(builder.Configuration.GetSection("VodafoneConfiguration"));
builder.Services.AddScoped<ISmsManager, SmsManager>();

#endregion

#region Email Configuration
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddScoped<IEmailManager, EmailManager>();
#endregion

#region Managers

builder.Services.AddScoped<IAttachmentManager, AttachmentManager>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<ICommodityCategoryManager, CommodityCategoryManager>();
builder.Services.AddScoped<ISharedPriceManager, SharedPriceManager>();
builder.Services.AddScoped<IStaticContentManager, StaticContentManager>();
builder.Services.AddScoped<IUploadFileManager, UploadFileManager>();
builder.Services.AddScoped<IUserManagementManager, UserManagementManager>();

#endregion

#region Repos

builder.Services.AddScoped<IStaticContentRepo, StaticContentRepo>();
builder.Services.AddScoped<ISharedPricesRepo, SharedPricesRepo>();
builder.Services.AddScoped<ICommodityCategoryRepo, CommodityCategoryRepo>();
builder.Services.AddScoped<IAttachmentRepo, AttachmentRepo>();

#endregion



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
