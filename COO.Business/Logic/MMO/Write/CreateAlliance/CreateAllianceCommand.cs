using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateAlliance
{
    public sealed class CreateAllianceCommand : IRequest<string>
    {
        public CreateAllianceCommand(int characterId, string allianceName)
        {
            CharacterId = characterId;
            AllianceName = allianceName;
        }

        public int CharacterId { get; }
        public string AllianceName { get; }
    }
}
