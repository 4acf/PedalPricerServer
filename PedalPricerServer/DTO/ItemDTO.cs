namespace PedalPricerServer.Dto
{
    public class ItemDto(string id, string brand, string name)
    {
        public string ID => id;
        public string Brand => brand;
        public string Name => name;
    }
}
