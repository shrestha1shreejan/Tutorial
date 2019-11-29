using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelsLibrary.DTOS
{
    public class UserForRegisterDto
    {

        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8 characters")]
        public string Password { get; set; }

    }
}
