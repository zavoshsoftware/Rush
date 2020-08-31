namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups");
            DropIndex("dbo.ServiceForms", new[] { "ServiceGroupId" });
            AddColumn("dbo.ServiceForms", "ServiceId", c => c.Guid());
            AddColumn("dbo.Services", "IsFormActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ServiceForms", "ServiceGroupId", c => c.Guid());
            CreateIndex("dbo.ServiceForms", "ServiceGroupId");
            CreateIndex("dbo.ServiceForms", "ServiceId");
            AddForeignKey("dbo.ServiceForms", "ServiceId", "dbo.Services", "Id");
            AddForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups");
            DropForeignKey("dbo.ServiceForms", "ServiceId", "dbo.Services");
            DropIndex("dbo.ServiceForms", new[] { "ServiceId" });
            DropIndex("dbo.ServiceForms", new[] { "ServiceGroupId" });
            AlterColumn("dbo.ServiceForms", "ServiceGroupId", c => c.Guid(nullable: false));
            DropColumn("dbo.Services", "IsFormActive");
            DropColumn("dbo.ServiceForms", "ServiceId");
            CreateIndex("dbo.ServiceForms", "ServiceGroupId");
            AddForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups", "Id", cascadeDelete: true);
        }
    }
}
