using ConcertBooking.Models;
using Microsoft.AspNetCore.Identity;

namespace ConcertBooking.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            
            if (!context.Concerts.Any())
            {
                context.Concerts.AddRange(
                    new Concert { ArtistName = "Dawid Podsiadło", Description = "Trasa stadionowa, największe hity.", EventDate = DateTime.Now.AddDays(30), TicketPrice = 199.00m, AvailableSeats = 50000 },
                    new Concert { ArtistName = "Sanah", Description = "Koncert akustyczny w kameralnym gronie.", EventDate = DateTime.Now.AddDays(15), TicketPrice = 249.50m, AvailableSeats = 1500 },
                    new Concert { ArtistName = "Taco Hemingway", Description = "Promocja nowego albumu.", EventDate = DateTime.Now.AddDays(45), TicketPrice = 149.00m, AvailableSeats = 15000 }
                );
                await context.SaveChangesAsync();
            }
            

            
            string[] roleNames = { "Admin", "User" };
            
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            
            var adminEmail = "admin@concerts.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Główny",
                    LastName = "Administrator",
                    EmailConfirmed = true 
                };

                
                var result = await userManager.CreateAsync(newAdmin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}