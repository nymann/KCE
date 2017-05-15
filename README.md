# KCE

[![KCE](http://malerpris.dk/output.gif)](https://github.com/nymann/KCE)

KCE is a chess engine that supports the Universal Chess Interface (UCI), and can thereby be used by UCI compatible chess GUI's, like Arena as seen above.

# Search
As of this writing, KCE has the following implemented into it's search function:
  - Itterative deepening
  - NegaMax with Alpha-Beta pruning.
  - Quiscence search


# Evaluation
To evaluate the board, KCE utilizes:
- Piece square stables
- Tapered evaluation for the King.
- King safety

# Move ordering
- Analyzing the best move at previous depth (depth-1) first.


### Extra Features

Besides playing, KCE can also:
- calculate the number of legal moves to x depth given a postion (FEN-string)
- Solve mate problems (given a FEN string)

fx. if the user wanted to see the number of legal moves, he would:
```
> position fen 8/P5R1/r3k3/5N1P/r7/p7/7K/8 w - - 2 2
> perft 3
```
which would give the result 13997. Or by using Divide:
```
> divide 3
```
Which would print out the number of moves given the first move, fx. `h2g2: 519` .. and so forth.

### Building, Installation and Releases.

KCE requires [.NET](https://www.microsoft.com/net) v4.5.2+ to run.

Building KCE can be done using Visual Studio, it's recommended that the binary platform target is x64, but x86 albeit not as strong, works aswell.

If you don't wish to build the source by yourself, look for the latest release on the [release page](https://github.com/nymann/kce/releases) (x64 only).

Installation instructions depends on the chosen UCI Chess GUI, for Arena:
- Engines > Install New Engine > Locate the executable.
