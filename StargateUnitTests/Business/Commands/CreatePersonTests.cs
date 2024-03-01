using Microsoft.AspNetCore.Http;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateUnitTests.Helpers;

namespace StargateUnitTests.Business.Commands
{
    public class CreatePersonTests
    {
        private CreatePersonPreProcessor? _preProcessor;
        private CreatePersonHandler? _handler;
        private CreatePerson? _request;

        [Test]
        public void CreatePerson_PreProcessor_ShouldHaveUniqueName_Fail()
        {
            // Arrange
            _request = new() { Name = "John Doe" };
            List<Person> people = new() { { new() { Name = "John Doe" } } };
            using StargateContext context = people.AsMockContext();

            // Act
            CreatePreProcessorService(context);

            // Assert
            Assert.That(_preProcessor, Is.Not.Null);
            Assert.ThrowsAsync<BadHttpRequestException>(() => _preProcessor.Process(_request, new CancellationToken()));
        }

        [Test]
        public void CreatePerson_PreProcessor_Success()
        {
            // Arrange
            _request = new() { Name = "Jane Doe" };
            List<Person> people = new() { { new() { Name = "John Doe" } } };
            using StargateContext context = people.AsMockContext();
            CreatePreProcessorService(context);

            // Act
            Assert.That(_preProcessor, Is.Not.Null);
            var actual = _preProcessor.Process(_request, new CancellationToken());

            // Assert            
            Assert.That(actual, Is.EqualTo(Task.CompletedTask));
        }

        [Test]
        public async Task CreatePersonHandler_Success()
        {
            // Arrange
            _request = new() { Name = "Jane Doe" };
            List<Person> people = new() { { new() { Id = 1, Name = "John Doe" } } };
            using StargateContext context = people.AsMockContext();
            CreateHandlerService(context);

            // Act
            Assert.That(_handler, Is.Not.Null);
            var actual = await _handler.Handle(_request, new CancellationToken());

            // Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Id, Is.EqualTo(2));
        }

        private void CreatePreProcessorService(StargateContext context)
        {
            _preProcessor = new CreatePersonPreProcessor(context);
        }

        private void CreateHandlerService(StargateContext context)
        {
            _handler = new CreatePersonHandler(context);
        }
    }
}