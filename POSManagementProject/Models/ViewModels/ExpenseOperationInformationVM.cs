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
    public class ExpenseOperationInformationVM
    {
        public long Id { get; set; }
        [Required]
        public string ExpenseNo { get; set; }
        [Required]
        [DisplayName("Branch")]
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        [Required]
        [DisplayName("Employee")]
        public long EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        [Required]
        [DisplayName("Total Amount")]
        public double TotalAmount { get; set; }
        [DisplayName("Paid Amount")]
        public double PaidAmount { get; set; }
        [DisplayName("Due Amount")]
        public double DueAmount { get; set; }
        [DisplayName("Expense Date")]
        public DateTime ExpenseDate { get; set; }
        public DateTime Date { get; set; }

        public List<ExpenseOperationItem> ExpenseItems { get; set; }

        public IEnumerable<SelectListItem> SelectListItem { get; set; }
        public IEnumerable<SelectListItem> SelectListBranch { get; set; }
        public IEnumerable<SelectListItem> SelectListEmployee { get; set; }
        public IEnumerable<ExpenseOperationInformation> ExpenseOpInfoList { get; set; }
    }
}