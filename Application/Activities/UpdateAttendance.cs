using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly DataContext _context;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities
                    .Include(a => a.Attendees).ThenInclude(u => u.AppUser)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);

                if (activity == null) return null;

                var user = await _context.Users.FirstOrDefaultAsync(x =>
                    x.UserName ==  _userAccessor.GetUsername());

                if (user == null) return null;

                var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

                var attenance = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

                if (attenance != null && hostUsername == user.UserName)
                    activity.IsCancelled = !activity.IsCancelled;

                if (attenance != null && hostUsername != user.UserName)
                    activity.Attendees.Remove(attenance);

                if (attenance == null)
                {
                    attenance = new Domain.ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };
                    
                    activity.Attendees.Add(attenance);
                }

                var result = await _context.SaveChangesAsync() > 0;

                return result ? Result<Unit>.Success(Unit.Value) : 
                    Result<Unit>.Failure("Problem updating attendance");
            }
        }
    }
}