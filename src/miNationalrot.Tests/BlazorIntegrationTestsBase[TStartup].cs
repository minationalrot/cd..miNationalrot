// --------------------------------------------------------------------------------------------------------------------
// <copyright company="o.s.i.s.a. GmbH" file="BlazorIntegrationTests[TStartup].cs">
//    (c) 2014. See licence text in binary folder.
// </copyright>
//  --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace miNationalrot.Tests
{
    public abstract class BlazorIntegrationTestsBase<TStartup>
    {
        #region Fields

        private HttpClient _client;

        private TestServer _server;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the test context.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public TestContext TestContext { get; set; }

        #endregion

        #region Properties

        protected HttpClient Client
        {
            get
            {
                if (_client != null)
                    return _client;

                Initialize();

                return _client;
            }
        }

        protected TestServer Server
        {
            get
            {
                if (_server != null)
                    return _server;

                Initialize();

                return _server;
            }
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            var hostBuilder = WebHost.CreateDefaultBuilder()
                .UseConfiguration(
                    new ConfigurationBuilder()
                        .Build())
                .UseStartup<StartupClientSide>()
                .UseEnvironment("Development");

            _server = new TestServer(hostBuilder);
            _client = _server.CreateClient();
        }

        #endregion

        public class StartupClientSide
        {
            #region Public Methods and Operators

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseResponseCompression();

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseBlazorDebugging();
                }

                app.UseStaticFiles();
                app.UseClientSideBlazorFiles<TStartup>();

                app.UseRouting();

                app.UseEndpoints(
                    endpoints =>
                    {
                        endpoints.MapDefaultControllerRoute();
                        endpoints.MapFallbackToClientSideBlazor<TStartup>("index.html");
                    });
            }

            // This method gets called by the runtime. Use this method to add services to the container.
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc().AddNewtonsoftJson();
                services.AddResponseCompression(opts => { opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" }); });
            }

            #endregion
        }
    }
}