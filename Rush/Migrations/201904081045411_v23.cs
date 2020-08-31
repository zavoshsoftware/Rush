namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v23 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportageGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        UrlParam = c.String(nullable: false, maxLength: 500),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Reportages", "ReportageGroupId", c => c.Guid(nullable: true));
            CreateIndex("dbo.Reportages", "ReportageGroupId");
            AddForeignKey("dbo.Reportages", "ReportageGroupId", "dbo.ReportageGroups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reportages", "ReportageGroupId", "dbo.ReportageGroups");
            DropIndex("dbo.Reportages", new[] { "ReportageGroupId" });
            DropColumn("dbo.Reportages", "ReportageGroupId");
            DropTable("dbo.ReportageGroups");
        }
    }
}
