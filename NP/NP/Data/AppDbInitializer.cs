using System;
using NP.Models;
using NP.Data;
using static System.Reflection.Metadata.BlobBuilder;

namespace NP.Data
{
	public class AppDbInitializer
	{
		public static void Seed(IApplicationBuilder applicationBuilder)
		{
			using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<NPDbContext>();
				context.Database.EnsureCreated();

                var myUser = new User()
                {
                    Id = "0001",
                    UserName = "user0001",
                    FirstName = "Template",
                    LastName = "User",
                    Email = "fakemail@fakemail.com",
                    Password = "00Tu00Tu00",
                    IsAdmin = false,
                    isLocation = false
                };
                var usrId = myUser.Id;

                //Users
                if (!context.Users.Any())
				{

                    context.Users.AddRange(new List<User>(){
						myUser
											
                });
                    context.SaveChanges();

                }

                if (!context.Locations.Any())
                {
					context.Locations.AddRange(new List<Location>()
					{
						new Location()
						{
							Id = "0000-0001",
							Email = "location@fakemail.com",
							Latitude = null,
							Longtitude = null,
							Name = "Template Location",
							Description = "Lorem Ipsum...",
							Img = null,
							isVerified = true
						}
				});
                    context.SaveChanges();
                }

				
				//Books
                if (!context.Books.Any())
				{
                    context.Books.AddRange(new List<Book>()
                    {
                        new Book()
                        {
                            BookId = "111dfhjikk",
                            Title = "1984",
                            Description = "Lorem Ipsum...",
                            Author = "George Orwell",
                            Published = new DateTime(2019, 05, 09, 9, 15, 0),
                            Genre = Genre.ScienceFiction,
                            Difficulty = Difficulty.Medium,
                            Img = null,
                            PostedDate = new DateTime(2019, 05, 09, 9, 15, 0),
                            CurrentlyHeldById = usrId,
                            PostedById = usrId
                    }
                });
                    context.SaveChanges();
                }
            }
		}
	}
}

