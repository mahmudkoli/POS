using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.EntityModels;

namespace POSManagementProject.Models.ViewModels
{
    public class PartyVM
    {
        public long Id { get; set; }
        [Required]
        [Remote("IsUniqueName", "Party", ErrorMessage = "The Name already exists", AdditionalFields = "initialName")]
        public string Name { get; set; }
        [Required]
        public string PreCode { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "The Code must be length of 4")]
        [Remote("IsUniqueCode", "Party", ErrorMessage = "The Code already exists", AdditionalFields = "PreCode,initialCode,initialPreCode")]
        public string Code { get; set; }
        public string Contact { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public byte[] Image { get; set; }
        public DateTime Date { get; set; }
        [DisplayName("Customer")]
        public bool IsCustomer { get; set; }
        [DisplayName("Supplier")]
        public bool IsSupplier { get; set; }

        public List<Party> Parties { get; set; }
        

    }
}