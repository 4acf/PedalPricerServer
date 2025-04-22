namespace PedalPricerServer.Dto
{
    public class ItemDto(Guid id, string brand, string name)
    {
        public Guid ID => id;
        public string Brand => brand;
        public string Name => name;
    }
}
