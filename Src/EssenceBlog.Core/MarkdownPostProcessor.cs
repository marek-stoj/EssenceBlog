using System;
using MarkdownSharp;

namespace EssenceBlog.Core
{
  public class MarkdownPostProcessor : IPostProcessor
  {
    public string CreatePostHtml(string postBody)
    {
      if (string.IsNullOrEmpty(postBody))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "postBody");
      }

      var markdown = new Markdown();

      return markdown.Transform(postBody);
    }
  }
}
