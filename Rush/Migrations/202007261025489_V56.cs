namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V56 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "IsSendPublishSms", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "IsSendPublishSms");
        }
    }
}
