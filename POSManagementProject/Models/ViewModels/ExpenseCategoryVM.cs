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
    public class ExpenseCategoryVM
    {
        public long Id { get; set; }

        [Required]
        [Remote("IsUniqueName", "ExpenseCategory", ErrorMessage = "The Name already exists", AdditionalFields = "initialName")]
        public string Name { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "The Code must be length of 3")]
        [Remote("IsUniqueCode", "ExpenseCategory", ErrorMessage = "The Code already exists", AdditionalFields = "initialCode")]
        public string Code { get; set; }

        public string Description { get; set; }
        public DateTime Date { get; set; }

        [DisplayName("Parent Category")]
        public long? ParentId { get; set; }
        public virtual ExpenseCategory Parent { get; set; }

        public List<ExpenseCategory> ExpenseCategories { get; set; }
        public IEnumerable<SelectListItem> SelectList { get; set; }
    }
}