using System;
using System.Collections;
using System.Collections.Generic;

namespace COO.Domain.Core
{
    public class GameServer
    {
        public int Id { get; set; }
        // 0 - game hub, 1 - open world, 2 - instance
        public int Type { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }
        public string MapName { get; set; }
        public string interiorIP { get; set; }
        public string interiorPort { get; set; }
        public string ExternalIP { get; set; }
        public string ExternalPort { get; set; }
        public int CurrentCount { get; set; }
        public int MaxCount { get; set; }
        // 0 - off, 1 - work
        public int Status { get; set; }
        public DateTime LastUpdate { get; set; }

        public ICollection<User> Users { get; set; }

        public GameServer()
        {
            Users = new HashSet<User>();
        }
    }
}
