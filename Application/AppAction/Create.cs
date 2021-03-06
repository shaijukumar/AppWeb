using System;
using System.Threading;
using FluentValidation;
using MediatR;
using System.Threading.Tasks;
using AutoMapper;
using Persistence;
using Application.Interfaces;
using Domain;
using System.Collections.Generic;
using Application.AppEngine;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Errors;
using System.Net;

namespace Application._AppAction
{
    public class Create
    {
        public class Command : IRequest<AppActionDto>
        {
            public int Id { get; set; }      
            public string UniqName { get; set; }    
            public int Order { get; set; }              
		    public virtual ICollection<AppStatusList> FromStatusList { get; set; } 							
            public int ToStatusId { get; set; }						
            public string Action { get; set; }
            public string ActionType { get; set; }
            public string WhenXml { get; set; }
            public int FlowId { get; set; }
            public bool InitStatus { get; set; }
            public int TableId { get; set; }
            public string ActionXml { get; set; }	
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {                
                //RuleFor(x => x.FlowId).NotEmpty();
                RuleFor(x => x.TableId).NotEmpty();
                RuleFor(x => x.Action).NotEmpty();
                RuleFor(x => x.ActionType).NotEmpty();    
                RuleFor(x => x.UniqName).NotEmpty(); 				
            }
        }

        public class Handler : IRequestHandler<Command, AppActionDto>
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

            public async Task<AppActionDto> Handle(Command request, CancellationToken cancellationToken)
            {       
                var unqName = await _context.AppActions
                        .Where( x => x.UniqName == request.UniqName )
                        .AnyAsync();
                if(unqName){
                    request.UniqName = "_" + Guid.NewGuid().ToString();
                }
                           
                try{
                    request.WhenXml = await XmlUpdate.UpdateXml(request.WhenXml, _context, request.TableId, true );                   
                }
                catch(Exception ex){
                    throw new RestException(HttpStatusCode.OK, new { Error = $"Invalid WhenXml, {ex.Message}." });                    
                }

                try{                    
                    request.ActionXml = await XmlUpdate.UpdateXml(request.ActionXml, _context, request.TableId, true );
                }
                catch(Exception ex){
                    throw new RestException(HttpStatusCode.OK, new { Error = $"Invalid ActionXml, {ex.Message}." });
                }
               
                //ActionXml
                var appAction = new AppAction
                {                    
					ToStatusId  = request.ToStatusId,
                    Order = request.Order,
                    Action  = request.Action,
                    ActionType  = request.ActionType,
                    WhenXml  = request.WhenXml,
					FlowId  = request.FlowId,
					InitStatus  = request.InitStatus,
					TableId  = request.TableId,
					ActionXml  = request.ActionXml,    
                    UniqName = request.UniqName,                                   
                };

                if( request.FromStatusList != null ){                
                    appAction.FromStatusList = new List<AppStatusList>();
                    foreach(var f in  request.FromStatusList){
                        var status = await _context.AppStatusLists
                        .Where( x => x.Id == f.Id )
                        .FirstOrDefaultAsync();

                        if( status != null ){
                            appAction.FromStatusList.Add(status);
                        }
                    }
                }
                _context.AppActions.Add(appAction);                
                
                try{
                    var success = await _context.SaveChangesAsync() > 0;          
                    return _mapper.Map<AppAction, AppActionDto>(appAction);        				                                       
                } 
                catch(Exception ex){
                     throw new RestException(HttpStatusCode.OK, new { Error = $"Problem saving changes. {ex.Message}. {ex.InnerException.Message}." });
                }              

                throw new RestException(HttpStatusCode.OK, new { Error = $"Problem saving changes." });
}
        }
    }
}
