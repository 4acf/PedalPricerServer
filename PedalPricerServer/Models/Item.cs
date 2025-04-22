namespace PedalPricerServer.Models
{
    public abstract class Item
    {
        public Guid ID { get; }
        public string Brand { get; } = string.Empty;
        public string Name { get; } = string.Empty;
        public float Width { get; }
        public float Height { get; set; }
        public string Filename { get; } = string.Empty;
    }
}
