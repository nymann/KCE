﻿using System.Collections.Generic;

namespace KCE.BoardRepresentation.MoveGeneration
{
    public class Pawn : IMoveGeneration
    {
        private readonly Board _board;
        private readonly int _square;

        public Pawn(int square, Board board)
        {
            _square = square;
            _board = board;
        }

        public List<int> MoveGeneration()
        {
            return _board.SideToMove
                ? WhiteMoveGeneration()
                : BlackMoveGeneration();
        }

        private List<int> BlackMoveGeneration()
        {
            var validMoves = new List<int>();

            if (_square / 10 == 8 && _board.BoardRepresentation[_square - 10] == 0 && _board.BoardRepresentation[_square - 20] == 0)
            {
                validMoves.Add(_square - 20);
            }

            if (_board.BoardRepresentation[_square - 10] == 0)
            {
                validMoves.Add(_square - 10);
            }

            if (_board.BoardRepresentation[_square - 11] != 0 && _board.BoardRepresentation[_square - 11] < 6)
            {
                validMoves.Add(_square - 11);
            }

            if (_board.BoardRepresentation[_square - 9] != 0 && _board.BoardRepresentation[_square - 9] < 6)
            {
                validMoves.Add(_square - 9);
            }

            return validMoves;
        }

        private List<int> WhiteMoveGeneration()
        {
            var validMoves = new List<int>();

            if (_square / 10 == 3 && _board.BoardRepresentation[_square + 10] == 0 && _board.BoardRepresentation[_square + 20] == 0)
            {
                validMoves.Add(_square + 20);
            }

            if (_board.BoardRepresentation[_square + 10] == 0)
            {
                validMoves.Add(_square + 10);
            }

            if (_board.BoardRepresentation[_square + 11] > 6 && _board.BoardRepresentation[_square + 11] < 13)
            {
                validMoves.Add(_square + 11);
            }

            if (_board.BoardRepresentation[_square + 9] > 6 && _board.BoardRepresentation[_square + 9] < 13)
            {
                validMoves.Add(_square + 9);
            }

            return validMoves;
        }
    }
}