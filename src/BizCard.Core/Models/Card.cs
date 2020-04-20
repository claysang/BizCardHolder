namespace BizCard.Core.Models
{
    public class Card : Entity
    {
        public string Company { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }
    }
}