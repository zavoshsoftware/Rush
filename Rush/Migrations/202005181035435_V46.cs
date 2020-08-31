namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V46 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportages", "IsDollar", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reportages", "IsDollar");
        }
    }
}
