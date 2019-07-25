namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blog", "Carousel", c => c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blog", "Carousel");
        }
    }
}
