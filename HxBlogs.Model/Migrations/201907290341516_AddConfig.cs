namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemConfig",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Register = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemConfig");
        }
    }
}
