namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v06 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogGroups", "PageTitle", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.BlogGroups", "PageDescription", c => c.String(storeType: "ntext"));
            AddColumn("dbo.Blogs", "PageTitle", c => c.String(nullable: false, maxLength: 500));
            AddColumn("dbo.Blogs", "PageDescription", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.ServiceGroups", "PageDescription", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Services", "PageDescription", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Services", "PageDescription", c => c.String(maxLength: 1000));
            AlterColumn("dbo.ServiceGroups", "PageDescription", c => c.String(maxLength: 1000));
            DropColumn("dbo.Blogs", "PageDescription");
            DropColumn("dbo.Blogs", "PageTitle");
            DropColumn("dbo.BlogGroups", "PageDescription");
            DropColumn("dbo.BlogGroups", "PageTitle");
        }
    }
}
