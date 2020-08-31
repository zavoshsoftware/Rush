namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V54 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "FileUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "FileUrl");
        }
    }
}
