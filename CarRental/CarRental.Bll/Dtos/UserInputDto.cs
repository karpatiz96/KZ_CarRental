using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarRental.Bll.Dtos
{
    public class UserInputDto
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "NAME")]
        public string Name { get; set; }

        [Display(Name = "ROLE")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "EMAIL_REQUIRED")]
        [EmailAddress(ErrorMessage = "EMAIL_INVALID")]
        [Display(Name = "EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PASSWORD_REQUIRED")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "CONFIRM_PASSWORD")]
        [Compare("Password", ErrorMessage = "CONFIRM_PASSWORD_NOT_MATCHING")]
        public string ConfirmPassword { get; set; }
    }
}
