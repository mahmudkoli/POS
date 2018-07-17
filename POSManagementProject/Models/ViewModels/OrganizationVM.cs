using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.EntityModels;

namespace POSManagementProject.Models.ViewModels
{
    public class OrganizationVM
    {
        public long Id { get; set; }
        [Required]
        [Remote("IsUniqueName", "Organization", ErrorMessage = "The Name already exists", AdditionalFields = "initialName")]
        public string Name { get; set; }
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "The Code must be length of 3")]
        [Remote("IsUniqueCode", "Organization", ErrorMessage = "The Code already exists", AdditionalFields = "initialCode")]
        public string Code { get; set; }
        public string Contact { get; set; }
        [Required]
        public string Address { get; set; }
        public byte[] Image { get; set; }
        public DateTime Date { get; set; }

        public List<Organization> Organizations { get; set; }
    }
}