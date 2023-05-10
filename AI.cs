using System;
using System.Collections.Generic;
using System.Text;

namespace B23_Ex02
{
    class AI
    {
        private static Random s_Random = new Random();
        private static int[] s_NumOfOInRows = null;
        private static int[] s_NumOfOInCols = null;

        public static void MakeNextMove(Board i_Board, ref int o_Row, ref int o_Col)
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("*************************");
            msg.AppendLine("AI performs the next move");
            msg.Append("*************************");
            UserInteraction.PrintMessageToConsole(msg.ToString());
            UserInteraction.PrintDotsWhileAIisThinking();
            if (i_Board.NumOfSignsInBoard <= 1)
            {
                initiateArrays(i_Board.Size);
            }

            findOptimalCell(i_Board, ref o_Row, ref o_Col);
        }

        private static void findOptimalCell(Board i_Board, ref int o_Row, ref int o_Col)
        {
            int numOfTotalCells = i_Board.Size * i_Board.Size;
            int numOfSigns = i_Board.NumOfSignsInBoard;
            float densityBoard = (float)numOfSigns / (float)numOfTotalCells;

            if (densityBoard <= 0.2f)
            {
                locateSignAtInnerCell(i_Board, ref o_Row, ref o_Col);
            }
            else if (densityBoard <= 0.7f)
            {
                locateSmartSign(i_Board, ref o_Row, ref o_Col);
            }
            else
            {
                makeAMoveAndDontLoose(i_Board, ref o_Row, ref o_Col);
            }

            s_NumOfOInRows[o_Row]++;
            s_NumOfOInCols[o_Col]++;
        }

        private static void makeAMoveAndDontLoose(Board i_Board, ref int o_Row, ref int o_Col)
        {
            List<(int, int)> emptyCells = takeEmptyCellsInBoard(i_Board);

            if (emptyCells.Count == 1)
            {
                (o_Row, o_Col) = emptyCells[0];
            }
            else
            {
                List<(int, int)> optionalCells = emptyCells;
                removeOptionalCellsFromMainDiagonal(i_Board, optionalCells);
                removeOptionalCellsFromSemiDiagonal(i_Board, optionalCells);

                removeRestOfOptionalCells(i_Board, optionalCells);

                if (optionalCells.Count != 0)
                {
                    (o_Row, o_Col) = optionalCells[s_Random.Next(optionalCells.Count)];
                }
                else // cant locate 'O' without loosing
                {
                    (o_Row, o_Col) = emptyCells[s_Random.Next(optionalCells.Count)];
                }
            }
        }

        private static void removeRestOfOptionalCells(Board i_Board, List<(int, int)> o_OptionalCells)
        {
            int boardSize = i_Board.Size;

            for (int i = 0; i < s_NumOfOInRows.Length; i++)
            {
                if (s_NumOfOInRows[i] == boardSize - 1) // row at risk
                {
                    for (int j = 0; j < s_NumOfOInCols.Length; j++)
                    {
                        if (o_OptionalCells.Contains((i, j)))
                        {
                            o_OptionalCells.Remove((i, j));
                        }
                    }
                }
            }

            for (int i = 0; i < s_NumOfOInCols.Length; i++)
            {
                if (s_NumOfOInCols[i] == boardSize - 1) // col at risk
                {
                    for (int j = 0; j < s_NumOfOInRows.Length; j++)
                    {
                        if (o_OptionalCells.Contains((j, i)))
                        {
                            o_OptionalCells.Remove((j, i));
                        }
                    }
                }
            }
        }

        private static void removeOptionalCellsFromSemiDiagonal(Board i_Board, List<(int, int)> o_OptionalCells)
        {
            int boardSize = i_Board.Size, countOInDiagonal = 0;
            int emptyCellIndex = 0;

            for (int i = 0; i < boardSize; i++)
            {
                if (i_Board[i, boardSize - i - 1] == Board.eXorO.O)
                {
                    countOInDiagonal++;
                }
                else if (i_Board[i, boardSize - i - 1] == null)
                {
                    emptyCellIndex = i;
                }
            }

            if (countOInDiagonal >= boardSize - 1 && o_OptionalCells.Contains((emptyCellIndex, boardSize - emptyCellIndex - 1)))
            {
                o_OptionalCells.Remove((emptyCellIndex, boardSize - emptyCellIndex - 1));
            }
        }

        private static void removeOptionalCellsFromMainDiagonal(Board i_Board, List<(int, int)> o_OptionalCells)
        {
            int boardSize = i_Board.Size, countOInDiagonal = 0;
            int emptyCellIndex = 0;

            for (int i = 0; i < boardSize; i++)
            {
                if (i_Board[i, i] == Board.eXorO.O)
                {
                    countOInDiagonal++;
                }
                else if (i_Board[i, i] == null)
                {
                    emptyCellIndex = i;
                }
            }

            if (countOInDiagonal >= boardSize - 1 && o_OptionalCells.Contains((emptyCellIndex, emptyCellIndex)))
            {
                o_OptionalCells.Remove((emptyCellIndex, emptyCellIndex));
            }
        }

        private static List<(int, int)> takeEmptyCellsInBoard(Board i_Board)
        {
            List<(int, int)> emptyCells = new List<(int, int)>();
            int boardSize = i_Board.Size;

            for(int i = 0; i < boardSize; i++)
            {
                for(int j = 0; j < boardSize; j++)
                {
                    if (i_Board[i, j] == null)
                    {
                        emptyCells.Add((i, j));
                    }
                }
            }
            
            return emptyCells;
        }

        private static void locateSmartSign(Board i_Board, ref int o_Row, ref int o_Col)
        {
            List<int> minRowsDensityOfO = findMinDensityIndices(s_NumOfOInRows);
            List<int> minColsDensityOfO = findMinDensityIndices(s_NumOfOInCols);
            List<(int, int)> optionalCells = new List<(int, int)>();

            foreach (int rowIndex in minRowsDensityOfO)
            {
                foreach (int colIndex in minColsDensityOfO)
                {
                    if (i_Board[rowIndex, colIndex] == null)
                    {
                        optionalCells.Add((rowIndex, colIndex));
                    }
                }
            }

            if (optionalCells.Count != 0)
            {
                (o_Row, o_Col) = optionalCells[s_Random.Next(optionalCells.Count)];
            }
            else
            {
                makeAMoveAndDontLoose(i_Board, ref o_Row, ref o_Col);
            }
        }

        private static List<int> findMinDensityIndices(int[] i_Arr)
        {
            List<int> minDensityIndices = new List<int>();
            int min = Int32.MaxValue;

            for (int i = 0; i < i_Arr.Length; i++)
            {
                if (min > i_Arr[i])
                {
                    min = i_Arr[i];
                }
            }

            for (int i = 0; i < i_Arr.Length; i++)
            {
                if (min == i_Arr[i])
                {
                    minDensityIndices.Add(i);
                }
            }

            return minDensityIndices;
        }

        private static void locateSignAtInnerCell(Board i_Board, ref int o_Row, ref int o_Col)
        {
            List<(int, int)> optionalCells = new List<(int, int)>();
            int boardSize = i_Board.Size;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (isInnerCell(i, j) && i_Board[i, j] == null)
                    {
                        optionalCells.Add((i, j));
                    }
                }
            }

            (o_Row, o_Col) = optionalCells[s_Random.Next(optionalCells.Count)];
        }

        private static bool isInnerCell(int i_RowIndex, int i_ColIndex)
        {
            return i_RowIndex != i_ColIndex && (i_RowIndex + i_ColIndex) != s_NumOfOInRows.Length - 1;
        }

        private static void initiateArrays(int i_Size)
        {
            s_NumOfOInRows = new int[i_Size];
            s_NumOfOInCols = new int[i_Size];
        }

        private static void makeRandomMove(Board i_Board, ref int o_Row, ref int o_Col)
        {
            List<(int, int)> emptyCells = new List<(int, int)>();
            int boardSize = i_Board.Size;

            for (int i = 0; i < boardSize ; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (i_Board[i, j] == null)
                    {
                        emptyCells.Add((i, j));
                    }
                }
            }

            (o_Row, o_Col) = emptyCells[s_Random.Next(emptyCells.Count)];
        }
    }
}
