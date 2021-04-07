using MediatR;

namespace COO.Business.Logic.MMO.Write.CreateCharacter
{
    public sealed class CreateCharacterCommand : IRequest<string>
    {
        public CreateCharacterCommand(int userId, string token, string name, int gender, int raceId, int classId)
        {
            UserId = userId;
            Token = token;
            Name = name;
            Gender = gender;
            RaceId = raceId;
            ClassId = classId;
        }

        public int UserId { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        // 0 - male, 1 - female
        public int Gender { get; set; }
        // 0 - human, 1 - elf, 2 - dark elf, 3 - Dwarf, 4 - Orc
        public int RaceId { get; set; }
        public int ClassId { get; set; }
    }
}
