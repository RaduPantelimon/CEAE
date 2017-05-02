using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CEAE.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
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

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailRequired")]
        [Display(ResourceType = typeof(Translations), Name = "Email")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Translations), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "EmailRequired")]
        [Display(ResourceType = typeof(Translations), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "PasswordLengthRequired")]
        [Display(ResourceType = typeof(Translations), Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [Display(ResourceType = typeof(Translations), Name = "ConfirmPassword")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "PasswordMismatch")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage =
            "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}