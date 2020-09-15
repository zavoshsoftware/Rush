namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V64 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportages", "IsSpecialOffer", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reportages", "IsSpecialOffer");
        }
    }
}
