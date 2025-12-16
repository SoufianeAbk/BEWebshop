BEWebshop - Desktop Webshop Applicatie

Een moderne desktop webshop applicatie gebouwd met WPF en .NET 9.0, met volledige e-commerce functionaliteit inclusief productbeheer, winkelwagen en orderbeheer.

📋 Inhoudsopgave

- [Functionaliteiten]
- [Technologie Stack]
- [Architectuur]
- [Database Schema]
- [Project Structuur]
- [Gebruikershandleiding]
- [Dependencies]

✨ Functionaliteiten

Productbeheer
- Blader door 25 voorgeladen producten verdeeld over 5 categorieën
- Zoek producten op naam of beschrijving
- Filter producten op categorie met real-time updates
- Bekijk actuele voorraadniveaus
- Voeg producten toe aan winkelwagen met één klik

Winkelwagen
- Bekijk alle geselecteerde producten
- Pas hoeveelheden aan met +/- knoppen
- Verwijder individuele artikelen
- Live totaalprijs berekening
- Optie om volledige winkelwagen te legen
- Voorraadvalidatie voor checkout

Bestellingen Plaatsen
- Compleet klantinformatie formulier (naam, email, verzendadres)
- Automatische voorraadvalidatie tijdens checkout
- Voorraadvermindering na succesvolle bestelling
- Unieke order ID generatie
- Orderbevestiging berichten

Orderbeheer
- Bekijk alle geplaatste bestellingen
- Filter bestellingen op status (Pending, Processing, Shipped, Delivered, Cancelled)
- Update orderstatus
- Annuleer bestellingen met automatische voorraad herstel
- Bekijk gedetailleerde orderinformatie
- Verwijder individuele of alle bestellingen

🛠 Technologie Stack

- Framework: .NET 9.0 Windows (WPF)
- Database: SQLite
- ORM: Entity Framework Core 9.0
- Architectuur: MVVM (Model-View-ViewModel)
- Design Pattern: Repository Pattern
- Data Loading: Lazy Loading Proxies

🏗 Architectuur

De applicatie volgt het MVVM architectuurpatroon met duidelijke scheiding van verantwoordelijkheden:

```
┌─────────────┐     ┌──────────────┐     ┌──────────────┐     ┌──────────┐
│    View     │────▶│  ViewModel   │────▶│ Controller   │────▶│  Model   │
│   (XAML)    │◀────│  (Commands)  │◀────│  (Logica)    │◀────│ (Entity) │
└─────────────┘     └──────────────┘     └──────────────┘     └──────────┘
                           │
                           ▼
                    ┌──────────────┐
                    │   DbContext  │
                    └──────────────┘
```

 Kerncomponenten

- Models: Data entiteiten (Product, Category, Order, CartItem)
- Controllers: Business logica laag (ProductController, CartController, OrderController, CategoryController)
- ViewModels: UI logica en data binding (ProductViewModel, CartViewModel, OrderViewModel)
- Views: XAML user controls voor UI presentatie
- DbContext: Entity Framework database context met lazy loading

💾 Database Schema

Tabellen

Categories (5 voorgeladen)
- Electronics (Electronica)
- Books (Boeken)
- Clothing (Kleding)
- Sports (Sport)
- Accessoires

Products (25 voorgeladen)

- Id (PK)
- Name (string, max 200 karakters)
- Description (string, max 1000 karakters)
- Price (decimal 18,2)
- Stock (int)
- CategoryId (FK)

CartItems (Winkelwagen items)

- Id (PK)
- ProductId (FK)
- Quantity (int)
- Price (decimal 18,2)
- OrderId (FK, nullable) -- null = in winkelwagen, niet null = onderdeel van bestelling


Orders (Bestellingen)

- Id (PK)
- OrderDate (DateTime)
- CustomerName (string, max 200 karakters)
- CustomerEmail (string, max 200 karakters)
- ShippingAddress (string, max 500 karakters)
- TotalAmount (decimal 18,2)
- Status (string, max 50 karakters)

Relaties
- Eén Categorie → Meerdere Producten
- Eén Product → Meerdere CartItems
- Eén Order → Meerdere CartItems (OrderItems)

🚀 Aan de Slag

Vereisten

- Visual Studio 2022 of later
- .NET 9.0 SDK
- Windows 10 of Windows 11

Installatie

1. Clone de repository
   bash
   git clone <repository-url>
   cd BEWebshop

2. Open de solution
   
   Open BEWebshop.sln in Visual Studio
   

3. Herstel NuGet packages
   - Rechtsklik op Solution → Restore NuGet Packages
   - Of gebruik: `dotnet restore`

4. Build de solution
   
   Build → Build Solution (Ctrl+Shift+B)
   

5. Start de applicatie
   
   Debug → Start Debugging (F5)
   

Eerste Keer Opstarten

Bij het eerste opstarten zal de applicatie:
- Automatisch de SQLite database (`webshop.db`) aanmaken
- 5 categorieën seeden
- 25 producten toevoegen verdeeld over categorieën
- Alle nodige relaties opzetten

Het database bestand wordt aangemaakt in de uitvoeringsmap van de applicatie.

📁 Project Structuur

```
BEWebshop/
│
├── BEWebshop.Core/                 # Core business logica bibliotheek
│   ├── Controllers/                # Business logica controllers
│   │   ├── CartController.cs
│   │   ├── CategoryController.cs
│   │   ├── OrderController.cs
│   │   └── ProductController.cs
│   ├── Data/
│   │   └── WebshopDbContext.cs    # EF Core DbContext
│   ├── Models/                     # Data modellen
│   │   ├── CartItem.cs
│   │   ├── Category.cs
│   │   ├── Order.cs
│   │   └── Product.cs
│   └── Services/
│       └── DatabaseInitializer.cs  # Database seeding
│
└── BEWebshop/                      # WPF Applicatie
    ├── ViewModels/                 # MVVM ViewModels
    │   ├── BaseViewModel.cs
    │   ├── CartViewModel.cs
    │   ├── MainViewModel.cs
    │   ├── OrderViewModel.cs
    │   ├── ProductViewModel.cs
    │   └── RelayCommand.cs
    ├── Views/                      # XAML Views
    │   ├── CartView.xaml
    │   ├── OrderView.xaml
    │   └── ProductView.xaml
    ├── App.xaml                    # Applicatie resources & styles
    └── MainWindow.xaml             # Hoofd navigatie window
```

📖 Gebruikershandleiding

 Navigeren in de Applicatie

De applicatie heeft drie hoofdsecties toegankelijk via de bovenste navigatiebalk:

1. Products - Blader door producten en voeg toe aan winkelwagen
2. Shopping Cart - Bekijk en beheer winkelwagen items
3. Orders - Bekijk en beheer bestellingen

Producten Toevoegen aan Winkelwagen

1. Navigeer naar de Products pagina
2. Gebruik de zoekbalk of categorie filter om producten te vinden
3. Selecteer een product uit de lijst
4. Klik op Add to Cart
5. Een bevestigingsbericht verschijnt

Een Bestelling Plaatsen

1. Navigeer naar Shopping Cart
2. Bekijk je items en pas hoeveelheden aan indien nodig
3. Vul de klantinformatie in:
   - Naam
   - E-mail
   - Verzendadres
4. Klik op Checkout
5. Je bestelling wordt aangemaakt en de winkelwagen wordt geleegd

Bestellingen Beheren

1. Navigeer naar Orders
2. Bekijk alle bestellingen in de lijst
3. Gebruik het status filter om specifieke bestellingen te vinden
4. Selecteer een bestelling om:
   - Details te bekijken
   - Status bij te werken
   - Bestelling te annuleren
   - Bestelling te verwijderen

🎨 UI Design

- Kleurenschema: Professioneel blauw (#007ACC, #005A9E)
- Layout: Responsief grid-gebaseerd ontwerp
- Navigatie: Tab-gebaseerde wisseling tussen views
- Data Weergave: DataGrid controls met aangepaste styling
- Interacties: Hover effecten, button states en visuele feedback

📦 Dependencies

XML
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.10" />
<PackageReference Include="SQLitePCLRaw.bundle_e_sqlite3" Version="2.1.10" />


🔑 Kernfunctionaliteiten in Detail

Voorraad Beheer
- Real-time voorraad tracking
- Automatische voorraadvermindering bij orderplaatsing
- Voorraad herstel bij order annulering
- Voorraadvalidatie tijdens checkout

Data Validatie
- E-mail formaat validatie
- Verplichte velden controle
- Voorraad beschikbaarheid checks
- Unieke categorie naam controle

Foutafhandeling
- Gebruiksvriendelijke foutmeldingen
- Exception logging via Debug output
- Graceful degradation bij database fouten

🔮 Toekomstige Verbeteringen

-  Categorie beheer interface
-  Export bestellingen naar CSV/PDF
-  Gebruikersauthenticatie & autorisatie
-  Ondersteuning voor productafbeeldingen
-  E-mail notificaties
-  Geavanceerd zoeken en filteren
-  Bestelgeschiedenis voor klanten
-  Kortings- en couponsysteem
-  Voorraad alerts bij lage voorraad

🧪 Testen

Momenteel bevat de applicatie:
- Handmatige test procedures
- Debug output voor tracking van operaties
- Data validatie op meerdere lagen

🛠️ Ontwikkeling

Het project volgt MVVM best practices:

- Strikte scheiding van concerns: View, ViewModel en Model zijn duidelijk gescheiden
- Data binding: Automatische UI updates via property change notifications
- Commands: RelayCommand implementatie voor user interactions
- Async/await: Asynchrone database operaties voor betere responsiviteit


📝 Belangrijke Opmerkingen

Database Locatie
De SQLite database (`webshop.db`) wordt aangemaakt in de uitvoeringsmap van de applicatie. Bij development is dit typisch:
```
BEWebshop/bin/Debug/net9.0-windows/webshop.db
```

Lazy Loading
De applicatie gebruikt Entity Framework lazy loading proxies. Dit betekent dat gerelateerde entiteiten automatisch geladen worden wanneer ze benaderd worden. Zorg ervoor dat de DbContext actief blijft tijdens het gebruik van entiteiten.

 Seed Data
Bij de eerste keer opstarten wordt automatisch seed data toegevoegd:
- 5 categorieën (Electronics, Books, Clothing, Sports, Accessoires)
- 25 producten (5 per categorie)
- Realistische prijzen en voorraadniveaus

🙏 Acknowledgments

- Entity Framework Core team voor de uitstekende ORM
- WPF community voor design inspiratie
- SQLite team voor de lightweight database
Opmerking: Deze applicatie is ontwikkeld voor educatieve doeleinden als onderdeel van een programmeercursus.

AI-gegenereerde code : https://chatgpt.com/c/6910d637-914c-832e-83bf-1e8a0a611f92, https://chatgpt.com/c/6910d763-7768-832a-bb75-f412fb99d095, https://chatgpt.com/c/6910dae0-c6b8-832c-9a06-44594abec578
