using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2MVCAuthO.Models.HomeViewModels
{
    public class DriverLocationModel
    {
        [MaxLength(256)]
        public string Id { get; set; }

        public ApplicationUser User { get; set; }

        [MaxLength(256)]
        public string Latitude { get; set; }

        [MaxLength(256)]
        public string Longitude { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DataType(DataType.DateTime)]
        public DateTime InsDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdDate { get; set; }

        [MaxLength(40)]
        public string Status { get; set; }

        [NotMapped]
        public double Distance { get; set; }
    }
}
