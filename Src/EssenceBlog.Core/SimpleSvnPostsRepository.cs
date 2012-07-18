using System;
using System.IO;

namespace EssenceBlog.Core
{
  public class SimpleSvnPostsRepository : IPostsRepository
  {
    private readonly string _workingCopyAbsolutePath;

    #region Constructor(s)

    public SimpleSvnPostsRepository(string workingCopyAbsolutePath)
    {
      if (string.IsNullOrEmpty(workingCopyAbsolutePath))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "workingCopyAbsolutePath");
      }

      _workingCopyAbsolutePath = workingCopyAbsolutePath;
    }

    #endregion

    #region IPostsRepository Members

    public Post GetById(string postId)
    {
      if (string.IsNullOrEmpty(postId))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "postId");
      }

      string filePath = Path.Combine(_workingCopyAbsolutePath, string.Format("{0}.md", postId));

      if (!File.Exists(filePath))
      {
        return null;
      }

      return
        new Post
          {
            Title = CreateTitleFromId(postId),
            Body = File.ReadAllText(filePath),
          };
    }

    #endregion

    #region Private members

    private static string CreateTitleFromId(string postId)
    {
      string title = postId;

      title = title.Replace("-", " ");
      title = Char.ToUpper(title[0]) + title.Substring(1, title.Length - 1);

      return title;
    }

    #endregion
  }
}
