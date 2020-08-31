namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V44 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reportages", "DomainAuthority", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reportages", "DomainAuthority", c => c.String());
        }
    }
}
