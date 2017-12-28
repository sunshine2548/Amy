namespace Imagine.BookManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_TeacherAllocation_Lucas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItem", "SetId", "dbo.Set");
            CreateTable(
                "dbo.TeacherAllocation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderItemId = c.Long(nullable: false),
                        SetId = c.Int(nullable: false),
                        TeacherId = c.Guid(nullable: false),
                        Credit = c.Int(nullable: false),
                        DateAllocated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admin", t => t.TeacherId)
                .ForeignKey("dbo.OrderItem", t => t.OrderItemId)
                .ForeignKey("dbo.Set", t => t.SetId)
                .Index(t => t.OrderItemId)
                .Index(t => t.SetId)
                .Index(t => t.TeacherId);
            
            AddColumn("dbo.Admin", "Picture", c => c.String(maxLength: 200));
            AddColumn("dbo.OrderItem", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderItem", "UserId", c => c.Guid(nullable: false));
            AddColumn("dbo.Set", "OriginalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddForeignKey("dbo.OrderItem", "SetId", "dbo.Set", "Id", cascadeDelete: true);
            DropColumn("dbo.Set", "Expiry");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Set", "Expiry", c => c.Int(nullable: false));
            DropForeignKey("dbo.OrderItem", "SetId", "dbo.Set");
            DropForeignKey("dbo.TeacherAllocation", "SetId", "dbo.Set");
            DropForeignKey("dbo.TeacherAllocation", "OrderItemId", "dbo.OrderItem");
            DropForeignKey("dbo.TeacherAllocation", "TeacherId", "dbo.Admin");
            DropIndex("dbo.TeacherAllocation", new[] { "TeacherId" });
            DropIndex("dbo.TeacherAllocation", new[] { "SetId" });
            DropIndex("dbo.TeacherAllocation", new[] { "OrderItemId" });
            DropColumn("dbo.Set", "OriginalPrice");
            DropColumn("dbo.OrderItem", "UserId");
            DropColumn("dbo.OrderItem", "Discount");
            DropColumn("dbo.Admin", "Picture");
            DropTable("dbo.TeacherAllocation");
            AddForeignKey("dbo.OrderItem", "SetId", "dbo.Set", "Id");
        }
    }
}
