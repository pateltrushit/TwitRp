using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitRp.Core.Interfaces;
using TwitRp.Core.Interfaces.Repository;
using TwitRp.Core.Models.Logging;
using TwitRp.Core.Models.Twitter;
using TwitRp.Data.Repositories;
using TwitRp.Services.Processors.Emoji;
using TwitRp.Services.Processors.Hashtags;
using TwitRp.Services.Processors.SampleStream;

namespace TwitRp.SampledStreamApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "TwitRp Api For Twitter", Version = "V1" });
            });



            services.AddCors();

            //services.AddSingleton<IProcessor,SampleStreamProcessor>();
            services.AddSingleton<IStreamProcessor,SampleStreamProcessor>();
            services.AddSingleton<IEmojiProcessor,EmojiProcessor>();
            services.AddSingleton<IHashtagProcessor, HashtagProcessor>();
            services.AddSingleton<ICacheRepository, CacheRepository>();
            services.AddSingleton<ITweetRepository, TweetRepository>();
            services.AddLogging();
            services.Configure<TwitterCredentialsOptions>(Configuration.GetSection(TwitterCredentialsOptions.TwitterCredentials));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TwitRp Api For Twitter");
            });
        }
    }
}
