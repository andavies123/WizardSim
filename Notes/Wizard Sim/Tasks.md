---

kanban-plugin: basic

---

## Card Templates

- [ ] <font color="cyan">**Task Template**</font><br>- [ ] task
- [ ] <font color="red">**Bug Template**</font><br><br>**Problem**<br>Describe Here<br><br>**Fix**<br>Not fixed yet


## Bugs

- [ ] <font color="red">**Bug Template**</font><br><br>**Problem**<br>When opening up the context menu on an interactable then opening up an info window on a different interactable, the context menu does not close<br><br>**Fix**<br>Not fixed yet


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

- [ ] <font color="cyan">**WorldObject Architecture**</font><br><br>Create the architecture for the world objects such as rocks, trees, bushes, etc...
- [ ] <font color="cyan">**Save System**</font><br>- [ ] Add save pause menu button<br>- [ ] Game world should be saved<br>- [ ] Wizard stats should be saved<br>- [ ] Add load main menu button
- [ ] <font color="cyan">**Basic Enemy**</font><br><br>- [ ] Add a basic enemy prefab<br>- [ ] Basic enemy should find the nearest wizard and attack<br>- [ ] Add context menu to tile to spawn an enemy
- [ ] <font color="cyan">**GameObject Pools**</font><br>- [ ] Create an Game Object Pool class<br>- [ ] This class should take in a prefab<br>- [ ] This class should take a max held objects<br>- [ ] This class should be able to supply objects when asked<br>- [ ] This class should be able to return objects to the pool when asked<br>- [ ] Create a Game Object Pool User class<br>- [ ] This class should be able to initialize when necessary<br>- [ ] This class should be able to clean up when necessary
- [ ] <font color="cyan">**Redo Info/Context Menu**</font><br>- [ ] Info/Context Menu should not be incharge of editing the Interactable info


## In Progress

- [ ] <font color="cyan">**Wizard Naming**</font><br><br>- [ ] When a wizard is created, the GameObject name should reflect the name of the wizard


## Completed

**Complete**
- [x] <font color="cyan">**Wizard Spawn Context Menu Item**</font><br><br>- [x] Add context menu item to tiles that will spawn a wizard on that tile<br>- [x] Wizard should be spawned down the correct paths using the wizard spawner
- [x] <font color="cyan">**Info Window**</font><br><br>- [x] Add an Info window when left clicking on an interactable<br>- [x] Window should contain useful info of the interactable<br>- [x] Remove useful data from context menu window<br>- [x] Have info window pop up along with context menu when right clicking interactable<br>- [x] Info window should disappear when clicking on nothing interactable<br>- [x] Show something in the shader to show that an object is selected without the context menu
- [x] <font color="red">**BUG**</font><br><br>**Problem**<br>Pause button is not pausing the game when selected<br><br>**Fix**<br>The unity event for when the button is pressed was removed. I updated it to listen for the event via the code rather than the inspector
- [x] **BUG**<br><br>**PROBLEM**<br>When selecting a wizard context menu option, then right clicking a tile, an error is thrown for a rect transform not found<br><br>**FIX**<br>Removed Content Size Fitter script from a parent of the context menu. Apparently that was all that was needed to fix...
- [x] **BUG**<br><br>**PROBLEM**<br>When the game is paused, I can still interact with interactable objects<br><br>**FIX**<br>The issue was that I moved to using interactables and the interactableRaycaster but the context menu was still using the mouse interaction events which weren't be turned off when the game was paused
- [x] **Canvas Updates**<br><br>- [x] Update all UI States to use their own canvas due to the fact that each canvas updates everything on them
- [x] **Update Object Hovering**<br><br>- [x] Create script that will be in charge of raycasting from the camera<br>- [x] These raycast should hit objects with a certain script on it to say they should be hit<br>- [x] This script on the objects should be in charge of coloring<br>- [x] The raycasting script should be handled from the input state as certain input states shouldn't handle raycasting
- [x] **Wizard Move To Action**<br>- [x] Add a wizard context menu item that will move a wizard to a select spot<br>- [x] Add a Move To wizard state to achieve this on the wizard side<br>- [x] When selecting the context menu item, the player should be able to select a tile on the map to have the wizard move
- [x] **State Machine Editor**<br><br>- [x] Create a base StateMachine Behaviour class<br>- [x] Create an editor class for the StateMachine<br>- [x] Display the current state<br>- [x] Display the current state status
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