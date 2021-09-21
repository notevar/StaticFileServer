### .netcore 文件服务器demo

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