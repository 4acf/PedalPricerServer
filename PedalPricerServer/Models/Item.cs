namespace PedalPricerServer.Models
{
    public abstract class Item
    {
        public Guid ID { get; private set; }
        public string Brand { get; set; } = string.Empty;
        public string Name { get; set;  } = string.Empty;
        public float Width { get; set; }
        public float Height { get; set; }
        public string Filename { get; set; } = string.Empty;
    }
}
