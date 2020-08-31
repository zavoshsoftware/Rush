namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V39 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportageGroups", "IsPackage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportageGroups", "IsPackage");
        }
    }
}
