namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceForms", "SiteTypeId", "dbo.SiteTypes");
            CreateTable(
                "dbo.ServiceTypes",
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
            
            AddColumn("dbo.ServiceForms", "ServiceTypeId", c => c.Guid(nullable: false));
            AddColumn("dbo.ServiceForms", "SiteType_Id", c => c.Guid());
            CreateIndex("dbo.ServiceForms", "ServiceTypeId");
            CreateIndex("dbo.ServiceForms", "SiteType_Id");
            AddForeignKey("dbo.ServiceForms", "ServiceTypeId", "dbo.SiteTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceForms", "ServiceTypeId", "dbo.ServiceTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ServiceForms", "SiteType_Id", "dbo.SiteTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceForms", "SiteType_Id", "dbo.SiteTypes");
            DropForeignKey("dbo.ServiceForms", "ServiceTypeId", "dbo.ServiceTypes");
            DropForeignKey("dbo.ServiceForms", "ServiceTypeId", "dbo.SiteTypes");
            DropIndex("dbo.ServiceForms", new[] { "SiteType_Id" });
            DropIndex("dbo.ServiceForms", new[] { "ServiceTypeId" });
            DropColumn("dbo.ServiceForms", "SiteType_Id");
            DropColumn("dbo.ServiceForms", "ServiceTypeId");
            DropTable("dbo.ServiceTypes");
            AddForeignKey("dbo.ServiceForms", "SiteTypeId", "dbo.SiteTypes", "Id", cascadeDelete: true);
        }
    }
}
