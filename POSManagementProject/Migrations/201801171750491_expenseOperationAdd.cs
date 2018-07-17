namespace POSManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expenseOperationAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpenseOperationInformations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BranchId = c.Long(nullable: false),
                        EmployeeId = c.Long(nullable: false),
                        TotalAmount = c.Double(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        ExpenseDate = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: false)
                .Index(t => t.BranchId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.ExpenseOperationItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Long(nullable: false),
                        Amount = c.Double(nullable: false),
                        LineTotal = c.Double(nullable: false),
                        Reason = c.String(),
                        Date = c.DateTime(),
                        ExpenseInformationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExpenseOperationInformations", t => t.ExpenseInformationId, cascadeDelete: true)
                .ForeignKey("dbo.ExpenseItems", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.ExpenseInformationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpenseOperationItems", "ItemId", "dbo.ExpenseItems");
            DropForeignKey("dbo.ExpenseOperationItems", "ExpenseInformationId", "dbo.ExpenseOperationInformations");
            DropForeignKey("dbo.ExpenseOperationInformations", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.ExpenseOperationInformations", "BranchId", "dbo.Branches");
            DropIndex("dbo.ExpenseOperationItems", new[] { "ExpenseInformationId" });
            DropIndex("dbo.ExpenseOperationItems", new[] { "ItemId" });
            DropIndex("dbo.ExpenseOperationInformations", new[] { "EmployeeId" });
            DropIndex("dbo.ExpenseOperationInformations", new[] { "BranchId" });
            DropTable("dbo.ExpenseOperationItems");
            DropTable("dbo.ExpenseOperationInformations");
        }
    }
}
