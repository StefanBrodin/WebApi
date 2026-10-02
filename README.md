# Attraction Rating App
Stefan Brodin

**OBS!** Det här projektet fokuserar på **Microsoft SQL Server**!

GitHub: https://github.com/StefanBrodin/WebApi.git
Anslutningssträngar och nycklar finns i [secrets.json](./secrets.json) i rotmappen (om man läser detta från .zip-filen).


### Modellering i C# för Entity Framework Core (DbModels)

<p>
   <a href="./ER-diagram.png" target="_blank">
      <img src="./ER-diagram.png" alt="ERD Diagram med Crow-foot notation" width="600" />
   </a>
   <br/>
   <sub>
      <i>Klicka på diagrammet för att öppna och zooma i full upplösning.</i>
   </sub>
</p>

Jag har utgått från samma SQL-databas som jag skapade i SQL-kursen. När ER-diagrammet gjordes om till C#-klasser i `2c_DbModels` 
och konfigurerades i `MainDbContext` så strukturerades koden enligt följande principer:

1. **Entitetsklasser (`DbM`) och gränssnitt (`Interfaces`):**
   Varje tabell i ER-diagrammet representeras av en C#-klass med suffixet `DbM` (t.ex. `AttractionDbM`, `CustomerDbM`). Jag började
   nerifrån och gick uppåt genom lagren/abstraktionsnivån när jag modellerade. Först gjorde jag de vanliga modellerna (utifrån 
   Interface) i 3_Models, som sedan kopplades ihop med databasmodellerna i 2c_DbModels genom arv. Detta gör att applikationslagret och 
   controllern kan arbeta mot rena abstraktioner.

2. **Nycklar och relationer i C#:**
   - **Primärnycklar:** Samtliga klasser använder `Guid` som primärnyckel dekorerad med `[Key]`.
   - **Navigeringsegenskaper:** För att tydliggöra för EF Core hur relationerna implementerades från ER-diagrammet så har klasserna 
   försetts med både främmande nycklar och navigeringsegenskaper:
     - I *en-till-många-relationer* har "många"-sidan en explicit foreign key-egenskap (`public Guid AddressId { get; set; }`) samt 
     navigationsobjektet (`public AddressDbM AddressDbM { get; set; }`).
     - "En"-sidan har motsvarande samling (`public List<AttractionDbM> AttractionsDbM { get; set; }`).
   - **Sammansatta nycklar:** I kopplingstabellerna `AttractionCategoryDbM` och `CustomerAttractionRatingDbM` definieras 
     de sammansatta primärnycklarna (Composite Keys) direkt på klassen:
     - `[PrimaryKey(nameof(AttractionId), nameof(CategoryId))]`
     - `[PrimaryKey(nameof(CustomerId), nameof(AttractionId))]`
   - **Sammansatta och omvända index:** För att optimera sökprestandan deklareras indexen direkt på AttractionCategoryDbM:
     - `[Index(nameof(AttractionId), nameof(CategoryId))]`
     - `[Index(nameof(CategoryId), nameof(AttractionId))]`

3. **Data Annotations & Dataintegritet:**
   För att tvinga fram databasens `NOT NULL` och kolumntyper direkt från C#-koden har Data Annotations använts:
   - `[Required]` används på alla obligatoriska strängar och främmande nycklar.
   - `[MaxLength]` (till exempel `[MaxLength(200)]`) styr att strängar inte blir godtyckliga `nvarchar(max)`, vilket optimerar lagring och prestanda.
   - Valfria fält (som `StreetNumber` och `RatingReview`) definieras som nullable (`string?`) för att matcha databasens `NULL`-tillåtelse.

4. **Seeding och hantering av testdata:**
   Varje entitetsklass i C# har försetts med egenskapen `public bool Seeded { get; set; }`. Detta gör det möjligt för seedgeneratorn
   att markera genererad testdata, vilket underlättar både filtrering och automatiserad upprensning via API:et.

5. **SQL Server Check Constraints via EF Core:**
   Villkor som inte kan uttryckas med vanliga C#-attribut (exempelvis att `RatingScore` måste ligga mellan 1 och 5, samt att e-postformat 
   valideras) lades till via `ToTable(t => t.HasCheckConstraint(...))` i `SqlServerDbContext.OnModelCreating`.

6. **Databassidor och vyer (SQL Views):**
   För att inte belasta API:et med onödigt tunga joins i C# skapades två SQL-vyer i databasen: en för att räkna 
   samman en snabb översikt av innehållet (antal användare, orter och sevärdheter), och en som filtrerar fram 
   sevärdheter som helt saknar recensioner. I C# modellerades dessa som vanliga DTO-klasser och kopplades 
   enkelt in i `MainDbContext` med `.ToView(...)` och `.HasNoKey()`.

7. **Upprensning med Stored Procedure:**
   För att snabbt kunna nollställa all genererad testdata utan att råka radera "riktiga" användare skapades en 
   stored procedure (`sp_RemoveSeed`). Den tar bort data i rätt ordning baklänges genom tabellerna så att inga 
   foreign key-regler protesterar. Från API:et anropas proceduren smidigt via EF Cores `ExecuteSqlInterpolatedAsync`.

8. **CRUD-operationer & CU-DTO-mönstret:**
   För att skapa och uppdatera entiteter på ett säkert sätt används dedikerade *Create/Update Data Transfer Objects* 
   (`AttractionCuDto`, `CustomerCuDto`, `CustomerAttractionRatingCuDto`).
   - Dessa DTO:er kapslar in nödvändiga fält och refererar relationer via ID:n (`AddressId`, `CategoryIds`).
   - I repository-lagret används metoder av typen `navProp_...` för att asynkront slå upp och validera främmande nycklar innan entiteten skrivs till databasen i en Unit of Work (`SaveChangesAsync`).
   - `Attraction` kan uppdateras vad gäller rubrik, beskrivning, kategorier (`CategoryIds`) samt ort/land via dess adresskoppling (`AddressId`).

9. **Kaskadborttagning (`Cascade Delete`):**
   För att upprätthålla absolut referensintegritet och förhindra herrelösa recensioner:
   - Raderas en **kund** (`Customer`), så raderas automatiskt alla kommentarer och betyg som kunden har lämnat via `[DeleteBehavior(DeleteBehavior.Cascade)]`.
   - Raderas en **sevärdhet** (`Attraction`), raderas automatiskt alla tillhörande betyg (`CustomerAttractionRating`) och kategorikopplingar (`AttractionCategory`).

---

### Översikt över Web API Endpoints

| Område | Metod | Endpoint | Beskrivning |
| :--- | :--- | :--- | :--- |
| **Admin** | GET | `/api/Admin/DatabaseOverview` | Hämtar antal användare, orter och sevärdheter via SQL View |
| **Admin** | GET | `/api/Admin/Seed?nrItems=4` | Fyller databasen med testdata |
| **Admin** | GET | `/api/Admin/RemoveSeed?seeded=true` | Tömmer testdata via lagrad procedur (`sp_RemoveSeed`) |
| **Customer** | GET | `/api/Customer/Read` | Hämtar paginerad lista med kunder |
| **Customer** | GET | `/api/Customer/ReadWithReviews` | Hämtar kunder tillsammans med alla deras inlagda recensioner |
| **Customer** | POST | `/api/Customer/CreateItem` | Skapar en ny kund i databasen |
| **Customer** | DELETE | `/api/Customer/DeleteItem/{id}` | Raderar en kund och kaskadraderar alla dess recensioner |
| **Attraction** | GET | `/api/Attraction/Read` | Hämtar sevärdheter med filter (kategori, namn, beskrivning, ort, land) |
| **Attraction** | GET | `/api/Attraction/ReadWithoutReviews`| Hämtar sevärdheter som saknar kommentarer via SQL View |
| **Attraction** | GET | `/api/Attraction/ReadItem?id=...` | Hämtar en sevärdhet med kategori, beskrivning och alla recensioner |
| **Attraction** | POST | `/api/Attraction/CreateItem` | Skapar en ny sevärdhet |
| **Attraction** | PUT | `/api/Attraction/UpdateItem/{id}` | Ändrar en sevärdhets rubrik, beskrivning, kategori och adress/ort/land |
| **Attraction** | DELETE | `/api/Attraction/DeleteItem/{id}` | Raderar en sevärdhet och kaskadraderar recensioner och kategorikopplingar |
| **Rating** | POST | `/api/CustomerAttractionRating/CreateItem` | Lägger till en recension/kommentar kopplad till kund och sevärdhet |
| **Rating** | DELETE | `/api/CustomerAttractionRating/DeleteItem` | Raderar en specifik recension/kommentar (`customerId` & `attractionId`) |