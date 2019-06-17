namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttention : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attention",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AttentionId = c.Long(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        UserId = c.Long(nullable: false),
                        UserName = c.String(maxLength: 50, storeType: "nvarchar"),
                        Delete = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        DeleteId = c.Long(),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Attention");
        }
    }
}
