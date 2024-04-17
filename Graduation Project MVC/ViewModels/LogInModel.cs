﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace presentationLayer.ViewModels
{
    public class LogInModel
    {
        [EmailAddress(ErrorMessage = "This Email Isn't Valid")]
        [Required(ErrorMessage = "Email's Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password's Required !!")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "This Must Be Checked")]
        public bool RememberMe { get; set; }
    }
}
