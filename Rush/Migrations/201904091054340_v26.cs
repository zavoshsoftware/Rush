namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportageGroups", "Priority", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportageGroups", "Priority");
        }
    }
}
