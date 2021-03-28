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
        public string MapName { get; set; }
        public string interiorIP { get; set; }
        public string interiorPort { get; set; }
        public string ExternalIP { get; set; }
        public string ExternalPort { get; set; }
        public int CurrentCount { get; set; }
        public int MaxCount { get; set; }
        // 0 - off, 1 - work
        public int Status { get; set; }
        // current number of characters
        public int CNC { get; set; }
        // maximum number of characters
        public int MNC { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
