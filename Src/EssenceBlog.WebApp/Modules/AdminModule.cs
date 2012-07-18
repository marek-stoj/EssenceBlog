using System;
using EssenceBlog.Core;
using Nancy;

namespace EssenceBlog.WebApp.Modules
{
  public class AdminModule : NancyModule
  {
    private readonly IBlogBuilder _blogBuilder;

    public AdminModule(IBlogBuilder blogBuilder)
      : base("admin")
    {
      if (blogBuilder == null)
      {
        throw new ArgumentNullException("blogBuilder");
      }

      _blogBuilder = blogBuilder;

      DefineRoutes();
    }

    private void DefineRoutes()
    {
      Get["/"] =
        @params => "Admin";

      Get[@"/build-blog"] =
        @params => BuildBlog(@params);
    }

    private Response BuildBlog(dynamic @params)
    {
      _blogBuilder.BuildBlog();

      return "OK";
    }
  }
}
