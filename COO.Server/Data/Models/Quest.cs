﻿namespace COO.Server.Data.Models
{
    public class Quest
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public short Completed { get; set; }
        public int Task1 { get; set; }
        public int Task2 { get; set; }
        public int Task3 { get; set; }
        public int Task4 { get; set; }
    }
}
