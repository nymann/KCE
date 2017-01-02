﻿using System.Collections.Generic;

namespace KCE.BoardRepresentation.MoveGeneration
{
    public class Rook : IMoveGeneration
    {
        private readonly Board _board;
        private readonly int _square;

        public Rook(int square, Board board)
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

        private List<int> WhiteMoveGeneration()
        {
            var validMoves = new List<int>();

            var i = 1;

            while (_board.BoardRepresentation[_square - 10 * i] == 0)
            {
                validMoves.Add(_square - 10 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square - 10 * i] > 6 && _board.BoardRepresentation[_square - 10 * i] < 13)
            {
                validMoves.Add(_square - 10 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square + 10 * i] == 0)
            {
                validMoves.Add(_square + 10 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square + 10 * i] > 6 && _board.BoardRepresentation[_square + 10 * i] < 13)
            {
                validMoves.Add(_square + 10 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square + 1 * i] == 0)
            {
                validMoves.Add(_square + 1 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square + 1 * i] > 6 && _board.BoardRepresentation[_square + 1 * i] < 13)
            {
                validMoves.Add(_square + 1 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square - 1 * i] == 0)
            {
                validMoves.Add(_square - 1 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square - 1 * i] > 6 && _board.BoardRepresentation[_square - 1 * i] < 13)
            {
                validMoves.Add(_square - 1 * i);
            }

            return validMoves;
        }

        private List<int> BlackMoveGeneration()
        {
            var validMoves = new List<int>();

            var i = 1;

            while (_board.BoardRepresentation[_square - 10 * i] == 0)
            {
                validMoves.Add(_square - 10 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square - 10 * i] < 6)
            {
                validMoves.Add(_square - 10 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square + 10 * i] == 0)
            {
                validMoves.Add(_square + 10 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square + 10 * i] < 6)
            {
                validMoves.Add(_square + 10 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square + 1 * i] == 0)
            {
                validMoves.Add(_square + 1 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square + 1 * i] < 6)
            {
                validMoves.Add(_square + 1 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square - 1 * i] == 0)
            {
                validMoves.Add(_square - 1 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square - 1 * i] < 6)
            {
                validMoves.Add(_square - 1 * i);
            }

            return validMoves;
        }
    }
}