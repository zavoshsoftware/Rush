namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BackLinks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(),
                        Address = c.String(),
                        OneMonthBackLink = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ThreeMonthBackLink = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Priority = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BackLinks");
        }
    }
}
