# KCE

A UCI chess engine.


##Move generator
See how many moves are possible at a certain depth, example at depth 3:


position fen 8/P5R1/r3k3/5N1P/r7/p7/7K/8 w - - 2 2


perft 3

> 13997.

Or by:

divide 3

h2g2: 519

h2g1: 485

h2h1: 464

h2h3: 468

h2g3: 440

h5h6: 482

f5g3: 407

f5e3: 490

f5h4: 453

f5d4: 147

f5e7: 418

f5d6: 505

f5h6: 436

g7g6: 152

g7g5: 444

g7g4: 564

g7g3: 559

g7g2: 556

g7g1: 574

g7g8: 594

g7f7: 406

g7e7: 83

g7d7: 496

g7c7: 509

g7b7: 509

g7h7: 432

a7a8q: 704

a7a8r: 636

a7a8b: 554

a7a8n: 511


Total moves: 30, Total nodes: 13997

##Search
Itterative deepening.

NegaMax with Alpha Beta pruning.

Quiscence search.


Move ordering:

Currently the only move ordering used is looking at the best move at last depth.


##Evaluation
Material.

Piece square tables

Tapered evaluation king piece square table.

###Solving mate problems

####Mate in 1

position fen 4k3/8/4K1R1/8/8/8/8/8 w - - 0 1

go infinite

info depth 1 nodes 1 time 19 score cp 570

info currmove g6g7

info depth 2 nodes 17 time 24 score mate 1

bestmove g6g8

info depth 1 nodes 1 time 20 score cp 570

info currmove g6g7

info depth 2 nodes 17 time 24 score mate 14500

bestmove g6g8

###Mate in 2

position fen 2bqkbn1/2pppp2/np2N3/r3P1p1/p2N2B1/5Q2/PPPPKPP1/RNB2r2 w - - 0 1

go infinite

info depth 1 nodes 1 time 24 score cp 810

info currmove e6d8

info depth 2 nodes 45 time 30 score cp 515

info currmove e6d8

info depth 3 nodes 178 time 35 score cp 655

info currmove e6d8

info depth 4 nodes 1429 time 60 score mate 2

bestmove f3f7

###Mate in 3

position fen r3Bb1r/pQ6/2p2p1p/k3q3/4P3/2N1B3/PP4PP/6K1 w - - 0 1

go infinite

info depth 1 nodes 1 time 24 score cp 310

info currmove b7a8

info depth 2 nodes 51 time 31 score cp 0

info currmove b7a8

info depth 3 nodes 313 time 41 score cp 405

info currmove b7a8

info depth 4 nodes 5757 time 182 score cp 295

info currmove b7a8

info depth 5 nodes 30370 time 1098 score cp 560

info currmove b7a8

info depth 6 nodes 301453 time 8121 score mate 3

bestmove b2b4
