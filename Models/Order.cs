/*
    This class contains the attributes for a Order in the database.
    Each order contains the OrderId, the date the order was placed,
    the price of the order, the user associated with the order and
    the products contained in the order.

    Author: Dexter Downey
    Date: 05-03-2023
*/

using System.ComponentModel.DataAnnotations;

namespace TheJawaShop.Models
{
    public class Order
    {
        public int OrderId { get; set; } // Primary key of Order

        // Order's class attributes
        [Display (Name = "Date")]
        [DataType (DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Display (Name = "Price")]
        [DataType (DataType.Currency)]
        public decimal OrderPrice { get; set; }

        // Foreign key to User
        public int UserId { get; set; }

        // Navigation properties to User and ProductOrder
        public User? User { get; set; }
        public List<ProductOrder> ProductOrders { get; set; } =  default!;
    }
}