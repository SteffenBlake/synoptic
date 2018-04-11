using System;
using System.Threading;

namespace SynopticS.Service
{
    public interface IPollingPreempter
    {
        void Listen(CancellationToken cancellationToken);
        void Stop();
        event EventHandler ShouldPreempt;
        void OnError(Exception exception, CancellationToken cancellationToken);
    }
}