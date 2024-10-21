# Enemy Types

The game will include multiple types of enemies spanning over multiple difficulty types.

These enemies should have both a counter and a vulnerability to different wizard types so the player doesn't try and use one single wizard type when growing their settlement.

Each of these enemies could have different variants for each of the different wizard types so that they will have those strengths / weaknesses against the different wizard types. 

---

### Slime

A slime can be considered as one of the most basic enemies in the game. It shouldn't take that much damage to take them down. They won't have a lot of attack power and won't attack at a range (for now).

#### Stats

| Name                | Value |
|:--------------------|------:|
| **Difficulty**      |  Easy |
| **Health**          |    25 |
| **Damage**          |     5 |

#### Attributes

- Slow movement speed
- Low attack damage
- Low health
- AI is pretty dumb (falls for traps, doesn't go for who they could take best)
- Can spawn in large groups
- Will contain multiple variants to counter certain wizard types

#### Todo

- [ ] When a slime gets destroyed, it should divide into smaller slimes (should have min size)
- [ ] Moves by hopping from tile to tile
- [ ] Has a close range attack that hops at the wizard to deal damage

#### Ideas

- Divide and Conquer: When a slime is defeated, it splits into two smaller slimes, each with a portion of the original's health.
- Acidic Touch: Deals damage over time to wizards upon contact, simulating a corrosive slime body.
- Sticky Trap: Temporarily slows down or immobilizes a wizard by trapping them in sticky goo.
- Absorption: Gains temporary resistance or health boost by absorbing other slimes.
- Bounce Attack: Leaps towards wizards, causing impact damage and possibly knocking them back.
- Camouflage: Blends into the environment, becoming invisible until attacked or moving.
- Poison Cloud: Releases a poisonous mist upon defeat, causing damage over time to nearby wizards.

---