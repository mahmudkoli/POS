namespace POSManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expenseOpAdd : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PurchaseInformations", newName: "PurchaseOperationInformations");
            RenameTable(name: "dbo.PurchaseItems", newName: "PurchaseOperationItems");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.PurchaseOperationItems", newName: "PurchaseItems");
            RenameTable(name: "dbo.PurchaseOperationInformations", newName: "PurchaseInformations");
        }
    }
}
