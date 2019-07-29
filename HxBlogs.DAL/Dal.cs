








using HxBlogs.Model;
using HxBlogs.IDAL;
namespace HxBlogs.DAL
{

	public partial class AttentionDal:BaseDal<Attention>,IAttentionDal
	{
	}

	public partial class BasicInfoDal:BaseDal<BasicInfo>,IBasicInfoDal
	{
	}

	public partial class JobInfoDal:BaseDal<JobInfo>,IJobInfoDal
	{
	}

	public partial class SystemConfigDal:BaseDal<SystemConfig>,ISystemConfigDal
	{
	}

	public partial class BlogDal:BaseDal<Blog>,IBlogDal
	{
	}

	public partial class BlogTagDal:BaseDal<BlogTag>,IBlogTagDal
	{
	}

	public partial class BlogTypeDal:BaseDal<BlogType>,IBlogTypeDal
	{
	}

	public partial class CategoryDal:BaseDal<Category>,ICategoryDal
	{
	}

	public partial class CommentDal:BaseDal<Comment>,ICommentDal
	{
	}

	public partial class ReplyCommentDal:BaseDal<ReplyComment>,IReplyCommentDal
	{
	}

	public partial class UserInfoDal:BaseDal<UserInfo>,IUserInfoDal
	{
	}

}