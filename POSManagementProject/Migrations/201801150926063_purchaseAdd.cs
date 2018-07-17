namespace POSManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchaseAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PurchaseInformations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BranchId = c.Long(nullable: false),
                        EmployeeId = c.Long(nullable: false),
                        SupplierId = c.Long(nullable: false),
                        Remarks = c.String(),
                        TotalAmount = c.Double(nullable: false),
                        PaidAmount = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: false)
                .ForeignKey("dbo.Parties", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.EmployeeId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.PurchaseItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ItemId = c.Long(nullable: false),
                        Quantity = c.Long(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        LineTotal = c.Double(nullable: false),
                        Date = c.DateTime(),
                        PurchaseInformationId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseInformations", t => t.PurchaseInformationId, cascadeDelete: true)
                .Index(t => t.ItemId)
                .Index(t => t.PurchaseInformationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseInformations", "SupplierId", "dbo.Parties");
            DropForeignKey("dbo.PurchaseItems", "PurchaseInformationId", "dbo.PurchaseInformations");
            DropForeignKey("dbo.PurchaseItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.PurchaseInformations", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.PurchaseInformations", "BranchId", "dbo.Branches");
            DropIndex("dbo.PurchaseItems", new[] { "PurchaseInformationId" });
            DropIndex("dbo.PurchaseItems", new[] { "ItemId" });
            DropIndex("dbo.PurchaseInformations", new[] { "SupplierId" });
            DropIndex("dbo.PurchaseInformations", new[] { "EmployeeId" });
            DropIndex("dbo.PurchaseInformations", new[] { "BranchId" });
            DropTable("dbo.PurchaseItems");
            DropTable("dbo.PurchaseInformations");
        }
    }
}
