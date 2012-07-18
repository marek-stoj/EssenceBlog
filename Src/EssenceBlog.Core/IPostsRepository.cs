namespace EssenceBlog.Core
{
  public interface IPostsRepository
  {
    Post GetById(string postId);
  }
}
