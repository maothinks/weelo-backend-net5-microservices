namespace Weelo.Microservices.Properties.Domain.DTOS
{
    /// <summary>
    /// Contains the properties for Pagination and Filtering
    /// </summary>
    public class ParamsDTO
    {
        private const int _maxItemsPerpage = 50;
        private int itemsPerPage;
        public int Page { get; set; } = 1;
        public int ItemsPerPage
        {
            get => itemsPerPage;
            set => itemsPerPage = value > _maxItemsPerpage ? _maxItemsPerpage : value;
        }

        public string Name { get; set; }
        public int MaxPrice{ get; set; }
        public int MaxViews { get; set; }
        public int MaxYear { get; set; }
        public int MinPrice { get; set; }
        public int MinViews { get; set; }
        public int MinYear{ get; set; }
    }
}
