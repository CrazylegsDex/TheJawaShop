/*
    This class is the many-to-many relationship model between
    Products and Orders in the database.

    Author: Dexter Downey
    Date: 05-03-2023
*/

namespace TheJawaShop.Models
{
    public class ProductOrder
    {
        // Primary key is a combination of Foreign keys
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        // Navigation property to both classes
        public Product Product { get; set; } = default!;
        public Order Order { get; set; } = default!;
    }
}