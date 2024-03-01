using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Helpers;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Commands
{
    // should create an UpdatePerson request object
    public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }
    // Should create an update Command with PreProcessor and Handler
    // PreProcessor logic differs from update and create.
    // Why is name being checked for uniqueness?

    public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly StargateContext _context;
        private readonly ILogHelper _logHelper;
        private readonly ILogger _logger;
        private const string _className = "CreatePersonPreProcessor";

        public CreatePersonPreProcessor(StargateContext context, ILogHelper logHelper, ILogger logger)
        {
            _context = context;
            _logHelper = logHelper;
            _logger = logger;
        }
        public Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            const string methodName = "Process";
            try
            {
                Person? person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

                if (person is not null) throw new BadHttpRequestException("Bad Request");

                _logger.LogInformation(_logHelper.CreateSuccessLogPrefix(_className, methodName), request);

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                _logger.LogError(e, _logHelper.CreateExceptionLogPrefix(_className, methodName), request);

                return Task.FromException(e);
            }
        }
    }

    public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly StargateContext _context;
        private readonly ILogHelper _logHelper;
        private readonly ILogger _logger;
        private const string _className = "CreatePersonHandler";

        public CreatePersonHandler(StargateContext context, ILogHelper logHelper, ILogger logger)
        {
            _context = context;
            _logHelper = logHelper;
            _logger = logger;
        }

        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {
            const string methodName = "Handle";
            try
            {
                var newPerson = new Person()
                {
                    Name = request.Name
                };

                await _context.People.AddAsync(newPerson);

                await _context.SaveChangesAsync();

                _logger.LogInformation(_logHelper.CreateSuccessLogPrefix(_className, methodName), request);

                return new CreatePersonResult()
                {
                    Id = newPerson.Id
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, _logHelper.CreateExceptionLogPrefix(_className, methodName), request);

                throw;
            }
        }
    }

    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
