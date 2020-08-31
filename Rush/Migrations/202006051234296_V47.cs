namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V47 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackLinks", "DomainAuthority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BackLinks", "DomainAuthority");
        }
    }
}
