using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PixelParbaj_CORE.Data;
using PixelParbaj_CORE.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddDbContextPool<PP2Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("PPConnectionString")));
builder.Services.AddScoped<IPP, MovieSql>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseDefaultFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();
/**app.UseStaticFiles(new StaticFileOptions
{
    //FileProvider = new PhysicalFileProvider(ContentRootPath),
    RequestPath = "/"
});
**/

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
