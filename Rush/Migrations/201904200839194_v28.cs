namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v28 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BackLinks", "OneMonthBackLink", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.BackLinks", "ThreeMonthBackLink", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BackLinks", "ThreeMonthBackLink", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.BackLinks", "OneMonthBackLink", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
