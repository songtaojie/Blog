using HxBlogs.Model;
namespace HxBlogs.IBLL
{
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
	public partial interface IUserInfoService:IBaseService<UserInfo>
	{
	}
}