using System;
using System.Threading;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using AutoMapper;
using Persistence;
using Application.Interfaces;
using Domain;
using Application.Errors;
using System.Net;

namespace Application._AppStatusList
{
    public class Create
    {
        public class Command : IRequest<AppStatusListDto>
        {

		    public string Title { get; set; }
             public int Order { get; set; }	
            public int TableId { get; set; }  
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.TableId).NotEmpty();				
            }
        }

        public class Handler : IRequestHandler<Command, AppStatusListDto>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
                _userAccessor = userAccessor;

            }

            public async Task<AppStatusListDto> Handle(Command request, CancellationToken cancellationToken)
            {                                                   
                var appStatusList = new AppStatusList
                {
					Title  = request.Title,
                    Order  = request.Order,  
                    TableId  = request.TableId,                                    
                };

                _context.AppStatusLists.Add(appStatusList);
                //var success = await _context.SaveChangesAsync() > 0;

                // if (success)
                // {
                //     var toReturn = _mapper.Map <AppStatusList, AppStatusListDto>(appStatusList);
                //     return toReturn;
                // } 

                try{
                    var success = await _context.SaveChangesAsync() > 0;
                    if (success) 
                        return  _mapper.Map <AppStatusList, AppStatusListDto>(appStatusList);
                    else    
                        throw new RestException(HttpStatusCode.OK, new { Error = $"No dows updated." });
                } 
                catch(Exception ex){
                    throw new RestException(HttpStatusCode.OK, new { Error = $"Problem saving changes. {ex.Message}. {ex.InnerException.Message}." });
                }                

                throw new Exception("Problem saving changes");
}
        }
    }
}
