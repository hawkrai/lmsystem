namespace LMPlatform.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTinCanObjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TinCanObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TinCanObjects");
        }
    }
}
