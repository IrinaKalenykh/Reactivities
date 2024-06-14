using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class ListActivities
    {
        public class Query : IRequest<Result<List<UserActivityDto>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserActivityDto>>>
        {
            private const string Hosting = "hosting";
            private const string Past = "past";
            private const string Future = "future";
        
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {                
                _dataContext = dataContext;
                _mapper = mapper;               
            }

            public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _dataContext.ActivityAttendees
                    .Where(u => u.AppUser.UserName == request.Username)
                    .OrderBy(d => d.Activity.Date)
                    .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();
                
                query = request.Predicate switch
                {
                    Hosting => query.Where(a => a.HostUsername == request.Username),
                    Past => query.Where(a => a.Date <= DateTime.Now),
                    _ => query.Where(d => d.Date > DateTime.Now)
                };

                var userActivities = await query.ToListAsync(cancellationToken);

                return Result<List<UserActivityDto>>.Success(userActivities);
            }
        }
    }
}