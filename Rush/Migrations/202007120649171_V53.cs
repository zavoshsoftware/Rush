namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V53 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetailStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 30),
                        Code = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OrderDetails", "OrderDetailStatusId", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrderDetails", "OrderDetailStatusId");
            AddForeignKey("dbo.OrderDetails", "OrderDetailStatusId", "dbo.OrderDetailStatus", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "OrderDetailStatusId", "dbo.OrderDetailStatus");
            DropIndex("dbo.OrderDetails", new[] { "OrderDetailStatusId" });
            DropColumn("dbo.OrderDetails", "OrderDetailStatusId");
            DropTable("dbo.OrderDetailStatus");
        }
    }
}
