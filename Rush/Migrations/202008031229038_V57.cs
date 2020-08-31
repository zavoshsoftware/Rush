namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V57 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportages", "Terms", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reportages", "Terms");
        }
    }
}
