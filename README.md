# BEWebshop - Desktop Webshop Applicatie

Een moderne desktop webshop applicatie gebouwd met WPF en .NET 9.0, met volledige e-commerce functionaliteit inclusief gebruikersauthenticatie, productbeheer, winkelwagen en orderbeheer.

📋 Inhoudsopgave

- [Functionaliteiten](#-functionaliteiten)
- [Technologie Stack](#-technologie-stack)
- [Architectuur](#-architectuur)
- [Database Schema](#-database-schema)
- [Project Structuur](#-project-structuur)
- [Aan de Slag](#-aan-de-slag)
- [Gebruikershandleiding](#-gebruikershandleiding)
- [Dependencies](#-dependencies)

✨ Functionaliteiten

🔐 Gebruikersauthenticatie & Beveiliging
- Volledig geïntegreerd ASP.NET Core Identity systeem
- Gebruikersregistratie met validatie
- Veilige login functionaliteit
- Wachtwoord hashing met SHA256
- Gebruikersprofiel informatie (voornaam, achternaam)
- Sessie management
- E-mail validatie (unieke e-mails vereist)
- Wachtwoord vereisten (minimaal 6 karakters, hoofdletters, kleine letters, cijfers)

📦 Productbeheer
- Blader door 25 voorgeladen producten verdeeld over 5 categorieën
- Zoek producten op naam of beschrijving
- Filter producten op categorie met real-time updates
- Bekijk actuele voorraadniveaus
- Voeg producten toe aan winkelwagen met één klik

🛒 Winkelwagen
- Bekijk alle geselecteerde producten
- Pas hoeveelheden aan met +/- knoppen
- Verwijder individuele artikelen
- Live totaalprijs berekening
- Optie om volledige winkelwagen te legen
- Voorraadvalidatie voor checkout

📋 Bestellingen Plaatsen
- Compleet klantinformatie formulier (naam, email, verzendadres)
- Automatische voorraadvalidatie tijdens checkout
- Voorraadvermindering na succesvolle bestelling
- Unieke order ID generatie
- Orderbevestiging berichten
- Koppeling van bestellingen aan ingelogde gebruiker

📊 Orderbeheer
- Bekijk alle geplaatste bestellingen
- Filter bestellingen op status (Pending, Processing, Shipped, Delivered, Cancelled)
- Update orderstatus
- Annuleer bestellingen met automatische voorraad herstel
- Bekijk gedetailleerde orderinformatie
- Verwijder individuele of alle bestellingen
- Ordergeschiedenis gekoppeld aan gebruikersaccount

🛠 Technologie Stack

- Framework: .NET 9.0 Windows (WPF)
- Database: SQLite
- ORM: Entity Framework Core 9.0
- Identity: ASP.NET Core Identity 9.0
- Architectuur: MVVM (Model-View-ViewModel)
- Design Pattern: Repository Pattern
- Data Loading: Lazy Loading Proxies
- Security: SHA256 Password Hashing
- Dependency Injection: Microsoft.Extensions.DependencyInjection

🏗 Architectuur

De applicatie volgt het MVVM architectuurpatroon met duidelijke scheiding van verantwoordelijkheden en een geïntegreerd authenticatiesysteem:

```
┌─────────────┐     ┌──────────────┐     ┌──────────────┐     ┌──────────┐
│    View     │────▶│  ViewModel   │────▶│ Controller   │────▶│  Model   │
│   (XAML)    │◀────│  (Commands)  │◀────│  (Logica)    │◀────│ (Entity) │
└─────────────┘     └──────────────┘     └──────────────┘     └──────────┘
       │                    │                     │
       │                    ▼                     │
       │             ┌──────────────┐             │
       │             │ AuthService  │             │
       │             └──────────────┘             │
       │                    │                     │
       └────────────────────┼─────────────────────┘
                            ▼
                     ┌──────────────┐
                     │   DbContext  │
                     │  + Identity  │
                     └──────────────┘
```

Kerncomponenten

- Models: Data entiteiten (Product, Category, Order, CartItem, User)
- Controllers: Business logica laag (ProductController, CartController, OrderController, CategoryController)
- ViewModels: UI logica en data binding (ProductViewModel, CartViewModel, OrderViewModel, MainViewModel)
- Views: XAML user controls en windows voor UI presentatie
- AuthenticationService: Centrale authenticatie service voor login, registratie en sessie management
- DbContext: Entity Framework database context met Identity integratie en lazy loading

💾 Database Schema

Identity Tabellen (ASP.NET Core Identity)

AspNetUsers (Gebruikers)
- Id (PK, string)
- UserName (string, max 256)
- Email (string, max 256, uniek)
- EmailConfirmed (bool)
- PasswordHash (string)
- FirstName (string)
- LastName (string)
- CreatedAt (DateTime)
- SecurityStamp (string)
- + Standard Identity velden

AspNetRoles (Rollen) - Voor toekomstige uitbreiding
AspNetUserRoles (Gebruiker-Rol koppeling)
AspNetUserClaims (Gebruikers claims)
AspNetRoleClaims (Rol claims)
AspNetUserLogins (Externe login providers)
AspNetUserTokens (Authenticatie tokens)

Applicatie Tabellen

Categories (5 voorgeladen)
- Electronics (Electronica)
- Books (Boeken)
- Clothing (Kleding)
- Sports (Sport)
- Accessoires (Accessoires)

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
- UserId (FK, nullable) -- Koppeling naar AspNetUsers

Relaties
- Eén Categorie → Meerdere Producten
- Eén Product → Meerdere CartItems
- Eén Order → Meerdere CartItems (OrderItems)
- Eén User → Meerdere Orders
- Delete Behavior: Restrict voor Product/Category, Cascade voor OrderItems

🚀 Aan de Slag

Vereisten

- Visual Studio 2022 of later
- .NET 9.0 SDK
- Windows 10 of Windows 11

### Installatie

1. Clone de repository
   ```bash
   git clone (https://github.com/SoufianeAbk/BEWebshop)>
   cd BEWebshop
   ```

2. Open de solution
   ```
   Open BEWebshop.sln in Visual Studio
   ```

3. Herstel NuGet packages
   - Rechtsklik op Solution → Restore NuGet Packages
   - Of gebruik: `dotnet restore`

4. Build de solution
   ```
   Build → Build Solution (Ctrl+Shift+B)
   ```

5. Start de applicatie
   ```
   Debug → Start Debugging (F5)
   ```

Eerste Keer Opstarten

Bij het eerste opstarten zal de applicatie:
1. Automatisch de SQLite database (`webshop.db`) aanmaken
2. Entity Framework migrations uitvoeren voor Identity tabellen
3. 5 categorieën seeden
4. 25 producten toevoegen verdeeld over categorieën
5. Alle nodige relaties opzetten

Eerste Gebruiker Aanmaken:
- Klik op "Register" in het login scherm
- Vul je gegevens in (voornaam, achternaam, e-mail, wachtwoord)
- Wachtwoord moet voldoen aan minimale eisen
- Na registratie kun je inloggen met je e-mail en wachtwoord

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
│   │   └── WebshopDbContext.cs    # EF Core DbContext met Identity
│   ├── Models/                     # Data modellen
│   │   ├── CartItem.cs
│   │   ├── Category.cs
│   │   ├── Order.cs
│   │   ├── Product.cs
│   │   └── User.cs                 # Identity gebruiker model
│   ├── Services/
│   │   ├── AuthenticationService.cs  # Authenticatie logica
│   │   └── DatabaseInitializer.cs    # Database seeding
│   └── Migrations/                 # EF Core migrations
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
    │   ├── ProductView.xaml
    │   ├── LoginWindow.xaml        # Login scherm
    │   └── RegisterWindow.xaml     # Registratie scherm
    ├── App.xaml                    # Applicatie resources & DI setup
    └── MainWindow.xaml             # Hoofd navigatie window
```

📖 Gebruikershandleiding

Login & Registratie

Eerste Keer Gebruiken:
1. Start de applicatie
2. Het login scherm verschijnt
3. Klik op "Register"
4. Vul het registratieformulier in:
   - Voornaam
   - Achternaam
   - E-mail adres (moet uniek zijn)
   - Wachtwoord (min. 6 karakters, hoofdletter, kleine letter, cijfer)
   - Bevestig wachtwoord
5. Klik op "Register"
6. Bij succes word je teruggeleid naar het login scherm

Inloggen:
1. Voer je e-mail adres in
2. Voer je wachtwoord in
3. Klik op "Login"
4. Bij succes wordt de hoofdapplicatie geopend

Navigeren in de Applicatie

De applicatie heeft drie hoofdsecties toegankelijk via de bovenste navigatiebalk:

1. Products - Blader door producten en voeg toe aan winkelwagen
2. Shopping Cart - Bekijk en beheer winkelwagen items
3. Orders - Bekijk en beheer bestellingen

Bovenaan wordt je naam weergegeven: "Welcome, [Voornaam] [Achternaam]"

Producten Toevoegen aan Winkelwagen

1. Navigeer naar de Products pagina
2. Gebruik de zoekbalk of categorie filter om producten te vinden
3. Selecteer een product uit de lijst
4. Klik op "Add to Cart"
5. Een bevestigingsbericht verschijnt

Een Bestelling Plaatsen

1. Navigeer naar Shopping Cart
2. Bekijk je items en pas hoeveelheden aan indien nodig
3. Vul de klantinformatie in:
   - Naam
   - E-mail
   - Verzendadres
4. Klik op "Checkout"
5. Je bestelling wordt aangemaakt en gekoppeld aan je account
6. De winkelwagen wordt geleegd

Bestellingen Beheren

1. Navigeer naar Orders
2. Bekijk alle bestellingen in de lijst (inclusief je eigen bestellingen)
3. Gebruik het status filter om specifieke bestellingen te vinden
4. Selecteer een bestelling om:
   - Details te bekijken
   - Status bij te werken
   - Bestelling te annuleren
   - Bestelling te verwijderen

🎨 UI Design

- Kleurenschema: Professioneel blauw (#007ACC, #005A9E)
- Layout: Responsief grid-gebaseerd ontwerp
- Navigatie: Tab-gebaseerde wisseling tussen views met visuele feedback
- Data Weergave: DataGrid controls met aangepaste styling
- Interacties: Hover effecten, button states en visuele feedback
- Login/Register: Dedicated windows met moderne styling

📦 Dependencies

BEWebshop.Core

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.10" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.10" />
<PackageReference Include="Microsoft.Extensions.Identity.Core" Version="10.0.2" />
<PackageReference Include="SQLitePCLRaw.bundle_e_sqlite3" Version="2.1.10" />
```

BEWebshop (WPF)

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="9.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.10" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.10" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="10.0.2" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="10.0.2" />
<PackageReference Include="SQLitePCLRaw.bundle_e_sqlite3" Version="2.1.10" />
```

🔑 Kernfunctionaliteiten in Detail

Authenticatie & Beveiliging
- Identity Framework: Volledige integratie met ASP.NET Core Identity
- Wachtwoord Hashing: SHA256 hashing voor veilige wachtwoordopslag
- Validatie: E-mail uniciteit, wachtwoordsterkte controles
- Sessie Management: Singleton AuthenticationService voor sessie persistentie
- Dependency Injection: Services geregistreerd in DI container
- Startup Flow: Login-first approach met OnExplicitShutdown mode

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
- Wachtwoord sterkte validatie

Foutafhandeling
- Gebruiksvriendelijke foutmeldingen
- Exception logging via Debug output
- Graceful degradation bij database fouten
- Try-catch blokken in alle async operaties


🧪 Testen

Momenteel bevat de applicatie:
- Handmatige test procedures
- Debug output voor tracking van operaties
- Data validatie op meerdere lagen
- Identity validatie en error handling

Test Gebruiker Aanmaken:
```
E-mail: test@example.com
Wachtwoord: Test123!
Voornaam: Test
Achternaam: User
```

🛠️ Ontwikkeling

Het project volgt MVVM best practices met moderne .NET patterns:

- Strikte scheiding van concerns: View, ViewModel en Model zijn duidelijk gescheiden
- Data binding: Automatische UI updates via property change notifications
- Commands: RelayCommand implementatie voor user interactions
- Async/await: Asynchrone database operaties voor betere responsiviteit
- Dependency Injection: Services en ViewModels via DI container
- Identity Integration: ASP.NET Core Identity volledig geïntegreerd in WPF

Belangrijke Design Patterns

1. MVVM Pattern: Clear separation tussen UI en logica
2. Repository Pattern: Controllers als data access laag
3. Singleton Pattern: AuthenticationService voor sessie management
4. Dependency Injection: Loose coupling tussen componenten
5. Factory Pattern: ViewModels via DI container

📝 Belangrijke Opmerkingen

Database Locatie
De SQLite database (`webshop.db`) wordt aangemaakt in de uitvoeringsmap van de applicatie.

Lazy Loading
De applicatie gebruikt Entity Framework lazy loading proxies. Dit betekent dat gerelateerde entiteiten automatisch geladen worden wanneer ze benaderd worden. Zorg ervoor dat de DbContext actief blijft tijdens het gebruik van entiteiten.

Seed Data
Bij de eerste keer opstarten wordt automatisch seed data toegevoegd:
- 5 categorieën (Electronics, Books, Clothing, Sports, Accessoires)
- 25 producten (5 per categorie)
- Realistische prijzen en voorraadniveaus

Identity Migrations
De applicatie gebruikt Entity Framework migrations voor database schema beheer. Bij de eerste start wordt automatisch `Database.Migrate()` aangeroepen om alle Identity tabellen aan te maken.

Wachtwoord Beveiliging
Wachtwoorden worden nooit in plain text opgeslagen. De AuthenticationService gebruikt SHA256 hashing voor veilige opslag. Bij productie gebruik wordt aangeraden om over te stappen naar de ingebouwde Identity PasswordHasher die gebruik maakt van PBKDF2 met salt.

Dependency Injection Setup
De `App.xaml.cs` configureert alle services in de `ConfigureServices` methode:
- DbContext als Scoped service
- Identity services (UserManager, UserStore, PasswordHasher)
- AuthenticationService als Singleton
- ViewModels als Transient services

🙏 Acknowledgments

- Entity Framework Core team voor de uitstekende ORM
- ASP.NET Core Identity team voor het robuuste authenticatie framework
- WPF community voor design inspiratie
- SQLite team voor de lightweight database
- Microsoft voor .NET 9.0 en dependency injection framework

AI-gegenereerde code : 
https://chatgpt.com/c/6910d637-914c-832e-83bf-1e8a0a611f92, 
https://chatgpt.com/c/6910d763-7768-832a-bb75-f412fb99d095, 
https://chatgpt.com/c/6910dae0-c6b8-832c-9a06-44594abec578,
