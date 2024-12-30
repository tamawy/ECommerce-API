﻿using System.ComponentModel.DataAnnotations;

namespace ECommerce.Api.Models.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
