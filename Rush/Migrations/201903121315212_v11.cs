namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogComments", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogComments", "FullName", c => c.Int(nullable: false));
        }
    }
}
