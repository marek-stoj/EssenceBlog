using System;
using EssenceBlog.Core;
using EssenceBlog.WebApp.ViewModels;
using Nancy;

namespace EssenceBlog.WebApp.Modules
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

      Get[@"/(?<PostId>\d+)"] =
        @params => ViewPost(@params);
    }

    private Response ViewPost(dynamic @params)
    {
      Post post = _postsRepository.GetById(Guid.NewGuid());
      
      var postViewModel =
        new PostViewModel
          {
            PostTitle = "Don't Let Architecture Astronauts Scare You",
            PostBodyHtml = _postProcessor.CreatePostHtml(post.Body),
          };

      return View["Post", postViewModel];
    }
  }
}
