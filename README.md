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
- [ ] Az ételek szakaszokba rendezése (kategória bevezetése)
  - [X] Létrehozni egy kategória (Category) listát
  - [X] webfelületet gyártani a kategória listához
  - [ ] kitenni a kategóriát a MenuItem-re is

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

### Kérdések
- [ ] hogy lehet az üres adatbázist feltölteni tesztadatokkal?
- [ ] az, hogy a MenuItem.Category kitöltése nem kötelező, ez vajon jó-e nekünk?