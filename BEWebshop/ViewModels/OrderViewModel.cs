using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BEWebshop.Controllers;
using BEWebshop.Data;
using BEWebshop.Models;

namespace BEWebshop.ViewModels
{
    internal class OrderViewModel : BaseViewModel
    {
        private readonly OrderController _orderController;
        private ObservableCollection<Order> _orders = new();
        private Order? _selectedOrder;
        private string _statusFilter = "All";

        public OrderViewModel(WebshopDbContext context)
        {
            _orderController = new OrderController(context);

            LoadOrdersCommand = new RelayCommand(async _ => await LoadOrdersAsync());
            FilterByStatusCommand = new RelayCommand(async _ => await FilterByStatusAsync());
            UpdateOrderStatusCommand = new RelayCommand(async param => await UpdateOrderStatusAsync(param), _ => SelectedOrder != null);
            CancelOrderCommand = new RelayCommand(async _ => await CancelOrderAsync(), _ => SelectedOrder != null);
            ViewOrderDetailsCommand = new RelayCommand(ViewOrderDetails, _ => SelectedOrder != null);

            _ = LoadOrdersAsync();
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }

        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }

        public string StatusFilter
        {
            get => _statusFilter;
            set => SetProperty(ref _statusFilter, value);
        }

        public ICommand LoadOrdersCommand { get; }
        public ICommand FilterByStatusCommand { get; }
        public ICommand UpdateOrderStatusCommand { get; }
        public ICommand CancelOrderCommand { get; }
        public ICommand ViewOrderDetailsCommand { get; }

        public async Task LoadOrdersAsync()
        {
            try
            {
                var orders = await _orderController.GetAllOrdersAsync();
                Orders = new ObservableCollection<Order>(orders);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task FilterByStatusAsync()
        {
            try
            {
                if (StatusFilter == "All")
                {
                    await LoadOrdersAsync();
                }
                else
                {
                    var orders = await _orderController.GetOrdersByStatusAsync(StatusFilter);
                    Orders = new ObservableCollection<Order>(orders);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering orders: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateOrderStatusAsync(object? parameter)
        {
            if (SelectedOrder == null || parameter is not string newStatus) return;

            try
            {
                var result = await _orderController.UpdateOrderStatusAsync(SelectedOrder.Id, newStatus);
                if (result)
                {
                    MessageBox.Show($"Order status updated to {newStatus}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadOrdersAsync();
                }
                else
                {
                    MessageBox.Show("Could not update order status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CancelOrderAsync()
        {
            if (SelectedOrder == null) return;

            try
            {
                var result = MessageBox.Show($"Cancel order {SelectedOrder.Id}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var success = await _orderController.CancelOrderAsync(SelectedOrder.Id);
                    if (success)
                    {
                        MessageBox.Show("Order cancelled successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadOrdersAsync();
                    }
                    else
                    {
                        MessageBox.Show("Could not cancel order. Only pending/processing orders can be cancelled.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cancelling order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewOrderDetails(object? parameter)
        {
            if (SelectedOrder == null) return;

            var details = $"Order ID: {SelectedOrder.Id}\n" +
                         $"Date: {SelectedOrder.OrderDate:g}\n" +
                         $"Customer: {SelectedOrder.CustomerName}\n" +
                         $"Email: {SelectedOrder.CustomerEmail}\n" +
                         $"Address: {SelectedOrder.ShippingAddress}\n" +
                         $"Status: {SelectedOrder.Status}\n" +
                         $"Total: €{SelectedOrder.TotalAmount:F2}\n\n" +
                         "Items:\n";

            foreach (var item in SelectedOrder.OrderItems)
            {
                details += $"- {item.Product?.Name ?? "Unknown"} x{item.Quantity} = €{item.Subtotal:F2}\n";
            }

            MessageBox.Show(details, "Order Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}