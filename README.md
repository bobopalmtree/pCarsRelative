# Program: pCarsRelative
Author: bobopalmtree (bobopalmtree78 at gmail dot com)

Uses the C# pCars API by MikeyTT (https://bitbucket.org/MikeyTT/pcars-api-demo).

Instalation
- Unzip the files

Requirements
- .NET Framework 4.5
- Project Cars ;)
- Shared Memory must be enabled in Project Cars

Running it (start pCarsRelative.exe)
- Shows the list of participants in a session, sorted by distance drive in this lap.
- You are shown in the middle.
- You can see the cars driving in front and behind you on the track.
- The cars about to lap you or that have lapped you are shown in red.
- The cars you are about to lap or have lapped are shown in blue.

pCarsRelative on top of pCars
- I only have 1 monitor, so I run this app on top of pCars. That's why the window is rather small.
- I run pCars in Windowed Mode.
- I run a app 'Borderless Gaming' that shows pCars fullscreen while in Windowed mode.
- With AlwaysOnTop setting on, this app is then shown on top of pCars en the small windows fits nicely in my HUD/Dashboard.

Settings/Config file (pCarsRelative.exe.config)
- KeepMeAtIndex - Default 4, so you yourself appear at line 5. (4 cars above and 4 below)

- AlwaysOnTop - True = Window stays on top of other windows.
- UpdateFrequency - milliseconds

Columns
- Position
- Name
- Difference in meters between yourself and other cars. This changes to seconds after you set your first valid lap time in that session.
- Number of completed laps

Bugs/limitations/weird stuff
- The list of participants shows up only after you start driving when you enter a session. Before that, pCars does not add you to the list of participants yet.
- Sometimes racers appear without name, sometimes a racers appears double. This is something that happens in pCars too.
- Small screen with only 9 participants. (yeah only 1 screen...)
- ...
