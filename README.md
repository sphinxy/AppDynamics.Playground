We are checking the possibility of usage AppDynamics on .Net core. Also, we are interested in handling Akka.Net calls. 


This repo is created as an example of the difference in handling outgoing HTTP request in two different setups:
1) Core use .netcore2.2 and NuGet package for agent, https://docs.appdynamics.com/display/PRO45/Install+the+.NET+Core+Microservices+Agent+for+Windows
2) Full use full .net4.7 and rely on external windows appdynamics agent, https://docs.appdynamics.com/display/PRO45/Configure+the+.NET+Agent+for+Windows+Services+and+Standalone+Applications

So each service has two endpoints:

  * /api/MicrosoftBingUmbrellaWithAkka use akka.net actor with http client to call microsoft/bing
  * /api/GoogleGmailWithoutAkka just call google/gmail via http client
  * /api/self is to test self-service call, we notice that sometimes it appears on appdynamics dashboards, sometimes no

It's expected that on appdynamics dashboard we will have four http backends: google, gmail, microsoft and bing.

Example of expected dashboard (it works fine for Full):

- <img src="https://raw.githubusercontent.com/sphinxy/AppDynamics.Playground/master/AppDynamicsDashboardExamples\appdyn_full_akka_works.png?sanitize=true&raw=true" />

Example of .net core that doesn't catch Akka.NET calls:

- <img src="raw.githubusercontent.com/sphinxy/AppDynamics.Playground/master/AppDynamicsDashboardExamples\appdyn_core_akka_doesnt_works.png?sanitize=true&raw=true" />

All together:

- <img src="raw.githubusercontent.com/sphinxy/AppDynamics.Playground/master/AppDynamicsDashboardExamples\appdyn_full_and_core_not_same.png?sanitize=true&raw=true" />


