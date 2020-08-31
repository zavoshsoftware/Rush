namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V36 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AskedQuestions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false),
                        Body = c.String(storeType: "ntext"),
                        Order = c.Int(nullable: false),
                        ServiceGroupId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceGroups", t => t.ServiceGroupId, cascadeDelete: true)
                .Index(t => t.ServiceGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AskedQuestions", "ServiceGroupId", "dbo.ServiceGroups");
            DropIndex("dbo.AskedQuestions", new[] { "ServiceGroupId" });
            DropTable("dbo.AskedQuestions");
        }
    }
}
