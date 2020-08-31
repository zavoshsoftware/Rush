namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v29 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "WritterId", c => c.Guid());
            CreateIndex("dbo.Blogs", "WritterId");
            AddForeignKey("dbo.Blogs", "WritterId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "WritterId", "dbo.Users");
            DropIndex("dbo.Blogs", new[] { "WritterId" });
            DropColumn("dbo.Blogs", "WritterId");
        }
    }
}
