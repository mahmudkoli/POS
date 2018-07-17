using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POSManagementProject.Models.Context;
using POSManagementProject.Models.EntityModels;
using POSManagementProject.Models.ViewModels;

namespace POSManagementProject.DAL
{
    public class ExpenseOperationDAL
    {
        POSDBContext dbContext = new POSDBContext();
        public IEnumerable<SelectListItem> GetExpenseItemSelectList()
        {
            var list = dbContext.ExpenseItems.ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Code+" - "+li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public IEnumerable<SelectListItem> GetBranchSelectList()
        {
            var list = dbContext.Branches.ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Address, Value = li.Id.ToString() }));

            return selectList;
        }
        public IEnumerable<SelectListItem> GetEmployeeSelectList()
        {
            var list = dbContext.Employees.ToList();

            var selectList = new List<SelectListItem>()
            {
                new SelectListItem { Text = "---Select---", Value = "", Selected = true }
            };

            selectList.AddRange(list.Select(li => new SelectListItem { Text = li.Name, Value = li.Id.ToString() }));

            return selectList;
        }
        public List<Employee> GetEmployeeList(long branchId)
        {
            var employeeList = dbContext.Employees.Where(x => x.BranchId == branchId).ToList();
            return employeeList;
        }

        public string GetExpenseNo()
        {
            long expenseNo;

            if (dbContext.ExpenseOperationInformations.ToList().Count > 0)
            {
                expenseNo = Convert.ToInt64(dbContext.ExpenseOperationInformations.Select(x => x.ExpenseNo).Max());
            }
            else
            {
                expenseNo = 0;
            }

            return string.Format("{0:0000000}", (expenseNo + 1));
        }

        public bool IsExpenseOperationSuccess(ExpenseOperationInformationVM itemVm)
        {
            ExpenseOperationInformation item = new ExpenseOperationInformation()
            {
                ExpenseNo = itemVm.ExpenseNo,
                BranchId = itemVm.BranchId,
                EmployeeId = itemVm.EmployeeId,
                ExpenseDate = itemVm.ExpenseDate,
                TotalAmount = itemVm.TotalAmount,
                PaidAmount = itemVm.PaidAmount,
                DueAmount = itemVm.DueAmount,
                ExpenseItems = itemVm.ExpenseItems,
                Date = itemVm.Date

            };

            dbContext.ExpenseOperationInformations.Add(item);
            var isSuccess = dbContext.SaveChanges() > 0;

            return isSuccess;
        }

        public ExpenseOperationInformation GetExpenseOpInfo(string expenseNo)
        {
            ExpenseOperationInformation item = dbContext.ExpenseOperationInformations.Where(x => x.ExpenseNo == expenseNo).Include(x => x.ExpenseItems).FirstOrDefault();
            return item;
        }

        public ExpenseOperationInformation GetExpenseOpInfo(long id)
        {
            ExpenseOperationInformation item = dbContext.ExpenseOperationInformations.Where(x => x.Id == id).Include(x => x.ExpenseItems).FirstOrDefault();

            return item;
        }

        public IEnumerable<ExpenseOperationInformation> GetExpenseOpInfoList()
        {
            var items = dbContext.ExpenseOperationInformations.Include(x => x.ExpenseItems);
            return items;
        }
        public IEnumerable<ExpenseOperationInformation> GetExpenseOpInfoList(long branchId, DateTime fromDate, DateTime toDate)
        {
            var items = dbContext.ExpenseOperationInformations
                .Where(x => x.BranchId == branchId && x.ExpenseDate >= fromDate && x.ExpenseDate <= toDate)
                .Include(x => x.ExpenseItems);
            return items;
        }
        
    }
}