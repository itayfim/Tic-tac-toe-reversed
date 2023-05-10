namespace B23_Ex02
{
    class TicTacToe
    {
        private static Board s_Board = new Board();
        private static Player s_Player1 = new Player(), s_Player2 = new Player();
        
        public static void Start()
        {
            bool player2IsHuman = false, gameOver = false, isVictory;
            eCurrentPlayer currentPlayer = eCurrentPlayer.First;

            UserInteraction.GetFirstDetails(ref player2IsHuman, s_Board, ref gameOver);
            while (!gameOver)
            {
                UserInteraction.GetMove(s_Board, currentPlayer, ref gameOver, player2IsHuman);
                isVictory = checkForVictory(currentPlayer);
                if (checkForTie() || isVictory)
                {
                    if (isVictory)
                    {
                        if (currentPlayer != eCurrentPlayer.First)
                        {
                            s_Player1.Score++;
                        }
                        else
                        {
                            s_Player2.Score++;
                        }
                    }

                    UserInteraction.PrintSummary(isVictory, s_Player1.Score, s_Player2.Score, currentPlayer);
                    UserInteraction.GetAnotherRound(ref gameOver);
                    if (!gameOver)
                    {
                        s_Board.CleanBoard();
                    }
                }

                currentPlayer = (currentPlayer == eCurrentPlayer.First ? eCurrentPlayer.Second : eCurrentPlayer.First);
            }

            UserInteraction.PrintStatistics(s_Player1.Score, s_Player2.Score);
        }

        private static bool checkForTie()
        {
            return s_Board.IsThereATie();
        }

        private static bool checkForVictory(eCurrentPlayer currentPlayer)
        {
            return s_Board.IsThereAStreak(currentPlayer);
        }

        public enum eCurrentPlayer
        {
            First, Second
        }
    }
}
