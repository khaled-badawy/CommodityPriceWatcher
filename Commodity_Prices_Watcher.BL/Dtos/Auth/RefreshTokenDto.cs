﻿using System.ComponentModel.DataAnnotations;

namespace Commodity_Prices_Watcher.BL
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
