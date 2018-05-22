# GameOfLifeKata
The Game of Life, also known simply as Life, is a cellular automaton devised by the British mathematician John Horton Conway in 1970.  [See wikipedia for more info](https://en.wikipedia.org/wiki/Conway_Game_of_Life).

The "game" is a zero-player game, meaning that its evolution is determined by its initial state, requiring no further input. One interacts with the Game of Life by creating an initial configuration and observing how it evolves, or, for advanced "players", by creating patterns with particular properties. This is a simplified version allowing 3 distinct seeds, a diamond shape, a square, and a cross shape.

The universe of the Game of Life is an infinite two-dimensional orthogonal grid of square cells, each of which is in one of two possible states, alive or dead. Every cell interacts with its *eight neighbours*, which are the cells that are horizontally, vertically, or diagonally adjacent. 

## Rules of the system:
1. Any live cell with *fewer than two live neighbours dies*, as if caused by **underpopulation**.
2. Any live cell with *two or three live neighbours lives* on to the **next generation**.
3. Any live cell with *more than three live neighbours dies*, as if by **overpopulation**.
4. Any dead cell with *exactly three live neighbours becomes a live cell*, as if by **reproduction**.

The initial pattern constitutes the **seed** of the system. The first generation is created by applying the above rules simultaneously to every cell in the seed—births and deaths occur simultaneously, and the discrete moment at which this happens is sometimes called a **tick** (in other words, each generation is a pure function of the preceding one). The rules continue to be applied repeatedly to create further generations.

*Conway chose his rules carefully*, after considerable experimentation, to meet these criteria:
1. There should be no **explosive growth**.
2. There should exist **small initial patterns** with chaotic, unpredictable outcomes.
3. There should be potential for von Neumann universal constructors.

The rules should be as simple as possible, whilst adhering to the above constraints.
