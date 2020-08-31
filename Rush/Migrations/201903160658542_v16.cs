namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups");
            DropForeignKey("dbo.ServiceForms", "ServiceTypeId", "dbo.ServiceTypes");
            DropForeignKey("dbo.ServiceForms", "UserId", "dbo.Users");
            DropIndex("dbo.ServiceForms", new[] { "ServiceGroupId" });
            DropIndex("dbo.ServiceForms", new[] { "UserId" });
            DropIndex("dbo.ServiceForms", new[] { "ServiceTypeId" });
            DropTable("dbo.ServiceForms");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ServiceForms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ServiceGroupId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        ServiceTypeId = c.Guid(nullable: false),
                        SiteAddress = c.String(),
                        Email = c.String(maxLength: 256),
                        MainWords = c.String(),
                        FormDescription = c.String(storeType: "ntext"),
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
            
            CreateIndex("dbo.ServiceForms", "ServiceTypeId");
            CreateIndex("dbo.ServiceForms", "UserId");
            CreateIndex("dbo.ServiceForms", "ServiceGroupId");
            AddForeignKey("dbo.ServiceForms", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceForms", "ServiceTypeId", "dbo.ServiceTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups", "Id", cascadeDelete: true);
        }
    }
}
