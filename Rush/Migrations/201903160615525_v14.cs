namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServiceForms", "ServiceId", "dbo.Services");
            DropIndex("dbo.ServiceForms", new[] { "ServiceId" });
            AddColumn("dbo.ServiceForms", "ServiceGroupId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ServiceForms", "ServiceGroupId");
            AddForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups", "Id", cascadeDelete: true);
            DropColumn("dbo.ServiceForms", "ServiceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceForms", "ServiceId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.ServiceForms", "ServiceGroupId", "dbo.ServiceGroups");
            DropIndex("dbo.ServiceForms", new[] { "ServiceGroupId" });
            DropColumn("dbo.ServiceForms", "ServiceGroupId");
            CreateIndex("dbo.ServiceForms", "ServiceId");
            AddForeignKey("dbo.ServiceForms", "ServiceId", "dbo.Services", "Id", cascadeDelete: true);
        }
    }
}
