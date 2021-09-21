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
            var physicalFileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));//�ļ�������·��
            //��̬�ļ����ļ�·��������·��
            app.UseStaticFiles(new StaticFileOptions
            {
                //���ó���Ĭ�ϵ�wwwroot�ļ��еľ�̬�ļ�������ļ����ṩ Web ��Ŀ¼����ļ� ,    
                //�����������Ժ󣬾Ϳ��Է��ʷ�wwwroot�ļ��µ��ļ�    
                FileProvider = physicalFileProvider,
                RequestPath = "/documents",
                DefaultContentType = "application/x-msdownload",//����Ĭ��MIMEType
                ServeUnknownFileTypes = true,
                ContentTypeProvider = contentTypeProvider,
                //OnPrepareResponse = ctx =>
                //{
                //    ctx.Context.Response.Headers.Append("Cache-Control", $"max-age=600");
                //},
            });

            //Ŀ¼������ļ�·��������·��
            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = physicalFileProvider,
                RequestPath = "/uploads",
            });


            //app.UseFileServer(true);

            //�ļ������� ���ļ�·��������·��
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
