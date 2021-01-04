using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using TwitRp.Core.Interfaces;
using TwitRp.Core.Interfaces.Repository;
using TwitRp.Core.Models.Logging;
using TwitRp.Data.Repositories;
using TwitRp.Services.Processors.Emoji;
using TwitRp.Services.Processors.Hashtags;
using TwitRp.Services.Processors.SampleStream;

namespace TwitRp.ConsoleApp
{
    class Program
    {

        static IConfiguration Configuration { get; set; }
        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            Process(host.Services);
            host.RunAsync();
        }

        static void Process(IServiceProvider services)
        {
            var emojiProcessor = new EmojiProcessor(Configuration, services.GetService<ILogger<EmojiProcessor>>());
            var tweetRepository = services.GetService<ITweetRepository>();
            var hashtagProcessor = new HashtagProcessor(Configuration, services.GetService<ILogger<HashtagProcessor>>());
            var processor = new SampleStreamProcessor(Configuration, tweetRepository, services.GetService<ICacheRepository>(), services.GetService<ILogger<SampleStreamProcessor>>(), emojiProcessor, hashtagProcessor);
            processor.StartStreamAsync();
            Console.WriteLine();
        }
        static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((builder, services) =>
                services.AddSingleton<IConfiguration>(Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build())
                .AddSingleton<IStreamProcessor, SampleStreamProcessor>()
                .AddSingleton<IEmojiProcessor, EmojiProcessor>()
                .AddSingleton<IHashtagProcessor, HashtagProcessor>()
                .AddSingleton<ICacheRepository, CacheRepository>()
                .AddSingleton<ITweetRepository, TweetRepository>()
                .AddLogging()
            ).ConfigureLogging(logging =>
                logging.AddConfiguration(Configuration.GetSection("Logging"))
                .AddConsole()
                .AddSerilog(logger: new LoggerConfiguration().WriteTo.RollingFile(
                    Configuration.GetSection("SeriLog").Get<SeriLogOptions>().LogFilePath, 
                    retainedFileCountLimit: Configuration.GetSection("SeriLog").Get<SeriLogOptions>().RetainedFileCountLimit).CreateLogger(), 
                    dispose: Configuration.GetSection("SeriLog").Get<SeriLogOptions>().Dispose)
            );
    }
}
