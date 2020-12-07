namespace COO.Server.Features.Follows
{
    using System.Threading.Tasks;
    using Infrastructure.Services;

    public interface IFollowService
    {
        Task<Result> Follow(string userId, string followerId);

        Task<bool> IsFollower(string userId, string followerId);
    }
}
