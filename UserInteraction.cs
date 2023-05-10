using System;
using System.Text;
using System.Threading;

namespace B23_Ex02
{
    class UserInteraction
    {
        public static void PrintStatistics(int i_Player1Score, int i_Player2Score)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("The game has ended.");
            message.AppendLine("Final score:");
            printStandings(message, i_Player1Score, i_Player2Score);
            Console.Write(message.ToString());
        }

        private static void printStandings(StringBuilder o_Message, int i_Player1Score, int i_Player2Score)
        {
            o_Message.AppendLine($"X: {i_Player1Score}");
            o_Message.AppendLine($"O: {i_Player2Score}");
        }

        public static void PrintSummary(bool i_Victory, int i_Player1Score, int i_Player2Score, TicTacToe.eCurrentPlayer i_CurrentPlayer)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("==============================================================================================");
            if (i_Victory)
            {
                message.AppendLine($"Congratulations! {(i_CurrentPlayer == TicTacToe.eCurrentPlayer.First ? Board.eXorO.O : Board.eXorO.X)} has won this round.");
            }
            else
            {
                message.AppendLine("This rounded has ended by a tie.");
            }

            message.AppendLine("Current players' standings:");
            printStandings(message, i_Player1Score, i_Player2Score);
            Console.Write(message.ToString());
        }

        public static void GetAnotherRound(ref bool o_GameOver)
        {
            StringBuilder message = new StringBuilder();
            string continuePlayStr;

            message.AppendLine("==============================================================================================");
            message.AppendLine("If you wish to continue playing, please enter 'y', otherwise enter 'n' or 'Q' to quit the game");
            Console.Write(message.ToString());
            while (true)
            {
                continuePlayStr = Console.ReadLine();
                if (continuePlayStr == "n" || continuePlayStr == "Q")
                {
                    o_GameOver = true;
                    break;
                }
                else if (continuePlayStr == "y")
                {
                    o_GameOver = false;
                    break;
                }

                Console.WriteLine("Invalid input. Please try again:");
            }
        }

        public static void GetFirstDetails(ref bool o_Player2IsHuman, Board io_Board, ref bool o_GameOver)
        {
            string isHumanInput, sizeStr;
            int boardSize;

            Console.WriteLine("Hello and welcome to Tictactoe!!!");
            Console.WriteLine("Please enter the board size (a number between 3 and 9): ");
            int.TryParse(sizeStr = Console.ReadLine(), out boardSize);
            if (sizeStr == "Q")
            {
                o_GameOver = true;
                return;
            }

            io_Board.Size = boardSize;
            Console.WriteLine("Will Player 2 be a human player? (y/n)");
            while (true)
            {
                isHumanInput = Console.ReadLine();
                if (isHumanInput == "y")
                {
                    o_Player2IsHuman = true;
                    break;
                }
                else if (isHumanInput == "n")
                {
                    o_Player2IsHuman = false;
                    break;
                }
                else if (isHumanInput == "Q")
                {
                    o_GameOver = true;
                    break;
                }

                Console.WriteLine("Invalid input. Please enter 'y' for human player or 'n' for computer player:");
            }
        }

        private static void printBoard(Board i_Board)
        {
            int boardSize = i_Board.Size;

            Ex02.ConsoleUtils.Screen.Clear();
            Console.Write("   ");
            for (int col = 0; col < boardSize; col++)
            {
                Console.Write((col + 1) + "   ");
            }

            Console.WriteLine();
            Console.WriteLine(new string('=', (boardSize * 4 + 2)));

            for (int row = 0; row < boardSize; row++)
            {
                Console.Write((row + 1) + "|");
                for (int col = 0; col < boardSize; col++)
                {
                    Console.Write(" ");
                    switch (i_Board[row, col])
                    {
                        case null:
                            Console.Write(" ");
                            break;
                        case Board.eXorO.X:
                            Console.Write("X");
                            break;
                        case Board.eXorO.O:
                            Console.Write("O");
                            break;
                    }

                    Console.Write(" |");
                }

                Console.WriteLine();
                Console.WriteLine(new string('=', (boardSize * 4 + 2)));
            }
        }

        public static void GetMove(Board io_Board, TicTacToe.eCurrentPlayer i_CurrentPlayer, ref bool io_GameOver, bool i_Player2IsHuman)
        {
            int row = 0, col = 0;

            printBoard(io_Board);
            io_Board.MakeMove(i_CurrentPlayer, ref io_GameOver, i_Player2IsHuman, ref row, ref col);
            if (!io_GameOver)
            {
                printBoard(io_Board);
            }
        }

        private static void printMessageToGetCellInput()
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.AppendLine("Please enter a cell number to place the next move.");
            messageBuilder.AppendLine("First enter a row number, then a column number.");
            messageBuilder.AppendLine("For example: 1 3");
            messageBuilder.AppendLine("At any time, you can quit the game by pressing 'Q'");
            Console.Write(messageBuilder.ToString());
        }

        public static void GetCellFromUser(ref int io_Row, ref int io_Col, ref bool o_GameOver)
        {
            printMessageToGetCellInput();
            while (true)
            {
                string input = Console.ReadLine();
                string[] inputs = input.Split(' ');
                if (input == "Q")
                {
                    o_GameOver = true;
                    break;
                }

                if (inputs.Length != 2)
                {
                    Console.WriteLine("Invalid input. Please enter a row and column number separated by a space.");
                    continue;
                }

                if (!int.TryParse(inputs[0], out io_Row) || !int.TryParse(inputs[1], out io_Col))
                {
                    Console.WriteLine("Invalid input. Please enter valid integer row and column numbers.");
                    continue;
                }

                io_Row--;
                io_Col--;
                break;
            }
        }

        public static void PrintDotsWhileAIisThinking()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write(".");
                Thread.Sleep(50);
            }
        }

        public static void PrintMessageToConsole(string msg)
        {
            Console.WriteLine(msg);
        }

        public static string GetInputFromUser()
        {
            return Console.ReadLine();
        }
    }
}
