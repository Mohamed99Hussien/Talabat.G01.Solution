namespace Talabat.Core.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }

        // in the future possible has some addresses so I make first name and last name
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string  AppUserId { get; set; } // Foreign Key

        public AppUser User { get; set; } // // Navigational property [OWN TO OWN]

    }
}