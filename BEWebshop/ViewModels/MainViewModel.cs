using System.Collections.ObjectModel;
using System.Windows.Input;
using BEWebshop.Data;
using BEWebshop.Services;
using BEWebshop.ViewModels;

namespace BEWebshop.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private readonly WebshopDbContext _context;
        private BaseViewModel? _currentViewModel;
        private string _currentView = "Products";

        public MainViewModel()
        {
            _context = new WebshopDbContext();
            _context.Database.EnsureCreated();

            // Initialize database with seed data
            DatabaseInitializer.Initialize(_context);

            ProductViewModel = new ProductViewModel(_context);
            CartViewModel = new CartViewModel(_context);
            OrderViewModel = new OrderViewModel(_context);

            _currentViewModel = ProductViewModel;

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