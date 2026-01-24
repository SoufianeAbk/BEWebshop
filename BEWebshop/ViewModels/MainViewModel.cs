using System.Collections.ObjectModel;
using System.Windows.Input;
using BEWebshop.Core.Data;
using BEWebshop.Core.Services;
using BEWebshop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BEWebshop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AuthenticationService _authService;
        private BaseViewModel? _currentViewModel;
        private string _currentView = "Products";
        private string _currentUserName = string.Empty;

        public MainViewModel(IServiceProvider serviceProvider, AuthenticationService authService)
        {
            _serviceProvider = serviceProvider;
            _authService = authService;

            // Get ViewModels from DI
            ProductViewModel = _serviceProvider.GetRequiredService<ProductViewModel>();
            CartViewModel = _serviceProvider.GetRequiredService<CartViewModel>();
            OrderViewModel = _serviceProvider.GetRequiredService<OrderViewModel>();

            _currentViewModel = ProductViewModel;

            // Set current user name
            if (_authService.CurrentUser != null)
            {
                CurrentUserName = $"Welcome, {_authService.CurrentUser.FirstName} {_authService.CurrentUser.LastName}";
            }

            NavigateToProductsCommand = new RelayCommand(NavigateToProducts);
            NavigateToCartCommand = new RelayCommand(NavigateToCart);
            NavigateToOrdersCommand = new RelayCommand(NavigateToOrders);

            // IMPORTANT: Initialize the view - load products immediately
            InitializeAsync();
        }

        public ProductViewModel ProductViewModel { get; }
        public CartViewModel CartViewModel { get; }
        public OrderViewModel OrderViewModel { get; }

        public BaseViewModel? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public string CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public string CurrentUserName
        {
            get => _currentUserName;
            set => SetProperty(ref _currentUserName, value);
        }

        public ICommand NavigateToProductsCommand { get; }
        public ICommand NavigateToCartCommand { get; }
        public ICommand NavigateToOrdersCommand { get; }

        private async void InitializeAsync()
        {
            try
            {
                // Load products when the application starts
                await ProductViewModel.LoadProductsAsync();
                System.Diagnostics.Debug.WriteLine("Initial products loaded successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing view: {ex.Message}");
            }
        }

        private void NavigateToProducts(object? parameter)
        {
            CurrentViewModel = ProductViewModel;
            CurrentView = "Products";
            // Refresh products list
            _ = ProductViewModel.LoadProductsAsync();
        }

        private void NavigateToCart(object? parameter)
        {
            CurrentViewModel = CartViewModel;
            CurrentView = "Cart";
            // IMPORTANT: Refresh cart when navigating to it
            _ = CartViewModel.LoadCartAsync();
        }

        private void NavigateToOrders(object? parameter)
        {
            CurrentViewModel = OrderViewModel;
            CurrentView = "Orders";
            // Refresh orders list
            _ = OrderViewModel.LoadOrdersAsync();
        }
    }
}