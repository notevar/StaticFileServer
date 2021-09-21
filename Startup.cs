using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace StaticFileServer
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
            services.AddControllersWithViews();
            //services.AddDirectoryBrowser();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings.Add(".img", "image/jpg");
            var physicalFileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));//文件的物理路径
            //静态文件的文件路径和请求路径
            app.UseStaticFiles(new StaticFileOptions
            {
                //配置除了默认的wwwroot文件中的静态文件以外的文件夹提供 Web 根目录外的文件 ,    
                //经过此配置以后，就可以访问非wwwroot文件下的文件    
                FileProvider = physicalFileProvider,
                RequestPath = "/documents",
                DefaultContentType = "application/x-msdownload",//设置默认MIMEType
                ServeUnknownFileTypes = true,
                ContentTypeProvider = contentTypeProvider,
                //OnPrepareResponse = ctx =>
                //{
                //    ctx.Context.Response.Headers.Append("Cache-Control", $"max-age=600");
                //},
            });

            //目录浏览的文件路径和请求路径
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = physicalFileProvider,
                RequestPath = "/uploads",
            });


            //app.UseFileServer(true);

            //文件服务器 的文件路径和请求路径
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = physicalFileProvider,
                RequestPath = "/uploads",
                EnableDirectoryBrowsing = true
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
