using InventoryApp.Commands.Commands;
using InventoryApp.Models.Models;
using InventoryApp.Services.Services;
using InventoryApp.ViewModels.ErrorModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InventoryApp.ViewModels.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly IInventoryService _inventoryService;
        private ObservableCollection<Item> _items;
        private Item? _selectedItem;
        private Item _newItem = new();
        private string _searchText;
        private string _filterOption = "All";
        public Item? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public Item NewItem
        {
            get => _newItem;
            set { _newItem = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Item> Items
        {
            get => _items;
            set { _items = value; OnPropertyChanged(); }
        }


        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                _ = FilterItems();
            }
        }

        public string FilterOption
        {
            get => _filterOption;
            set
            {
                _filterOption = value;
                _ = FilterItems();
            }
        }

        private bool _isAdding;
        public bool IsAdding
        {
            get => _isAdding;
            set
            {
                _isAdding = value;
                OnPropertyChanged(nameof(IsAdding));
                if (_isAdding)
                {
                    SelectedItem = null;
                }
            }
        }
        public bool IsEditing
        {
            get
            {
                if (SelectedItem != null)
                {
                    _isAdding = false;
                    OnPropertyChanged(nameof(IsAdding));
                }
                return SelectedItem != null;
            }
        }

        public ICommand ShowAddFormCommand { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand FilterItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand SaveItemCommand { get; }

        private Dictionary<string, List<string>> _errors = [];
        public bool HasErrors => _errors.Any();

        private AddErrorModel _addErrorsMessages = new();

        public dynamic AddErrorMessages
        {
            get
            {
                if (_errors.ContainsKey("Name"))
                    _addErrorsMessages.Name = string.Join(", ", _errors["Name"]);

                if (_errors.ContainsKey("StockQuantity"))
                    _addErrorsMessages.StockQuantity = string.Join(", ", _errors["StockQuantity"]);

                return _addErrorsMessages;
            }
        }

        public MainViewModel(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            LoadItemsCommand = new RelayCommand(async _ => await LoadItems());
            FilterItemsCommand = new RelayCommand(async _ => await FilterItems());
            AddItemCommand = new RelayCommand(async _ => await AddItemAsync());
            Items = new ObservableCollection<Item>();
            ShowAddFormCommand = new RelayCommand(_ => IsAdding = !IsAdding);
            SaveItemCommand = new RelayCommand(async _ => await SaveItemAsync(), _ => CanSaveItem());
            if (LoadItemsCommand.CanExecute(null))
            {
                LoadItemsCommand.Execute(null);
            }
        }

        private async Task LoadItems()
        {
            var items = await _inventoryService.GetAllItems();
            Items = new ObservableCollection<Item>(items);
        }

        private async Task AddItemAsync()
        {
            if (Validate(NewItem))
            {
                OnPropertyChanged(nameof(AddErrorMessages));
                return;
            }


            NewItem.LastUpdated = DateTime.Now;
            await _inventoryService.AddItem(NewItem);

            // Reset form
            NewItem = new Item();

            OnPropertyChanged(nameof(NewItem.Name));
            OnPropertyChanged(nameof(NewItem.Category));
            OnPropertyChanged(nameof(NewItem.StockQuantity));

            if (LoadItemsCommand.CanExecute(null))
            {
                LoadItemsCommand.Execute(null);
            }

            IsAdding = false;
        }
        private bool CanSaveItem()
        {
            return SelectedItem != null &&
                !string.IsNullOrWhiteSpace(SelectedItem.Name) &&
                SelectedItem.StockQuantity >= 0;
        }

        private async Task SaveItemAsync()
        {
            if (Validate(SelectedItem))
            {
                OnPropertyChanged(nameof(AddErrorMessages));
                return;
            }
            // Save the changes to the selected item
            await _inventoryService.UpdateItem(SelectedItem);
            // Refresh or update the list
            if (LoadItemsCommand.CanExecute(null))
            {
                LoadItemsCommand.Execute(null);
            }
            SelectedItem = null;
        }
        private bool Validate(Item item)
        {
            // Clear previous errors
            ClearErrors();

            // Validate fields
            if (string.IsNullOrWhiteSpace(item.Name))
            {
                AddError("Name", "Name is required.");
            }
            if (item.StockQuantity < 0)
            {
                AddError("StockQuantity", "Stock Quantity cannot be negative.");
            }

            return HasErrors;
        }
        private async Task FilterItems()
        {
            var allItems = await _inventoryService.GetAllItems();
            var filtered = allItems.Where(i => i.Name.Contains(SearchText ?? "", StringComparison.OrdinalIgnoreCase));

            if (FilterOption.Contains("Low Stock"))
                filtered = filtered.Where(i => i.StockQuantity < 5);
            else if (FilterOption.Contains("In Stock"))
                filtered = filtered.Where(i => i.StockQuantity >= 5);

            Items = new ObservableCollection<Item>(filtered);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Validation helpers
        private void AddError(string propertyName, string errorMessage)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }
            _errors[propertyName].Add(errorMessage);
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(propertyName);
        }

        private void ClearErrors()
        {
            _errors.Clear();
            OnPropertyChanged(nameof(HasErrors));
        }

        // Implement INotifyDataErrorInfo
        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                return _errors[propertyName];
            }
            return Enumerable.Empty<string>();
        }

    }

}
