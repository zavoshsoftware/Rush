namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v24 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reportages", "ReportageGroupId", "dbo.ReportageGroups");
            DropIndex("dbo.Reportages", new[] { "ReportageGroupId" });
            AlterColumn("dbo.Reportages", "ReportageGroupId", c => c.Guid());
            CreateIndex("dbo.Reportages", "ReportageGroupId");
            AddForeignKey("dbo.Reportages", "ReportageGroupId", "dbo.ReportageGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reportages", "ReportageGroupId", "dbo.ReportageGroups");
            DropIndex("dbo.Reportages", new[] { "ReportageGroupId" });
            AlterColumn("dbo.Reportages", "ReportageGroupId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Reportages", "ReportageGroupId");
            AddForeignKey("dbo.Reportages", "ReportageGroupId", "dbo.ReportageGroups", "Id", cascadeDelete: true);
        }
    }
}
