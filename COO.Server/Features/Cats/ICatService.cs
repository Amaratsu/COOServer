namespace COO.Server.Features.Cats
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Services;
    using Models;

    public interface ICatService
    {
        Task<int> Create(string imageUrl, string description, string userId);

        Task<Result> Update(int id, string description, string userId);

        Task<Result> Delete(int id, string userId);

        Task<IEnumerable<CatListingServiceModel>> ByUser(string userId);

        Task<CatDetailsServiceModel> Details(int id);
    }
}
