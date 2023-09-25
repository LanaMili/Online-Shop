namespace Spg.FlowerShop.Domain.Model
{
    public class Review : EntityBase
    {
        public int CustomerNavigationId { get; private set; } // Customer manages Reviews
        public virtual Customer CustomerNavigation { get; private set; } = default!;

        public string ProductNavigationName { get; set; } = string.Empty; 
        public virtual Product ProductNavigation { get; set; } = default!;

        public DateTime ReviewDate { get; } 
        public int ReviewScore { get; set; }
        public string Description { get; set; } = string.Empty;

        public Review(DateTime reviewDate, int reviewScore, string description, Product productNavigation)
        {
            ReviewDate = reviewDate;
            ReviewScore = reviewScore;
            Description = description;
            ProductNavigation = productNavigation;
        }
        protected Review()
        { }
 
    }
}