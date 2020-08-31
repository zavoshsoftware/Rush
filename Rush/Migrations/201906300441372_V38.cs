namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V38 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReportageGroups", "Value", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ReportageGroups", "Price", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReportageGroups", "Price");
            DropColumn("dbo.ReportageGroups", "Value");
        }
    }
}
