using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateCharacter
{
    public sealed class CreateCharacterCommand : IRequest<string>
    {
        public CreateCharacterCommand(int userId, string name, int gender, int raceId, int classId, int serverId)
        {
            UserId = userId;
            Name = name;
            Gender = gender;
            RaceId = raceId;
            ClassId = classId;
            ServerId = serverId;
        }

        public int UserId { get; }
        public string Name { get; }
        // 0 - male, 1 - female
        public int Gender { get; }
        // 0 - human, 1 - elf, 2 - dark elf, 3 - Dwarf, 4 - Orc
        public int RaceId { get; }
        public int ClassId { get; }
        public int ServerId { get; }
    }
}
