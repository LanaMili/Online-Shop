using Spg.FlowerShop.Domain.Interfaces;

namespace Spg.FlowerShop.Domain.Model
{
    public enum Genders 
    { 
        Male, 
        Female, 
        Other
    }

    public class Customer : EntityBase, IFindableByGuid, IFindableByMail
    {
        public Guid Guid { get; private set; }
        public Genders Gender { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? BirthDate { get; private set; } 
        public Address? Address { get; set; } = default!;
        public long CustomerNumber { get; private set; }
        public DateTime RegistrationDateTime { get; }

        private List<ShoppingCart> _shoppingCarts = new();
        public virtual IReadOnlyList<ShoppingCart> ShoppingCarts => _shoppingCarts;

        private List<Review> _reviews = new();
        public virtual IReadOnlyList<Review> Reviews => _reviews;

        public Customer(Guid guid, Genders gender, string firstName, string lastName, string email, long customerNumber, DateTime registrationDateTime, DateTime? birthDate = null, Address? address = null)
        {
            Guid = guid;
            Gender = gender;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CustomerNumber = customerNumber;
            RegistrationDateTime = registrationDateTime;
            BirthDate = birthDate;
            Address = address;
        }

        protected Customer()
        { }

        public void AddShopingCart(ShoppingCart shoppingCart)
        {
            _shoppingCarts.Add(shoppingCart);
        }

        public void AddReview(Review review)
        {
            _reviews.Add(review);
        }

        // TO DO : Authentification 
        // TO DO : Username
        // TO DO : Password string
    }
}
