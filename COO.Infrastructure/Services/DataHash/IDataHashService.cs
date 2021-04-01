namespace COO.Infrastructure.Services.DataHash
{
    public interface IDataHashService
    {
        string EncryptData(string data, string dataSalt);
        bool IsCorrectDataHash(string data, string dataHash, string salt);
        string GenerateSalt();
    }
}
