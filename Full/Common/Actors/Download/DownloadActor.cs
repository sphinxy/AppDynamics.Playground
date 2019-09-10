using Akka.Actor;
using Common.Actors.Download.Commands;

namespace Common.Actors.Download
{
    public sealed class DownloadActor : ReceiveActor
    {
        private readonly IAsyncBackgroundCaller _asyncBackgroundCaller;

        public DownloadActor(IAsyncBackgroundCaller asyncBackgroundCaller)
        {
            _asyncBackgroundCaller = asyncBackgroundCaller;
            Receive<DownloadMessage>(d =>
            {
                var sender = Sender;
                _asyncBackgroundCaller.Call(d.BaseAddress, d.RelativePath).PipeTo(sender, Self, _ => _.Length > 0 ? d.BaseAddress + " ok" : d.BaseAddress + " fail", ex => null );
            });
        }
    }
}
