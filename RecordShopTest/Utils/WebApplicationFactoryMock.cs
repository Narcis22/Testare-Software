using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using RecordShop.DAL;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RecordShop.DAL.Entities;

namespace RecordShopTest.Utils
{
    public class WebApplicationFactoryMock : WebApplicationFactory<Program>
    {
   
        public readonly AppDbContext _appDbContext = DbContextMock.createMock(); 

        public static AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DatabaseTest")
                .Options;
            var dbContext = new AppDbContext(options);

            dbContext.Database.EnsureDeleted();
            DbContextSeed(dbContext);

            return dbContext;
        }

        public WebApplicationFactory<Program> MockServices(params ServiceDescriptor[] mocks)
        {
            return WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    Array.ForEach(mocks, mock => services.Replace(mock));
                    services.Replace(new ServiceDescriptor(typeof(IPolicyEvaluator),
                        typeof(DisableAuthenticationPolicyEvaluator), ServiceLifetime.Singleton));
                });
            });
        }

        public class DisableAuthenticationPolicyEvaluator : IPolicyEvaluator
        {
            public async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
            {
                var authenticationTicket = new AuthenticationTicket(new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                            {
                                    new Claim(ClaimTypes.Email, "dummyEmail@gmail.com"),
                            },
                            "Basic")), new AuthenticationProperties(),
                    JwtBearerDefaults.AuthenticationScheme);
                return await Task.FromResult(AuthenticateResult.Success(authenticationTicket));
            }

            public async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
                AuthenticateResult authenticationResult, HttpContext context, object resource)
            {
                return await Task.FromResult(PolicyAuthorizationResult.Success());
            }
        }

        private static void DbContextSeed(AppDbContext appDbContext)
        {
            List<Role> roleList = new List<Role>
            {
                new Role() { Id = 1, Name = "Admin" },
                new Role() { Id = 2, Name = "User" }
            };

            List<User> userList = new List<User>
            {
                new User() { Id = 1, Email = "narcis@gmail.com"},
                new User() { Id = 2, Email = "narcis@user.com"}
            };

            List<UserRole> userRoleList = new List<UserRole>
            {
                new UserRole() { UserId = 1, RoleId = 1 },
                new UserRole() { UserId = 2, RoleId = 1 }
            };

            List<Song> songsList = new List<Song>
            {
                new Song() { Id = 1, Name = "Smooth Criminal", Price = 12, Duration = 2, Year = 2001, Remix = 0, Album = "Smooth Criminal", ArtistId = 1 },
                new Song() { Id = 2, Name = "Beat It", Price = 20, Duration = 3, Year = 1982, Remix = 0, Album = "Thriller", ArtistId = 1 },
                new Song() { Id = 3, Name = "Billie Jean", Price = 15, Duration = 2, Year = 1982, Remix = 0, Album = "Thriller", ArtistId = 1 },
                new Song() { Id = 4, Name = "Dark Side Of The Moon", Price = 25, Duration = 4, Year = 2019, Remix = 0, Album = "The Carter V", ArtistId = 2 },
                new Song() { Id = 5, Name = "Mona Lisa", Price = 10, Duration = 3, Year = 2019, Remix = 0, Album = "The Carter V", ArtistId = 2 }
            };

            List<Cart> cartList = new List<Cart>
            {
                new Cart() { Id = 1, Sent = 0, UserEmail= "narcis@gmail.com" },
                new Cart() { Id = 2, Sent = 1, UserEmail= "narcis@gmail.com" },
                new Cart() { Id = 3, Sent = 1, UserEmail= "narcis@gmail.com" },
                new Cart() { Id = 4, Sent = 1, UserEmail = "narcis@user.com" }

            };

            List<SongCart> songCartsList = new List<SongCart>
            {
                new SongCart() { Id = 1, SongId = 1, CartId = 1, NoCopies = 1},
                new SongCart() { Id = 2, SongId = 2, CartId = 1, NoCopies = 5},
                new SongCart() { Id = 3, SongId = 3, CartId = 2, NoCopies = 3},
                new SongCart() { Id = 4, SongId = 4, CartId = 2, NoCopies = 2},
                new SongCart() { Id = 5, SongId = 5, CartId = 3, NoCopies = 1},
                new SongCart() { Id = 6, SongId = 1, CartId = 3, NoCopies = 1},
                new SongCart() { Id = 7, SongId = 1, CartId = 4, NoCopies = 4}
            };

            List<Genre> genresList = new List<Genre>
            {
                new Genre() { Id = 1, Name = "Pop"},
                new Genre() { Id = 2, Name = "Hip hop"}
            };

            List<SongGenre> songGenresList = new List<SongGenre>
            {
                new SongGenre() { Id = 1, SongId = 1, GenreId = 1},
                new SongGenre() { Id = 2, SongId = 2, GenreId = 1},
                new SongGenre() { Id = 3, SongId = 3, GenreId = 1},
                new SongGenre() { Id = 4, SongId = 4, GenreId = 2},
                new SongGenre() { Id = 5, SongId = 5, GenreId = 2}
            };

            List<Artist> artistsList = new List<Artist>
            {
                new Artist() { Id = 1, FirstName = "Michael", LastName = "Jackson"},
                new Artist() { Id = 2, FirstName = "Lil", LastName = "Wayne"}
            };

            List<ArtistInfo> artistInfoList = new List<ArtistInfo>
            {
                new ArtistInfo() { Id = 1, Nationality = "American", BirthYear = 1958, DeathYear = 2009, ArtistId = 1 },
                new ArtistInfo() { Id = 2, Nationality = "American", BirthYear = 1982, DeathYear = 0, ArtistId = 2 }
            };

            roleList.ForEach(x => x.UserRoles = userRoleList.Where(y => y.RoleId == x.Id).ToList());
            userList.ForEach(x => x.UserRoles = userRoleList.Where(y => y.UserId == x.Id).ToList());
            userRoleList.ForEach(x => x.Role = roleList.FirstOrDefault(y => y.Id == x.RoleId));
            userRoleList.ForEach(x => x.User = userList.FirstOrDefault(y => y.Id == x.UserId));

            songsList.ForEach(x => x.Artist = artistsList.FirstOrDefault(y => y.Id == x.ArtistId));
            songsList.ForEach(x => x.SongCarts = songCartsList.Where(y => y.SongId == x.Id).ToList());
            songsList.ForEach(x => x.SongGenres = songGenresList.Where(y => y.SongId == x.Id).ToList());

            cartList.ForEach(x => x.SongCarts = songCartsList.Where(y => y.CartId == x.Id).ToList());

            songCartsList.ForEach(x => x.Song = songsList.FirstOrDefault(y => y.Id == x.SongId));
            songCartsList.ForEach(x => x.Cart = cartList.FirstOrDefault(y => y.Id == x.CartId));

            genresList.ForEach(x => x.SongGenres = songGenresList.Where(y => y.GenreId == x.Id).ToList());

            songGenresList.ForEach(x => x.Song = songsList.FirstOrDefault(y => y.Id == x.SongId));
            songGenresList.ForEach(x => x.Genre = genresList.FirstOrDefault(y => y.Id == x.GenreId));

            artistsList.ForEach(x => x.ArtistInfo = artistInfoList.FirstOrDefault(y => y.ArtistId == x.Id));
            artistsList.ForEach(x => x.Songs = songsList.Where(y => y.ArtistId == x.Id).ToList());

            artistInfoList.ForEach(x => x.Artist = artistsList.FirstOrDefault(y => y.Id == x.ArtistId));

            appDbContext.AddRange(roleList);
            appDbContext.AddRange(userList);
            appDbContext.AddRange(userRoleList);
            appDbContext.AddRange(songsList);
            appDbContext.AddRange(cartList);
            appDbContext.AddRange(songCartsList);
            appDbContext.AddRange(genresList);
            appDbContext.AddRange(songGenresList);
            appDbContext.AddRange(artistsList);
            appDbContext.AddRange(artistInfoList);

            appDbContext.SaveChanges();
        }
    }
}
