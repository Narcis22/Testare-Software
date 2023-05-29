using FluentAssertions;
using Microsoft.VisualBasic;
using Moq;
using RecordShop.BLL.Interfaces;
using RecordShop.BLL.Managers;
using RecordShop.DAL.Models;
using RecordShop.DAL.Repositories;
using RecordShopTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RecordShopTest
{
    public class StructuralTest
    {
        public StructuralTest() { }

        [Fact]
        public async Task UpdateSentCartSuccess()
        {
            // ARRANGE
            var dbContextMock = WebApplicationFactoryMock.CreateDbContext();

            var cartRepository = new CartRepository(dbContextMock);
            var cartManager = new CartManager(cartRepository);

            string email = "narcis@gmail.com";

            // ACT
            var result = cartManager.UpdateSentCart(email);

            // ASSERT
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateSentCartInvalidEmail()
        {
            // ARRANGE
            var dbContextMock = WebApplicationFactoryMock.CreateDbContext();

            var cartRepository = new CartRepository(dbContextMock);
            var cartManager = new CartManager(cartRepository);

            const string email = "not.a.real.email";

            // ACT
            var result = cartManager.UpdateSentCart(email);

            // ASSERT
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateSentCartEmailNotFound()
        {
            // ARRANGE
            var dbContextMock = WebApplicationFactoryMock.CreateDbContext();

            var cartRepository = new CartRepository(dbContextMock);
            var cartManager = new CartManager(cartRepository);

            const string email = "inexistent@test.com";

            // ACT
            var result = cartManager.UpdateSentCart(email);

            // ASSERT
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateSentCartNoActiveCart()
        {
            // ARRANGE
            var dbContextMock = WebApplicationFactoryMock.CreateDbContext();

            var cartRepository = new CartRepository(dbContextMock);
            var cartManager = new CartManager(cartRepository);

            const string email = "narcis@user.com";

            // ACT
            var result = cartManager.UpdateSentCart(email);

            // ASSERT
            Assert.False(result);
        }
    }
}
