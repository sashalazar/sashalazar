﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2MVCAuthO.Models.HomeViewModels
{
    public class ClientRequestModel
    {
        [MaxLength(256)]
        [Column(Order = 0)]
        public string Id { get; set; }

        [Column(Order = 1)]
        public ApplicationUser User { get; set; }

        [MaxLength(256)]
        [Column(Order = 2)]
        public string Latitude { get; set; }

        [MaxLength(256)]
        [Column(Order = 3)]
        public string Longitude { get; set; }

        [MaxLength(40)]
        [Column(Order = 4)]
        public string Status { get; set; }

        [DataType(DataType.DateTime)]
        [Column(Order = 5)]
        public DateTime InsDate { get; set; }

        [DataType(DataType.DateTime)]
        [Column(Order = 6)]
        public DateTime UpdDate { get; set; }
    }
}
