---

kanban-plugin: basic

---

## <font color="lightblue">**Card Templates**</font>

- [ ] <font color="orange">**Visuals Template**</font>
- [ ] <font color="violet">**Documentation Template**</font>
- [ ] <font color="lightgreen">**Audio Template**</font>
- [ ] <font color="cyan">**Task Template**</font>
- [ ] <font color="red">**Bug Template**</font>


## <font color="red">**Bugs**</font>

- [ ] <font color="red">**Always Zooming**</font><br><br>Whenever the scroll wheel is used, the camera always zooms in no matter where the mouse is. When trying to scroll while on an other window, the camera still zooms in/out
- [ ] <font color="red">**Wizard Spawn Task Issue**</font><br><br>When a task exists, and a wizard spawns, the task gets assigned to the wizard but the wizard doesn't change its state to handle the task


## Documentation

- [ ] <font color="violet">**Add a diagram for the UIManager**</font>


## <font color="orange">**Visuals Backlog**</font>

- [ ] <font color="orange">**Wizard 3D Model**</font>
- [ ] <font color="orange">**Enemy 3D Model**</font>
- [ ] <font color="orange">**Bush 3D Model**</font>
- [ ] <font color="orange">**Tree 3D Model**</font>
- [ ] <font color="orange">**Rock 3D Model**</font>
- [ ] <font color="orange">**Town Hall 3D Model**</font>
- [ ] <font color="orange">**Placement Mode Visual**</font>
- [ ] <font color="orange">**Settings Menu**</font><br><br>- [ ] Add UI for pause menu settings menu<br>- [ ] Add button on pause menu to go to the settings menu<br>- [ ] Add back button to go back to main pause menu<br>- [ ] Add tabbed area for different types of settings


## <font color="lightgreen">**Audio Backlog**</font>

- [ ] <font color="lightgreen">**Button Hover Audio**</font>
- [ ] <font color="lightgreen">**Button Click Audio**</font>


## <font color="cyan">**Coding Backlog**</font>

- [ ] <font color="cyan">**Save System**</font><br>- [ ] Add save pause menu button<br>- [ ] Game world should be saved<br>- [ ] Wizard stats should be saved<br>- [ ] Add load main menu button
- [ ] <font color="cyan">**Settings Menu**</font><br>- [ ] Wire up buttons for settings menu<br>- [ ] Wire up pause menu settings button
- [ ] <font color="cyan">**Pathing**</font><br><br>- [ ] Implement pathing for the movement component<br>- [ ] When using Move To context menu item, it should be able to path to the position<br>- [ ] Implement NavMesh<br>- [ ] WorldObjects should be blockers


## In Progress

- [ ] <font color="cyan">**Redo Context Menu**</font><br><br>- [ ] When right clicking on an interactable to open the context menu the context menu should open next to the item that is being clicked on<br>- [ ] Context menu should have multiple levels that open up to the side of the selected context menu option<br>- [ ] If there are multiple levels, an arrow should show on the label<br>- [ ] If opening a context menu too close to one side of the screen, it should open up the other way
- [ ] <font color="cyan">**Moving**</font><br><br>- [ ] Moving to a new spot should place them in the center of a tile


## Completed

**Complete**
- [x] <font color="cyan">**Map Item Destruction**</font><br><br>- [x] Should contain health<br>- [x] When damaging, it should damage the health<br>- [x] If the item hasn't been touched in a while, the health should slowly heal to full health to avoid having half broken rocks/trees everywhere
- [x] <font color="red">**Context Menu Click Throughs**</font><br><br>When clicking on a context menu item and an interactable is underneath, the interactable is selected instead of the context menu item
- [x] <font color="cyan">**Wizard Color by Type**</font><br><br>- [x] The player should be able to tell a wizard by their type
- [x] <font color="cyan">**Wizard Task System**</font><br><br>- [x] Set up the architecture for wizards to have a task system<br>- [x] Wizards should be assigned a single task to work on<br>- [x] Tasks should have a priority<br>- [x] Wizards should have ability to have multiple tasks<br>- [x] If wizards don't have a task, they should idle<br>- [x] User should be able to see current task of a wizard<br>- [x] User should be able to see task list<br>- [x] User should be able to remove a task from a wizards list<br>- [x] User should be able to rearrange a task in the wizard list<br>- [x] Right clicking on a rock and hitting destroy should count as a task and a wizard should be summoned
- [x] <font color="red">**Floating Health Bar Bug**</font><br><br>The floating healthbar for wizards/enemies do not always face the camera
- [x] <font color="red">**Interactable Hover Bug**</font><br><br>When in the pause menu, interactables are still highlighted even though they shouldn't be
- [x] <font color="cyan">**Hot Bar**</font><br>- [x] When a button is pressed the user should be able to place an item on the map<br>- [x] A preview should be shown when placing<br>- [x] The object should be added to the the world collection
- [x] <font color="orange">**Hot Bar UI**</font><br>- [x] UI should contain a horizontal layout to hold different buttons for placing objects on the ground<br>- [x] UI should contain clickable buttons<br>- [x] UI should be set up to easily add new items
- [x] <font color="cyan">**Context Menu Item Validation**</font><br>- [x] Context menu items should have a validation on whether or not they should show up in the menu<br>- [x] Items that are invalid don't show up in the menu on open<br>- [x] Items that are valid should show up in the menu on open
- [x] <font color="cyan">**Context Menu User**</font><br>- [x] Update the way context menu user scripts work<br>- [x] Should be more generic if possible
- [x] <font color="cyan">**World Generators**</font><br>- [x] World generators should have their own classes<br>- [x] These classes could be private and held inside of the WorldBuilder to save space
- [x] <font color="cyan">**WorldObject Architecture**</font><br><br>- [x] Create the architecture for world objects such as rocks, trees, bushes<br>- [x] Add tile context menu items to place these objects for testing
- [x] <font color="red">**Bug Template**</font><br><br>**Problem**<br>When opening up the context menu on an interactable then opening up an info window on a different interactable, the context menu does not close<br><br>**Fix**<br>Updated game state manager
- [x] <font color="cyan">**Redo Info/Context Menu**</font><br>- [x] Info/Context Menu should not be incharge of editing the Interactable info
- [x] <font color="cyan">**Game State**</font><br>- [x] Create a game state object<br>- [x] Game state should combine input states and ui states<br>- [x] Create a game state manager class<br>- [x] Have each game state have their own ui state machine and input state machine
- [x] <font color="cyan">**UI Manager Updates**</font><br>- [x] Make UI Manager a Singleton<br>- [x] Have a public CurrentUIState or something like that
- [x] <font color="cyan">**Refactoring Components**</font><br>- [x] Look into a way to not have so many "WizardXYZ" and "EnemyXYZ" scripts
- [x] <font color="cyan">**Basic Enemy**</font><br><br>- [x] Add a basic enemy prefab<br>- [x] Add context menu to tile to spawn an enemy
- [x] <font color="red">**Info Window wizard health not updating**</font><br><br>**Problem**<br>When selecting a wizard, the info window will show the current health. When the wizards health changes, the info window does not update.<br><br>**Fix**<br>The interactable info text is only set once on startup, it doesn't update. Added event listeners when a wizards health changes
- [x] <font color="cyan">**Health Context Menu Items**</font><br>- [x] Add a heal 10% menu item<br>- [x] Add a hurt 10% menu item<br>- [x] Add a Full Heal menu item<br>- [x] Add a Full Hurt menu item
- [x] <font color="orange">**Ring Health Bar**</font>
- [x] <font color=cyan>**Health Bar UI**</font><br>- [x] Create a UI for wizards health<br>- [x] Should only display on mouse hover<br>- [x] Follows around the health gameobject<br>- [x] Health bar should be circular to save space<br>- [x] Add health to the info window<br>- [x] Update the color based on the health
- [x] <font color="cyan">**Health Script**</font><br>- [x] Create a health script<br>- [x] Health should be able to heal<br>- [x] Health should be able to go down<br>- [x] Event should be raised when health reaches 0<br>- [x] Event should be raised when health reaches 100%<br>- [x] See if it would make sense to keep it in its own assembly definition
- [x] <font color="cyan">**Update Rider**</font><br>- [x] Download<br>- [x] Install
- [x] <font color="cyan">**World Position Helpers**</font><br>- [x] Add methods to help get positions based on a given tile<br>- [x] Add a world generation details scriptable object that holds helpful information
- [x] <font color="cyan">**GameObject Pools**</font><br>- [x] Create an Game Object Pool class<br>- [x] This class should take in a prefab<br>- [x] This class should take a max held objects<br>- [x] This class should be able to supply objects when asked<br>- [x] This class should be able to return objects to the pool when asked<br>- [x] Add assembly definition to folder
- [x] <font color="cyan">**Wizard Naming**</font><br><br>- [x] When a wizard is created, the GameObject name should reflect the name of the wizard
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