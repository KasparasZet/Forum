*Forumas*

1.Sistemos paskirtis

Forumas skirtas klausimų/pastebėjimų/teiginių publikavimui. Jį sudaro (pagal hierarchinį ryšį) 3 taikomosios srities objektai: sritis(skirta grupuoti įrašus) <- įrašas <- komentaras/atsakymas.

2.Funkciniai reikalavimai

Kiekvienai sričiai bus taikomi 4 sąsajos metodai: CRUD(create, read, update, delete) ir kiekvieno objekto atitinkamo sąrašo grąžinimas.

Forume bus 3 galymos rolės, su atitinkamais galimais funkcionalumais: 
  svečias (prisiregistruoti ir peržiūrėti forumo klausimus bei atsakymus), 
  narys (prisijungti/atsijungti, paskelbti/ištrinti/atnaujinti savo įrašą, parašyti komentarą po visais įrašais, savo komentarų ir įrašų nebus galima redaguoti ar šalinti), 
  administratorius (šalinti narius, trinti įrašus ar komentarus, skelbti įrašus atskiroje srityje (forumo administravimo)).

3.Sistemos architektūra

Duomenų bazė - MySql.
Back-End - ASP.NET.
Front-End - HTML.

*Sistemos talpinimui yra naudojamas Azure serveris. Kiekviena sistemos dalis yra diegiama tame pačiame serveryje. Internetinė aplikacija yra pasiekiama per HTTP protokolą. Šios sistemos veikimui yra reikalingas Pharma API, kuris pasiekiamas per aplikacijų programavimo sąsają. Pats Pharma API vykdo duomenų mainus su duomenų baze - tam naudojama ORM sąsaja.* *KINTAMAS VYSTYMO EIGOJE*
