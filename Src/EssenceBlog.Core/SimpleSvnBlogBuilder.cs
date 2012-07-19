using System;
using System.IO;
using System.Net;
using SharpSvn;

namespace EssenceBlog.Core
{
  public class SimpleSvnBlogBuilder : IBlogBuilder
  {
    private readonly string _repositoryBaseUrl;
    private readonly string _repositoryPostsPath;
    private readonly string _repositoryUsername;
    private readonly string _repositoryPassword;
    private readonly string _workingCopyPath;

    private readonly string _repositoryPostsUrl;

    #region Constructor(s)

    public SimpleSvnBlogBuilder(string repositoryBaseUrl, string repositoryPostsPath, string repositoryUsername, string repositoryPassword, string workingCopyPath)
    {
      if (string.IsNullOrEmpty(repositoryBaseUrl))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "repositoryBaseUrl");
      }

      if (string.IsNullOrEmpty(repositoryPostsPath))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "repositoryPostsPath");
      }

      if (string.IsNullOrEmpty(repositoryUsername))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "repositoryUsername");
      }

      if (string.IsNullOrEmpty(repositoryPassword))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "repositoryPassword");
      }

      if (string.IsNullOrEmpty(workingCopyPath))
      {
        throw new ArgumentException("Argument can't be null nor empty.", "workingCopyPath");
      }

      _repositoryBaseUrl = repositoryBaseUrl;
      _repositoryPostsPath = repositoryPostsPath;
      _workingCopyPath = workingCopyPath;
      _repositoryUsername = repositoryUsername;
      _repositoryPassword = repositoryPassword;

      _repositoryPostsUrl = CreateRepositoryPostsUrl();
    }

    #endregion

    #region IBlogBuilder Members

    public void BuildBlog()
    {
      using (var svnClient = new SvnClient())
      {
        svnClient.UseDefaultConfiguration();

        svnClient.Authentication.Clear();

        svnClient.Authentication.DefaultCredentials =
          new NetworkCredential(_repositoryUsername, _repositoryPassword);

        SvnUpdateResult result;

        if (!Directory.Exists(_workingCopyPath)
         || svnClient.GetUriFromWorkingCopy(_workingCopyPath) == null)
        {
          svnClient.CheckOut(
            new SvnUriTarget(_repositoryPostsUrl),
            _workingCopyPath,
            out result);
        }
        else
        {
          svnClient.Conflict +=
            (sender, args) => { args.Choice = SvnAccept.TheirsFull; };

          svnClient.Update(
            _workingCopyPath,
            out result);
        }

        if (!result.HasRevision)
        {
          throw new InternalException("Couldn't checkout or update the repository.");
        }
      }
    }

    #endregion

    #region Private members

    private string CreateRepositoryPostsUrl()
    {
      return
        string.Format(
          "{0}/{1}",
          _repositoryBaseUrl.TrimEnd('/'),
          _repositoryPostsPath.TrimStart('/'));
    }

    #endregion
  }
}
