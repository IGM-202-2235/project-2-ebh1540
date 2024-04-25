# Fish Tank Simulation

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Ethan Herbst
-   Section: 01

## Simulation Design

My simulation contains a few kinds of fish that all interact with each other and their environment. One fish will seek out food until it's satisfied (then start when it gets hungry again). The hungry fish will starve if they go too long without food. Another type will school together and get food if it's nearby (otherwise the hungry ones will get it first). And a third type avoids all other fish, but especially the center of the school. The simulation spawns 30 fish in random positions, each with an equal chance to be each type. In testing, I have never come across a run that completely lacks a single type, but it is theoretically possible, except in the case of the hungry fish.

### Controls

- The player will be able to point with the mouse and click drop a piece of food for the food oriented group to seek out. The player can place one piece of food every quarter of a second.

## Avoidant

Large amounts of other fish make this species nervous. It will flee the center of the school of the schooling fish type.

#### Steering Behaviors

- Flee - the center of the school of schooling fish
- Bounds - the fish will attempt to stay in the bounds of the screen
- Wander - the fish will wander around while trying to avoid high traffic areas
- Obstacles - Steers around rocks
- Separation - All other fish, not just avoidant ones

## Schooling

The schooling fish stick together because of strength in numbers.

#### Steering Behaviors

- Cohesion - These fish will seek out the centerpoint of all of their friends
- Seek - The school will also be a little food motivated to make their movement more dynamic, but it'll be lower priority than staying in the school. Unlike the hungry fish, their desire to eat is constant, and stays lower than what the hungry fish default to.
- Bounds - the fish will attempt to stay in the bounds of the screen
- Wander - these fish wander just to make sure the school doesn't get stuck in one spot
- Obstacles - Steers around rocks
- Separation - These fish separate very weakly from other schooling fish to prevent them from congealing and moving together as one

## Hungry

These fish just wanna eat. The longer they go without eating, the stronger their desire (the weight of the force) to seek out food is, and it returns to 0 when they eat before it starts growing again.

#### Steering Behaviors

- Seek - The food that the player places. They only move towards the closest food to them. This force gets stronger the longer the fish has gone without eating. NOTABLY: at very high strengths, this makes the fish ignore obstacles in search of food. They're still avoiding, they're just tunnel visioned on the food.
- Bounds - the fish will attempt to stay in the bounds of the screen
- Wander - in the absence of food and other motivators, these fish will just wander about
- Obstacles - Steers around rocks
- Separation - Separate from all other hungry fish

## Sources

- Fish sprites: [https://agdawkwardgamedev.itch.io/free-fish-assets](https://agdawkwardgamedev.itch.io/free-fish-assets)
- Rocks: [https://freegameassets.blogspot.com/2013/09/asteroids-and-planets-if-you-needed-to.html](https://freegameassets.blogspot.com/2013/09/asteroids-and-planets-if-you-needed-to.html)
- Food (I thought the cheese was funny): [https://freegameassets.blogspot.com/2013/08/blog-post_30.html](https://freegameassets.blogspot.com/2013/08/blog-post_30.html)
- Background: [https://olgas-lab.itch.io/underwater-background](https://olgas-lab.itch.io/underwater-background)

## Make it Your Own

- The hungry fish will die if they go too long without eating. This typically takes 2-3 minutes. The food weight starts at 1 and gains 0.05 to 0.15 every second. When it reaches 15, the fish will starve. However, since it's closer to starving, it will move much, *much* faster whenever food is placed. When the fish eats, the food weight goes back to 0 since the fish won't want to seek out more food while it's full, and it'll start gaining 0.05 to 0.15 per second again.
- Since hungry fish can die, they can also reproduce. Every time one eats, there's a 10% chance that a new hungry fish spawns in the spot that the food was in. Yes, that means asexual reproduction for a fish. Just go with it.
- In order to ensure that the hungry fish doesn't go extinct, one will spawn randomly if there aren't any in the tank. If you let them all die, that's on you, but you can take advantage of the random cloning chance to replenish their population by feeding them well.

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

