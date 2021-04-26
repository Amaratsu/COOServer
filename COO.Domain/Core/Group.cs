using System.Collections.Generic;

namespace COO.Domain.Core
{
    public class Group
    {
        public int Id { get; set; }
        public int LeaderId { get; set; }
        public int CurrentCountCharacters { get; set; }
        public int MaxCountCharacters { get; set; }
        public List<Character> Characters { get; set; }
    }
}
