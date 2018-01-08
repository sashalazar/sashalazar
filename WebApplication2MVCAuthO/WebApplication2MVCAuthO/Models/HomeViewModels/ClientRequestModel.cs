using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2MVCAuthO.Models.HomeViewModels
{
    public class ClientRequestModel
    {
        [MaxLength(256)]
        public string Id { get; set; }

        public ApplicationUser User { get; set; }

        [MaxLength(256)]
        public string Latitude { get; set; }

        [MaxLength(256)]
        public string Longitude { get; set; }
    }
}
