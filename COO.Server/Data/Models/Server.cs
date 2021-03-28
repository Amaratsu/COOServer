namespace COO.Server.Data.Models
{
    using System;

    public class Server
    {
        public int Id { get; set; }
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
        public DateTime Date { get; set; }
    }
}
