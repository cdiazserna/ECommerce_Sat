using ECommerce_Sat.DAL.Entities;
using ECommerce_Sat.Enum;
using ECommerce_Sat.Helpers;
using ECommerce_Sat.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Sat.DAL
{
	public class SeederDb
	{
		private readonly DataBaseContext _context;
		private readonly IUserHelper _userHelper;
        private readonly IAzureBlobHelper _azureBlobHelper;

        public SeederDb(DataBaseContext context, IUserHelper userHelper, IAzureBlobHelper azureBlobHelper)
		{
			_context = context;
			_userHelper = userHelper;
            _azureBlobHelper = azureBlobHelper;
        }

		public async Task SeedAsync()
		{
			await _context.Database.EnsureCreatedAsync();
			await PopulateCategoriesAsync();
			await PopulateCountriesStatesCitiesAsync();
			await PopulateRolesAsync();
            await PopulateUserAsync("Steve", "Jobs", "steve_jobs_admin@yopmail.com", "3002323232", "Street Apple", "102030", "SteveJobs.png", UserType.Admin);
            await PopulateUserAsync("Bill", "Gates", "bill_gates_user@yopmail.com", "4005656656", "Street Microsoft", "405060", "BillGates.png", UserType.User);
			await PopulateProductAsync();

            await _context.SaveChangesAsync();
		}

        private async Task PopulateCategoriesAsync()
		{
			if (!_context.Categories.Any())
			{
				_context.Categories.Add(new Category { Name = "Tecnología", Description = "Elementos tech", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Implementos de Aseo", Description = "Detergente, jabón, etc.", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Ropa interior", Description = "Tanguitas, narizonas", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Gamers", Description = "PS5, XBOX SERIES", CreatedDate = DateTime.Now });
				_context.Categories.Add(new Category { Name = "Mascotas", Description = "Concentrado, jabón para pulgas.", CreatedDate = DateTime.Now });
			}
		}

		private async Task PopulateCountriesStatesCitiesAsync()
		{
			if (!_context.Countries.Any())
			{
				_context.Countries.Add(
				new Country
				{
					Name = "Colombia",
					CreatedDate = DateTime.Now,
					States = new List<State>()
					{
						new State
						{
							Name = "Antioquia",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Medellín", CreatedDate= DateTime.Now },
								new City { Name = "Bello", CreatedDate= DateTime.Now },
								new City { Name = "Itagüí", CreatedDate= DateTime.Now },
								new City { Name = "Sabaneta", CreatedDate= DateTime.Now },
								new City { Name = "Envigado", CreatedDate= DateTime.Now },
							}
						},

						new State
						{
							Name = "Cundinamarca",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Bogotá", CreatedDate= DateTime.Now },
								new City { Name = "Fusagasugá", CreatedDate= DateTime.Now },
								new City { Name = "Funza", CreatedDate= DateTime.Now },
								new City { Name = "Sopó", CreatedDate= DateTime.Now },
								new City { Name = "Chía", CreatedDate= DateTime.Now },
							}
						},

						new State
						{
							Name = "Atlántico",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Barranquilla", CreatedDate= DateTime.Now },
								new City { Name = "La Chinita", CreatedDate= DateTime.Now },
							}
						},
					}
				});

				_context.Countries.Add(
				new Country
				{
					Name = "Argentina",
					CreatedDate = DateTime.Now,
					States = new List<State>()
					{
						new State
						{
							Name = "Buenos Aires",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Avellaneda", CreatedDate= DateTime.Now },
								new City { Name = "Ezeiza", CreatedDate= DateTime.Now },
								new City { Name = "La Boca", CreatedDate= DateTime.Now },
								new City { Name = "Río de la Plata", CreatedDate= DateTime.Now },
							}
						},

						new State
						{
							Name = "La Pampa",
							CreatedDate = DateTime.Now,
							Cities = new List<City>()
							{
								new City { Name = "Santa María", CreatedDate= DateTime.Now },
								new City { Name = "Obrero", CreatedDate= DateTime.Now },
								new City { Name = "Rosario", CreatedDate= DateTime.Now }
							}
						}
					}
				});
			}
		}


		private async Task PopulateRolesAsync()
		{
			await _userHelper.AddRoleAsync(UserType.Admin.ToString());
			await _userHelper.AddRoleAsync(UserType.User.ToString());
		}

		private async Task PopulateUserAsync(string firstName, string lastName, string email, string phone, string address, string document, string image, UserType userType)
		{
			User user = await _userHelper.GetUserAsync(email);
            if (user == null)
			{
                Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");

                user = new User
				{
					CreatedDate = DateTime.Now,
					FirstName = firstName,
					LastName = lastName,
					Email = email,
					UserName = email,
					PhoneNumber = phone,
					Address = address,
					Document = document,
					City = _context.Cities.FirstOrDefault(),
					UserType = userType,
					ImageId = imageId
				};

				await _userHelper.AddUserAsync(user, "123456");
				await _userHelper.AddUserToRoleAsync(user, userType.ToString());
			}
		}


        private async Task PopulateProductAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Medias Grises", "Gray socks", 270000M, 12F, new List<string>() { "Ropa Interior", "Calzado" }, new List<string>() { "Medias1.png" });
                await AddProductAsync("Medias Negras", "Black socks", 300000M, 12F, new List<string>() { "Ropa Interior", "Calzado" }, new List<string>() { "Medias2.png" });
                await AddProductAsync("TV Samsung OLED", "Wonderful TV", 5000000M, 12F, new List<string>() { "Tecnología", "Gamers" }, new List<string>() { "TvOled.png", "TvOled2.png" });
            }
        }

        private async Task AddProductAsync(string name, string description, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product product = new()
            {
                Description = description,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (string? category in categories)
            {
                product.ProductCategories.Add(new ProductCategory { Category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category) });
            }


            foreach (string? image in images)
            {
                Guid imageId = await _azureBlobHelper.UploadAzureBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\products\\{image}", "products");
                product.ProductImages.Add(new ProductImage { ImageId = imageId });
            }

            _context.Products.Add(product);
        }
    }
}
