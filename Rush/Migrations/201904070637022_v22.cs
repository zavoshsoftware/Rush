namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v22 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reportages", "Address", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reportages", "Address", c => c.String(storeType: "ntext"));
        }
    }
}
