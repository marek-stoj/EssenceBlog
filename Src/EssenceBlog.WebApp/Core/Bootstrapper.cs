using System;
using System.Configuration;
using System.Web;
using EssenceBlog.Core;
using Nancy;
using Nancy.Diagnostics;
using Nancy.Hosting.Aspnet;
using TinyIoC;

namespace EssenceBlog.WebApp.Core
{
  public class Bootstrapper : DefaultNancyAspNetBootstrapper
  {
    private const string _AppSettingsKey_RepositoryBaseUrl = "RepositoryBaseUrl";
    private const string _AppSettingsKey_RepositoryPostsPath = "RepositoryPostsPath";
    private const string _AppSettingsKey_RepositoryUsername = "RepositoryUsername";
    private const string _AppSettingsKey_RepositoryPassword = "RepositoryPassword";

    private const string _WorkingCopyVirtualPath = "~/App_Data/WorkingCopy";

    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
      HttpContext httpContext = HttpContext.Current;

      string repositoryBaseUrl = ReadAppSettingsString(_AppSettingsKey_RepositoryBaseUrl);
      string repositoryPostsPath = ReadAppSettingsString(_AppSettingsKey_RepositoryPostsPath);
      string repositoryUsername = ReadAppSettingsString(_AppSettingsKey_RepositoryUsername);
      string repositoryPassword = ReadAppSettingsString(_AppSettingsKey_RepositoryPassword);
      string workingCopyAbsolutePath = httpContext.Server.MapPath(_WorkingCopyVirtualPath);

      container
        .Register<IBlogBuilder>(
          (_, __) =>
          new SimpleSvnBlogBuilder(
            repositoryBaseUrl,
            repositoryPostsPath,
            repositoryUsername,
            repositoryPassword,
            workingCopyAbsolutePath));

      container
        .Register<IPostsRepository>(
          (_, __) => new SimpleSvnPostsRepository(workingCopyAbsolutePath));

      container.AutoRegister(
        t => t != typeof(IBlogBuilder)
          && t != typeof(IPostsRepository));

      container.Register<INancyModuleCatalog>(this);
    }

    protected override DiagnosticsConfiguration DiagnosticsConfiguration
    {
      get { return new DiagnosticsConfiguration { Password = "123." }; }
    }

    private static string ReadAppSettingsString(string appSettingsKey)
    {
      if (string.IsNullOrEmpty(appSettingsKey))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "appSettingsKey");
      }

      string value = ConfigurationManager.AppSettings[appSettingsKey];

      if (value == null)
      {
        throw new ConfigurationErrorsException(string.Format("No app setting with key '{0}'.", appSettingsKey));
      }

      return value;
    }
  }
}
