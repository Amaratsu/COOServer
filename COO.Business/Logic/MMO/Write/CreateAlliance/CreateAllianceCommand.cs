using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateAlliance
{
    public sealed class CreateAllianceCommand : IRequest<string>
    {
        public CreateAllianceCommand(int userId, int characterId, string allianceName)
        {
            UserId = userId;
            CharacterId = characterId;
            AllianceName = allianceName;
        }

        public int UserId { get; }
        public int CharacterId { get; }
        public string AllianceName { get; }
    }
}
