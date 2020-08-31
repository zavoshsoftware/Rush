namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V62 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BackLinks", "ProductId", "dbo.Products");
            DropIndex("dbo.BackLinks", new[] { "ProductId" });
            CreateTable(
                "dbo.BackLinkDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Duration = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BackLinkId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
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
                .ForeignKey("dbo.BackLinks", t => t.BackLinkId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.BackLinkId)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.OrderDetailInformations", "Duration", c => c.Int(nullable: false));
            DropColumn("dbo.BackLinks", "ProductId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BackLinks", "ProductId", c => c.Guid());
            DropForeignKey("dbo.BackLinkDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.BackLinkDetails", "BackLinkId", "dbo.BackLinks");
            DropIndex("dbo.BackLinkDetails", new[] { "ProductId" });
            DropIndex("dbo.BackLinkDetails", new[] { "BackLinkId" });
            DropColumn("dbo.OrderDetailInformations", "Duration");
            DropTable("dbo.BackLinkDetails");
            CreateIndex("dbo.BackLinks", "ProductId");
            AddForeignKey("dbo.BackLinks", "ProductId", "dbo.Products", "Id");
        }
    }
}
