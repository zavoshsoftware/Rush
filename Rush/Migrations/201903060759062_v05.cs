namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v05 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NewsLetters", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NewsLetters", "Email", c => c.Int(nullable: false));
        }
    }
}
