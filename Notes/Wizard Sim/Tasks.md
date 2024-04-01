---

kanban-plugin: basic

---

## Bugs



## Visuals Backlog

- [ ] Wizard 3D Model
- [ ] Bush 3D Model
- [ ] Tree 3D Model
- [ ] Rock 3D Model
- [ ] Grass 3D Model


## Audio Backlog

- [ ] Button Hover Audio
- [ ] Button Click Audio


## Coding Backlog

- [ ] **Multiplayer**<br><br>Set up multiplayer for the new project
- [ ] **WorldObject Architecture**<br><br>Create the architecture for the world objects such as rocks, trees, bushes, etc...
- [ ] **Save System**<br><br>Create a save system that can be saved/loaded at will<br><br>- [ ] Add save pause menu button<br>- [ ] Game world should be saved<br>- [ ] Wizard stats should be saved<br>- [ ] Add load main menu button
- [ ] **Wizard Move To Action**<br>- [ ] Add a tile context menu item that will move the closest wizard to that tile<br>- [ ] Add a Move To wizard state to achieve this on the wizard side


## In Progress

- [ ] **State Machine Editor**<br><br>- [ ] Create a base StateMachine Behaviour class<br>- [ ] Create an editor class for the StateMachine<br>- [ ] Display the current state<br>- [ ] Display the current state status


## Completed

**Complete**
- [x] **Wizard Context Menu Update**<br><br>- [x] Add state machine text to the context menu
- [x] **BUG**<br><br>Unable to open the context menu for tiles or wizards<br><br>**FOUND FIX**<br><br>Adding the rigidbody to the prefabs made the MouseInteractionEvents component not work. Changing the script to not need a collider and moving it to the object that the rigidbody was on was able to fix the issue
- [x] **Wizard Idle State**<br>- [x] Create idle state<br>- [x] Idle state should be default state<br>- [x] Wizard should move to random position<br>- [x] Wizard should wait at position for a time then move again<br>- [x] Add to the wizard context menu to Idle
- [x] **Unity Events**<br><br>Change the way that events are listened to. They should be setup via scripting to avoid disambiguity
- [x] **Interaction Shader**<br><br>Update the shader for interactions to work well with any item<br>- [x] Update shader to work well with any item<br>- [x] Add shader logic to the Context Menu Interactions or Mouse Interactions script<br>- [x] Update existing Tile interaction to separate the logic
- [x] **Random Name Generator**<br><br>Add a random name generator that wizards can call when they are created so all wizards don't share the same name<br>- [x] Generate a random name from a list of first and last names<br>- [x] Read the names from a text file<br>- [x] Implement wizards to use these names
- [x] Wizard Architecture<br><br>Create the architecture for wizards<br>- [x] Wizard Manager<br>- [x] Wizard Prefab<br>- [x] Base Wizard Class<br>- [x] Connect to the context menu<br>- [x] Spawn wizards into the world on startup
- [x] **Tile Context Menu**<br><br>Add a context menu to the tiles when the player right clicks on one.<br>- [x] Add popup UI at mouse position on right click<br>- [x] Menu should always face towards the camera<br>- [x] Add ability to add items to menu<br>- [x] Each menu item should have a following action<br>- [x] Menu should close when an item is clicked<br>- [x] Menu should close when clicking out of it<br>- [x] Tile should change colors when the context menu is open for it<br>- [x] Add a title for the context menu to describe what is selected
- [x] Mouse Tile Selection<br><br>Add ability to get the tile that is being clicked on with the mouse
- [x] **Tile Hover Highlighting**<br><br>The tile that is being hovered over by the player mouse should show that by changing color or something along those lines
- [x] Look into switching over to URP
- [x] **Input Basics**<br><br>Set up the basics for input<br>- [x] Create Input Asset<br>- [x] Create Input class<br>- [x] Update existing inputs
- [x] **BUG**<br><br>Unity takes a while to build and load
- [x] **BUG**<br><br>Camera doesn't snap to clamps at startup
- [x] Create a texture for tiles so it is not just a blank white background
- [x] **Camera Controller**<br><br>The player should be able to have control over the camera.<br>- [x] Move camera position<br>- [x] Free rotate camera<br>- [x] Zoom in/out<br>- [x] Camera clamps
- [x] Game World<br><br>Create a base flat world object group. This would include chunks and the Base WorldGameObject
- [x] **Pause Menu Quit Button**<br><br>The pause menu should have a quit button that will close the current scene and load the Main Menu Scene
- [x] **Pause Menu Play Button**<br><br>The pause menu should have a play button that resumes the game and hides the pause menu
- [x] **Gameplay UI Pause Button**<br><br>The main gameplay screen should have a pause button that will pause the game and open the pause menu
- [x] Add Quit Button to Start Menu that will quit the game when pressed
- [x] Add Play Button to Start Menu that will run the Game Scene when pressed
- [x] Move Obsidian files over to new repo
- [x] Add a Game Scene
- [x] Add a Start Menu Scene
- [x] Create GitHub Repo
- [x] Create new Unity project




%% kanban:settings
```
{"kanban-plugin":"basic"}
```
%%