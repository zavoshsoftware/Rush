namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V45 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportageGroups", "StartDa", c => c.Int(nullable: false));
            AddColumn("dbo.ReportageGroups", "FinishDa", c => c.Int(nullable: false));
            AddColumn("dbo.ReportageGroups", "SiteNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportageGroups", "SiteNumber");
            DropColumn("dbo.ReportageGroups", "FinishDa");
            DropColumn("dbo.ReportageGroups", "StartDa");
        }
    }
}
