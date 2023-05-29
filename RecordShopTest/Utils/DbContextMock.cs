using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecordShop.DAL;
using RecordShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordShopTest.Utils
{
    internal static class DbContextMock
    {
        internal static AppDbContext createMock()
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

            var roles = roleList.AsQueryable();
            var roleSet = new Mock<DbSet<Role>>();
            roleSet.As<IQueryable<Role>>().Setup(m => m.Provider).Returns(roles.Provider);
            roleSet.As<IQueryable<Role>>().Setup(m => m.Expression).Returns(roles.Expression);
            roleSet.As<IQueryable<Role>>().Setup(m => m.ElementType).Returns(roles.ElementType);
            roleSet.As<IQueryable<Role>>().Setup(m => m.GetEnumerator()).Returns(roles.GetEnumerator());

            var users = userList.AsQueryable();
            var userSet = new Mock<DbSet<User>>();
            userSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            userSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            userSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            userSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var userRoles = userRoleList.AsQueryable();
            var userRoleSet = new Mock<DbSet<UserRole>>();
            userRoleSet.As<IQueryable<UserRole>>().Setup(m => m.Provider).Returns(userRoles.Provider);
            userRoleSet.As<IQueryable<UserRole>>().Setup(m => m.Expression).Returns(userRoles.Expression);
            userRoleSet.As<IQueryable<UserRole>>().Setup(m => m.ElementType).Returns(userRoles.ElementType);
            userRoleSet.As<IQueryable<UserRole>>().Setup(m => m.GetEnumerator()).Returns(userRoles.GetEnumerator());

            var songs = songsList.AsQueryable();
            var songSet = new Mock<DbSet<Song>>();
            songSet.As<IQueryable<Song>>().Setup(m => m.Provider).Returns(songs.Provider);
            songSet.As<IQueryable<Song>>().Setup(m => m.Expression).Returns(songs.Expression);
            songSet.As<IQueryable<Song>>().Setup(m => m.ElementType).Returns(songs.ElementType);
            songSet.As<IQueryable<Song>>().Setup(m => m.GetEnumerator()).Returns(songs.GetEnumerator());

            var carts = cartList.AsQueryable();
            var cartSet = new Mock<DbSet<Cart>>();
            cartSet.As<IQueryable<Cart>>().Setup(m => m.Provider).Returns(carts.Provider);
            cartSet.As<IQueryable<Cart>>().Setup(m => m.Expression).Returns(carts.Expression);
            cartSet.As<IQueryable<Cart>>().Setup(m => m.ElementType).Returns(carts.ElementType);
            cartSet.As<IQueryable<Cart>>().Setup(m => m.GetEnumerator()).Returns(carts.GetEnumerator());

            var songCarts = songCartsList.AsQueryable();
            var songCartSet = new Mock<DbSet<SongCart>>();
            songCartSet.As<IQueryable<SongCart>>().Setup(m => m.Provider).Returns(songCarts.Provider);
            songCartSet.As<IQueryable<SongCart>>().Setup(m => m.Expression).Returns(songCarts.Expression);
            songCartSet.As<IQueryable<SongCart>>().Setup(m => m.ElementType).Returns(songCarts.ElementType);
            songCartSet.As<IQueryable<SongCart>>().Setup(m => m.GetEnumerator()).Returns(songCarts.GetEnumerator());

            var genres = genresList.AsQueryable();
            var genreSet = new Mock<DbSet<Genre>>();
            genreSet.As<IQueryable<Genre>>().Setup(m => m.Provider).Returns(genres.Provider);
            genreSet.As<IQueryable<Genre>>().Setup(m => m.Expression).Returns(genres.Expression);
            genreSet.As<IQueryable<Genre>>().Setup(m => m.ElementType).Returns(genres.ElementType);
            genreSet.As<IQueryable<Genre>>().Setup(m => m.GetEnumerator()).Returns(genres.GetEnumerator());

            var songGenres = songGenresList.AsQueryable();
            var songGenreSet = new Mock<DbSet<SongGenre>>();
            songGenreSet.As<IQueryable<SongGenre>>().Setup(m => m.Provider).Returns(songGenres.Provider);
            songGenreSet.As<IQueryable<SongGenre>>().Setup(m => m.Expression).Returns(songGenres.Expression);
            songGenreSet.As<IQueryable<SongGenre>>().Setup(m => m.ElementType).Returns(songGenres.ElementType);
            songGenreSet.As<IQueryable<SongGenre>>().Setup(m => m.GetEnumerator()).Returns(songGenres.GetEnumerator());

            var artists = artistsList.AsQueryable();
            var artistSet = new Mock<DbSet<Artist>>();
            artistSet.As<IQueryable<Artist>>().Setup(m => m.Provider).Returns(artists.Provider);
            artistSet.As<IQueryable<Artist>>().Setup(m => m.Expression).Returns(artists.Expression);
            artistSet.As<IQueryable<Artist>>().Setup(m => m.ElementType).Returns(artists.ElementType);
            artistSet.As<IQueryable<Artist>>().Setup(m => m.GetEnumerator()).Returns(artists.GetEnumerator());

            var artistInfo = artistInfoList.AsQueryable();
            var artistInfoSet = new Mock<DbSet<ArtistInfo>>();
            artistInfoSet.As<IQueryable<ArtistInfo>>().Setup(m => m.Provider).Returns(artistInfo.Provider);
            artistInfoSet.As<IQueryable<ArtistInfo>>().Setup(m => m.Expression).Returns(artistInfo.Expression);
            artistInfoSet.As<IQueryable<ArtistInfo>>().Setup(m => m.ElementType).Returns(artistInfo.ElementType);
            artistInfoSet.As<IQueryable<ArtistInfo>>().Setup(m => m.GetEnumerator()).Returns(artistInfo.GetEnumerator());

            var context = new Mock<AppDbContext>();
            context.Setup(x => x.Songs).Returns(songSet.Object); 
            context.Setup(x => x.Artists).Returns(artistSet.Object);
            context.Setup(x => x.ArtistInfos).Returns(artistInfoSet.Object);
            context.Setup(x => x.Carts).Returns(cartSet.Object);
            context.Setup(x => x.SongCarts).Returns(songCartSet.Object);
            context.Setup(x => x.Roles).Returns(roleSet.Object);
            context.Setup(x => x.Users).Returns(userSet.Object);
            context.Setup(x => x.UserRoles).Returns(userRoleSet.Object);
            context.Setup(x => x.Genres).Returns(genreSet.Object);
            context.Setup(x => x.SongGenres).Returns(songGenreSet.Object);

            return context.Object;
        }
    }
}
