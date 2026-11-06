# G.A.P. - uživatelský manuál
*(Instrukce je v angliètinì také možno zobrazit v menu (**How to play**))*

### Instalace a použití:
Všechny zkompilované soubory  je tøeba mít pohromadì(tak jak jsou v bin/release/netcoreapp3.1/). Pak staèí jen spustit Platformer.exe a spustí se program pøijímající pøíkazy:
  - `help`: zobrazí struèný návod ovládání a principu hry
  - `exit`: ukonèí program
  - `play`: spustí hru

### Menu
Po startu programu nabìhne menu, kde lze zvolit 4 možnosti :
  - **Play** - zaèít hrát
  - **How to play** - krátký návod
  - **Configure keybinds** - nastavení funkcí kláves ve høe
  - **Exit** - ukonèit program

V menu je možné se pohybovat šipkami nahoru a dolù, klávesou `enter` neboli `return` se potvrdí oznaèená možnost (ve speciálních pøípadech text v dolní stranì okna napovídá, co jaká klávesa dìlá).

Dvojitým kliknutím klávesy `escape` se ukonèí program.

### Hra
Po celou dobu hry se hráè nachází v jedné arénì, kde se po dobu 20 úrovní spawnují nepøátelé ve vlnách (nová vlna nepøátel pøijde, jakmile byli všichni nepøátelé z pøedchozí vlny poraženi). Cílem hry je dostat se pøes všech 20 úrovní.

### Pohyb
Jako v každé plošinovce, hráè se mùže horizontálnì pohybovat, standardnì šipkami doleva a doprava.

Hráè mùže skákat, standardnì klávesou `S`, a stejnou klávesou mùže jednou ve vzduchu skoèit znovu. Puštìním klávesy skoku pøed dosažením vrcholné výšky lze zrušit svoji vertikální rychlost.

Stisknutím šipky dolù je možné rychle zvýšit rychlost hráèova pádu.

Stisknutím klávesy `W` lze provést horizontální úhyb na stranu, na kterou se hráèova postava dívá. Bìhem úhybu je hráè nezranitelný nepøáteli.

### Útok
Hráè má k dispozici 2 zpùsoby zranìní nepøátel - støílení (klávesou `D`), zpùsobující 1 bod zranìní, a úhyb (klávesou `W`), kdy pøi kontaktu s nepøítelem zpùsobí úhyb 3  body zranìní (úhybem je také možno zranit více nepøátel najednou).

### Ostatní
Bìhem hry lze stisknout `F1` pro zobrazení dodateèných informací o prùbìhu programu (tato funkce existuje hlavnì kvùli testování).

Ve høe dvojité kliknutí `escape` ukonèí souèasnou hru a vrátí hráèe do menu.

### Konfigurace funkcí kláves
Funkci všech kláves ve høe (kromì `escape`, z dùvodu spolehlivosti) je možné nakonfigurovat v menu (**Configure keybinds**).

### Zdraví
Hráè zaèíná s 5 body zdraví. V souèasné verzi hry každý nepøítel mùže hráèi ubrat jen jeden bod zdraví najednou (po zranìní je hráè krátkou dobu nezranitelný). Každý zabitý nepøítel zároveò uzdraví hráèe o 1 bod zdraví, až na 10 bodù zdraví (ale ne víc).

Hodnì štìstí!

### Nepøátelé
*(poznámka: tato èást není nutná na pochopení hry a pro nìkteré by mohla být považována za spoiler)*

V následujícím poøadí se budou v levelech objevovat noví nepøátelé :

#### Walker
Základní pozemní nepøítel. Pohybuje se rovnou èarou, dokud nenarazí na hranici platformy, pøièemž obrátí svùj smìr a skoèí.

#### Flyer
Jednoduchý létající nepøítel typu útoku na blízko. Snaží se dostat za hráèem, ale jeho chování je trochu nepøedvídatelné, takže pokud jich je více, jejich trajektorie nesplyne v jednu.

#### Artillery
Pokroèilý pozemní nepøítel. Chodí ze strany platformy na druhou, a periodicky støílí 3 padající projektily na hráèovu stranu.

#### ShotgunFly
Jednoduchý létající nepøítel typu útoku na dálku. Jeho pohyb je víceménì náhodný, ale každou chvíli støelí hráèovým smìrem vlnu pomalých projektilù.

#### Snake
Pokroèilý létající nepøítel typu útoku na blízko. Jeho chování je obdobné jako u Flyera, ale zároveò má projektilový "ocas".

#### Bomber
Pokroèilý létající nepøítel typu útoku na dálku. Periodicky najde náhodnou pozici a spawne 2 projektily, pomalou rychlostí je "nabije" a potom je vysokou rychlostí spustí na hráèe. Jeho 2 projektily kroužící kolem nìj také komplikují úhyb zkrz nìj.

#### Spawner
Nepøítel schopný spawnovat nové nepøátele. Periodicky spawnuje Flyery s redukovaným zdravím po dvojicích (i když poèet živých spawnutých flyerù nikdy nepøesáhne 4). Sám má velmi vysoké zdraví.