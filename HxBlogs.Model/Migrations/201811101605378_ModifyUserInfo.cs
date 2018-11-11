namespace HxBlogs.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyUserInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfo", "CreateTime", c => c.DateTime( precision: 0));
            AddColumn("dbo.UserInfo", "IsDeleted", c => c.String(maxLength: 1, fixedLength: true, unicode: false, storeType: "char"));
            AddColumn("dbo.UserInfo", "DeleteTime", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfo", "DeleteTime");
            DropColumn("dbo.UserInfo", "IsDeleted");
            DropColumn("dbo.UserInfo", "CreateTime");
        }
    }
}
