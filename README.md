# John-Conway-s-Game-of-Life

In this project, implement the Game of Life a cellular automaton devised by the British
mathematician John Horton Conway in 1970.

The universe of the Game of Life is an infinite, two-dimensional orthogonal grid of square cells,
each of which is in one of two possible states, alive or dead, (or populated and unpopulated,
respectively). Every cell interacts with its eight neighbours, which are the cells that are
horizontally, vertically, or diagonally adjacent. At each step in time, the following transitions
occur:

      1. Any live cell with fewer than two live neighbors dies, as if by underpopulation.
      2. Any live cell with two or three live neighbors lives on to the next generation.
      3. Any live cell with more than three live neighbors dies, as if by overpopulation.
      4. Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.

The first generation is created by applying
the above rules simultaneously to every cell in the seed; births and deaths occur simultaneously,
and the discrete moment at which this happens is sometimes called a tick. Each generation is a
pure function of the preceding one. The rules continue to be applied repeatedly to create further
generations.

![Screen Shot 2022-10-11 at 4 28 13 PM](https://user-images.githubusercontent.com/83393163/195013968-265ddbf3-a4ce-4bab-977a-dde471440cb0.png)</n>
![Screen Shot 2022-10-11 at 4 28 37 PM](https://user-images.githubusercontent.com/83393163/195013976-cd75aab9-c0cb-475f-a0d7-17547e38b81c.png)
![Screen Shot 2022-10-11 at 4 30 15 PM](https://user-images.githubusercontent.com/83393163/195013990-daf6c022-f355-463a-a235-ae21c8da31f0.png)
