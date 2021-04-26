using System.Collections;
using System.Collections.Generic;

namespace COO.Domain.Core
{
    public class Alliance
    {
        public int Id { get; set; }
        public int LeaderId { get; set; }
        public string LeaderName { get; set; }
        public string Name { get; set; }
        public int CurrentCountClans { get; set; }
        public int MaxCountClans { get; set; }
        public ICollection<Character> Characters { get; set; }
    }
}
