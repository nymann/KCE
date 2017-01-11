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

Move: e2e4, Score: 30, Depth: 1, Time: 10 ms. FH: 0, FHF: 0

Move: e2e4, Score: 0, Depth: 2, Time: 14 ms. FH: 18, FHF: 0

Move: e2e4, Score: 30, Depth: 3, Time: 26 ms. FH: 104, FHF: 0

Move: g1f3, Score: -10, Depth: 4, Time: 171 ms. FH: 2355, FHF: 0

Move: e2e4, Score: 110, Depth: 5, Time: 1016 ms. FH: 9055, FHF: 0

Move: e2e4, Score: -75, Depth: 6, Time: 16501 ms. FH: 191974, FHF: 65
