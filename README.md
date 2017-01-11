# KCE

A chess engine.


##Move generator
In it's current state KCE is able to generate all legalmoves. (Tested via Perft(int depth) and Divide(int depth)). It's not fast at all though.


##Search

Currently using NegaMax with Alpha Beta pruning.
To do, at depth level 0 it should call the quiescence search function instead of returning the heuristic evaluation to prevet the horizon effect problem.


##Evaluation

Piece square tables and material.

##Strength of engine

###Start Position

Move: e2e4, Score: 30, Depth: 1, Time: 10 ms. FH: 0, FHF: 0

Move: e2e4, Score: 0, Depth: 2, Time: 14 ms. FH: 18, FHF: 0

Move: e2e4, Score: 30, Depth: 3, Time: 26 ms. FH: 104, FHF: 0

Move: g1f3, Score: -10, Depth: 4, Time: 171 ms. FH: 2355, FHF: 0

Move: e2e4, Score: 110, Depth: 5, Time: 1016 ms. FH: 9055, FHF: 0

Move: e2e4, Score: -75, Depth: 6, Time: 16501 ms. FH: 191974, FHF: 65

###Solving mate problems

As of this writing 13:39 11/01/2017, the engine can solve the following mate problems.

####Mate in 1

4k3/8/4K1R1/8/8/8/8/8 w - - 0 1

Move: g6g2, Score: 525, Depth: 1, Time: 9 ms. FH: 0, FHF: 0

Move: g6g8, Score: 29000, Depth: 2, Time: 12 ms. FH: 11, FHF: 1

###Mate in 2

2bqkbn1/2pppp2/np2N3/r3P1p1/p2N2B1/5Q2/PPPPKPP1/RNB2r2 w - - 0 1

Move: e6d8, Score: 781, Depth: 1, Time: 10 ms. FH: 0, FHF: 0

Move: e6d8, Score: 481, Depth: 2, Time: 16 ms. FH: 38, FHF: 2

Move: e6d8, Score: 596, Depth: 3, Time: 101 ms. FH: 595, FHF: 4

Move: f3f7, Score: 29000, Depth: 4, Time: 256 ms. FH: 2606, FHF: 40

###Mate in 3

r3Bb1r/pQ6/2p2p1p/k3q3/4P3/2N1B3/PP4PP/6K1 w - - 0 1

Move: b7a8, Score: 284, Depth: 1, Time: 11 ms. FH: 0, FHF: 0

Move: b7a8, Score: -26, Depth: 2, Time: 18 ms. FH: 45, FHF: 1

Move: b7a8, Score: 369, Depth: 3, Time: 115 ms. FH: 466, FHF: 3

Move: b7a8, Score: 249, Depth: 4, Time: 825 ms. FH: 9046, FHF: 136

Move: b7a8, Score: 544, Depth: 5, Time: 13474 ms. FH: 83341, FHF: 652

Move: b2b4, Score: 29000, Depth: 6, Time: 52985 ms. FH: 533597, FHF: 9787
