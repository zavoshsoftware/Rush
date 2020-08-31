namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v09 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlogComments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.Int(nullable: false),
                        Email = c.Int(nullable: false),
                        Body = c.String(storeType: "ntext"),
                        BlogId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.Blogs", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.BlogId);
            
            CreateTable(
                "dbo.ServiceForms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ServiceId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        SiteTypeId = c.Guid(nullable: false),
                        SiteAddress = c.String(),
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
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.SiteTypes", t => t.SiteTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ServiceId)
                .Index(t => t.UserId)
                .Index(t => t.SiteTypeId);
            
            CreateTable(
                "dbo.SiteTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
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
            
            AddColumn("dbo.Services", "FormActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceForms", "UserId", "dbo.Users");
            DropForeignKey("dbo.ServiceForms", "SiteTypeId", "dbo.SiteTypes");
            DropForeignKey("dbo.ServiceForms", "ServiceId", "dbo.Services");
            DropForeignKey("dbo.BlogComments", "BlogId", "dbo.Blogs");
            DropIndex("dbo.ServiceForms", new[] { "SiteTypeId" });
            DropIndex("dbo.ServiceForms", new[] { "UserId" });
            DropIndex("dbo.ServiceForms", new[] { "ServiceId" });
            DropIndex("dbo.BlogComments", new[] { "BlogId" });
            DropColumn("dbo.Services", "FormActive");
            DropTable("dbo.SiteTypes");
            DropTable("dbo.ServiceForms");
            DropTable("dbo.BlogComments");
        }
    }
}
