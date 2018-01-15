using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2MVCAuthO.Models.HomeViewModels
{
    public class OrderModel
    {
        [MaxLength(256)]
        [Column(Order = 0)]
        public string Id { get; set; }

        [Column(Order = 1, TypeName = "nvarchar(256)")]
        public ClientRequestModel ClientRequest { get; set; }

        //[MaxLength(256)]
        //public string ClientRequestId { get; set; }

        [Column(Order = 2, TypeName = "nvarchar(256)")]
        public DriverLocationModel DriverLocation { get; set; }

        [MaxLength(40)]
        [Column(Order = 3)]
        public string Status { get; set; }

        [DataType(DataType.DateTime)]
        [Column(Order = 4)]
        public DateTime CreatDate { get; set; }

        [DataType(DataType.DateTime)]
        [Column(Order = 4)]
        public DateTime UpdStatusDate { get; set; }
    }
}
