namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V43 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Texts", "PageTitle", c => c.String());
            AddColumn("dbo.Texts", "MetaDescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Texts", "MetaDescription");
            DropColumn("dbo.Texts", "PageTitle");
        }
    }
}
