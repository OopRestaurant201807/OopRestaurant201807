# OopRestaurant201807
A NetAcadémia "Betekintés az objektumorientált programozásba a weben is: az étterem projekt" tanfolyamának kódtára

## ASP.NET MVC és varázslói

## ASP.NET Identity

```
    Felhasználók                                             Webalkalmazás

+----------------------------+                            +-------------------------------------------+
|                            |                            |                                           |
|  +----------------------+  |                            |     Publikus oldalak                      |
|  | Ismeretlen           |  |                            |   +-----------------------+               |
|  | felhasználó          |  | +----------------------->  |   |                       |               |
|  | (nem azonosítottuk)  |  |                            |   |  Étlap megtekintése   |               |
|  |                      |  |                            |   |                       |               |
|  |                      |  |          ^-------------->  |   |                       |               |
|  |                      |  |          |                 |   |                       |               |
|  +----------------------+  |          |                 |   +-----------------------+               |
|                            |          |                 |                                           |
|                            |          |                 |                                           |
|                            |          |                 |                                           |
|                            |          |                 |     Nem publikus oldalak                  |
|  +---------------+------+  |          |                 |   +-----------------------+               |
|  | Azonosított   |      |  |          |                 |   |                       |               |
|  | felhasználó   |jogok |  |          |                 |   |  Étlap módosítása     |               |
|  | (átment az    |      |  | +--------> ha van joga     |   |                       |               |
|  | azonosításon) |      |  |          |                 |   |                       |               |
|  |               |      |  |          +-------------->  |   |                       |               |
|  +---------------+------+  |                            |   +-----------------------+               |
|                            |                            |                                           |
+----------------------------+                            |                                           |
                                                          |                                           |
                                                          +-------------------------------------------+
```
Ez egyáltalán nem triviális feladat: azonosítás és jogosultságkezelés

### Azonosítás
- név+jelszó
  - jelszó tárolása hash formában
  - jelszó bonyolultságának ellenőrzése
  - felhasználó regisztrációja
  - elfelejtett jelszó
  - jelszóváltoztatás
  - felhaszáló lockolása megadott számú rossz próbálkozás után
  - e-mail cím ellenőrzése egy ellenőrző e-mail-lel
  - két faktoros azonosítás
- külső bejelntkeztetési szolgáltatás használata
  - google
  - facebook
  - twitter
  - microsoft
  - linkedin

Az ASP.NEt Identity mindezt helyettünk megoldja.

### ASP.NET MVC alkalmazás létrehozása Identity-vel
C# projekt, ASP.NET Web Appplication (.NET Framework) kiválasztása után:

![varázsló](img/ASP.NET-Identity-wizard.png)


## Code First Migrations

```
 Adatbázis                               Alkalmazás
+------------------------+              +---------------------------+
|                        |              |                           |
| Text állomány          |   +------->  |  ami az adatokat          |
|  - csv                 |              |  használja                |
|  | xml                 |   <-------+  |                           |
|  - json                |              |                           |
| Komolyabb adatbázis    |              |                           |
| -sqlight               |              |                           |
| -sql szerver           |              |                           |
|                        |              |                           |
|                        |              |                           |
|                        |              |                           |
|                        |              |                           |
+------------------------+              +---------------------------+


      Telepítéskor: egyszerre kell kialakítani az adatbázist és az alkalmazást
      a célgépen

```

Az Identity nagyon csábító megoldást mutat: elindítom az alkamazást, és nem kell semmilyen telepítő, ő a háttérben létrehozza a saját adatbázisát.

### 1. Feladat
valahogy elérni, hogy ne automatikusan hozza létre az adatbázist, hanem legyen rá hatásunk.
- [X] mikor hozza létre?
- [X] hova hozza létre?
- [X] meg tudjuk-e adni az adatbázis helyét?


```
                          Entity Framework
 Adatbázis                Code First     Alkalmazás
+------------------------+Migrations    +---------------------------+
|                        |              |                           |
| -MS SQL Szerver        |      +       |  ami az adatokat          |
|                        |      |       |  használja                |
|                        |      v       |                           |
|                        |              |                           |
|                        |  <--------+  |  Adatmodell módosítás     |
|                        |              |                           |
|                        |  <--------+  |  Adatmodell módosítás     |
|                        |              |                           |
|                        |  <--------+  |  Adatmodell módosítás     |
|                        |              |                           |
|                        |  <--------+  |  Adatmodell módosítás     |
+------------------------+              +---------------------------+

      Telepítéskor:   Az alkalmazás hozza létre az adatbázist magának
```

#### A Code First Migrations beüzemelése
- előfeltétel: az [EntityFramework Nuget csomag](https://www.nuget.org/packages/EntityFramework/) megléte (Az ASP.NET MVC Identity telepítette, ezért ez megvan)
- engedélyezés
  ```
    PM> enable-migrations
    Checking if the context targets an existing database...
    Code First Migrations enabled for project OopRestaurant201807.
  ```

  ez létrehozta a **Migrations\Configuration.cs** állomámnyt.

- az Identity modelljének kiírása egy **migration step**-be
  ```
    PM> add-migration 'Identity datamodel'
    Scaffolding migration 'Identity datamodel'.
    The Designer Code for this migration file includes a snapshot of your current Code First model. This snapshot is used to calculate the changes to your model when you scaffold the next migration. If you make additional changes to your model that you want to include in this migration, then you can re-scaffold it by running 'Add-Migration Identity datamodel' again.
  ```
  a lépés megnevezése tetszőleges, én úgy hívtam, hogy *'Identity datamodel'*, hogy be tudjam azonosítani később ezt a lépést.

  ez létrehozta a **Migrations\201807050914249_Identity datamodel.cs** állományt (meg még két technikai állományt)

  ezt hívjuk: egy db módosító lépés, migration step. Két fontos része van: az **Up()** és a **Down()** függvények. 
  - Az Up() fügvény akkor kell, ha a módosítást bejátsszuk az adatbázisba, 
  - a Down() pedig akkor dolgozik, ha visszavonjuk ezt a módosítást.

- A Migration step kiírása adatbázisba
  ```
    PM> update-database
    Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
    Applying explicit migrations: [201807050914249_Identity datamodel].
    Applying explicit migration: 201807050914249_Identity datamodel.
  ```

Első körben nem fut le, ennek oka, hogy a **web.config**-ban meg van adva az adatelérési út, amit keres. Ezt az mdf állományt már töröltem, így nincs ilyen ezért hibával elszáll.
Ha töröljük a **web.config**-ból az adatkapcsolati beállítást, akkor gond nélkül lefut.
 
ezt az adatbázist hozza létre:

![adatbázis szerkezet](img/ASP.NET-Identity-Db-Schema.png)

Az adatbázis a saját gépen a **Default SQL Instance**-ra kerül, a neve pedig **DefaultConnection**.

#### Saját adatbázis megadása
Készítünk egy kapcsolati beállítást a [https://www.connectionstrings.com/](https://www.connectionstrings.com/) segítségével.

```xml
<connectionStrings>
    <add name="DefaultConnection" connectionString="Server=.\SQLEXPRESS;Database=OopRestaurantDb;Trusted_Connection=True"
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

Fontos, hogy megadjuk a szerver nevét, az adatbázis nevét, és a módszert, ahogy a felhasználó bejelentkezik.

```
PM> update-database
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Applying explicit migrations: [201807050914249_Identity datamodel].
Applying explicit migration: 201807050914249_Identity datamodel.
Running Seed method.
```

Ha újra futtatjuk az update-database-t, akkor ezt írja:

```
PM> update-database
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
No pending explicit migrations.
Running Seed method.
```

tehát, az eszközünk ismeri a modell verzióját, az adatbázis verzióját, és tudja, hogy nem hiányzik semmi.

Ezt a __MigrationHistory tábla segítségével tudja.

A Migrations könyvtárban lévő állománynevek tartalmazzák az adatbázis módosító lépéseket, amik a kódban vannak.
A __MigrationHistory tábla tartalmazza azokat a lépéseket, amik az adatbázisban vannak.

#### Visszavonni az egyes lépéseket
az összes visszavonása (visszaállás a 0. verzióra)

```
PM> update-database -t 0
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Reverting migrations: [201807050914249_Identity datamodel].
Reverting explicit migration: 201807050914249_Identity datamodel.
PM> 
```

A **-t** a TargetMigration paraméter rövidítése, a 0 pedig a minden lépés előtti állapot

#### Mit csinálnak az egyes lépések?
```
PM> update-database -Script
Applying explicit migrations: [201807050914249_Identity datamodel].
Applying explicit migration: 201807050914249_Identity datamodel.
```

A -Script paraméterrel nem fut le a módosítás, viszont megmutatja nekünk azt az SQL scriptet, amit a migration step-ből generál.

```
 A kódban lévő módosító lépések                                    Az adatbázisban lévő módosító lépések

+-------------------------------------+                           +---------------------------------+
|                                     |                           |                                 |
|                                     |   A hiányzó lépések       |                                 |
|  A \Migrations mappa alatt lévő     |   kerülnek az adatbázisba |                                 |
|  egyes lépések állományai           |                           |  A __MigrationHistory táblában  |
|                                     |                           |  lévő sorok                     |
|                                     |    update-database        |                                 |
|                                     |                           |                                 |
|                                     |  +--------------------->  |                                 |
|                                     |                           |                                 |
|                                     |  Ez a migration step-ben  |                                 |
|                                     |  lévő köztes nyelvből     |                                 |
|                                     |  az adatbázisnak megfelelő|                                 |
|                                     |  SQL scriptet gyárt, majd |                                 |
|                                     |  lefuttatja az SQL        |                                 |
|                                     |  szerveren                |                                 |
|                                     |                           |                                 |
|                                     |                           |                                 |
|                                     |                           |                                 |
|                                     |                           |                                 |
|    ^                                |                           |                                 |
|    |                                |                           |                                 |
+-------------------------------------+                           +---------------------------------+
     |
     |
     +

   A modell módosítása után,
   az add-migration paranccal készülnek
   a módosító lépések
```

### 2. Feladat
- [X] saját adatokat is el lehet benne helyezni?


Megnéztük a Gundel étterem étlapját, és a következőkre jutottunk:
- Minden ételnek van neve, leírása, ára
- elképzelhető, hogy nem egy étlap van egy étteremben, hanem modjuk
  - napi ajánlatok
  - szezonális ajánlatok
  - itallap
  Ezért érdemes felkészülni arra, hogy több étlapunk van. Az étlapot hívjuk **Menu**-nek.
- az étlapon lévő ételek, azok a menü egy-egy tételei, ezért nem ételnek (food) hanem menütételnek (**MenuItem**) nevezzük a sorokat.

A saját modellt létrehozva úgy tudjuk bekötni az identity adatbázisába, hogy a \Models\IdentityModels.cs-ben lévő ApplicationDbContext osztályt használjuk.

Ha olyan adatbázissal dolgozom, ahonnan lépések hiányoznak, nem tudok újabb migrációs lépést hozzáadni:

```
PM> add-migration 'add MenuItem table'
Unable to generate an explicit migration because the following explicit migrations are pending: [201807050914249_Identity datamodel]. Apply the pending explicit migrations before attempting to generate a new explicit migration.
``` 

Előtte kell egy **update-database**:

```
PM> update-database
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Applying explicit migrations: [201807050914249_Identity datamodel].
Applying explicit migration: 201807050914249_Identity datamodel.
Unable to update database to match the current model because there are pending changes and automatic migration is disabled. Either write the pending model changes to a code-based migration or enable automatic migration. Set DbMigrationsConfiguration.AutomaticMigrationsEnabled to true to enable automatic migration.
You can use the Add-Migration command to write the pending model changes to a code-based migration.
```

A megjegyzés arra vonatkozik, hogy nem tudja az adatbázist összhangba hozni a modellünkkel, mert van olyan modell változás, ami még nincs változási scriptben.
Viszont ez csak egy sárga figyelmeztetés, a migrációs lépéseket bejátszotta az adatbázisba, így létre tudjuk hozni a modell változásából a következő adatbázis módosítást.

```
PM> add-migration 'add MenuItem table'
Scaffolding migration 'add MenuItem table'.
The Designer Code for this migration file includes a snapshot of your current Code First model. This snapshot is used to calculate the changes to your model when you scaffold the next migration. If you make additional changes to your model that you want to include in this migration, then you can re-scaffold it by running 'Add-Migration add MenuItem table' again.
```

majd az update-database-zel frissíthetjük az adatbázist.

A teljes lépéssorozatot vissza tudjuk vonni, illetve egyben le tudjuk futtatni:
```
PM> update-database -t 0
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Reverting migrations: [201807051033248_add MenuItem table, 201807050914249_Identity datamodel].
Reverting explicit migration: 201807051033248_add MenuItem table.
Reverting explicit migration: 201807050914249_Identity datamodel.
PM> update-database
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Applying explicit migrations: [201807050914249_Identity datamodel, 201807051033248_add MenuItem table].
Applying explicit migration: 201807050914249_Identity datamodel.
Applying explicit migration: 201807051033248_add MenuItem table.
Running Seed method.
```

Egyesével is futtathatjuk oda és vissza is ezeket a lépéseket:

Ha egy üres adatbázisban megadom a célverzió nevét, akkor csak addig futnak a módosító lépések
```
PM> update-database -t '201807050914249_Identity datamodel'
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Applying explicit migrations: [201807050914249_Identity datamodel].
Applying explicit migration: 201807050914249_Identity datamodel.
```

Erre persze a hiányzóakat paraméter nélkül rá tudom futtatni
```
PM> update-database
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Applying explicit migrations: [201807051033248_add MenuItem table].
Applying explicit migration: 201807051033248_add MenuItem table.
Running Seed method.
```

És visszavonni is tudok egy adott verzióig bezárólag:
```
PM> update-database -t '201807050914249_Identity datamodel'
Specify the '-Verbose' flag to view the SQL statements being applied to the target database.
Reverting migrations: [201807051033248_add MenuItem table].
Reverting explicit migration: 201807051033248_add MenuItem table.
```

### 3. feladat
- [X] Módosítani az adatmodellen és bejátszani az SQL szerverre


## 1. Házi feladat
- [ ] Nézegetni különböző éttermek menüit és adatmodellt készíteni belőlük a CF migration-nel
- [ ] tetszőleges adatmodell kialakítása több lépésben és adatbázissá konvertálása


Ismétlés

```
                                                                                                Felhasználó böngésző

   SQL ADATBÁZIS                                    ASP.NET MVC szerveralkalmazás               (HTML/CSS/JavaScript)

+----------------------------+                    +------------------------+                    +------------------------+
|                            |                    |                        |                    |                        |
|                            |  +-------------->  |                        | +----------------> |                        |
|                            |                    |                        |                    |                        |
|                            |                    |                        |                    |                        |
|                            |  <--------------+  |                        | <----------------+ |                        |
|                            |                    |                        |                    |                        |
|                            |                    |                        |                    |                        |
|                            |                    | +-------+    +------+  |                    |                        |
|                            |                    | |       |    |      |  |                    |                        |
|                            |                    | |       | <+ |      |  |                    |                        |
|                            |  <--------------+  | |       |    |      |  |                    |                        |
|                            |                    | |       |    |      |  |                    |                        |
|                            |                    | +-------+    +------+  |                    |                        |
+----------------------------+                    +------------------------+                    +------------------------+
                                                                      ^
                                                                      |
                                                  Migrációs           +
                                                  lépések          Adatmodell

```

### 4. feladat
- [X] csak bejelentkezett felhasználónak szabad a menü módosítását/új elem felvitelét engedélyezni
- [X] be nem jelentkezett felhasználó csak megtekinteni tudja a menüt

### 5. feladat
- [X] amit látsz azt használhatod, amit nem használhatsz azt nem látod elv érvényesítése

### 6. feladat
- [X] Étlap link megjelenítése a weboldalon
- [X] Az ételek szakaszokba rendezése (kategória bevezetése)
  - [X] Létrehozni egy kategória (Category) listát
  - [X] webfelületet gyártani a kategória listához
  - [X] étlap nézet elkészítése

[Olvasnivaló: A szivárgó absztrakciók törvénye](http://hungarian.joelonsoftware.com/Articles/LeakyAbstractions.html)

Létrehozunk minden étel elemhez egy kategóriát. De nem tesszük bele a MenuItems táblába, (vagyis, a MenuItem sorba), hanem kiemeljük egy saját tánlázatba, ami azt jelenti, hogy saját osztályba.

| Id | Name | Description |	Price | CategoryId |
|-|-|-|-|-|
|1|Tengeri hal trió | Atlanti lazactatár, pácolt lazacfilé és tonhal lazackaviárral | 7500 | 1 |
|3| Borjúesszencia | Zöldséges gyöngytyúk galuska | 4500 | 1 |

| Id | Category |
| - | - |
| 1 | Levesek, előételek |

Létrehozzuk a Category osztályt, és egy hivatkozást rá a MenuItem osztályból.

majd ez a két lépés következik:

```
PM> add-migration 'add Category table, and MenuItem.Category column'
PM> update-database
```

Az étlap megjelenítéséhez 
- először végig kell menni a kategóriákon, 
- majd az azonos kategóriához tartozó ételeket megjeleníteni
- szükséges, hogy kategóriák szerinti sorrendet adjunk meg a lekérdezésben

linq információk ezzel a google kereséssel: **linq 101**

| Category.Name | Name | Description |	Price | 
|-|-|-|-|
| Előételek | Tengeri hal trió | Atlanti lazactatár, pácolt lazacfilé és tonhal lazackaviárral | 7500 | 
| Előételek | Borjúesszencia | Zöldséges gyöngytyúk galuska | 4500 | 

### 2. Házi feladat
- bootstrap felfedezés: a példa étlapon szereplő kinyíló és becsukódó panel megvalósítása
- kitenni a kategóriát a MenuItem-re is, átgondolni!

### 7. feladat
- [X] bootstrap felfedezés: a példa étlapon szereplő kinyíló és becsukódó panel megvalósítása
  - [X] megfelelő adatok előállítása a nézet számára: 
    - [X] kell egy category lista, 
    - [X] és az egyes category-hoz tartozó menuItem-ek listája

Megoldási lehetőségek:
- a kapott adatokat késznek véve elkezdünk faragni a Controllerben, hogy a nézetbe megfelelő adatok kerüljenek
- átalakítjuk az adatforrást, hogy eleve a megfelelő adatokat kapjuk.

Categories
- Hideg előételek
  - Tengeri hal trió
- Meleg előételek
  - Hirtelen sült fogasderék illatos erdei gombákkal 
- Levesek
  - Borjúesszencia
  - Szarvasgomba cappuccino

### 8. Feladat
- [X] kitenni a kategóriát a MenuItem-re is
  - [X] egyben biztosítani, hogy kategória nélküli étel ne kerüljön az adatbázisba
    ezt a Required annotációval lehet elérni. Problémák:
        - [ ] ha már vannak adatok, amik nem illeszkednek ehhez a megszorításhoz, nem fog lefutni a migráció
          - vagy javítjuk az adatokat az adatbázisban (eltüntetjük a null-t). Mivel ez kézi megoldás, és kívül van migrációs lépések automatizmusán, minden adatbázison kézzel kell külön megoldani.
          - vagy megadjuk az adott mező alapértelmezett értékét 
        - [ ] ha van le nem futó migrációs lépésünk, akkor az alkalmazásunk sem fog lefutni, addig, amíg az alkalmazás modellje és az adatbázismodell nincs szinkronban.
  - [X] A MenuItem felviteli űrlapra a Category választó (Lenyílómező) megjelenítése

```
+-------------------------------------------------+                     +---------------------------------------+
|                                                 | A választott+-----> |                                       |
|                                                 | érték               +---------------------------------------+
|                                                 |                     |                                       |
|                                                 |                     |   +--------------------------------+  |
|                +----------------------+         |                     |                                       |
|          Name: |                      |         |                     |   +--------------------------------+  |
|                +----------------------+         | A lehetséges        |                                       |
|                +----------------------+         | értékek     +-----> |   +--------------------------------+  |
|   Description: |                      |         | felsorolása         |                                       |
|                +----------------------+         |                     |   +--------------------------------+  |
|                +----------------------+         |                     |                                       |
|         Price: |                      |         |                     |   +--------------------------------+  |
|                +----------------------+         |                     |                                       |
|                                                 |                     |                                       |
|                                                 |                     |                                       |
|                +----------------------+---+     |                     |                                       |
|      Category: |                      |   |     |                     |                                       |
|                +----------------------+---+     |                     |                                       |
|                                                 |                     |                                       |
|                                                 |                     |                                       |
|                                                 |                     |                                       |
|                                                 |                     |                                       |
|    +---------+                  +---------+     |                     |                                       |
|    |  Save   |                  | Cancel  |     |                     |                                       |
|    +---------+                  +---------+     |                     |                                       |
|                                                 |                     +---------------------------------------+
|                                                 |
|                                                 |
|                                                 |
+-------------------------------------------------+
```

A lenyílómező adatszükséglete a megjelenítéshez:
- lehetséges választási lehetőségek
- a kiválasztott lehetőség
A lenyílómező adatszükséglete az adatrögzítéshez:
- a kiválasztott lehetőség

### 3. Házi feladat
- az eredeti táblázatból ellátni az MenuController.Index View-t a megfelelő adatokkal
  - betölteni a menuItem listát a Category mezővel
  - Készíteni egy CategoryModel-t
  - Készíteni egy CategoryModel gyűjteményt az adatbázisból olvasott MenuItem listából
  - Minden CategoryModel-en legye egy MenuItem gyűjtemény, és töltsük fel ezt is ugyanabból az adatforrásból

### 9. feladat (8. folytatása)
- [X] kitenni a kategóriát a MenuItem-re is
  - [X] A MenuItem edit képernyőre kitenni a Category választót
  - [X] A bejövő adatokat menteni
  - [X] A MenuItem Details képernyőre kitenni a Category értékét
  - [X] A MenuItem Delete képernyőre kitenni a Category értékét

```
                               <-------------------------------------------------^
                               |                                                 |
    Adatbázis                  |                        EntityFramework          |                     Adatmodell
+---------------------------------------------+       +---------------------------------------+      +-----------------------------+
|                              |              |       | +----------------------------------+  |      |     Attach                  |
|                              |              |       | | DbContext.MenuItems              |  | <--------------------------^       |
|                              |              |       | +----------------------------------+  |      |    MenuItem         |       |
|                              |              |       |      MenuItemEntry                    |      |  +------------------+-+     |
|                              |              |       |     +----------------+     Entry      |      |  |                    |     |
|                              v              |       |     |                | <------------------------+                    |     |
|                                             |       |     |                |                |      |  |                    |     |
|    Categories              MenuItems        |       |     |                |                |      |  |                    |     |
|  +-------------+         +-------------+    |       |     |                |                |      |  |                    |     |
|  |             |         |             |    |       |     |                |                |      |  |   Category         |     |
|  |             |         |             |    |       |     |                |                |      |  |  +---------------+ |     |
|  +-------------+         +-------------+    |       |     |                |                |      |  |  |               | |     |
|  | Category    | <-----+ | MenuItem    |    |       |     |                |                |      |  |  |               | |     |
|  +-------------+         +-------------+    |       |     |                |                |      |  |  |               | |     |
|  |             |         |             |    |       |     +-+--------------+                |      |  |  +---------------+ |     |
|  |             |         |             |    |       |       |                               |      |  |                    |     |
|  +-------------+         +-------------+    |       |       | Reference                     |      |  |                    |     |
|                                             |       |       | Load                          |      |  |                    |     |
|                                             |       |       |          +-----------+        |      |  +--------------------+     |
|                        <----SaveChanges-------------+       +--------> | Category  |        |      |                             |
|                                             |       |                  +-----------+        |      |                             |
+---------------------------------------------+       +---------------------------------------+      +-----------------------------+

```

Az adatbázis és az EntityFramework "beszélgetéseibe" az SQL Server Profilerrel tudunk belesni. 

SQL Server Profiler indítása: SQL Server management Studio\Tools\SQL Server Profiler


### 10. feladat
- [X] hogy lehet az üres adatbázist feltölteni tesztadatokkal?
- [X] a defaultValue és a notnull rossz sorrendjének a javítása
      a probléma gyökere az, hogy MIUTÁN a Required annotációt kiadtuk, csatoltuk a defaultValue beállítást EGY ÉS UGYANAZON migrációs lépésben. Ezzel az SQL generátorra bíztuk, hogy milyen sorrendben generálja őket. Szedjük szét a két lépést:
    - [X] adjuk meg a default value értékét a Category_Id mezőnek MAJD
    - [X] állítsuk be, hogy ne lehessen null az értéke_

### Kérdések
- [X] az, hogy a MenuItem.Category kitöltése nem kötelező, ez vajon jó-e nekünk?
- [X] hogy kell javítani a Seed-et ahhoz, hogy ne duplázza az adatokat
- [X] hogy lehet a felhasználókat rögzíteni a Seed-ben

Az ASP.NET Identity adatkezelése több rétegből áll:
```
+-------------------------------------------------------------+
|                                                             |
|                                                             |
|                                                             |
|                                                             |
| +-------------+   +--------------+  +-----------------+     |
| |             |   |              |  |                 |     |
| | Adatbázis   |   |  UserStore   |  | UserManager     | <------+
| |             |   |              |  |                 |     |
| |             |   |              |  |                 |     |
| |             |   |         <-------------+           |     |
| |             |   |              |  |                 |     |
| |             |   |              |  |                 |     |
| |      <----------------+        |  |                 |     |
| |             |   |              |  |                 |     |
| |             |   |              |  |                 |     |
| |             |   |              |  |                 |     |
| +-------------+   +--------------+  +-----------------+     |
|                                                             |
|                                                             |
|                                                             |
|                                                             |
|                                                             |
+-------------------------------------------------------------+
```

- A UserStore felel az adatok karbantartásáért
- A UserManager pedig az a felület, amit a programozó használhat


### 11. feladat
- [ ] Magyar nyelvű alkalmazást készítünk
  - [X] Bejelentkező és regisztrációs oldal magyarítása
  - [X] A menuItem idex oldal magyarítása
  - [X] Alapértelmezett kezdőoldal legyen a Menu nézet
- [X] a lenyíló mező alapértelmezett értéke Create esetén legyen egy ki nem választott érték.
- [X] a lenyíló mező formázásának a javítása.

```html
<select data-val="true" 
        data-val-number="The field CategoryId must be a number." 
        data-val-required="The CategoryId field is required." 
        htmlattributes="{ class = form-control }" 
        id="CategoryId" name="CategoryId">
    <option value="">- Válassz egy lehetőséget -</option>
    <option value="1">Hideg előételek</option>
    <option value="2">Levesek</option>
    <option value="3">Meleg előételek</option>
</select>
````
A hiba oka tehát az, hogy ez van
```html
   htmlattributes="{ class = form-control }" 
```
```html
   class="form-control"
```
a megoldás az, hogy a paramétert az előző EDIT input generáló kifejezésről másoltuk, ahol **additionalViewData** paraméterként volt megadva. De a @Html.DropDownListFor() pedig **htmlAttributes** paramétert vár, ami más formátumú.

### 12. feladat
- [X] Asztalok és helyszínek modellje és megjelenítése
- [X] DisplayTemplate: a Details nézet módosítását szeretném a Delete nézeten is megjeleníteni.
  - [X] A \View\Shared\DisplayTemplates mappában lévő cshtml-ek a felhasználható alkotóelemek
  - [X] Ezeket több helyről is meg lehet hivatkozni.

### 5. házi feladat
- a login oldal jobboldalának a magyarítása
- Menu, MenuItem és Category többi oldalak magyarítása
- lenyíló másféleképpen: FillAssignableCategories vegyen föl egy adatbázisban nem létező kategóriát, és ez legyen az első elem
- DisplayTemplate használata a Table, Category és a MenuItem nézetekben

### Kérdések
- [DefaultValue] //todo: ez hogy működik?

### 13. feladat
- [X] EditorTemplate: az Edit nézet és a Create nézet ugyanazt előidéző megjelenítő kódját kiemelni egy külön állományba
- [ ] Lenyílómező használata, ha az adatmodell LazyLoading-ot használ (virtual kulcsszó)
  - [X] DisplayTemplate a közös megtekintő nézetekre (Details és Delete)
  - [X] EditorTemplate a közös módosító nézetekre (Create és Edit)
  - [X] A közös nézeteken lenyílómezővel módosítani
        Ha ilyen hibaüzenetet kapunk:
        ```
        System.InvalidOperationException
          HResult=0x80131509
          Message=The ViewData item that has the key 'LocationId' is of type 'System.Int32' but must be of type 'IEnumerable<SelectListItem>'.
          Source=System.Web.Mvc
          StackTrace:
           at System.Web.Mvc.Html.SelectExtensions.GetSelectData(HtmlHelper htmlHelper, String name)
           at System.Web.Mvc.Html.SelectExtensions.SelectInternal(HtmlHelper htmlHelper, ModelMetadata metadata, String optionLabel, String name, IEnumerable`1 selectList, Boolean allowMultiple, IDictionary`2 htmlAttributes)
           at System.Web.Mvc.Html.SelectExtensions.DropDownListFor[TModel,TProperty](HtmlHelper`1 htmlHelper, Expression`1 expression, IEnumerable`1 selectList, String optionLabel, IDictionary`2 htmlAttributes)
           at System.Web.Mvc.Html.SelectExtensions.DropDownListFor[TModel,TProperty](HtmlHelper`1 htmlHelper, Expression`1 expression, IEnumerable`1 selectList, Object htmlAttributes)
           at ASP._Page_Views_Shared_EditorTemplates_Table_cshtml.Execute() in D:\Repos\OopRestaurant201807\OopRestaurant201807\Views\Shared\EditorTemplates\Table.cshtml:line 19
            (...)
       ```
       akkor a hiba oka az, hogy a lenyíló adattartalmát (jelen esetben AssignablesLocations nem inicializáltuk) 


  - [X] A közös nézeteken lenyílómezővel megjeleníteni
  


A validálás különböző lehetséges pontjai: minden alkalommal validáljon az alkalmazás, és ahol lehet, ott védje magát az adatbázis is!

```
  Felület                                              Alkalmazás                                       Adatbázis

+----------------------+                             +-------------------+                            +----------------------+
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |   +--------------------->   |                   |    +-------------------->  |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
|                      |                             |                   |                            |                      |
+----------------------+                             +-------------------+                            +----------------------+

                  ^                                         ^                                             ^
                  |                                         |                                             |
                  |                                         |                                             |
                  +                                         +                                             +

            Validálás                                   Validálás                                      Validálás
```

### 14. feladat
- [ ] hozzunk létre különböző jogosultságcsoportokat
- [ ] hozzunk létre mindegyiknek felhasználót
- [ ] igazítsuk az alkalmazás működését ennek megfelelően

```
+----------------------------------------------------------------------------------------------------------------------+
| Mindenki                                                                                                             |
|                                                                                                                      |
|                                                                                                                      |
|                                        +-------------------------------------------------------------------------+   |
|                                        | Bejelentkezett felhasználók                                             |   |
|                                        |                                                                         |   |
|                                        |                                                                         |   |
|                                        |                                                                         |   |
|                                        |                                                                         |   |
|                                        |                                                                         |   |
|                                        |   +----------------------------+   +------------------+  +------------+ |   |
|                                        |   | Admin                      |   | Pincér           |  | Szakács    | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   | Ő mindent tud              |   | Asztalokat       |  | Menüt      | |   |
|                                        |   |                            |   | mozgathat        |  | írhat      | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   |                            |   |                  |  |            | |   |
|                                        |   +----------------------------+   +------------------+  +------------+ |   |
|                                        |                                                                         |   |
|                                        +-------------------------------------------------------------------------+   |
|                                                                                                                      |
+----------------------------------------------------------------------------------------------------------------------+

```


### 6. házi feladat
- A required bekpacsolása a Table.Location mezőn, adatbázisba írása és a következmények lekezelése mindenhol


### Hibajavítás

Ha ilyen hibába futtok a kód letöltése után:
```
PM> update-database
& : File C:\Users\admin\Repos\OopRestaurant201807\packages\EntityFramework.6.2.0\tools\init.ps1 cannot be loaded because running scripts is disabled on this system. Fo
r more information, see about_Execution_Policies at https:/go.microsoft.com/fwlink/?LinkID=135170.
At line:1 char:45
+ ... rgs+=$_}; & 'C:\Users\admin\Repos\OopRestaurant201807\packages\Entity ...
+                 ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : SecurityError: (:) [], PSSecurityException
    + FullyQualifiedErrorId : UnauthorizedAccess
update-database : The term 'update-database' is not recognized as the name of a cmdlet, function, script file, or operable program. Check the spelling of the name, or 
if a path was included, verify that the path is correct and try again.
At line:1 char:1
+ update-database
+ ~~~~~~~~~~~~~~~
    + CategoryInfo          : ObjectNotFound: (update-database:String) [], CommandNotFoundException
    + FullyQualifiedErrorId : CommandNotFoundException
```

Ez lehet egy megoldás:
```
Set-ExecutionPolicy -Scope CurrentUser Unrestricted
```

Visual Studióból kilépés és belépés, majd rebuild, és update-database.
