using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Luffy.RetryMechanism
{
    public abstract class RetryMechanismBase
    {
        public Task<bool> IsTransient(Exception ex)
        {
            bool isTransient = false;
            var webException = ex as WebException;

            if(webException != null)
            {
                isTransient = new[] {WebExceptionStatus.ConnectionClosed,
                              WebExceptionStatus.Timeout,
                              WebExceptionStatus.RequestCanceled,
                              WebExceptionStatus.KeepAliveFailure,
                              WebExceptionStatus.PipelineFailure,
                              WebExceptionStatus.ReceiveFailure,
                              WebExceptionStatus.ConnectFailure,
                              WebExceptionStatus.SendFailure}
                              .Contains(webException.Status);

                return Task.FromResult(isTransient);
            }

            return Task.FromResult(isTransient);
        }
    }
}