# 3DGB-2D-Platformer-2

A 2D platformer made with Unity.

## Controls

| Action     | Keyboard                    | Gamepad                  |
|------------|-----------------------------|--------------------------|
| Move left  | `A` / `Left Arrow`          | Left stick               |
| Move right | `D` / `Right Arrow`         | Left stick               |
| Jump       | `Space`                     | Joystick button 3        |

## Features

### Player
- Horizontal movement with sprite flipping based on direction
- Jump (only when grounded), with sound effect
- Idle, run, jump, hit and death animations
- Health system with lives; hit and death sounds
- Controls are frozen on death and when the level ends

### Enemies
- Patrolling enemies that turn around when they hit a wall or obstacle
- Stomp mechanic: jumping on an enemy damages it and bounces the player up
- Touching an enemy from any other side damages the player
- Hit and death animations; enemies despawn after dying

### Collectibles
- Coins that can be picked up, with a pickup sound

### Hazards
- Kill zones that instantly kill the player or enemies falling into them

### Level & Game Flow
- Tilemap-based level
- Finish flag with animation: reaching it completes the level
- Win and lose conditions; the game pauses when the level ends
- Camera following the player
- Background music

### UI
- HUD displaying remaining lives and collected coins
- End screen showing "Level Complete!" or "You Lost!"
