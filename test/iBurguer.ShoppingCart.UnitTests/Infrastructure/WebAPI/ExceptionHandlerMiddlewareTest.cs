using iBurguer.ShoppingCart.Infrastructure.WebApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;

namespace iBurguer.ShoppingCart.UnitTests.Infrastructure.WebAPI
{
    public class ExceptionHandlerMiddlewareTest
    {
        [Fact]
        public async Task Invoke_NextMiddlewareCalled_When_NoException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var nextMiddlewareMock = new Mock<RequestDelegate>();

            var middleware = new ExceptionHandlerMiddleware(nextMiddlewareMock.Object);

            // Act
            await middleware.Invoke(context);

            // Assert
            nextMiddlewareMock.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task Invoke_Returns_UnprocessableEntity_When_ApplicationException()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var nextMiddlewareMock = new Mock<RequestDelegate>();
            nextMiddlewareMock.Setup(next => next(context)).Throws<ApplicationException>();

            var middleware = new ExceptionHandlerMiddleware(nextMiddlewareMock.Object);

            // Act
            await middleware.Invoke(context);

            // Assert
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, context.Response.StatusCode);
        }
    }
}
