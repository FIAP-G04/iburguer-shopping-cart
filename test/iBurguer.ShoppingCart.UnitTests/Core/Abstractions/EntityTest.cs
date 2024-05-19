using iBurguer.ShoppingCart.Core.Abstractions;
using Moq;

namespace iBurguer.ShoppingCart.UnitTests.Core.Abstractions
{
    public class EntityTest
    {
        [Fact]
        public void Events_ShouldReturnReadOnlyCollectionOfDomainEvents()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid());
            var domainEvent = new Mock<IDomainEvent>().Object;

            // Act
            entity.RaiseEvent(domainEvent);

            // Assert
            var events = entity.Events;
            Assert.Single(events);
            Assert.Contains(domainEvent, events);
        }

        [Fact]
        public void ClearEvents_ShouldRemoveAllDomainEvents()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid());
            var domainEvent = new Mock<IDomainEvent>().Object;

            // Act
            entity.RaiseEvent(domainEvent);
            entity.ClearEvents();

            // Assert
            Assert.Empty(entity.Events);
        }

        [Fact]
        public void RaiseEvent_ShouldAddDomainEvent()
        {
            // Arrange
            var entity = new TestEntity(Guid.NewGuid());
            var domainEvent = new Mock<IDomainEvent>().Object;

            // Act
            entity.RaiseEvent(domainEvent);

            // Assert
            Assert.Contains(domainEvent, entity.Events);
        }

        [Fact]
        public void Equals_ShouldReturnTrueForSameId()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            // Act
            var result = entity1.Equals(entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalseForDifferentId()
        {
            // Arrange
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new TestEntity(Guid.NewGuid());

            // Act
            var result = entity1.Equals(entity2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ShouldReturnFalseForDifferentType()
        {
            // Arrange
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new AnotherTestEntity(entity1.Id);

            // Act
            var result = entity1.Equals(entity2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_ShouldReturnConsistentHashCode()
        {
            // Arrange
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            // Act
            var hashCode1 = entity1.GetHashCode();
            var hashCode2 = entity2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }
        public class TestEntity : Entity<Guid>
        {
            public TestEntity(Guid id)
            {
                Id = id;
            }

            public void RaiseEvent(IDomainEvent domainEvent) => base.RaiseEvent(domainEvent);
        }

        private class AnotherTestEntity : Entity<Guid>
        {
            public AnotherTestEntity(Guid id)
            {
                Id = id;
            }

            public void RaiseEvent(IDomainEvent domainEvent) => base.RaiseEvent(domainEvent);
        }
    }
}
