namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V61 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackLinks", "ProductId", c => c.Guid());
            AddColumn("dbo.OrderDetailInformations", "BacklinkKeyword", c => c.String());
            AddColumn("dbo.OrderDetailInformations", "BacklinkUrl", c => c.String());
            CreateIndex("dbo.BackLinks", "ProductId");
            AddForeignKey("dbo.BackLinks", "ProductId", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BackLinks", "ProductId", "dbo.Products");
            DropIndex("dbo.BackLinks", new[] { "ProductId" });
            DropColumn("dbo.OrderDetailInformations", "BacklinkUrl");
            DropColumn("dbo.OrderDetailInformations", "BacklinkKeyword");
            DropColumn("dbo.BackLinks", "ProductId");
        }
    }
}
