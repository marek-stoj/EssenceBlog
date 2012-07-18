using System;

namespace EssenceBlog.Core
{
  public interface IPostsRepository
  {
    Post GetById(Guid postId);
  }
}
