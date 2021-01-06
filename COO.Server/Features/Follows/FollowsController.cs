namespace COO.Server.Features.Follows
{
    using System.Threading.Tasks;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class FollowsController : ApiController
    {
        //private readonly IFollowService follows;
        //private readonly ICurrentUserService currentUser;

        //public FollowsController(
        //    IFollowService follows,
        //    ICurrentUserService currentUser)
        //{
        //    this.follows = follows;
        //    this.currentUser = currentUser;
        //}

        //[HttpPost]
        //public async Task<ActionResult> Follow(FollowRequestModel model)
        //{
        //    var result = await this.follows.Follow(
        //        model.UserId,
        //        this.currentUser.GetId());

        //    if (result.Failure)
        //    {
        //        return BadRequest(result.Error);
        //    }

        //    return Ok();
        //}
    }
}
