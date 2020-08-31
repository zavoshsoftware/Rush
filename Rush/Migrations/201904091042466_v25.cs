namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reportages", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reportages", "Priority");
        }
    }
}
