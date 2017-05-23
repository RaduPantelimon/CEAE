using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CEAE.Utils;

namespace CEAE.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [RequiredT]
        [DisplayNameT]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class LoginViewModel
    {
        [RequiredT]
        [DisplayNameT]
        public string Email { get; set; }

        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayNameT]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [RequiredT]
        [DisplayNameT]
        [EmailAddress]
        public string Email { get; set; }

        [RequiredT]
        [DisplayNameT]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Translations),
            ErrorMessageResourceName = "PasswordLengthRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RequiredT]
        [DisplayNameT]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Translations),
            ErrorMessageResourceName = "PasswordMismatch")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [RequiredT]
        [DisplayNameT]
        [StringLength(100, ErrorMessageResourceType = typeof(Translations),
            ErrorMessageResourceName = "PasswordLengthRequired", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Translations),
            ErrorMessageResourceName = "PasswordMismatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [RequiredT]
        [DisplayNameT]
        [EmailAddress]
        public string Email { get; set; }
    }
}