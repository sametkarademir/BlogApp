using AutoMapper;
using Business.Profiles;
using System.Text.Json.Serialization;
using NToastNotify;
using Data.Contexts;
using Data.Repositories.Concrete;
using Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Shared.Utilities.Extenstions.Concrete;
using Shared.Utilities.Extenstions.Interface;
using Business.Services.Concrete;
using Business.Services.Interface;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using WebUI.Helpers.Concrete;
using WebUI.Helpers.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{

    cfg.AddProfile(new CollectionProfile());

}).CreateMapper());

builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
}).AddRazorRuntimeCompilation().AddNToastNotifyToastr(new ToastrOptions
{
    ProgressBar = false,
    PositionClass = ToastPositions.TopCenter
});
builder.Services.AddSession();

builder.Services.AddDbContext<BloggerAppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));
builder.Services.AddTransient<IUow, Uow>();
builder.Services.AddTransient<IDateTimeExtensions, DateTimeExtensions>();
builder.Services.AddTransient<IGenerateBase64SessionIdExtenstion, GenerateBase64SessionIdExtenstion>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IImageHelper, ImageHelper>();
builder.Services.AddTransient<IHttpContextHelper, HttpContextHelper>();

builder.Services.AddTransient<IWebInfoService, WebInfoManager>();
builder.Services.AddTransient<ISystemLogService, SystemLogManager>();
builder.Services.AddTransient<IFolderService, FolderManager>();
builder.Services.AddTransient<IResumeService, ResumeManager>();
builder.Services.AddTransient<IProjectService, ProjectManager>();
builder.Services.AddTransient<ICategoryService, CategoryManager>();
builder.Services.AddTransient<IArticleService, ArticleManager>();




builder.Services.AddIdentity<User, Role>(opt =>
{
    //User Password Options
    opt.Password.RequireDigit = true;
    opt.Password.RequiredLength = 8;
    opt.Password.RequiredUniqueChars = 1;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    //User Username and Email Options
    opt.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    opt.User.RequireUniqueEmail = false;

    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.AllowedForNewUsers = true;

}).AddEntityFrameworkStores<BloggerAppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<SecurityStampValidatorOptions>(opt =>
{
    opt.ValidationInterval = TimeSpan.FromMinutes(30);
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = new PathString("/Login/Index");
    opt.LogoutPath = new PathString("/Login/Logout");
    opt.Cookie = new CookieBuilder
    {
        Name = "Blog",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.Always

    };
    opt.SlidingExpiration = true;
    opt.ExpireTimeSpan = TimeSpan.FromDays(7);
    opt.AccessDeniedPath = new PathString("/Login/AccessDenied");

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseNToastNotify();
app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Blog",
        areaName: "Blog",
        pattern: "Blog/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapDefaultControllerRoute();
});

app.Run();
