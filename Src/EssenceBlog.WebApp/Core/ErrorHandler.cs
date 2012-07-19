using System;
using System.Reflection;
using Nancy;
using Nancy.ErrorHandling;
using log4net;

namespace EssenceBlog.WebApp.Core
{
  public class ErrorHandler : IErrorHandler
  {
    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    #region IErrorHandler Members

    public void Handle(HttpStatusCode statusCode, NancyContext context)
    {
      // do nothing
    }

    public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
    {
      if (statusCode == HttpStatusCode.InternalServerError)
      {
        if (_log.IsErrorEnabled)
        {
          object exceptionObj;

          context.Items.TryGetValue(NancyEngine.ERROR_EXCEPTION, out exceptionObj);

          var exception = exceptionObj as Exception;

          _log.Error("Unhandled exception.", exception);
        }
      }

      return false;
    }

    #endregion
  }
}
