using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BEWebshop.Data;
using BEWebshop.Services;

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

            // Initialize database with proper error handling
            try
            {
                DatabaseInitializer.Initialize(_context);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to initialize database. The application may not work correctly.\n\nError: {ex.Message}",
                    "Initialization Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

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

    // Simple RelayCommand implementation
    internal class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object? parameter) => _execute(parameter);
    }
}