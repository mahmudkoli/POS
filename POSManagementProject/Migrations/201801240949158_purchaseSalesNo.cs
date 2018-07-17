namespace POSManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchaseSalesNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOperationInformations", "PurchaseNo", c => c.String(nullable: false));
            AddColumn("dbo.SalesOperationInformations", "SalesNo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesOperationInformations", "SalesNo");
            DropColumn("dbo.PurchaseOperationInformations", "PurchaseNo");
        }
    }
}
