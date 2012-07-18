namespace EssenceBlog.Core
{
  public interface IPostProcessor
  {
    string CreatePostHtml(string postBody);
  }
}
