using Nancy;

namespace EssenceBlog.WebApp.Modules
{
  public class IndexModule : NancyModule
  {
    public IndexModule()
    {
      Get["/"] =
        @params => "Hello, world!" ;
    }
  }
}
