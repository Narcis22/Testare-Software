using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using RecordShop.BLL.Interfaces;
using RecordShop.BLL.Managers;
using RecordShop.Controllers;
using RecordShop.DAL.Entities;
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
    public class FunctionalTestForMutationExample
    {

        [Theory]
        [ClassData(typeof(GetSongsByFiltersEquivalenceClass.ValidCases))]
        [ClassData(typeof(GetSongsByFiltersPartitioningCategories.ValidCases))]
        public async Task GetSongsByFiltersValidTest(int idArtist, int minPrice, int maxPrice)
        {
            // ARRANGE
            var dbContextMock = WebApplicationFactoryMock.CreateDbContext();

            var songRepositoryMock = new SongRepository(dbContextMock);
            var songManagerMock = new SongManager(songRepositoryMock);

            // ACT
            var result = songManagerMock.GetSongsByFilters(idArtist, minPrice, maxPrice);

            // ASSERT
            Assert.NotNull(result);
        }

        [Theory]
        [ClassData(typeof(GetSongsByFiltersEquivalenceClass.InvalidCases))]
        [ClassData(typeof(GetSongsByFiltersPartitioningCategories.InvalidCases))]
        public async Task GetSongsByFiltersInvalidTest(int idArtist, int minPrice, int maxPrice)
        {
            // ARRANGE
            var dbContextMock = WebApplicationFactoryMock.CreateDbContext();

            var songRepositoryMock = new SongRepository(dbContextMock);
            var songManagerMock = new SongManager(songRepositoryMock);

            // ACT
            var thrownExceptions = Record.ExceptionAsync(() => songManagerMock.GetSongsByFilters(idArtist, minPrice, maxPrice));

            // ASSERT
            Assert.NotNull(thrownExceptions);
        }
    }
}
