using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using POSManagementProject.Models.EntityModels;

namespace POSManagementProject.Models.ViewModels
{
    public class PurchaseOperationItemVM
    {
        public long Id { get; set; }

        [Required]
        public long ItemId { get; set; }
        public virtual Item Item { get; set; }
        [Required]
        public long Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        [Required]
        public double LineTotal { get; set; }
        public DateTime? Date { get; set; }
        [Required]
        public long PurchaseInformationId { get; set; }
        public virtual PurchaseOperationInformation PurchaseInformation { get; set; }
    }
}