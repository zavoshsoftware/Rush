namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V41 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BackLinks", "AverageRate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BackLinks", "AverageRate", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
