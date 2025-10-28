using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BEWebshop.Controllers;
using BEWebshop.Data;
using BEWebshop.Models;

namespace BEWebshop.ViewModels
{
    internal class CartViewModel : BaseViewModel
    {
        private readonly CartController _cartController;
        private readonly OrderController _orderController;
        private ObservableCollection<CartItem> _cartItems = new();
        private CartItem? _selectedCartItem;
        private decimal _cartTotal;
        private int _cartItemCount;
        private string _customerName = string.Empty;
        private string _customerEmail = string.Empty;
        private string _shippingAddress = string.Empty;

        public CartViewModel(WebshopDbContext context)
        {
            _cartController = new CartController(context);
            _orderController = new OrderController(context);

            LoadCartCommand = new RelayCommand(async _ => await LoadCartAsync());
            RemoveFromCartCommand = new RelayCommand(async _ => await RemoveFromCartAsync(), _ => SelectedCartItem != null);
            UpdateQuantityCommand = new RelayCommand(async param => await UpdateQuantityAsync(param));
            ClearCartCommand = new RelayCommand(async _ => await ClearCartAsync());
            CheckoutCommand = new RelayCommand(async _ => await CheckoutAsync(), _ => CartItems.Count > 0);

            _ = LoadCartAsync();
        }

        public ObservableCollection<CartItem> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        public CartItem? SelectedCartItem
        {
            get => _selectedCartItem;
            set => SetProperty(ref _selectedCartItem, value);
        }

        public decimal CartTotal
        {
            get => _cartTotal;
            set => SetProperty(ref _cartTotal, value);
        }

        public int CartItemCount
        {
            get => _cartItemCount;
            set => SetProperty(ref _cartItemCount, value);
        }

        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value);
        }

        public string CustomerEmail
        {
            get => _customerEmail;
            set => SetProperty(ref _customerEmail, value);
        }

        public string ShippingAddress
        {
            get => _shippingAddress;
            set => SetProperty(ref _shippingAddress, value);
        }

        public ICommand LoadCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand UpdateQuantityCommand { get; }
        public ICommand ClearCartCommand { get; }
        public ICommand CheckoutCommand { get; }

        public async Task LoadCartAsync()
        {
            try
            {
                var items = await _cartController.GetCartItemsAsync();
                CartItems = new ObservableCollection<CartItem>(items);
                CartTotal = await _cartController.GetCartTotalAsync();
                CartItemCount = await _cartController.GetCartItemCountAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cart: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task RemoveFromCartAsync()
        {
            if (SelectedCartItem == null) return;

            try
            {
                var result = MessageBox.Show("Remove this item from cart?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await _cartController.RemoveFromCartAsync(SelectedCartItem.Id);
                    await LoadCartAsync();
                    MessageBox.Show("Item removed from cart.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateQuantityAsync(object? parameter)
        {
            if (SelectedCartItem == null || parameter is not string action) return;

            try
            {
                int newQuantity = SelectedCartItem.Quantity;

                if (action == "Increase")
                    newQuantity++;
                else if (action == "Decrease" && newQuantity > 1)
                    newQuantity--;

                await _cartController.UpdateCartItemQuantityAsync(SelectedCartItem.Id, newQuantity);
                await LoadCartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating quantity: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ClearCartAsync()
        {
            try
            {
                var result = MessageBox.Show("Clear all items from cart?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    await _cartController.ClearCartAsync();
                    await LoadCartAsync();
                    MessageBox.Show("Cart cleared.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing cart: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CheckoutAsync()
        {
            if (string.IsNullOrWhiteSpace(CustomerName) ||
                string.IsNullOrWhiteSpace(CustomerEmail) ||
                string.IsNullOrWhiteSpace(ShippingAddress))
            {
                MessageBox.Show("Please fill in all customer information.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var (isValid, errors) = await _cartController.ValidateCartAsync();
                if (!isValid)
                {
                    MessageBox.Show($"Cart validation failed:\n{string.Join("\n", errors)}", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    await LoadCartAsync();
                    return;
                }

                var order = await _orderController.CreateOrderFromCartAsync(CustomerName, CustomerEmail, ShippingAddress);
                if (order != null)
                {
                    MessageBox.Show($"Order created successfully! Order ID: {order.Id}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear form
                    CustomerName = string.Empty;
                    CustomerEmail = string.Empty;
                    ShippingAddress = string.Empty;

                    await LoadCartAsync();
                }
                else
                {
                    MessageBox.Show("Could not create order. Please check stock availability.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during checkout: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}