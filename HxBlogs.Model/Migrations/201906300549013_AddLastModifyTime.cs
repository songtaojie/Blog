namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLastModifyTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("User", "LastModifyTime", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("User", "LastModifyTime");
        }
    }
}
