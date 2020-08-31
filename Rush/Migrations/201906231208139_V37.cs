namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V37 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AskedQuestions", "ServiceGroupId", "dbo.ServiceGroups");
            DropIndex("dbo.AskedQuestions", new[] { "ServiceGroupId" });
            AddColumn("dbo.AskedQuestions", "Param", c => c.String());
            DropColumn("dbo.AskedQuestions", "ServiceGroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AskedQuestions", "ServiceGroupId", c => c.Guid(nullable: false));
            DropColumn("dbo.AskedQuestions", "Param");
            CreateIndex("dbo.AskedQuestions", "ServiceGroupId");
            AddForeignKey("dbo.AskedQuestions", "ServiceGroupId", "dbo.ServiceGroups", "Id", cascadeDelete: true);
        }
    }
}
