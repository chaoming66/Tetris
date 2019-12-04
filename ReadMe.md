### GamePlay Instructions

To start the game, click play button on editor and a "start" button will appear. Click the start button to start the board and pieces will start getting
spawned. Here are the controls:

Up arrow : Rotate the piece
Left arrow : Move the piece toward the left by one
Right arrow : Move the piece toward the right by one
Down arrow : Move the piece doward by one 

There is an editor tool within this project for adding and removing piece data. It's in the "Tools/Piece Data Tool".
To use it, click  "Load Data" button and then you can expand "Pieces Info" list. Each element consists of arrays of Blocks X and Blocks Y, which are 
the data that represents coordinate x and y for every block of each piece. For instance, element 0 from Blocks x will be in pair with same element 
from Blocks Y to represent x and y vlaue of the first block on the piece. If the piece has 5 blocks, it will have 5 elements in bloth Blocks X and Blocks Y.

The piece patterns that are created already are based on this diagram

[![](http://colinfahey.com/tetris/tetris_diagram_pieces_orientations_new.jpg)]

