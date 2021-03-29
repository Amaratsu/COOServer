namespace COO.Domain.Core
{
    public class Quest
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public int Task1 { get; set; }
        public int Task2 { get; set; }
        public int Task3 { get; set; }
        public int Task4 { get; set; }
    }
}
