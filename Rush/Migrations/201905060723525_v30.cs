namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v30 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "PdfUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogs", "PdfUrl");
        }
    }
}
