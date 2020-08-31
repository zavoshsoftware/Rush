namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceForms", "Email", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceForms", "Email");
        }
    }
}
