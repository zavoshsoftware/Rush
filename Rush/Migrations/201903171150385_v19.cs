namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogComments", "ParentId", c => c.Guid());
            CreateIndex("dbo.BlogComments", "ParentId");
            AddForeignKey("dbo.BlogComments", "ParentId", "dbo.BlogComments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlogComments", "ParentId", "dbo.BlogComments");
            DropIndex("dbo.BlogComments", new[] { "ParentId" });
            DropColumn("dbo.BlogComments", "ParentId");
        }
    }
}
