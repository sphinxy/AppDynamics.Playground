using System;
using System.Threading.Tasks;
using Akka.Actor;
using Microsoft.AspNetCore.Mvc;
using Common;
using Common.Actors;
using Common.Actors.Download;
using Common.Actors.Download.Commands;

namespace ServiceAkkaFull.Controllers
{
    [Route(Addresses.ApiRoute)]
    public class HomeController : Controller
    {
        private readonly IAsyncBackgroundCaller _asyncBackgroundCaller;
        private readonly ActorSystem _actorSystem;

        public HomeController(IAsyncBackgroundCaller asyncBackgroundCaller, ActorSystem actorSystem)
        {
            _asyncBackgroundCaller = asyncBackgroundCaller;
            _actorSystem = actorSystem;
        }
        [HttpGet]
        [Route(Addresses.Self)]
        public async Task<string> Self()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            return "ServiceAkkaFull";
        }

        [HttpGet]
        [Route(Addresses.GoogleGmailWithoutAkka)]
        public async Task<string> GoogleGmailWithoutAkka()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100));

            var responseGoogle = await _asyncBackgroundCaller.Call(Addresses.Google, $"/");
            var responseGmail = await _asyncBackgroundCaller.Call(Addresses.Gmail, $"/");


            return $"Without akka: {responseGoogle}, {responseGmail}";
        }

        [HttpGet]
        [Route(Addresses.MicrosoftBingUmbrellaWithAkka)]
        public async Task<string> MicrosoftBingUmbrellaWithAkka()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            var downloadActorRef = _actorSystem.ActorOf(Props.Create(() => new DownloadActor(_asyncBackgroundCaller)), Guid.NewGuid().ToString());
            
            var responseMicrosoft = await downloadActorRef.Ask<string>(new DownloadMessage(Addresses.Microsoft, "/"));
            var responseBing = await downloadActorRef.Ask<string>(new DownloadMessage(Addresses.Bing, "/"));
            _actorSystem.Stop(downloadActorRef);

            return $"With akka: {responseMicrosoft}, {responseBing}";
        }
    }
}