# Gemscapes - Match3-Battle Rpg

This is an early version of the game, made with [Unity](https://unity.com) and [Lex](https://github.com/SinyavtsevIlya/Lex).

## Features

### Deterministic Simulation
Every operation in the simulation layer is deterministic. 
Pieces at the same time can have intermediate positions between cells. And even to move with acceleration.
The intermediate position of the piece on the board is represented as `Vector2IntFixed`. 

### View interpolation
The view layer is represented as simple monobehaviors called by the presentation layer when something needs to be displayed.
Presentaion and view can be connected/disconected with a single line of code (which is used for simulation rewinding).
Views interpolates any transition (e.g. movement). So, the movement of the piece will always be uniform, even with unstable fps. 
### Input records
The presentation layer (Presentation Systems) subscribes to the view's callbacks to fire the (input) requst-events.


### Gems acceleration
### continuous input
### Map editor
### Integration tests environment
### Core features
* Multi-directional gravity
* Line matches
* Combat system


## Features in progress

### Matches prediction

https://user-images.githubusercontent.com/43283381/195053753-2c2455fe-4841-45b1-8408-86afa192141d.mp4

