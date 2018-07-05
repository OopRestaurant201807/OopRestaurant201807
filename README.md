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
- [ ] mikor hozza létre?
- [ ] hova hozza létre?
- [ ] saját adatokat is el lehet benne helyezni?


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
 