namespace Imagine.BookManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_StudenAllocation_ExpiryData_Lucas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentAllocation", "ExpiryDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentAllocation", "ExpiryDate");
        }
    }
}
