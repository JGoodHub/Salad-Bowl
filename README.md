# Salad-Bowl

Salad Bowl is a fun new mobile game, connect fruits and form links to complete your order and progress to ever more difficult levels.

The project also allows for easy creation of new levels through the following method:

1. Create a Board Layout using the menu item (Create/ScriptableObjects/Board Layout)
2. Fill out the fields of the board layout, setting grid size, seed, prefabs and enabled squares
3. Create a Level Quota object using the menu item (Create/ScriptableObjects/Level Quota)
4. Set which tile colours should be generated for this level
5. Set the number of tiles needed for each quota
6. Set the move limit
7. Set the board layout to be used for this level
8. Add the new level to the levels array present on the GameCoordinator prefab