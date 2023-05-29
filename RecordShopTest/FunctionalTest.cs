using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using RecordShop.BLL.Interfaces;
using RecordShop.BLL.Managers;
using RecordShop.Controllers;
using RecordShop.DAL.Interfaces;
using RecordShop.DAL.Models;
using RecordShop.DAL.Repositories;
using RecordShopTest.Maping;
using RecordShopTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RecordShopTest
{
    public class FunctionalTest
    {
        private readonly Mock<ISongCartManager> _songCartManager = new Mock<ISongCartManager>();

        public FunctionalTest() { }

        [Theory]
        [ClassData(typeof(GetSongCartForUserEquivalenceClass.ValidCases))]
        [ClassData(typeof(GetSongCartForUserPartitioningCategories.ValidCases))]
        public async Task GetSongCartForUserValidTest(string email, int sent)
        {
            // ARRANGE
            var dbContextMock = WebApplicationFactoryMock.CreateDbContext();

            var songCartRepositoryMock = new SongCartRepository(dbContextMock);
            var songCartManagerMock = new SongCartManager(songCartRepositoryMock);

            // ACT
            var result = songCartManagerMock.GetSongCartForUser(email, sent);

            // ASSERT
            Assert.NotNull(result);
        }

        [Theory]
        [ClassData(typeof(GetSongCartForUserEquivalenceClass.InvalidCases))]
        [ClassData(typeof(GetSongCartForUserPartitioningCategories.InvalidCases))]
        public async Task GetSongCartForUserInvalidTest(string email, int sent)
        {
            // ARRANGE
            var dbContextMock = WebApplicationFactoryMock.CreateDbContext();

            var songCaryRepositoryMock = new SongCartRepository(dbContextMock);
            var songCartManagerMock = new SongCartManager(songCaryRepositoryMock);

            // ACT
            var result = songCartManagerMock.GetSongCartForUser(email, sent);

            // ASSERT
            Assert.Null(result);
        }

        // Testare fara partitionarea categoriilor si clase de echivalenta
        //[Fact]
        //public async Task GetSongCartForUserTest()
        //{
        //    // ARRANGE
        //    const string email = "narcis@gmail.com";

        //    SongCartModel expectedResult = new SongCartModel()
        //    {
        //        SongId = 1,
        //        CartId = 1,
        //        Name = "Smooth Criminal",
        //        Price = 12,
        //        NoCopiesInCart = 1,
        //        PriceOfNoCopies = 12
        //    };

        //    _songCartManager
        //        .Setup(x => x.GetSongCartForUser(email, 0))
        //        .Returns(new List<SongCartModel> { expectedResult });

        //    // ACT
        //    var result = _songCartManager.Object.GetSongCartForUser(email, 0);

        //    // ASSERT
        //    Assert.NotNull(result);
        //    Assert.Single(result);
        //    Assert.Equal(expectedResult, result.FirstOrDefault());
        //}

        //[Fact]
        //public async Task GetSongCartForUserNotFoundTest()
        //{
        //    // ARRANGE
        //    const string email = "narcis_dummy@gmail.com";

        //    List<SongCartModel> expectedResult = new List<SongCartModel>();

        //    _songCartManager
        //        .Setup(x => x.GetSongCartForUser(email, 0))
        //        .Returns(expectedResult);

        //    // ACT
        //    var result = _songCartManager.Object.GetSongCartForUser(email, 0);

        //    // ASSERT
        //    Assert.Equal(expectedResult, result);
        //}
    }
}
