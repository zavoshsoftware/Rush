namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V55 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "PublishLink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "PublishLink");
        }
    }
}
