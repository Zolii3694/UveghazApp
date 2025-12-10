Szenzorhálózat szimulációja – Üvegház rendszer
Haladó szoftverfejlesztés – félévi beadandó
Készítők:
Pelády Ádám
Török Zoltán
Dátum: 2025. november. 25
1. Felhasználói dokumentáció
1.1 A feladat megfogalmazása
A beadandó egy üvegház működésének szimulációját valósítja meg C# konzolos alkalmazás formájában. A
szimuláció során több szenzor (hőmérséklet‑, páratartalom‑ és talajnedvesség‑szenzor) véletlenszerűen generált
mérési adatokat állít elő. A rendszer figyeli az adatokat, eseménykezeléssel riaszt, ha a mért értékek az előre
beállított tartományon kívül esnek, és a méréseket automatikusan JSON‑fájlba valamint SQLite adatbázisba menti.
1.2 A bemenet leírása
A felhasználótól nincs szükség közvetlen bemenetre. A program bemenete a szenzorok által generált random
értékek, amelyek reális tartományban (hőmérséklet °C, páratartalom %, talajnedvesség %) mozognak. A
felhasználó a konzolos kimeneten tájékozódhat arról, hogy milyen értékek érkeznek és mikor lépett életbe riasztás
(pl. fűtés, szellőztetés vagy öntözés).
1.3 A várt kimenet
A program futásakor a következők jelennek meg:
● A szenzorok tesztelése értékek monitorozása.
● Szöveges riasztások, tartományon kívüli értékek kezelésének monitorozása
○ (pl. „Hideg van → fűtés bekapcsolva!”).
● Végül a MeresiBlokk.cs-ben megírt ciklust lefuttatjuk és 3 valós értéket elmentünk
● A mérési blokkok SQLite adatbázisban kerülnek tárolásra, soronként: időpont, hőmérséklet, páratartalom,
talajnedvesség. Ez mellett még egy Json fájlba is mentésre kerül az összes érték.
1.4 Kezelési útmutató
1. Indítsd a programot Visual Studio vagy parancssoros futtatás révén.
2. A szenzorok automatikusan elkezdik generálni a mérési adatokat.
3. A konzolon megjelenik minden új mérési blokk és az esetleges riasztások.
4. A program futása végén a mérési adatok JSON‑fájlba és SQLite adatbázisba mentődnek.
5. A program a konzol bezárásával vagy egy billentyű lenyomásával lezárható.
2. Fejlesztői dokumentáció
2.1 A projekt felépítése
● Program.cs – A belépési pont. A tesztciklusok indítása, a főprogram futtatása és a konzol felügyelete.
● Uveghaz.cs – Az üvegház vezérlőosztálya. Példányosítja a szenzorokat, kezeli az eseményeket, gyűjti a
méréseket és hívja az adatbázis‑ illetve JSON‑mentést.
● MeresBlokk.cs - Egy mérési blokk adatszerkezete. Time (DateTime), Homerseklet (double), Paratartalom
(double), Talaj (double).
● AdatbazisKezelo.cs - Gondoskodik az SQLite adatbázis kezeléséről: a táblák létrehozásáról és a mérési
blokkok elmentéséről.
● UveghazSzenzorok.dll - Külső DLL, amely tartalmazza az absztrakt osztályt és a konkrét szenzorok
(hőmérséklet, páratartalom, talajnedvesség) implementációit.
● Linq_Lekerdezesek.dll - Külső DLL amelyben a Linq Lekérdezéseket tartalamzó Lefuttat void metódus van.
Ebben van 3 lekérdezés amik funkció szerint ismertetve vannak a dokumentumban lejjebb.
2.2 Osztályok rövid leírása
Uveghaz.cs
Az üvegház központi modulja. Három szenzort példányosít (hőmérséklet, páratartalom, talajnedvesség) és
feliratkozik az értékváltozás‑eseményekre. Eseménykezelői kiértékelik a mért adatokat, és riasztást generálnak,
ha azok a megadott küszöbértékeken kívül esnek. A mérési ciklus végén összeállít egy objektumot és azt listában
eltárolja, majd JSON‑fájlba és adatbázisba menti.
MeresBlokk.cs
Adatszerkezet, amely egy mérési ciklus összes adatát tartalmazza. Mezői:
● Time: a mérés időpontja;
● Homerseklet: a hőmérő által mért érték;
● Paratartalom: a páratartalom‑szenzor által mért érték;
● Talaj: a talajnedvesség‑szenzor által mért érték.
AdatbazisKezelo.cs
Feladata, hogy létrehozza a szenzoradatok tárolására szolgáló SQLite táblát (Meresek), ha az még nem létezik,
valamint biztosítsa a beszúrást. A Ment() metódus paraméterként kap egy MeresBlokk objektumot, majd az
adatbázisba egy sorként beszúrja a négy mezőt.
Linq_Lekerdezesek(DLL)
A Lefuttat metódus 3 fontos adat projektálást tartalmaz:
- Nem jó értékek projektálása
- Csak a páratartalmat adja vissza
- A napi átlagos hőmérsékletet adja vissza
Szenzor és leszármazottai (DLL)
A Szenzor absztrakt osztály tartalmazza az eseménykezelés keretrendszerét és a szenzorok közös tulajdonságait
(id, név, aktuális érték, mértékegység). A konkrét szenzorok (hőmérséklet, páratartalom, talajnedvesség) az
ErtekOlvasas() metódusukban véletlenszám‑generálással állítanak elő mért adatot, majd eseményt váltanak ki.
2.3 Választott adatszerkezetek
List<MeresBlokk>
A teljes futás során a mérések sorrendben kerülnek ide, ami lehetőséget ad a JSON‑fájl elkészítésére és a
statisztikák (LINQ) lefuttatására.
SQLite adatbázis
A Meresek tábla az Id (autoincrement), Time, Homerseklet, Paratartalom és Talaj mezőkből áll. A struktúrában
egy sor egy mérési ciklus összes adatát tárolja. Az adatbázis előnye, hogy kérdezhető és tartósan tárolja a
méréseket.
JSON fájl
A JSON‑mentésnek köszönhetően a mérési adatok könnyen exportálhatók, más programok által feldolgozhatók. A
fájl egy listát tartalmaz, melyben minden elem megegyezik a MeresBlokk objektum szerkezetével.
