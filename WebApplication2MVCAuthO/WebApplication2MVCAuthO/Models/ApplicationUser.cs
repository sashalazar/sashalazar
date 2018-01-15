using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication2MVCAuthO.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(250)]
        public string ProfilePhoto { get; set; }

        [MaxLength(250)]
        public string FullName { get; set; }

        //[Required(ErrorMessage = "Номер не может быть пустым")]
        //[Phone(ErrorMessage = "Не допустимый формат номера")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Мой номер телефона:")]
        [RegularExpression(@"^(\+38)\(?(\d{3})\)?[-. ]?(\d{3})[-. ]?(\d{4})$", ErrorMessage = "Не допустимый формат номера")]
        [MaxLength(250)]
        public override string PhoneNumber { get; set; }

        [MaxLength(250)]
        public string NormPhoneNum { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegDate { get; set; }
    }
}
