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

        private void NavigateToProducts(object? parameter)
        {
            CurrentViewModel = ProductViewModel;
            CurrentView = "Products";
            // Rafraîchir la liste des produits
            _ = ProductViewModel.LoadProductsAsync();
        }

        private void NavigateToCart(object? parameter)
        {
            CurrentViewModel = CartViewModel;
            CurrentView = "Cart";
            // IMPORTANT: Rafraîchir le panier quand on y navigue
            _ = CartViewModel.LoadCartAsync();
        }

        private void NavigateToOrders(object? parameter)
        {
            CurrentViewModel = OrderViewModel;
            CurrentView = "Orders";
            // Rafraîchir la liste des commandes
            _ = OrderViewModel.LoadOrdersAsync();
        }
    }
}