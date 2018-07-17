namespace POSManagementProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expenseNoAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExpenseOperationInformations", "ExpenseNo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExpenseOperationInformations", "ExpenseNo");
        }
    }
}
