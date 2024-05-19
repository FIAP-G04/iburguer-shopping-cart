using iBurguer.ShoppingCart.Infrastructure.Redis;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace iBurguer.ShoppingCart.UnitTests.Infrastructure.Redis
{
    public class CacheContextTest
    {
        [Fact]
        public async Task Get_Returns_Null_If_Value_Not_Found()
        {
            // Arrange
            var redisMock = new Mock<IConnectionMultiplexer>();
            var dbMock = new Mock<IDatabase>();
            dbMock.Setup(db => db.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(RedisValue.Null);
            redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(dbMock.Object);

            var configuration = new RedisConfiguration { SlidingExpirationInMinutes = 60 };
            var cacheContext = new CacheContext(redisMock.Object, configuration);

            var groupName = "testGroup";
            var key = "testKey";

            // Act
            var result = await cacheContext.Get<TestObject>(groupName, key, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Get_Returns_Deserialized_Value_If_Found()
        {
            // Arrange
            var redisMock = new Mock<IConnectionMultiplexer>();
            var dbMock = new Mock<IDatabase>();
            var serializedValue = JsonConvert.SerializeObject(new TestObject { Id = 1, Name = "Test" });
            dbMock.Setup(db => db.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(serializedValue);
            redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(dbMock.Object);

            var configuration = new RedisConfiguration { SlidingExpirationInMinutes = 60 };
            var cacheContext = new CacheContext(redisMock.Object, configuration);

            var groupName = "testGroup";
            var key = "testKey";

            // Act
            var result = await cacheContext.Get<TestObject>(groupName, key, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
        }

        // Helper class for testing
        private class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
