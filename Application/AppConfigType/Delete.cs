using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Persistence;
namespace Application._AppConfigType
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var appConfigType = await _context.AppConfigTypes
                    .FindAsync(request.Id);
                if (appConfigType == null)
                    throw new RestException(HttpStatusCode.NotFound, new { AppConfigType = "Not found" });

                var CurrentUsername = _userAccessor.GetCurrentUsername();

                _context.Remove(appConfigType);
				var success = await _context.SaveChangesAsync() > 0;
				if (success) return Unit.Value;

                throw new Exception("Problem saving changes");

            }

        }

    }
}
