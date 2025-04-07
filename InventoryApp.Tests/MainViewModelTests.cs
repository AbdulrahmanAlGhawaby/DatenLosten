using InventoryApp.Models.Models;
using InventoryApp.Services.Services;
using InventoryApp.ViewModels;
using InventoryApp.ViewModels.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApp.Tests
{
    public class MainViewModelTests
    {
        private Mock<IInventoryService> _mockService;
        private MainViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IInventoryService>();
        }

        [Test]
        public void FilterItemsAsync_WithSearchText_FiltersByName()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllItems()).ReturnsAsync(new List<Item>
            {
                new Item { Name = "Apple", StockQuantity = 10 },
                new Item { Name = "Banana", StockQuantity = 5 }
            });

            _viewModel = new MainViewModel(_mockService.Object)
            {
                SearchText = "App",
                
            };

            // Act
            _viewModel.FilterItemsCommand.Execute(null);

            // Assert
            Assert.That(_viewModel.Items.Count, Is.EqualTo(1));
            Assert.That(_viewModel.Items.First().Name, Is.EqualTo("Apple"));
        }

        [Test]
        public void FilterItemsAsync_WithLowStockOption_FiltersCorrectly()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllItems()).ReturnsAsync(new List<Item>
            {
                new Item { Name = "Screw", StockQuantity = 2 },
                new Item { Name = "Hammer", StockQuantity = 10 }
            });

            _viewModel = new MainViewModel(_mockService.Object)
            {
                FilterOption = "Low Stock"
            };

            // Act
            _viewModel.FilterItemsCommand.Execute(null);

            // Assert
            Assert.That(_viewModel.Items.Count, Is.EqualTo(1));
            Assert.That(_viewModel.Items.First().Name, Is.EqualTo("Screw"));
        }
    }
}
