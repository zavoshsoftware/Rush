namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V48 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportages", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reportages", "ImageUrl");
        }
    }
}
