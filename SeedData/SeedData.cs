using Estacionei.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

public class SeedData
{


    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var roleName = "Admin";
        var userName = "admin@default.com";
        var password = "P@ssw0rd123!";

        // Cria a role se não existir
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Cria o usuário se não existir
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            user = new ApplicationUser { UserName = userName, Email = userName };
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Adiciona a role ao usuário
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
