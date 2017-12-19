namespace Imagine.BookManager.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class brian : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        InstitutionId = c.Int(),
                        FullName = c.String(maxLength: 30),
                        UserType = c.Int(nullable: false),
                        Gender = c.Boolean(nullable: false),
                        Mobile = c.String(maxLength: 12),
                        Email = c.String(maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 23),
                        DateCreated = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        LastLoginDate = c.DateTime(),
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Institution", t => t.InstitutionId)
                .Index(t => t.InstitutionId);
            
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        DateCreated = c.DateTime(nullable: false),
                        ReminderInterva = c.Int(),
                        InstitutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institution", t => t.InstitutionId, cascadeDelete: true)
                .Index(t => t.InstitutionId);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        StudentId = c.Guid(nullable: false, identity: true),
                        FullName = c.String(maxLength: 50),
                        Gender = c.Boolean(nullable: false),
                        DateOfBirth = c.DateTime(),
                        Picture = c.String(maxLength: 50),
                        GuardianName = c.String(maxLength: 50),
                        Mobile = c.String(maxLength: 12),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 23),
                        DateCreated = c.DateTime(nullable: false),
                        ClassId = c.Int(),
                        IsDelete = c.Boolean(nullable: false),
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Class", t => t.ClassId)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Institution",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Tel = c.String(maxLength: 20),
                        District = c.String(maxLength: 100),
                        Address = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderRef = c.String(nullable: false, maxLength: 64),
                        UserId = c.Guid(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryCharge = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Paid = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.OrderRef)
                .ForeignKey("dbo.Admin", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderRef = c.String(nullable: false, maxLength: 64),
                        SetId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        RemainCredit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Set", t => t.SetId)
                .ForeignKey("dbo.Order", t => t.OrderRef)
                .Index(t => t.OrderRef)
                .Index(t => t.SetId);
            
            CreateTable(
                "dbo.Set",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SetName = c.String(nullable: false, maxLength: 50),
                        Synopsis = c.String(nullable: false, maxLength: 1000),
                        ImageUrl = c.String(nullable: false, maxLength: 100),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Expiry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookName = c.String(nullable: false, maxLength: 100),
                        SetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Set", t => t.SetId)
                .Index(t => t.SetId);
            
            CreateTable(
                "dbo.CartItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CartId = c.Long(nullable: false),
                        SetId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Set", t => t.SetId)
                .ForeignKey("dbo.ShoppingCart", t => t.CartId)
                .Index(t => t.CartId)
                .Index(t => t.SetId);
            
            CreateTable(
                "dbo.Payment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderRef = c.String(nullable: false, maxLength: 64),
                        GatewayRef = c.String(maxLength: 64),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymenGateway = c.Int(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DatePaid = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderRef)
                .Index(t => t.OrderRef);
            
            CreateTable(
                "dbo.ShoppingCart",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrderRef = c.String(maxLength: 64),
                        UserId = c.Guid(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderRef)
                .ForeignKey("dbo.Admin", t => t.UserId)
                .Index(t => t.OrderRef)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TeachClass",
                c => new
                    {
                        TeacherId = c.Guid(nullable: false),
                        ClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TeacherId, t.ClassId })
                .ForeignKey("dbo.Admin", t => t.TeacherId, cascadeDelete: true)
                .ForeignKey("dbo.Class", t => t.ClassId, cascadeDelete: true)
                .Index(t => t.TeacherId)
                .Index(t => t.ClassId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingCart", "UserId", "dbo.Admin");
            DropForeignKey("dbo.Order", "UserId", "dbo.Admin");
            DropForeignKey("dbo.ShoppingCart", "OrderRef", "dbo.Order");
            DropForeignKey("dbo.CartItem", "CartId", "dbo.ShoppingCart");
            DropForeignKey("dbo.Payment", "OrderRef", "dbo.Order");
            DropForeignKey("dbo.OrderItem", "OrderRef", "dbo.Order");
            DropForeignKey("dbo.OrderItem", "SetId", "dbo.Set");
            DropForeignKey("dbo.CartItem", "SetId", "dbo.Set");
            DropForeignKey("dbo.Book", "SetId", "dbo.Set");
            DropForeignKey("dbo.Class", "InstitutionId", "dbo.Institution");
            DropForeignKey("dbo.Admin", "InstitutionId", "dbo.Institution");
            DropForeignKey("dbo.TeachClass", "ClassId", "dbo.Class");
            DropForeignKey("dbo.TeachClass", "TeacherId", "dbo.Admin");
            DropForeignKey("dbo.Student", "ClassId", "dbo.Class");
            DropIndex("dbo.TeachClass", new[] { "ClassId" });
            DropIndex("dbo.TeachClass", new[] { "TeacherId" });
            DropIndex("dbo.ShoppingCart", new[] { "UserId" });
            DropIndex("dbo.ShoppingCart", new[] { "OrderRef" });
            DropIndex("dbo.Payment", new[] { "OrderRef" });
            DropIndex("dbo.CartItem", new[] { "SetId" });
            DropIndex("dbo.CartItem", new[] { "CartId" });
            DropIndex("dbo.Book", new[] { "SetId" });
            DropIndex("dbo.OrderItem", new[] { "SetId" });
            DropIndex("dbo.OrderItem", new[] { "OrderRef" });
            DropIndex("dbo.Order", new[] { "UserId" });
            DropIndex("dbo.Institution", new[] { "Name" });
            DropIndex("dbo.Student", new[] { "ClassId" });
            DropIndex("dbo.Student", new[] { "UserName" });
            DropIndex("dbo.Class", new[] { "InstitutionId" });
            DropIndex("dbo.Admin", new[] { "InstitutionId" });
            DropTable("dbo.TeachClass");
            DropTable("dbo.ShoppingCart");
            DropTable("dbo.Payment");
            DropTable("dbo.CartItem");
            DropTable("dbo.Book");
            DropTable("dbo.Set");
            DropTable("dbo.OrderItem");
            DropTable("dbo.Order");
            DropTable("dbo.Institution");
            DropTable("dbo.Student");
            DropTable("dbo.Class");
            DropTable("dbo.Admin");
        }
    }
}
