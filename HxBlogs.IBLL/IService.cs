using HxBlogs.Model;
namespace HxBlogs.IBLL
{
	public partial interface IAttentionService:IBaseService<Attention>
	{
	}
	public partial interface IBlogService:IBaseService<Blog>
	{
	}
	public partial interface IBlogTagService:IBaseService<BlogTag>
	{
	}
	public partial interface IBlogTypeService:IBaseService<BlogType>
	{
	}
	public partial interface ICategoryService:IBaseService<Category>
	{
	}
	public partial interface ICommentService:IBaseService<Comment>
	{
	}
	public partial interface IReplyCommentService:IBaseService<ReplyComment>
	{
	}
	public partial interface IUserService:IBaseService<User>
	{
	}
}