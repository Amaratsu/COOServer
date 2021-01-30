namespace COO.Server.Features.Follows
{
    using System.Threading.Tasks;
    using Infrastructure.Services;

    public interface IFollowService
    {
        Task<Result> Follow(int userId, int followerId);

        Task<bool> IsFollower(string userId, int followerId);
    }
}
