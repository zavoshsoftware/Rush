namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceGroups", "IsFormActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.Services", "FormActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Services", "FormActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.ServiceGroups", "IsFormActive");
        }
    }
}
