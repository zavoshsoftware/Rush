namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V42 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogGroups", "Body", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogGroups", "Body");
        }
    }
}
