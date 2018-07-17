namespace POSManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class preCodeAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExpenseItems", "PreCode", c => c.String(nullable: false));
            AddColumn("dbo.Items", "PreCode", c => c.String(nullable: false));
            AddColumn("dbo.Parties", "PreCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Parties", "PreCode");
            DropColumn("dbo.Items", "PreCode");
            DropColumn("dbo.ExpenseItems", "PreCode");
        }
    }
}
