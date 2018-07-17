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
    public class ExpenseItemVM
    {
        public long Id { get; set; }
        [Required]
        [Remote("IsUniqueName", "ExpenseItem", ErrorMessage = "The Name already exists", AdditionalFields = "initialName")]
        public string Name { get; set; }
        [Required]
        public string PreCode { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "The Code must be length of 6")]
        [Remote("IsUniqueCode", "ExpenseItem", ErrorMessage = "The Code already exists", AdditionalFields = "PreCode,initialCode,initialPreCode")]
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [DisplayName("Category")]
        public long CategoryId { get; set; }
        public virtual ExpenseCategory Category { get; set; }

        public List<ExpenseItem> Items { get; set; }
        public IEnumerable<SelectListItem> SelectList { get; set; }
    }
}