using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class List
{
    public class Query : IRequest<Result<List<Activity>>> { }

    public class Handler : IRequestHandler<Query, Result<List<Activity>>>
    {
        public DataContext _dataContext;
        public Handler(DataContext dataContext)
        {
            _dataContext = dataContext;            
        }

        public async Task<Result<List<Activity>>> Handle(Query request, 
            CancellationToken cancellationToken)
        {
            return Result<List<Activity>>.Success(
                await _dataContext.Activities.ToListAsync(cancellationToken));
        }
    }
};