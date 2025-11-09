using System.Collections.ObjectModel;
using System.Windows.Input;
using BEWebshop.Data;
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
            ProductViewModel.LoadProductsCommand.Execute(null);
        }

        private void NavigateToCart(object? parameter)
        {
            CurrentViewModel = CartViewModel;
            CurrentView = "Cart";
            CartViewModel.LoadCartCommand.Execute(null);
        }

        private void NavigateToOrders(object? parameter)
        {
            CurrentViewModel = OrderViewModel;
            CurrentView = "Orders";
            OrderViewModel.LoadOrdersCommand.Execute(null);
        }
    }
}