BEWebshop - Desktop Webshop Applicatie
BEWebshop is een moderne desktop webshop applicatie gebouwd met WPF en .NET 9.0. 
De applicatie biedt een complete e-commerce oplossing met productbeheer, winkelwagen functionaliteit en orderbeheer.
🚀 Functionaliteiten
Producten Beheer

Overzicht van alle 25 beschikbare producten
Zoeken naar producten op naam of omschrijving
Filteren op categorieën (5 categorieën beschikbaar)
Real-time voorraadweergave
Toevoegen aan winkelwagen met één klik

Winkelwagen

Overzicht van geselecteerde producten
Aanpassen van aantallen (+/- knoppen)
Verwijderen van artikelen
Live totaalberekening
Volledige winkelwagen legen

Bestelling Plaatsen

Klantinformatie invoeren (naam, email, verzendadres)
Voorraad validatie bij checkout
Automatische voorraadvermindering na bestelling
Order bevestiging met unieke order ID

Order Beheer

Overzicht van alle geplaatste bestellingen
Filteren op status (Pending, Processing, Shipped, Delivered, Cancelled)
Status wijzigen van bestellingen
Bestellingen annuleren (met automatische voorraad herstel)
Gedetailleerde orderinformatie bekijken
Verwijderen van individuele of alle orders

🏗️ Technische Architectuur
Technologie Stack

Framework: .NET 9.0 Windows (WPF)
Database: SQLite met Entity Framework Core 9.0
Architecture Pattern: MVVM (Model-View-ViewModel)
ORM: Entity Framework Core met Lazy Loading Proxies

Project Structuur
BEWebshop/
├── Controllers/           # Business logic controllers
│   ├── CartController.cs
│   ├── CategoryController.cs
│   ├── OrderController.cs
│   └── ProductController.cs
├── Data/                  # Database context
│   └── WebshopDbContext.cs
├── Models/                # Data models
│   ├── CartItem.cs
│   ├── Category.cs
│   ├── Order.cs
│   └── Product.cs
├── ViewModels/            # MVVM ViewModels
│   ├── BaseViewModel.cs
│   ├── CartViewModel.cs
│   ├── MainViewModel.cs
│   ├── OrderViewModel.cs
│   ├── ProductViewModel.cs
│   └── RelayCommand.cs
├── Views/                 # XAML User Controls
│   ├── CartView.xaml
│   ├── OrderView.xaml
│   └── ProductView.xaml
└── Services/              # Helper services
    └── DatabaseInitializer.cs
💾 Database Schema
5 Categories

- Electronics
- Books 
- Clothing
- Sports 
- Accessoires

25 voorgedefinieerde producten

Naam, beschrijving, prijs, voorraad, categorie

CartItems - Winkelwagen items

Product referentie, hoeveelheid, prijs
Wordt order item na checkout

Orders - Klantbestellingen

Klantgegevens, verzendadres, status, totaalbedrag
Relatie met CartItems via OrderId

🎨 User Interface

Modern Design: Professioneel blauw kleurenschema (#007ACC)
Responsive Layout: Aanpasbare kolommen en rijen
Navigatie: Drie hoofdsecties via top menu bar
DataGrids: Overzichtelijke tabellen voor alle lijsten
Interactieve Buttons: Hover effecten en disabled states

📦 Installatie & Setup
Vereisten

Visual Studio 2022
.NET 9.0 SDK
Windows 10/11

Installatie Stappen
Clone het project

bashgit clone <repository-url>
cd BEWebshop

2. Open in Visual Studio
Open BEWebshop.sln

3. Restore NuGet Packages
Rechtsklik op Solution → Restore NuGet Packages

4. Build & Run
F5 of Start Debugging
De database wordt automatisch aangemaakt met seed data bij eerste run.

📊 Database Initialisatie
Bij het eerste opstarten wordt automatisch:

Een SQLite database (webshop.db) aangemaakt
5 categorieën toegevoegd
25 producten over de categorieën verdeeld
Alle relaties zijn correct ingesteld

🔑 Key Features Details
Voorraad Beheer

Real-time voorraad tracking
Automatische voorraad vermindering bij order
Voorraad herstel bij order annulering
Voorraad validatie tijdens checkout

Order Workflow

Producten toevoegen aan winkelwagen
Winkelwagen reviewen en aanpassen
Klantinformatie invullen
Checkout → Order wordt aangemaakt
Status updates via Orders pagina

Data Validatie

Email format validatie
Verplichte velden voor checkout
Voorraad beschikbaarheid checks
Unieke category names

🛠️ Dependencies
xml<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.10" />
<PackageReference Include="SQLitePCLRaw.bundle_e_sqlite3" Version="2.1.10" />

📝 Toekomstige Verbeteringen

 Category management interface
 Export orders naar CSV/PDF
 User authentication & authorization
 Email notificaties
 Product afbeeldingen

👨‍💻 Development
Het project volgt MVVM best practices:

Strikte scheiding van concerns
Data binding voor UI updates
Commands voor user interactions
Async/await voor database operaties

AI-gegenereerde code : https://chatgpt.com/c/6910d637-914c-832e-83bf-1e8a0a611f92, https://chatgpt.com/c/6910d763-7768-832a-bb75-f412fb99d095, https://chatgpt.com/c/6910dae0-c6b8-832c-9a06-44594abec578
