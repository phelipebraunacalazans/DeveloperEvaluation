using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a item of cart in the system.
    /// <para>
    /// A collection of products that are selected by a website customer.
    /// </para>
    /// </summary>
    /// <remarks>
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </remarks>
    public class CartItem : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItem"/> class.
        /// </summary>
        public CartItem()
        {
            CreatedAt = DateTime.UtcNow;
            PurchaseStatus = PurchaseStatus.Created;
        }

        /// <summary>
        /// Gets the quantity of products.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the unit price of product.
        /// </summary>
        public decimal UnitPrice { get; private set; }

        /// <summary>
        /// Gets the total discount percentage applied.
        /// </summary>
        public decimal DiscountPercentage { get; private set; }

        /// <summary>
        /// Gets the discount amount.
        /// </summary>
        public decimal DiscountAmount { get; private set; }

        /// <summary>
        /// Gets the total without discounts.
        /// </summary>
        public decimal TotalPreDiscounts { get; private set; }

        /// <summary>
        /// Gets the total amount.
        /// </summary>
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Gets a status of purchase.
        /// </summary>
        public PurchaseStatus PurchaseStatus { get; private set; }

        /// <summary>
        /// Gets the date and time when the category was created.
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// Gets the date and time of the last update to the category's information.
        /// </summary>
        public DateTime? UpdatedAt { get; private set; }

        /// <summary>
        /// Gets the date and time when was cancelled the cart.
        /// </summary>
        public DateTime? CancelledAt { get; private set; }

        /// <summary>
        /// Gets the date and time when was deleted the cart.
        /// </summary>
        public DateTime? DeletedAt { get; private set; }

        /// <summary>
        /// Gets identifier of product.
        /// </summary>
        public Guid ProductId { get; private set; }

        /// <summary>
        /// Gets product or order.
        /// </summary>
        public virtual Product Product { get; private set; } = default!;

        /// <summary>
        /// Gets a user was created the cart.
        /// </summary>
        public required virtual User CreatedBy { get; set; }

        /// <summary>
        /// Gets a user who cancelled the cart.
        /// </summary>
        public virtual User? CancelledBy { get; set; }

        /// <summary>
        /// Gets a user who deleted the cart.
        /// </summary>
        public virtual User? DeletedBy { get; set; }

        /// <summary>
        /// Create cart item for product.
        /// </summary>
        /// <param name="product">Product the item of cart</param>
        /// <param name="quantity">Quantity do buy</param>
        /// <param name="createdBy">User who created a item of cart</param>
        /// <returns>New instance of <see cref="CartItem"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Occurs when quantity is negative value.</exception>
        public static CartItem CreateForProduct(
            Product product,
            int quantity,
            User createdBy)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));
            ArgumentNullException.ThrowIfNull(createdBy, nameof(createdBy));

            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive value");
            }

            CartItem cartItem = new()
            {
                CreatedBy = createdBy,
                Quantity = quantity,
                UnitPrice = product.Price,
                Product = product,
                ProductId = product.Id,
                TotalPreDiscounts = quantity * product.Price,
            };
            cartItem.RefreshTotalAmount();

            return cartItem;
        }

        /// <summary>
        /// Apply discounts percentage.
        /// </summary>
        /// <param name="percentage">Percentage to discount</param>
        /// <exception cref="ArgumentOutOfRangeException">Occurs when percentage is negative value.</exception>
        public void ApplyDiscount(decimal percentage)
        {
            DiscountPercentage = percentage;
            DiscountAmount = CalculateDiscount(percentage);

            RefreshTotalAmount();
        }

        /// <summary>
        /// Calculate discounts from percentage.
        /// </summary>
        /// <param name="percentage">Percentage to discount</param>
        /// <exception cref="ArgumentOutOfRangeException">Occurs when percentage is negative value.</exception>
        public decimal CalculateDiscount(decimal percentage)
        {
            if (percentage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(percentage), "Percentage must be positive or zero value");
            }

            return TotalPreDiscounts * (percentage / 100);
        }

        /// <summary>
        /// Cancellation a item of cart.
        /// </summary>
        /// <param name="cancelledBy">User who cancelled the item of cart.</param>
        public void Cancel(User cancelledBy)
        {
            ArgumentNullException.ThrowIfNull(cancelledBy, nameof(cancelledBy));

            PurchaseStatus = PurchaseStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
            CancelledBy = cancelledBy;
            UpdatedAt = DateTime.UtcNow;

            Product.IncreaseQuantity(Quantity);
        }

        /// <summary>
        /// Change quantity.
        /// </summary>
        /// <param name="newQuantity">New quantitty to be changed.</param>
        /// <exception cref="ArgumentOutOfRangeException">Occurs when value is negative or zero.</exception>
        public void ChangeQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newQuantity), "New quantity must be positive non zero value");
            }

            UpdateProductStock(newQuantity);

            Quantity = newQuantity;
            TotalPreDiscounts = newQuantity * UnitPrice;
            UpdatedAt = DateTime.UtcNow;

            RefreshTotalAmount();
        }

        /// <summary>
        /// Delete a item of cart.
        /// </summary>
        /// <param name="deletedBy">User that deleted item of cart</param>
        internal void Delete(User deletedBy)
        {
            ArgumentNullException.ThrowIfNull(deletedBy, nameof(deletedBy));

            Cancel(deletedBy);

            PurchaseStatus = PurchaseStatus.Deleted;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
        }

        /// <summary>
        /// Calculate total amount.
        /// Called when change product in item of cart.
        /// </summary>
        private void RefreshTotalAmount()
        {
            TotalAmount = TotalPreDiscounts - DiscountAmount;
        }

        /// <summary>
        /// Update the stock of product.
        /// </summary>
        /// <param name="newQuantity">New quantity value.</param>
        private void UpdateProductStock(int newQuantity)
        {
            if (newQuantity < Quantity)
            {
                var giveBackQuantity = Quantity - newQuantity;
                Product.IncreaseQuantity(giveBackQuantity);
            }
            else if (newQuantity > Quantity)
            {
                var buyMoreQuantity = newQuantity - Quantity;
                Product.DecreaseQuantity(buyMoreQuantity);
            }
        }
    }
}
