namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v10 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BlogComments", "Email", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BlogComments", "Email", c => c.Int(nullable: false));
        }
    }
}
