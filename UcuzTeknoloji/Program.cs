using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UcuzTeknoloji.Data;
using Microsoft.AspNetCore.Identity;

namespace UcuzTeknoloji
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddScoped(typeof(UcuzTeknoloji.Repositories.IRepository<>), typeof(UcuzTeknoloji.Repositories.Repository<>));
            builder.Services.AddSession();
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // E-Posta zorunluluðunu ve abartý þifre kurallarýný çöpe atýyoruz!
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false; // E-posta onayýný KAPAT!

                options.Password.RequireDigit = false; // Sayý zorunluluðunu KAPAT!
                options.Password.RequireLowercase = false; // Küçük harf zorunluluðunu KAPAT!
                options.Password.RequireNonAlphanumeric = false; // Özel karakter (*,!) zorunluluðunu KAPAT!
                options.Password.RequireUppercase = false; // Büyük harf zorunluluðunu KAPAT!
                options.Password.RequiredLength = 4; // Þifre en az 4 hane olsun yeter!
            })
            .AddEntityFrameworkStores<AppDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();
            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            // --- ADMÝN HESABINI OTOMATÝK OLUÞTURMA SÝSTEMÝ ---
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Microsoft.AspNetCore.Identity.IdentityUser>>();

                // (Burada senin o önceki Admin ekleme kodlarýn duruyor olacak)
                if (userManager.FindByNameAsync("SistemAdmin").Result == null)
                {
                    var adminUser = new Microsoft.AspNetCore.Identity.IdentityUser { UserName = "SistemAdmin", Email = "admin@ucuzteknoloji.com" };
                    userManager.CreateAsync(adminUser, "Proje.2026").Wait();
                }

                // YENÝ EKLENECEK KISIM BURASI: Kategoriler boþsa 1, 2 ve 3 numaralý kategorileri yarat!
                var dbContext = scope.ServiceProvider.GetRequiredService<UcuzTeknoloji.Data.AppDbContext>();
                if (!dbContext.Categories.Any())
                {
                    dbContext.Categories.Add(new UcuzTeknoloji.Models.Category { Name = "Hazýr Sistem / Kasa" });
                    dbContext.Categories.Add(new UcuzTeknoloji.Models.Category { Name = "Bilgisayar Parçasý" });
                    dbContext.Categories.Add(new UcuzTeknoloji.Models.Category { Name = "Oyuncu Ekipmaný" });
                    dbContext.SaveChanges();
                }
            }

            app.Run();
        }
    }
}
