namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceForms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ServiceGroupId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        SiteTypeId = c.Guid(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceGroups", t => t.ServiceGroupId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceTypes", t => t.ServiceTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SiteTypes", t => t.SiteTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ServiceGroupId)
                .Index(t => t.UserId)
                .Index(t => t.SiteTypeId)
                .Index(t => t.ServiceTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceForms", "UserId", "dbo.Users");
            DropForeignKey("dbo.ServiceForms", "SiteTypeId", "dbo.SiteTypes");
            DropForeignKey("dbo.ServiceForms", "ServiceTypeId", "dbo.ServiceTypes");
            DropForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups");
            DropIndex("dbo.ServiceForms", new[] { "ServiceTypeId" });
            DropIndex("dbo.ServiceForms", new[] { "SiteTypeId" });
            DropIndex("dbo.ServiceForms", new[] { "UserId" });
            DropIndex("dbo.ServiceForms", new[] { "ServiceGroupId" });
            DropTable("dbo.ServiceForms");
        }
    }
}
