namespace LMPlatform.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsCoursProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLabFiles", "IsCoursProject", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLabFiles", "IsCoursProject");
        }
    }
}
