namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v35 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "AverageRate", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ServiceGroups", "AverageRate", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceGroups", "AverageRate");
            DropColumn("dbo.Services", "AverageRate");
        }
    }
}
