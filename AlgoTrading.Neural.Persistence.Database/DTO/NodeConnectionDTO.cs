namespace AlgoTrading.Neural.Persistence.Database.DTO
{
    public class NodeConnectionDTO
    {
        public int Id { get; set; }
        public int LocalId { get; set; }
        public double Weight { get; set; }
        public NodeDTO InputNode { get; set; }
        public NodeDTO OutputNode { get; set; }
    }
}
