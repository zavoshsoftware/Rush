namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V66 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Portfolios", "ServiceGroupId", c => c.Guid());
            CreateIndex("dbo.Portfolios", "ServiceGroupId");
            AddForeignKey("dbo.Portfolios", "ServiceGroupId", "dbo.ServiceGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Portfolios", "ServiceGroupId", "dbo.ServiceGroups");
            DropIndex("dbo.Portfolios", new[] { "ServiceGroupId" });
            DropColumn("dbo.Portfolios", "ServiceGroupId");
        }
    }
}
