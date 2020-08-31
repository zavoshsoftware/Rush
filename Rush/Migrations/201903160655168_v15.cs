namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceForms", "SiteTypeId", "dbo.SiteTypes");
            DropIndex("dbo.ServiceForms", new[] { "SiteTypeId" });
            DropColumn("dbo.ServiceForms", "SiteTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceForms", "SiteTypeId", c => c.Guid());
            CreateIndex("dbo.ServiceForms", "SiteTypeId");
            AddForeignKey("dbo.ServiceForms", "SiteTypeId", "dbo.SiteTypes", "Id");
        }
    }
}
