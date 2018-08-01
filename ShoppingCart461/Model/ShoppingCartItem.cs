namespace ShoppingCart.Model
{
    public class ShoppingCartItem
    {
        public int Id { get; }
        public string ProductName { get; }
        public string ProductDescription { get; }
        public Money Price { get; }

        public ShoppingCartItem(int id, string productName, string productDescription, Money price)
        {
            Id = id;
            ProductName = productName;
            ProductDescription = productDescription;
            Price = price;
        }
    }
}