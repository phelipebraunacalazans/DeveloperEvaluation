using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a cart in the system.
    /// <para>(A cart is also known as a shopping cart or basket.)</para>
    /// </summary>
    /// <remarks>
    /// This entity follows domain-driven design principles and includes business rules validation.
    /// </remarks>
    public class Cart : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cart"/> class.
        /// </summary>
        public Cart()
        {
            CreatedAt = DateTime.UtcNow;
            PurchaseStatus = PurchaseStatus.Created;
        }

        /// <summary>
        /// Gets the sale number.
        /// </summary>
        public required long SaleNumber { get; init; }

        /// <summary>
        /// Gets the date when the sale was made.
        /// </summary>
        public DateTime SoldAt { get; set; }

        /// <summary>
        /// Gets the total a sale amount.
        /// </summary>
        public decimal TotalSaleAmount { get; private set; }

        /// <summary>
        /// Gets the branch where the sale was made.
        /// </summary>
        public string StoreName { get; set; }

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
        /// Gets a identifier of user who bought.
        /// </summary>
        public Guid BoughtById { get; private set; }

        /// <summary>
        /// Gets a user was created the cart.
        /// </summary>
        public required virtual User CreatedBy { get; set; }

        /// <summary>
        /// Gets a user who bought the cart items.
        /// </summary>
        public required virtual User BoughtBy { get; set; }

        /// <summary>
        /// Gets a user who cancelled the cart.
        /// </summary>
        public virtual User? CancelledBy { get; set; }

        /// <summary>
        /// Gets a user who deleted the cart.
        /// </summary>
        public virtual User? DeletedBy { get; set; }

        /// <summary>
        /// Gets a product items of cart.
        /// </summary>
        public virtual ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

        /// <summary>
        /// Gets list of active items.
        /// </summary>
        public IEnumerable<CartItem> ActiveItems =>
            Items.Where(i => i.PurchaseStatus is not PurchaseStatus.Deleted);

        /// <summary>
        /// Indicates if can be cancel cart.
        /// </summary>
        public bool CanBeCancel => PurchaseStatus is not PurchaseStatus.Cancelled;

        /// <summary>
        /// Indicates if can be edited.
        /// </summary>
        public bool CanBeEdited => PurchaseStatus is not PurchaseStatus.Cancelled;

        /// <summary>
        /// Indicates if has any deleted item.
        /// </summary>
        public bool HasAnyDeletedItem =>
            Items.Any(i => i.PurchaseStatus is PurchaseStatus.Deleted);

        /// <summary>
        /// Adding new product item and quantity to the cart.
        /// </summary>
        /// <param name="items">Adding items of cart.</param>
        public void AddItems(params CartItem[] items)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (var item in items)
            {
                item.Product.DecreaseQuantity(item.Quantity);
                Items.Add(item);
            }

            RefreshTotalAmount();
        }

        /// <summary>
        /// Cancel the cart.
        /// </summary>
        /// <param name="cancelledBy">User who cancelled the cart.</param>
        public void Cancel(User cancelledBy)
        {
            ArgumentNullException.ThrowIfNull(cancelledBy, nameof(cancelledBy));

            foreach (var item in Items)
            {
                item.Cancel(cancelledBy);
            }

            PurchaseStatus = PurchaseStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
            CancelledBy = cancelledBy;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Change cart info.
        /// </summary>
        /// <param name="customer">User that purchased.</param>
        /// <param name="date">Price of product.</param>
        /// <param name="branch">Description of product.</param>
        public void Change(
            User customer,
            DateTime date,
            string branch)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));

            BoughtBy = customer;
            BoughtById = customer.Id;
            SoldAt = date;
            StoreName = branch;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Delete the cart.
        /// </summary>
        /// <param name="deletedBy">User who deleted the cart.</param>
        public void Delete(User deletedBy)
        {
            ArgumentNullException.ThrowIfNull(deletedBy, nameof(deletedBy));

            Cancel(deletedBy);

            PurchaseStatus = PurchaseStatus.Deleted;
            DeletedAt = DateTime.UtcNow;
            DeletedBy = deletedBy;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Calculate total amount.
        /// Called when change product in item of cart.
        /// </summary>
        public void RefreshTotalAmount()
        {
            TotalSaleAmount = Items
                .Where(i => i.PurchaseStatus is PurchaseStatus.Created)
                .Sum(i => i.TotalAmount);
        }

        /// <summary>
        /// Remove items.
        /// </summary
        /// <param name="deletedBy">User that removed the items.</param>
        /// <param name="items">Items to remove</param>
        public void DeleteItems(User deletedBy, params CartItem[] items)
        {
            ArgumentNullException.ThrowIfNull(deletedBy, nameof(deletedBy));
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            foreach (var item in items)
            {
                item.Delete(deletedBy);
            }

            RefreshTotalAmount();
        }

        /// <summary>
        /// Change quantity of item.
        /// </summary>
        /// <param name="items">Items to update the quantity.</param>
        public void ChangeQuantities(params CartItem[] items)
        {
            ArgumentNullException.ThrowIfNull(items, nameof(items));

            var joinedCartItems =
                from pi in items
                join ci in Items on pi.ProductId equals ci.ProductId
                where ci.PurchaseStatus is PurchaseStatus.Created
                select new
                {
                    ParameterCartItem = pi,
                    CurrentCartItem = ci,
                };

            foreach (var joined in joinedCartItems)
            {
                joined.CurrentCartItem.ChangeQuantity(joined.ParameterCartItem.Quantity);
            }

            RefreshTotalAmount();
        }
    }
}
