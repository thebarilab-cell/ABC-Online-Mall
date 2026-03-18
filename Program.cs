using ABCRMALLWebsite.Data;
using ABCRMALLWebsite.Models;
using Microsoft.EntityFrameworkCore;
namespace ABCRMALL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ? SESSION SUPPORT ADD KAREIN
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // ? SESSION USE KAREIN
            app.UseSession();

            // Database creation and sample data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (!context.Admins.Any())
                {
                    context.Admins.Add(new Admin
                    {
                        Username = "admin",
                        Email = "admin@abcmall.com",
                        Password = "admin123"
                    });
                    context.SaveChanges();
                }
            }

            // Add routing for BookingController
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "booking",
                pattern: "Booking/{action}/{id?}",
                defaults: new { controller = "Booking" });


            //var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.EnsureCreated();

                    if (!context.Shops.Any())
                    {
                        // Sample Shops
                        context.Shops.AddRange(
                            new Shop
                            {
                                Name = "Nike",
                                Category = "Sports",
                                Description = "Premium sports apparel and footwear",
                                Floor = "Ground Floor",
                                ItemsList = "Running Shoes, Sports T-shirts, Track Pants, Accessories"
                            },
                            new Shop
                            {
                                Name = "Apple Store",
                                Category = "Electronics",
                                Description = "Official Apple products and accessories",
                                Floor = "First Floor",
                                ItemsList = "iPhone, iPad, MacBook, Apple Watch, AirPods"
                            }
                        );

                        // Sample Movies
                        context.Movies.AddRange(
                            new Movie
                            {
                                Title = "Avatar: The Way of Water",
                                Genre = "Sci-Fi",
                                Duration = "3h 12m",
                                ShowTimings = "10:00 AM, 2:00 PM, 6:00 PM, 10:00 PM",
                                TotalSeats = 200,
                                AvailableSeats = 150,
                                TicketPrice = 350
                            }
                        );

                        // Sample Food Courts
                        context.FoodCourts.AddRange(
                            new FoodCourt
                            {
                                CounterName = "McDonald's",
                                CuisineType = "Fast Food",
                                MenuItems = "Burgers, Fries, Soft Drinks, Wraps",
                                PriceRange = "$100 - $300",
                                Floor = "Third Floor"
                            }
                        );

                        // Add Admin User
                        context.Admins.Add(new Admin
                        {
                            Username = "admin",
                            Email = "admin@rmall.com",
                            Password = "admin123"
                        });

                        // Add Contact Information
                        context.Contacts.Add(new Contact
                        {
                            MallName = "R Mall",
                            Address = "Defence, Karachi, Pakistan 400001",
                            Phone = "+92-22-12345678",
                            Email = "info@rmall.com",
                            OperatingHours = "10:00 AM - 10:00 PM (All Days)",
                            GoogleMapLink = "https://maps.google.com/?q=R+Mall+Mumbai"
                        });

                        context.SaveChanges();
                        Console.WriteLine("Sample data added successfully!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding sample data: {ex.Message}");
                }
            }


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

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
