using System;
using EssenceBlog.Core;
using EssenceBlog.WebApp.Core.ViewModels;
using Nancy;

namespace EssenceBlog.WebApp.Core.Modules
{
  public class PostsModule : NancyModule
  {
    private readonly IPostsRepository _postsRepository;
    private readonly IPostProcessor _postProcessor;

    public PostsModule(IPostsRepository postsRepository, IPostProcessor postProcessor)
      : base("posts")
    {
      if (postsRepository == null)
      {
        throw new ArgumentNullException("postsRepository");
      }

      if (postProcessor == null)
      {
        throw new ArgumentNullException("postProcessor");
      }

      _postsRepository = postsRepository;
      _postProcessor = postProcessor;

      DefineRoutes();
    }

    // TODO IMM HI: ask Nancy: why does it have to be defined in an instance constructor??
    private void DefineRoutes()
    {
      Get["/"] =
        @params => "Posts Index";

      Get[@"/(?<PostId>.+)"] =
        @params => ViewPost(@params);
    }

    private Response ViewPost(dynamic @params)
    {
      Post post = _postsRepository.GetById(@params.PostId);

      if (post == null)
      {
        return 404;
      }

      var postViewModel =
        new PostViewModel
          {
            PostTitle = post.Title ?? "Untitled",
            PostBodyHtml = !string.IsNullOrEmpty(post.Body) ? _postProcessor.CreatePostHtml(post.Body) : "",
          };

      return View["Post", postViewModel];
    }
  }
}
