using ILSCREEN_UI;
using ILSCREEN_UI.Common;
using ILSCREEN_UI.Models;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5000");
builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
AppSetting.Configuration = builder.Configuration;
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToString())) throw new ArgumentException("Not found ASPNETCORE_ENVIRONMENT Variable");


// Add services to the container.
var accessRedis = $"{AppSetting.Configuration["RedisSSO:Host"]}:{AppSetting.Configuration["RedisSSO:Port"]},Password={AppSetting.Configuration["RedisSSO:Password"]},connectTimeout:{AppSetting.Configuration["RedisSSO:ConnectionTime"]}";
var connectRedis = ConnectionMultiplexer.Connect(accessRedis);
builder.Services.AddSingleton<IConnectionMultiplexer>(connectRedis);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoScope();
//builder.Services.AddMvc(x => x.Filters.Add(new SessionExpireAttribute(connectRedis)));
builder.Services.AddMvc();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

//app.MapStaticAssets();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();


namespace ILSCREEN_UI
{
    public static class AppSetting
    {
        public static IConfiguration? Configuration { get; set; }
        public static string AssemblyName { get; set; } = Assembly.GetExecutingAssembly().GetName().Name ?? "";
        public static string SystemMessage(string code) => Configuration?[$"SystemMessage:{code.ToUpper()}"] ?? "";
        public static List<MicroServicesModel> MicroServices() => Configuration?.GetSection("MicroServices")?.Get<List<MicroServicesModel>>()?.ToList() ?? new List<MicroServicesModel>();
        public static string MicroService(string? name) => Configuration?.GetSection("MicroServices")?.Get<List<MicroServicesModel>>()?.Find(x => (x.name ?? "").ToUpper().Equals(name?.ToUpper()))?.host ?? "";
    }

    public static class SetupServiceLifeTime
    {
        public static void AddAutoScope(this IServiceCollection services)
        {
            List<Type> allType = new List<Type>();
            List<string> nsRange = new List<string> { $"{AppSetting.AssemblyName}.Services" };
            nsRange.ForEach(n =>
            {
                List<Type> srvTyp = Assembly.GetExecutingAssembly().GetTypes()
                                        .Where(t => t.Namespace != null && t.Namespace.StartsWith(n))
                                        //.Where(t => t.Namespace != null && !t.Namespace.EndsWith(".Common"))
                                        .Where(t => t.Namespace != null && t.Name != "ApiService")
                                        .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic).ToList();

                allType.AddRange(srvTyp);
            });

            if (!allType.IsEmpty())
            {
                allType.ForEach(clss =>
                {
                    var intrf = clss.GetInterfaces().FirstOrDefault();
                    if (intrf != null)
                    {
                        services.AddScoped(intrf, clss);
                    }
                    else
                    {
                        services.AddScoped(clss);
                    }
                });
            }
        }
    }
}