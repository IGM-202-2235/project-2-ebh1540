# Fish Tank Simulation

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Ethan Herbst
-   Section: 01

## Simulation Design

I plan for my simulation to be a few kinds of fish (taken from the sprites in the source). Each of them will interact differently. This is subject to change, but my current idea is for one type to avoid another, one type to seek out food (player controls), and one type to school (cohesion) though this one I think will probably need another behavior.

### Controls

- The player will be able to point with the mouse and click drop a piece of food for the food oriented group to seek out.

## Avoidant

Large amounts of other fish make this species nervous. It will flee the center of the school of the schooling fish type.

#### Steering Behaviors

- Flee - the center of the school of schooling fish
- Separation - All other avoidant type fish 

## Schooling

The schooling fish stick together because of strength in numbers.

#### Steering Behaviors

- Cohesion - These fish will seek out the centerpoint of all of their friends
- Seek - The school will also be a little food motivated to make their movement more dynamic, but it'll be lower priority than staying in the school

## Food Motivated

These fish just wanna eat.

#### Steering Behaviors

- Seek - The food that the player places. Otherwise, this fish will just wander
- Seperation - Separate from all other food motivated fish

## Sources

- Fish sprites: [https://agdawkwardgamedev.itch.io/free-fish-assets](https://agdawkwardgamedev.itch.io/free-fish-assets)

## Make it Your Own

- I'm not entirely sure how I'll differentiate yet, but I'm hoping to add more behaviors than just the ones listed.
- Inspired by the example in the project writeup, the food motivated fish will likely slow down the longer they've gone without eating. This could be collective or based on the actual fish, I'll have to balance how many food motivated fish spawn with how easy it is to keep them fed.

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

