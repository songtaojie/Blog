using HxBlogs.IBLL;
using HxBlogs.Model;
using HxBlogs.IDAL;
namespace HxBlogs.BLL
{
	public partial class BlogService:BaseService<Blog>,IBlogService
	{
		private IBlogDal _dal;
		public BlogService(IBlogDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}
	public partial class BlogTagService:BaseService<BlogTag>,IBlogTagService
	{
		private IBlogTagDal _dal;
		public BlogTagService(IBlogTagDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}
	public partial class BlogTypeService:BaseService<BlogType>,IBlogTypeService
	{
		private IBlogTypeDal _dal;
		public BlogTypeService(IBlogTypeDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}
	public partial class CommentService:BaseService<Comment>,ICommentService
	{
		private ICommentDal _dal;
		public CommentService(ICommentDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}
	public partial class UserInfoService:BaseService<UserInfo>,IUserInfoService
	{
		private IUserInfoDal _dal;
		public UserInfoService(IUserInfoDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}
}