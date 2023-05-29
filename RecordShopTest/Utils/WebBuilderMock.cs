using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RecordShop.DAL;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace RecordShopTest.Utils
{
    public class WebBuilderMock : RecordShop<Program>
    {
        public readonly AppDbContext _dbContext = DbContextMock.createMock();
        // public readonly MANDATContext _dbContext = MockWebApplicationFactory.CreateDbContext(); 

        public static MANDATContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<MANDATContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var dbContext = new MANDATContext(options);
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
                    // services.Replace(new ServiceDescriptor(typeof(MANDATContext), MockDbContext.CreateContext()));
                    // services.Replace(new ServiceDescriptor(typeof(MANDATContext), provider => MockDbContext.CreateContext(), ServiceLifetime.Singleton));
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

                                    new Claim(ClaimTypes.Email, "defaultEmailFromToken@example.com"),
                            },
                            "Basic")), new AuthenticationProperties(),
                    JwtBearerDefaults.AuthenticationScheme);
                return await Task.FromResult(AuthenticateResult.Success(authenticationTicket));
            }

            public async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
                AuthenticateResult authenticationResult, HttpContext context, object resource)
            {
                // Always pass authorization
                return await Task.FromResult(PolicyAuthorizationResult.Success());
            }
        }

        public static ServiceDependencies CreateServiceDependenciesMockObject()
        {
            var unitOfWorkMock = new Mock<UnitOfWork>();
            var currentUserMock = new Mock<CurrentUserDto>();
            var serviceDependencies = new ServiceDependencies(unitOfWorkMock.Object, currentUserMock.Object);
            return serviceDependencies;
        }

    }
