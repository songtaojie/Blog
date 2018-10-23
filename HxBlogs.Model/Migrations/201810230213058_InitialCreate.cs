namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        ReadNumber = c.Int(),
                        CommentNumber = c.Int(),
                        IsHome = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        IsMyHome = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        IsForward = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        IsShare = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        ShareDate = c.DateTime(precision: 0),
                        IsTop = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        IsEssence = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        ForwardUrl = c.String(maxLength: 255, storeType: "nvarchar"),
                        OldPublishTime = c.DateTime(precision: 0),
                        CanCmt = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        LikeCount = c.Int(),
                        FavCount = c.Int(),
                        PersonTop = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        ImgUrl = c.String(maxLength: 255, storeType: "nvarchar"),
                        ImgName = c.String(maxLength: 100, storeType: "nvarchar"),
                        Location = c.String(maxLength: 255, storeType: "nvarchar"),
                        OrderFactor = c.Decimal(precision: 18, scale: 4),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        CreatorId = c.Int(),
                        CreatorName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        DeleteId = c.Int(),
                        DeleteName = c.String(maxLength: 50, storeType: "nvarchar"),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                        LastModifiyId = c.Int(),
                        LastModifiyName = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.BlogBlogTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blog", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.BlogTag", t => t.TagId, cascadeDelete: true)
                .Index(t => t.BlogId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.BlogTag",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50, storeType: "nvarchar"),
                        Remark = c.String(unicode: false, storeType: "text"),
                        UserId = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        CreatorId = c.Int(),
                        CreatorName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        DeleteId = c.Int(),
                        DeleteName = c.String(maxLength: 50, storeType: "nvarchar"),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                        LastModifiyId = c.Int(),
                        LastModifiyName = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        NickName = c.String(maxLength: 100, storeType: "nvarchar"),
                        PassWord = c.String(nullable: false, maxLength: 40, storeType: "nvarchar"),
                        RealName = c.String(maxLength: 100, storeType: "nvarchar"),
                        CardId = c.String(maxLength: 20, storeType: "nvarchar"),
                        Birthday = c.DateTime(precision: 0),
                        Email = c.String(maxLength: 80, storeType: "nvarchar"),
                        Gender = c.String(maxLength: 8, storeType: "nvarchar"),
                        Telephone = c.String(maxLength: 20, storeType: "nvarchar"),
                        Remarks = c.String(unicode: false, storeType: "text"),
                        OpenId = c.String(maxLength: 80, storeType: "nvarchar"),
                        CanLogin = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        AvatarName = c.String(maxLength: 100, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 20, storeType: "nvarchar"),
                        IsRoot = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BlogBlogType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blog", t => t.BlogId, cascadeDelete: true)
                .ForeignKey("dbo.BlogType", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.BlogId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.BlogType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Remark = c.String(unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        CreatorId = c.Int(),
                        CreatorName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        DeleteId = c.Int(),
                        DeleteName = c.String(maxLength: 50, storeType: "nvarchar"),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                        LastModifiyId = c.Int(),
                        LastModifiyName = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(unicode: false, storeType: "text"),
                        Top = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        IPAddress = c.String(maxLength: 40, storeType: "nvarchar"),
                        BlogId = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        CreatorId = c.Int(),
                        CreatorName = c.String(maxLength: 50, storeType: "nvarchar"),
                        IsDeleted = c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"),
                        DeleteId = c.Int(),
                        DeleteName = c.String(maxLength: 50, storeType: "nvarchar"),
                        DeleteTime = c.DateTime(precision: 0),
                        LastModifyTime = c.DateTime(precision: 0),
                        LastModifiyId = c.Int(),
                        LastModifiyName = c.String(maxLength: 50, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Blog", t => t.BlogId, cascadeDelete: true)
                .Index(t => t.BlogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blog", "CreatorId", "dbo.UserInfo");
            DropForeignKey("dbo.Comment", "BlogId", "dbo.Blog");
            DropForeignKey("dbo.BlogType", "CreatorId", "dbo.UserInfo");
            DropForeignKey("dbo.BlogBlogType", "TypeId", "dbo.BlogType");
            DropForeignKey("dbo.BlogBlogType", "BlogId", "dbo.Blog");
            DropForeignKey("dbo.BlogTag", "UserId", "dbo.UserInfo");
            DropForeignKey("dbo.BlogBlogTag", "TagId", "dbo.BlogTag");
            DropForeignKey("dbo.BlogBlogTag", "BlogId", "dbo.Blog");
            DropIndex("dbo.Comment", new[] { "BlogId" });
            DropIndex("dbo.BlogType", new[] { "CreatorId" });
            DropIndex("dbo.BlogBlogType", new[] { "TypeId" });
            DropIndex("dbo.BlogBlogType", new[] { "BlogId" });
            DropIndex("dbo.BlogTag", new[] { "UserId" });
            DropIndex("dbo.BlogBlogTag", new[] { "TagId" });
            DropIndex("dbo.BlogBlogTag", new[] { "BlogId" });
            DropIndex("dbo.Blog", new[] { "CreatorId" });
            DropTable("dbo.Comment");
            DropTable("dbo.BlogType");
            DropTable("dbo.BlogBlogType");
            DropTable("dbo.UserInfo");
            DropTable("dbo.BlogTag");
            DropTable("dbo.BlogBlogTag");
            DropTable("dbo.Blog");
        }
    }
}
