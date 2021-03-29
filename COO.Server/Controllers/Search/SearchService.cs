namespace COO.Server.Controllers.Search
{
    public class SearchService : ISearchService
    {
        private readonly COODbContext data;

        public SearchService(COODbContext data) => this.data = data;

        //public async Task<IEnumerable<ProfileSearchServiceModel>> Profiles(string query)
        //    => await this.data
        //        .Users
        //        .Where(u => u.UserName.ToLower().Contains(query.ToLower()) ||
        //            u.Profile.Name.ToLower().Contains(query.ToLower()))
        //        .Select(u => new ProfileSearchServiceModel
        //        {
        //            UserId = u.Id,
        //            UserName = u.UserName,
        //            ProfilePhotoUrl = u.Profile.MainPhotoUrl
        //        })
        //        .ToListAsync();
    }
}
