namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V60 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetailInformations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrderDetailId = c.Guid(nullable: false),
                        FileUrl = c.String(),
                        OrderDetailStatusId = c.Guid(),
                        ProductId = c.Guid(),
                        PublishLink = c.String(),
                        IsSendPublishSms = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreateUserId = c.Guid(),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        DeleteUserId = c.Guid(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderDetails", t => t.OrderDetailId, cascadeDelete: true)
                .ForeignKey("dbo.OrderDetailStatus", t => t.OrderDetailStatusId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .Index(t => t.OrderDetailId)
                .Index(t => t.OrderDetailStatusId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetailInformations", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetailInformations", "OrderDetailStatusId", "dbo.OrderDetailStatus");
            DropForeignKey("dbo.OrderDetailInformations", "OrderDetailId", "dbo.OrderDetails");
            DropIndex("dbo.OrderDetailInformations", new[] { "ProductId" });
            DropIndex("dbo.OrderDetailInformations", new[] { "OrderDetailStatusId" });
            DropIndex("dbo.OrderDetailInformations", new[] { "OrderDetailId" });
            DropTable("dbo.OrderDetailInformations");
        }
    }
}
