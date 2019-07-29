








using HxBlogs.IBLL;
using HxBlogs.Model;
using HxBlogs.IDAL;
namespace HxBlogs.BLL
{

	public partial class AttentionService:BaseService<Attention>,IAttentionService
	{
		private IAttentionDal _dal;
		public AttentionService(IAttentionDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}

	public partial class BasicInfoService:BaseService<BasicInfo>,IBasicInfoService
	{
		private IBasicInfoDal _dal;
		public BasicInfoService(IBasicInfoDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}

	public partial class JobInfoService:BaseService<JobInfo>,IJobInfoService
	{
		private IJobInfoDal _dal;
		public JobInfoService(IJobInfoDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}

	public partial class SystemConfigService:BaseService<SystemConfig>,ISystemConfigService
	{
		private ISystemConfigDal _dal;
		public SystemConfigService(ISystemConfigDal dal)
        {
			this._dal = dal;
			this.baseDal = dal;
        }
	}

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

	public partial class CategoryService:BaseService<Category>,ICategoryService
	{
		private ICategoryDal _dal;
		public CategoryService(ICategoryDal dal)
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

	public partial class ReplyCommentService:BaseService<ReplyComment>,IReplyCommentService
	{
		private IReplyCommentDal _dal;
		public ReplyCommentService(IReplyCommentDal dal)
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