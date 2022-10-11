# Gemscapes - Match3-Battle Rpg

![GemscapesHeader](https://user-images.githubusercontent.com/43283381/195123411-65d11090-3e8d-4590-9925-58bcfa08eb2c.png)

This is an early version of the game, made with [Unity](https://unity.com) and [Lex](https://github.com/SinyavtsevIlya/Lex).
## Features

### Deterministic Simulation
Every operation in the simulation layer is deterministic. 
Pieces at the same time can have intermediate positions between cells. And even to move with (integer) acceleration.
The intermediate position of the piece on the board is represented as `Vector2IntFixed`. 
### View interpolation
The view layer is represented as simple monobehaviors called by the presentation layer when something needs to be displayed.
Presentaion and view can be connected/disconected with a single line of code (which is used for simulation rewinding).
Views interpolates any transition (e.g. movement). So, the movement of the piece will always be uniform, even with unstable fps. 

https://user-images.githubusercontent.com/43283381/195053753-2c2455fe-4841-45b1-8408-86afa192141d.mp4

### Input records
The presentation layer (Presentation Systems) subscribes to the view's callbacks to fire the (input) requst-components.
All these input events are recorded in the game session record. This record can be "replayed" in the editor as "show-only" mode.
In case of any exception caught on a real device, a game record is written to the log.
This allows the developer to jump right into the exception and fix it, without trying to manually reproduce it.
### Gems acceleration
Pieces accelerate as they fall, which improves look and feel of the game.
### Continuous input
The game has no concept of input blocking. When the pieces match, the player can make more and more new combinations.
### Map editor
Each enemy in the game has its own match3 level. Levels are presented as scenes. Each tile contains entity authorings that determine which behaviors the piece will have. Maybe it's a gem, or an obstacle - or a bomb. Or maybe both at the same time.
### Integration tests environment
The game has a set of integration tests and helper-classes for their creation:
```csharp
[Test]
public void TestGravityFall()
{
    // arrange: create a testing startup, containing only simulation inside it
    var m3 = new TestMatch3Startup(pattern:
    @"
    --1-
    11-1
    ");
    
    // act: tick it's world until all the pieces fall
    m3.TickUntilIdle();

    // assert: that pattern is expected 
    Assert.AreEqual(expected:
    @"
    ----
    1111
    ", m3.ToString());
}
```
### Core features
* Multi-directional gravity
* Line matches
* Combat system

## Features in progress

### Matches prediction

