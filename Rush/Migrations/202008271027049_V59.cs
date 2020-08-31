namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V59 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "OrderType");
        }
    }
}
