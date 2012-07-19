using Nancy;

namespace EssenceBlog.WebApp.Core.Modules
{
  public class IndexModule : NancyModule
  {
    public IndexModule()
    {
      Get["/"] =
        @params => "Hello, world!!!" ;
    }
  }
}
