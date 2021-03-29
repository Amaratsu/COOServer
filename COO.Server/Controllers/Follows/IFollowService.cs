using System.Threading.Tasks;
using COO.Server.Infrastructure.Services;

namespace COO.Server.Controllers.Follows
{
    public interface IFollowService
    {
        Task<Result> Follow(int userId, int followerId);

        Task<bool> IsFollower(string userId, int followerId);
    }
}
