using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BEWebshop.Controllers;
using BEWebshop.Data;
using BEWebshop.Models;

namespace BEWebshop.ViewModels
{
    internal class ProductViewModel : BaseViewModel
    {
        private readonly ProductController _productController;
        private readonly CategoryController _categoryController;
        private readonly CartController _cartController;
        private ObservableCollection<Product> _products = new();
        private ObservableCollection<Category> _categories = new();
        private Product? _selectedProduct;
        private int _selectedCategoryId;
        private string _searchText = string.Empty;

        public ProductViewModel(WebshopDbContext context)
        {
            _productController = new ProductController(context);
            _categoryController = new CategoryController(context);
            _cartController = new CartController(context);

            LoadProductsCommand = new RelayCommand(async _ => await LoadProductsAsync());
            LoadCategoriesCommand = new RelayCommand(async _ => await LoadCategoriesAsync());
            AddToCartCommand = new RelayCommand(async _ => await AddToCartAsync(), _ => SelectedProduct != null);
            SearchCommand = new RelayCommand(async _ => await SearchProductsAsync());
            FilterByCategoryCommand = new RelayCommand(async _ => await FilterByCategoryAsync());

            // Load data synchronously to ensure it's available when view is shown
            LoadInitialData();
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }

        public int SelectedCategoryId
        {
            get => _selectedCategoryId;
            set => SetProperty(ref _selectedCategoryId, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public ICommand LoadProductsCommand { get; }
        public ICommand LoadCategoriesCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterByCategoryCommand { get; }

        private async void LoadInitialData()
        {
            try
            {
                await LoadCategoriesAsync();
                await LoadProductsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading initial data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task LoadProductsAsync()
        {
            try
            {
                var products = await _productController.GetAllProductsAsync();

                if (products == null || products.Count == 0)
                {
                    MessageBox.Show(
                        "No products found in the database. The database may not be properly initialized.",
                        "Information",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }

                Products = new ObservableCollection<Product>(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _categoryController.GetAllCategoriesAsync();

                // Create a new list with "All Categories" option at the beginning
                var categoriesWithAll = new List<Category>
                {
                    new Category { Id = 0, Name = "All Categories", Description = "Show all products" }
                };
                categoriesWithAll.AddRange(categories);

                Categories = new ObservableCollection<Category>(categoriesWithAll);

                // Set default selection to "All Categories"
                SelectedCategoryId = 0;

                if (categories == null || categories.Count == 0)
                {
                    MessageBox.Show(
                        "No categories found in the database. The database may not be properly initialized.",
                        "Information",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddToCartAsync()
        {
            if (SelectedProduct == null) return;

            try
            {
                if (SelectedProduct.Stock <= 0)
                {
                    MessageBox.Show($"{SelectedProduct.Name} is out of stock.", "Out of Stock",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = await _cartController.AddToCartAsync(SelectedProduct.Id, 1);
                if (result != null)
                {
                    MessageBox.Show($"{SelectedProduct.Name} added to cart!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Could not add product to cart. Please check stock availability.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding to cart: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SearchProductsAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadProductsAsync();
                return;
            }

            try
            {
                var products = await _productController.SearchProductsAsync(SearchText);
                Products = new ObservableCollection<Product>(products);

                if (products.Count == 0)
                {
                    MessageBox.Show($"No products found matching '{SearchText}'.", "Search Results",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching products: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task FilterByCategoryAsync()
        {
            if (SelectedCategoryId == 0)
            {
                // Show all products
                await LoadProductsAsync();
                return;
            }

            try
            {
                var products = await _productController.GetProductsByCategoryAsync(SelectedCategoryId);
                Products = new ObservableCollection<Product>(products);

                if (products.Count == 0)
                {
                    var categoryName = Categories.FirstOrDefault(c => c.Id == SelectedCategoryId)?.Name ?? "Unknown";
                    MessageBox.Show($"No products found in category '{categoryName}'.", "Filter Results",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering products: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}