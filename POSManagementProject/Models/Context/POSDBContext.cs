using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using POSManagementProject.Models.EntityModels;

namespace POSManagementProject.Models.Context
{
    public class POSDBContext : DbContext
    {
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Party> Parties { get; set; }

        public DbSet<PurchaseOperationInformation> PurchaseOperationInformations { get; set; }
        public DbSet<PurchaseOperationItem> PurchaseOperationItems { get; set; }

        public DbSet<ExpenseOperationInformation> ExpenseOperationInformations { get; set; }
        public DbSet<ExpenseOperationItem> ExpenseOperationItems { get; set; }

        public DbSet<SalesOperationInformation> SalesOperationInformations { get; set; }
        public DbSet<SalesOperationItem> SalesOperationItems { get; set; }

        public DbSet<Stock> Stocks { get; set; }


    }
}