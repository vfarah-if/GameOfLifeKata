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

## Uniqueish design features
* I used an observer pattern for calculating the the life expectancy as well as to transition the life state in a non chaotic way, and utilised multicast delegates and event handlers to facilitate this.
* I wanted a visual way for representing the test, to make it obvious what was being tested within the life matrix and so I outputed a visual array of +(alive) and -(dead) lifes by position.
* My testing structure is always based on a base class that tests everything generically related to tests like a common constructor or function that may be utilised commonly in all the other tests inheriting from this base. I tend to add helper methods specifically for all tests within this base class, and then every test with a specific scenario is tested under a different class in a format reading nicely with the test explorer, simplifying the text in a BDD type format to focus what is being tested.
* Autofixture is a great way of generating values as well as extending Moq as a dependency injection framework through customizations.
* Fluent assertions lends to making tests more readable, as a natural extension or decorator of what is being tested.
* My domain is fairly simple and easy to read. The pattern has a simple spiral analysis around the life in question, constituting a fair amount of repitition in getting descriptive variables to build the pattern, which may need some refactoring, but because it makes it easy to read I am happy to overlook the repition and happy to leave it like it is for the first version and consider upgrading on a next round of refactoring or enhancements. 
The initialize is only done once, so does not need to be optimised in anyway. The generate utilises the push model to calculate and process the state in two seperate invokes, making it fairly efficient and simple to utilise

## TODO for next version
* Generate a WPF app that will allow simple visualization of the pulsar pattern or a test harness where we can seed any data and automatically run and visualise stuff with an option to show metrics
* Generate statistics built on each generation about how many lifes were created and destroyed
* Generate a way of simulate a run by periods and amount of time
