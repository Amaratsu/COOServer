namespace COO.Server.Data.Models
{
    using System;

    public class Server
    {
        public int Id { get; set; }
        // 0 - game hub, 1 - open world, 2 - instance
        public int Type { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }
        // 0 - on, 1 - off 
        public int Status { get; set; }
        // current number of characters
        public int CNC { get; set; }
        // maximum number of characters
        public int MNC { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
