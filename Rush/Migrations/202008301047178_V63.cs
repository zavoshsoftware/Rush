namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V63 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetailInformations", "StartDate", c => c.DateTime());
            AddColumn("dbo.OrderDetailInformations", "FinishDate", c => c.DateTime());
            DropColumn("dbo.OrderDetailInformations", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetailInformations", "Duration", c => c.Int(nullable: false));
            DropColumn("dbo.OrderDetailInformations", "FinishDate");
            DropColumn("dbo.OrderDetailInformations", "StartDate");
        }
    }
}
