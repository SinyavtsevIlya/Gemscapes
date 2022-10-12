# Gemscapes - Match3-Battle Rpg

![GemscapesHeader](https://user-images.githubusercontent.com/43283381/195123411-65d11090-3e8d-4590-9925-58bcfa08eb2c.png)

This is an early version of the game, made with [Unity](https://unity.com) and [Lex](https://github.com/SinyavtsevIlya/Lex).
The main goal of this project is to polish the `Lex` in the real production conditions.

## Features

### Deterministic Simulation
Every operation in the simulation layer is deterministic. 
Pieces at the same time can have intermediate positions between cells. And they move with (integer) acceleration.
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
### Continuous input
The game has no concept of input blocking. When the pieces match, the player can make more and more new combinations.
Which makes the game more fun and dynamic.
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

### Runnable scenes
Each scene can be run and tested by anyone in isolation.
If you run say "Wolf" scene, you can just play m3 mechanics and test how they work.
But if you run "Rpg" scene and then choose the wolf as the enemy to fight with, you will experience combat mechanics as well.

### Multiple worlds
Player and it's enemy exist in the outer world. They can do some damage to each other (and later they will also be able to walk, talk and do other rpg stuff)
But the board and pieces are existing in the other m3 world. 
To attack the enemy player of course must match some gems on the board, and this results in a (oneframe) request "AttackReqest" to be created.
Due to the fact, that the board has a link to it's owner, these requests refers directly to the player and it's enemy.
These "bridge" systems are contained within the `Match3ToBattle` Feature, and only exists when the should exist.

### Core features
* Multi-directional gravity
* Line matches
* Combat system

### Application layers
The code structure and relationship may be shown this way:

`Simulation` <--- `Presentation` ---> `View`

* `Simulation` - describes the buisness logic of the game. Represented as a world with its systems and components. Can be updated or rewinded from the outside.
Normally it runs on `fixed update` so every tick has the same `fixed` delta time. Knows nothing about presentation or view.

* `Presentation` - mediates between simulation and view. Represented as systems. It subscribes to the view's callbacks (e.g. buttons.onclicks) for creating (oneframe) request components in the simulation world. And on the other hand, it listen to a simulation changes and calls the view api.

* `View` - handles the view logic. Represented by plain monobehaviors that has zero dependencies. They only have a public api to be called when something need to be displayed. Runs in "Update" with a varios delta time. Interpolates itself.

## Features in progress

### Matches prediction

## Dependencies

* C#7.3 or higher
* Unity 2021.3.3f1 ()
* Lex (upm)
* DOTween (upm)

## How to play

* Open unity project using a proper `Unity` version
* Open `Rpg` scene
* Enjoy the game

## Feedback
if you have any ideas or questions feel free to use github issues/discussions or message me on discord: `Ilya Sinyavtsev#6472`.
