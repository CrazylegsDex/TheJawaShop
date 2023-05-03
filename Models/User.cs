/*
    This class contains the attributes for a User in the database.
    Each user has a username, password and list of Products/Orders.

    Author: Dexter Downey
    Date: 05-03-2023
*/

using System.ComponentModel.DataAnnotations; // To use Decorators

namespace TheJawaShop.Models
{
    public class User
    {
        public int UserId { get; set; } // Primary key of User

        // User's class attributes
        [Required]
        [Display (Name = "User Name")]
        [StringLength (maximumLength: 30, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Display (Name = "Password")]
        [DataType (DataType.Password)]
        [StringLength (maximumLength: 16, MinimumLength = 8)]
        public string UserPassword { get; set; } = string.Empty;

        // Navigation properties to Product and Order
        public List<Product> Products { get; set; } = new List<Product>(); // 1:M Relationship
        public List<Order> Orders { get; set; } = new List<Order>(); // 1:M Relationship
    }
}