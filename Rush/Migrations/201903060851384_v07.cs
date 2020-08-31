namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v07 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogGroups", "UrlParam", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Blogs", "UrlParam", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "UrlParam");
            DropColumn("dbo.BlogGroups", "UrlParam");
        }
    }
}
