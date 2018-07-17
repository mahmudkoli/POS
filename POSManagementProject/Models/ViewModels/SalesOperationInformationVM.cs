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
    public class SalesOperationInformationVM
    {
        public long Id { get; set; }
        [Required]
        public string SalesNo { get; set; }
        [Required]
        [DisplayName("Branch")]
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        [Required]
        [DisplayName("Employee")]
        public long EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Customer Contact")]
        public string CustomerContact { get; set; }

        [Required]
        [DisplayName("Total Amount")]
        public double TotalAmount { get; set; }
        public double? VAT { get; set; }
        [DisplayName("Discount Amount")]
        public double? DiscountAmount { get; set; }
        [Required]
        [DisplayName("Payable Amount")]
        public double PayableAmount { get; set; }
        [DisplayName("Paid Amount")]
        public double PaidAmount { get; set; }
        [DisplayName("Due Amount")]
        public double DueAmount { get; set; }
        [DisplayName("Sales Date")]
        public DateTime SalesDate { get; set; }
        public DateTime Date { get; set; }

        public List<SalesOperationItem> SalesItems { get; set; }

        public IEnumerable<SelectListItem> SelectListItem { get; set; }
        public IEnumerable<SelectListItem> SelectListBranch { get; set; }
        public IEnumerable<SelectListItem> SelectListEmployee { get; set; }

        public IEnumerable<SalesOperationInformation> SalesOpInfoList { get; set; }
    }
}