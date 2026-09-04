# Interred

A first-person atmospheric horror game built with **Unity 6**. Navigate a fog-laden graveyard, balance sight and sound, and survive against two distinct entity behaviors.

* **Play / Download:** [Interred on itch.io](https://phymoss.itch.io/interred)

## Dual-AI Entity Systems

The core gameplay relies on counterbalancing two complementary monster archetypes:

- **1. The Gaze-Bound Stalker (SCP-173 Mechanic):**
  - **Vision Line-of-Sight:** Uses camera frustum checking paired with physics raycasts to verify if the entity is within the player's direct field of view.
  - **Dynamic State:** Freezes in place while observed; instantly triggers fast NavMesh pursuit the moment visual contact breaks.

- **2. The Sound-Sensitive Hunter:**
  - **Acoustic Detection Radius:** Features an auditory awareness system where sprinting or unstealthed movement emits noise events.
  - **Investigation & Pursuit:** Automatically calculates pathfinding toward the noise source, forcing the player to manage movement speed while avoiding the gaze-bound entity.

## Additional Technical Features
- **URP Shaders & Retro Aesthetic:** Custom Universal Render Pipeline pixelation and dithering post-processing passes.
- **Environment & Puzzles:** State machine-driven gate/key puzzle mechanics across two distinct graveyard zones.

## Tech Stack
- **Engine:** Unity 6
- **Language:** C#
- **Render Pipeline:** Universal Render Pipeline (URP)
