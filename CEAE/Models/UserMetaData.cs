using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CEAE.Utils;

namespace CEAE.Models
{
    [MetadataType(typeof(Metadata))]
    public partial class User
    {
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private class Metadata
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

            [RequiredT]
            [DisplayNameT]
            public string Administrator { get; set; }

            [DisplayNameT]
            [Url]
            public string ImgPath { get; set; }
        }
    }
}