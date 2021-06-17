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


namespace Application._AppUserAccess
{
    public class List
    {
        public class Query : IRequest<List<AppUserAccessDto>> { }

        public class Handler : IRequestHandler<Query, List<AppUserAccessDto>>
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

            public async Task<List<AppUserAccessDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var appUserAccess = await _context.AppUserAccesss
                    .ToListAsync();
					
                return _mapper.Map<List<AppUserAccess>, List<AppUserAccessDto>>(appUserAccess);
                
            }
        }
    }
}
