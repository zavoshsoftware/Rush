namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V58 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportageGroups", "ProductId", c => c.Guid());
            CreateIndex("dbo.ReportageGroups", "ProductId");
            AddForeignKey("dbo.ReportageGroups", "ProductId", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportageGroups", "ProductId", "dbo.Products");
            DropIndex("dbo.ReportageGroups", new[] { "ProductId" });
            DropColumn("dbo.ReportageGroups", "ProductId");
        }
    }
}
