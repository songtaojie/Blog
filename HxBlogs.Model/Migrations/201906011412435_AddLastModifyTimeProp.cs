namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastModifyTimeProp : DbMigration
    {
        public override void Up()
        {
            AddColumn("Blog", "LastModifyTime", c => c.DateTime(precision: 0));
            AddColumn("BlogType", "LastModifyTime", c => c.DateTime(precision: 0));
            AddColumn("Category", "LastModifyTime", c => c.DateTime(precision: 0));
            AddColumn("Comment", "LastModifyTime", c => c.DateTime(precision: 0));
            AddColumn("BlogTag", "LastModifyTime", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("BlogTag", "LastModifyTime");
            DropColumn("Comment", "LastModifyTime");
            DropColumn("Category", "LastModifyTime");
            DropColumn("BlogType", "LastModifyTime");
            DropColumn("Blog", "LastModifyTime");
        }
    }
}
