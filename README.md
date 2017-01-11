# KCE
A chess engine.

Move generator:
In it's current state KCE is able to generate all legalmoves. (Tested via Perft(int depth) and Divide(int depth)). It's not fast at all though.

Search:
Currently using NegaMax with Alpha Beta pruning.
To do, at depth level 0 it should call the quiescence search function instead of returning the heuristic evaluation to prevet the horizon effect problem.

Evaluation:
Piece square tables and material.
