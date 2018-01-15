using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication2MVCAuthO.Models.HomeViewModels
{
    public class DriverModel
    {
        [MaxLength(256)]
        [Column(Order = 0)]
        public string Id { get; set; }

        [Column(Order = 1, TypeName = "nvarchar(256)")]
        public ApplicationUser User { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Город не может быть пустым")]
        [Display(Name = "Город:")]
        public string City { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Марка и модель не может быть пустым")]
        [Display(Name = "Марка и модель:")]
        public string CarModel { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Тип кузова не может быть пустым")]
        [Display(Name = "Тип кузова:")]
        public string CarType { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Год выпуска не может быть пустым")]
        [Display(Name = "Год выпуска:")]
        public string CarYearProd { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Цвет кузова не может быть пустым")]
        [Display(Name = "Цвет кузова:")]
        public string CarColor { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Гос. номер не может быть пустым")]
        [Display(Name = "Гос. номер:")]
        public string CarNum { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Серия и номер не может быть пустым")]
        [Display(Name = "Серия и номер вод. удостоверения:")]
        public string DrLicense { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage = "Водительский стаж не может быть пустым")]
        [Display(Name = "Водительский стаж:")]
        public string DrLFromDate { get; set; }

        [MaxLength(456)]
        public string AddServices { get; set; }

    }
}
