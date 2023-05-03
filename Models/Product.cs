/*
    This class contains the attributes for a Product in the database.
    Each product has a name, price and connection to the user who
    bought it/list of orders the product is in.

    Author: Dexter Downey
    Date: 05-03-2023
*/

using System.ComponentModel.DataAnnotations; // To use Decorators

namespace TheJawaShop.Models
{
    public class Product
    {
        public int ProductId { get; set; } // Primary key of Product

        // Product's class attributes
        [Required]
        [Display (Name = "Product Name")]
        [StringLength (maximumLength: 50, MinimumLength = 3)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Display (Name = "Price")]
        [DataType (DataType.Currency)]
        [Range (minimum: 50, maximum: 100000.01)]
        public decimal ProductPrice { get; set; }

        // Foreign key to User
        public int UserId { get; set; }

        // Navigation properties to User and ProductOrder
        public User? User { get; set; }
        public List<ProductOrder> ProductOrders { get; set; } = default!;
    }
}