namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v32 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceForms", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceForms", "Phone");
        }
    }
}
