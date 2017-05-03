using System.ComponentModel.DataAnnotations;
using CEAE.Utils;

namespace CEAE.Models.DTO
{
    public class User
    {
        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Text)]
        public string Account { get; set; }

        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [RequiredT]
        [DisplayNameT]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [DisplayNameT]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [RequiredT]
        [DisplayNameT]
        [EmailAddress]
        public string Email { get; set; }

        [RequiredT]
        [DisplayNameT]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [DisplayNameT]
        [Url]
        public string ImgPath { get; set; }
    }
}