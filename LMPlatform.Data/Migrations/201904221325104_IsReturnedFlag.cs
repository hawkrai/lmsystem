namespace LMPlatform.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsReturnedFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLabFiles", "IsReturned", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLabFiles", "IsReturned");
        }
    }
}
