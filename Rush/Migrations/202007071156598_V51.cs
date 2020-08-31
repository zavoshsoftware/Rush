namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V51 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "RefId", c => c.String());
            DropColumn("dbo.Orders", "SaleReferenceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "SaleReferenceId", c => c.Long());
            DropColumn("dbo.Orders", "RefId");
        }
    }
}
