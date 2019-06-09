namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Content = c.String(unicode: false, storeType: "text"),
                        ContentHtml = c.String(unicode: false, storeType: "text"),
                        IsMarkDown = c.Boolean(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        IsForward = c.Boolean(nullable: false),
                        IsPublish = c.Boolean(nullable: false),
                        PublishDate = c.DateTime(precision: 0),
                        IsTop = c.Boolean(nullable: false),
                        IsEssence = c.Boolean(nullable: false),
                        ForwardUrl = c.String(maxLength: 255, storeType: "nvarchar"),
                        OldPublishTime = c.DateTime(precision: 0),
                        TypeId = c.Int(nullable: false),
                        CatId = c.Int(nullable: false),
                        BlogTags = c.String(maxLength: 40, storeType: "nvarchar"),
                        CanCmt = c.Boolean(nullable: false),
                        ReadCount = c.Int(),
                        LikeCount = c.Int(),
                        FavCount = c.Int(),
                        CmtCount = c.Int(),
                        PersonTop = c.Boolean(nullable: false),
                        ImgUrl = c.String(maxLength: 255, storeType: "nvarchar"),
                        ImgName = c.String(maxLength: 100, storeType: "nvarchar"),
                        Location = c.String(maxLength: 255, storeType: "nvarchar"),
                        City = c.String(maxLength: 50, storeType: "nvarchar"),
                        OrderFactor = c.Decimal(precision: 18, scale: 4),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteId = c.Int(),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogType", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.CatId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TypeId)
                .Index(t => t.CatId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BlogType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 40, storeType: "nvarchar"),
                        Order = c.Int(nullable: false),
                        Remark = c.String(unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteId = c.Int(),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 40, storeType: "nvarchar"),
                        Order = c.Int(nullable: false),
                        Remarks = c.String(unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteId = c.Int(),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        PassWord = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        NickName = c.String(maxLength: 100, storeType: "nvarchar"),
                        RealName = c.String(maxLength: 100, storeType: "nvarchar"),
                        CardId = c.String(maxLength: 20, storeType: "nvarchar"),
                        Birthday = c.DateTime(precision: 0),
                        Gender = c.String(maxLength: 8, storeType: "nvarchar"),
                        QQ = c.String(maxLength: 20, storeType: "nvarchar"),
                        WeChat = c.String(maxLength: 20, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 80, storeType: "nvarchar"),
                        Telephone = c.String(maxLength: 20, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 20, storeType: "nvarchar"),
                        Description = c.String(maxLength: 1000, storeType: "nvarchar"),
                        OpenId = c.String(maxLength: 80, storeType: "nvarchar"),
                        IsLock = c.Boolean(nullable: false),
                        AvatarUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                        IsAdmin = c.Boolean(nullable: false),
                        IsActivate = c.Boolean(nullable: false),
                        RegisterTime = c.DateTime(precision: 0),
                        RegisterIp = c.String(unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteTime = c.DateTime(precision: 0),
                        Address = c.String(maxLength: 100, storeType: "nvarchar"),
                        School = c.String(maxLength: 100, storeType: "nvarchar"),
                        UseMdEdit = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BlogTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 40, storeType: "nvarchar"),
                        Description = c.String(unicode: false),
                        Order = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.Boolean(nullable: false),
                        DeleteId = c.Int(),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blog", "UserId", "dbo.User");
            DropForeignKey("dbo.Blog", "CatId", "dbo.Category");
            DropForeignKey("dbo.Blog", "TypeId", "dbo.BlogType");
            DropIndex("dbo.Blog", new[] { "UserId" });
            DropIndex("dbo.Blog", new[] { "CatId" });
            DropIndex("dbo.Blog", new[] { "TypeId" });
            DropTable("dbo.BlogTag");
            DropTable("dbo.User");
            DropTable("dbo.Category");
            DropTable("dbo.BlogType");
            DropTable("dbo.Blog");
        }
    }
}
