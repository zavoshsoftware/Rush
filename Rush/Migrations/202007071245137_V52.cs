namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V52 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StepDiscountDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StepDiscountId = c.Guid(nullable: false),
                        TargetValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountPercent = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                .ForeignKey("dbo.StepDiscounts", t => t.StepDiscountId, cascadeDelete: true)
                .Index(t => t.StepDiscountId);
            
            CreateTable(
                "dbo.StepDiscounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
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
            
            AddColumn("dbo.Reportages", "BuyAmount", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Reportages", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reportages", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.StepDiscountDetails", "StepDiscountId", "dbo.StepDiscounts");
            DropIndex("dbo.StepDiscountDetails", new[] { "StepDiscountId" });
            DropColumn("dbo.Reportages", "BuyAmount");
            DropTable("dbo.StepDiscounts");
            DropTable("dbo.StepDiscountDetails");
        }
    }
}
