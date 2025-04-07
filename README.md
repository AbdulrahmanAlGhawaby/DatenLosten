Inventory Management Application
A WPF-based desktop application designed for managing inventory items, with features to add, view, and filter stock, as well as track inventory changes.

Overview
The Inventory Management Application is a Windows-based application built using WPF (Windows Presentation Foundation) that allows users to manage inventory items. This includes functionalities to add new items, edit existing items, and filter inventory by name and stock status.

The application uses Entity Framework for data persistence and follows the MVVM (Model-View-ViewModel) design pattern to separate concerns between the UI, logic, and data layers.

Features
View Inventory: View a list of all inventory items with columns for name, category, stock quantity, and last updated date.

Add New Items: Add new items to the inventory, including name, category, stock quantity, and description.

Edit Item: View and edit details of a selected inventory item.

Search & Filter: Filter items by stock status (low stock, in stock) and search by item name.

Validation: Ensure that item names and stock quantities are valid when adding or editing items.

Database Integration: Use of Entity Framework to persist data locally.

Technologies Used
WPF (Windows Presentation Foundation)

C#

Entity Framework Core (for local data storage)

MVVM Design Pattern

XAML for UI definition

NUnit for unit testing

Installation
Prerequisites:
.NET 8.0 or higher

Visual Studio 2022 or higher (for development and running the app)

SQL Server (or local database via Entity Framework)

Steps to Install:
Clone this repository to your local machine using the following command:

Open the solution in Visual Studio.

Build the solution.

Update-Database

Usage
Launch the Application: Open the solution in Visual Studio and run the application.

Adding Items: Click on the "Add" button to add a new inventory item. Fill in the required fields, and press "Save" to add the item to the inventory.

Search and Filter: Use the search box to filter items by name, and use the dropdown to filter items based on their stock status.

Editing Items: Select an item from the list to view its details and make changes if needed.

Author
Abdulrahman Al-Ghawaby
Author of the Inventory Management Application.
GitHub: AbdulrahmanAlGhawaby
Email: abdulrahman_alghawaby@outlook.com
