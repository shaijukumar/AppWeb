using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;


namespace Application._AppTableMaster
{
    public class List
    {
        public class Query : IRequest<List<AppTableMasterDto>> { }

        public class Handler : IRequestHandler<Query, List<AppTableMasterDto>>
        {
            private readonly IMapper _mapper;

            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor, UserManager<AppUser> userManager )
            {
                _mapper = mapper;
                _context = context;
                _userAccessor = userAccessor;
                _userManager = userManager;
            }

            public async Task<List<AppTableMasterDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var appTableMaster = await _context.AppTableMasters
                    .ToListAsync();
					
                return _mapper.Map<List<AppTableMaster>, List<AppTableMasterDto>>(appTableMaster);
                
            }
        }
    }
}
