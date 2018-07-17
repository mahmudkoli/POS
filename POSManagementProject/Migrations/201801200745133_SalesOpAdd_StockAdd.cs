namespace POSManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesOpAdd_StockAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalesOperationInformations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BranchId = c.Long(nullable: false),
                        EmployeeId = c.Long(nullable: false),
                        CustomerName = c.String(),
                        CustomerContact = c.String(),
                        TotalAmount = c.Double(nullable: false),
                        VAT = c.Double(),
                        DiscountAmount = c.Double(),
                        PayableAmount = c.Double(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        SalesDate = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: false)
                .Index(t => t.BranchId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.SalesOperationItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Long(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        LineTotal = c.Double(nullable: false),
                        Date = c.DateTime(),
                        SalesOperationInformationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.SalesOperationInformations", t => t.SalesOperationInformationId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.SalesOperationInformationId);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BranchId = c.Long(nullable: false),
                        ItemId = c.Long(nullable: false),
                        StockQuantity = c.Long(nullable: false),
                        AveragePrice = c.Double(nullable: false),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Stocks", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.SalesOperationItems", "SalesOperationInformationId", "dbo.SalesOperationInformations");
            DropForeignKey("dbo.SalesOperationItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.SalesOperationInformations", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.SalesOperationInformations", "BranchId", "dbo.Branches");
            DropIndex("dbo.Stocks", new[] { "ItemId" });
            DropIndex("dbo.Stocks", new[] { "BranchId" });
            DropIndex("dbo.SalesOperationItems", new[] { "SalesOperationInformationId" });
            DropIndex("dbo.SalesOperationItems", new[] { "ItemId" });
            DropIndex("dbo.SalesOperationInformations", new[] { "EmployeeId" });
            DropIndex("dbo.SalesOperationInformations", new[] { "BranchId" });
            DropTable("dbo.Stocks");
            DropTable("dbo.SalesOperationItems");
            DropTable("dbo.SalesOperationInformations");
        }
    }
}
