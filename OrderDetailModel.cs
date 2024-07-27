using BookShopCart.Models;

namespace BookShopCart.DTOs
{
    public class OrderDetailModel
    {
        public string DivId { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
