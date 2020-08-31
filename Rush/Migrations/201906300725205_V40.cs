namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V40 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackLinks", "AverageRate", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Blogs", "FileUrl", c => c.String());
            AddColumn("dbo.Texts", "AverageRate", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Texts", "AverageRate");
            DropColumn("dbo.Blogs", "FileUrl");
            DropColumn("dbo.BackLinks", "AverageRate");
        }
    }
}
