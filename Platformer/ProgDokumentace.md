## Název projektu : Platformer

## Zadání:
Plošinová hra s hráèem se schopnostmi alespoò : { pohyb, skok, dvojitý skok, superskok, úhyb (a zároveò útok na blízko), støílení }, alespoò 5 rùzných druhù nepøátel (a možná nìkolika jejich variací) a postupnì se mìnící arénu vèetnì spawnù nepøátel po dobu 20 až 50 levelù.

Použitá knihovna grafiky: SFML.

## Instalace a použití:
Všechny zkompilované soubory  je tøeba mít pohromadì(tak jak jsou v bin/release/netcoreapp3.1/). Pak staèí jen spustit Platformer.exe a spustí se program pøijímající pøíkazy:
  - `help`: zobrazí struèný návod ovládání a principu hry
  - `exit`: ukonèí program
  - `play`: spustí hru

## Program:

### GameFrame
Základem hry je tøída **GameFrame**, která se stará o pravidelné updatování jejích modulù - všechny tøídy , o které se stará (splòující rozhraní **IModule**), pracují na bázi updatování. V dobì psaní dokumentace je v programu využívána její zdìdìná tøída **Platformer**, která má 4 tøídy splòující rozhraní **IModule** :
  - **PlayerControls** : stará se o vstup hráèe na klávesnici. Každý update zachytí souèasný stav klávesnice a podle nìj upraví vlastnosti uložených klíèù.
  - **TextUserInterface** : pøijímá informace ze tøídy GameObjectHandler a GameFramu samotného a zobrazuje je na obrazovku.
  - **GameObjectHandler** : hlavní položka hry starající se o stav všech (herních) objektù ve høe.
  - **MenuHandler** : tøída starající se o funkci menu v programu.

Metodou **Run** si GameFrame nastaví hodiny a podle pøedem urèeného framerate bude volat update na všech jeho modulech, dokud bude okno hry otevøené.

### IModule
Rozhraní IModule je navrženo tak, aby s ním mohl pracovat GameFrame. Proto obsahuje 3 metody:
  - **Load** : Volá se na zaèátku metody Run a je urèena pro naètení všech potøebných dat
  - **Update** : Volá se jednou za frame, je pro aktualizaci potøebných dat
  - **Draw** : Volá se jednou za frame, je pro zobrazení dat na obrazovku 
  *(poznámka : zatím není navrženo vícevrstvé zobrazování, tak pro zabránìní pøípadnému pøekrytí je tøeba umístit pøíslušné moduly do správného poøadí)*

### MenuHandler
Tøída MenuHandler obsluhuje jednotlivé "stránky" (v kódu nazývané "slidy") menu a interakci s nimi. Všechny stránky musí splòovat rozhraní **IMenuSlide** ( obsahující metodu **Draw** pro zobrazování stránky a 3 metody volané pøi stisknutí kláves odpovídajících pøíkazùm **Up**, **Down** a **Confirm**).

Pomocí zásobníku stránky fungují tak, že pøi naètení nìjaké další stránky zùstává stará stránka naètená (aby zásobník fungoval spolehlivì, mìl by graf pøístupu stránek tvoøit strom).

Protože vstup v menu má  mnohem jednodušší formu, MenuHandler nepoužívá PlayerControls, a tedy na rozdíl od ovládání ve høe ovládání v menu není konfigurovatelné.

### GameObjectHandler

Tøída GameObjectHandler se stará o všechny procesy na hracím poli, tedy interakce hráèovy postavy s nepøáteli, projektily a prostøedím. Souèasnì k tomu má 4 Listy objektù daných typù: 
- **Platforms** (StationaryObject)
- **Effects** (Effect)
- **Enemies** (EnemyObject)
- **Bullets** (BulletObject)

Zároveò má promìnnou **Player**, kterou se reprezentuje postava hráèe. V metodì **Update** dochází k interakci mezi tìmito listy (napøíklad kontrola kolize s podlahou mezi enemies a platforms), a pro objekty ve všech listech kromì platform dochází i k samostatné aktualizaci (v kódu metoda **GameObject.Action**). Na konci metody Update se projdou všechny listy (kromì platforms), a pokud metoda ShouldDie nìjakého objektu vrátí true, objekt se smaže.

Pro naèítání levelù GameObjectHandler obsahuje tøídu **MapLoader**. Tato tøída rozhoduje, kdy se má naèíst další level, a také toto naèítání provádí.
*(poznámka: pro testovací úèely lze doèasnì zmìnit promìnou MapLoaderu **nextLevelNumber** na vyšší èíslo, aby se zaèlo hrát od vyššího levelu)*

Stav hry je uchováván v promìnné **state**, která mùže být buï výhra, prohra, nebo hra (stále se hraje).

### GameObject
Všechny objekty v Platformeru mají tzv. *axis-aligned-bounding-box* (AABB) hitbox, tzn. jediný tvar objektù, se kterými mùže hra pracovat, je obdélníkový a pouze s vodorovnými a svislými hranami. Tento systém umožòuje jednoduchou a rychlou práci s nimi.

Objekt mùže nebo nemusí mít stejný tvar fyzický jako vizuální (to se dá rozlišit podle promìnné **UsesVisualBox**).

Dùležitá promìnná je promìnná Group, podle které se objekt mùže chovat jinak (tj. Group = Falling znamená, že GameObjectHandler bude tomuto objektu pøipoèítávat gravitaci k pohybu a kontrolovat kolizi s podlahou)

Metody tøídy GameObject (a všech od ní zdìdìných):
- **Draw** - volá se automaticky GameObjectHandlerem, vrátí obraz objektu na jeho místì ve høe
- **Collides** - statická metoda urèující, zda se 2 objekty pøekrývají
- **CheckFloorCollision** - statická metoda, která vrací true, pokud je objekt na platformì, a pokud ano, také tento objekt pøesune nad ni (simulace pøistání na platformì)

Následují tøídy odvozené od tøídy GameObject :

#### StationaryObject : GameObject
Používá se pro plošiny ve høe, je automaticky generován.

#### ActiveObject : GameObject
Objekt schopný èinit vlastní akce, obsahuje k tomu urèené metody :
- **Action** - virtuální metoda, kterou lze pøepsat pro rùzné chování objektu
- **ShouldDie** - virtuální metoda, urèuje, za jakých podmínek má být objekt odstranìn (tehdy kdy vrací true)
- **OnDeath** - tato virtuální metoda se zavolá tìsnì pøed odstranìním objektu

#### Effect : ActiveObject
Fyzicky neinteraguje s ostatními objekty a existuje hlavnì z vizuálních dùvodù.

#### MovingObject : ActiveObject
Objekt, který je urèen k pohybu. Pøidané metody:
- **Move** - automaticky volaná metoda, která aplikuje souèasný pohyb hráèe na jeho pozici
- **OnCollision** - virtuální metoda s parametrem damage, volá se, když objekt dostal ránu, urèuje, co se s ním v této situaci stane
- **TouchedGround, LeftGround** - 2 virtuální metody volané, když objekt pøistál na platformì nebo ji opustil
- **OnEdge** - urèuje, zda je objekt na hranici platformy


MovingObject má navíc promìnné **health** a **OutOfBounds**, které ovlivòují pøepsanou ShouldDie (vrací true pokud `health <= 0 || OutOfBounds == true` ).

#### BulletObject : MovingObject
Má stejné promìnné i metody jako **MovingObject** a jednu pøidanou metodu **FloorCollision** volanou GameHandlerem, která pøi kolizi s podlahou nastaví health objektu na 0.

#### PlayerObject : MovingObject
O vìtšinu promìnných PlayerObjectu se stará tøída **PlayerData**, kterou vidí tøída **TextUserInterface** pro zobrazování dat.

Protože hráè je už celkem komplikovaná tøída, místo konstruktoru se používá statická metoda **MakePlayerObject**.

#### EnemyObject : MovingObject
Metody Action a OnDeath EnemyObject pøedává své tøídì EnemyAI. Dùležitá promìnná je **damage**, která urèuje, kolik poškození nepøítel hráèi zpùsobí.

#### EnemySpawnClass
Statická tøída obsahující všechny statické metody pro spawnování nových nepøátel (**SpawnWalker**, **SpawnFlyer**, ...).

### EnemyAI
Je používáno pouze tøídou EnemyObject, ovládá jeho chování (obsahuje metody Action a OnDeath, které pøebírá od svého objektu). Pro AI jednotlivých nepøátel se vytvoøí odvozená tøída se stejným rozhraním (tedy od **EnemyAI** je **WalkerAI**, **FlyerAI**, ...).

## Ostatní (poznámky o vývoji) :

### Chybìjící featury :
Hra neobsahuje zvuk. To je z hlediska kvality hry asi nejzávažnìjší problém, následovaný prací s animacemi. Ovšem, tento projekt vznikl jako zápoètový program pro pøedmìt programování, takže jsem na to nedával velký dùraz. Hra by se samozøejmì mohla dále rozšiøovat, ale to by šlo dìlat celé léto, a jako zápoètový program je podle mì jeho souèasný stav dostateèný.

### Vlastní zhodnocení programu
Práce se ukázala být zdlouhavìjší a pracnìjší, než jsem oèekával, nicménì jsem rád, že jsem si jednou vyzkoušel udìlat game engine od zaèátku. Asi by se ještì hodilo podívat se, jak vlastní game enginy udìlali ostatní, abych mìl, pøedstavu, jak jsem si vlastnì vedl.