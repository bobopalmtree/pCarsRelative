# pCarsRelative
Program: pCarsRelative
Autheur: bobopalmtree (bobopalmtree78@gmail.com)

Maakt gebruik van de C# pCars API van MikeyTT (https://bitbucket.org/MikeyTT/pcars-api-demo).

Installatie
- Unzip de bestanden

Requirements
- .NET Framework 4.5
- Project Cars ;)
- Shared Memory moet aan staan in pCars

Werking (start pCarsRelative.exe)
- Toont lijst van deelnemers in een sessie gesorteerd per afgelegde afstand in de lap.
- Jijzelf wordt in het midden getoond.
- Je ziet de wagens die voor en achter je rijden op de baan.
- De wagens die je zouden kunnen lappen of dat al gedaan hebben zijn rood.
- De wagens die jij gaat lappen of al hebt gelapt zijn blauw.

pCarsRelative tonen bovenop pCars
- Ik heb maar 1 beeldscherm. Ik draai dit programma bovenop pCars. Vandaar dat het scherm vrij klein is.
- Ik draai pCars in Windowed Mode.
- Ik heb een tooltje 'Borderless Gaming' draaien dat zorgt dat pCars in Windowed mode toch fullscreen draait.
- Door de AlwaysOnTop setting wordt het venster dan bovenop getoond en past het kleine venster mooi in mijn HUD/Dashboard.

Settings/Config file (pCarsRelative.exe.config)
- KeepMeAtIndex - Default 4, zodat jijzelf op positie 5 wordt getoond. (4 wagens boven en 4 onder)
- AlwaysOnTop - True = venster blijft boven andere vensters staan.
- UpdateFrequency - milliseconden

Kolommen
- Positie
- Naam
- Aantal meters verschil tussen jezelf en de respectievelijke wagen
- Aantal compleet afgelegde laps

Bugs/beperkingen/rare dingen
- Als je een sessie start in pCars begint pCarsRelative pas de lijst te tonen nadat je een eerste keer begint te rijden.
- Als je van de ene sessie naar een andere gaat, klopt de sortering van de lijst vaak niet meer. --> herstarten
- Soms verschijnen piloten zonder naam, of staat iemand er dubbel in. Ligt mogelijk gewoon aan pCars, daar zie je dat ook soms.
- Kleuren van de wagens lijkt niet altijd correct te werken.
- Het venster flikkert
- Klein scherm met slechts 9 piloten te zien.
- ...
