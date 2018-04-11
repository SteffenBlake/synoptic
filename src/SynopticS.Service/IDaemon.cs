using System.Threading;

namespace SynopticS.Service
{
    public interface IDaemon
    {
        void Start(CancellationToken cancellationToken);
        void Stop();
    }
}