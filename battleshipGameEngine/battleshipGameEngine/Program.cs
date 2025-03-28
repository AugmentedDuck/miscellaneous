namespace batteshipGameEngine
{
    internal class Program
    {
        //////////////////////////////////////////////////////// 
        // SHIPS:
        // 1x size 5 
        // 1x size 4
        // 2x size 3
        // 1x size 2
        /////////////////////////////////////////////////////////

        static void Main(string[] args)
        {
            Random random = new Random();

            int[] boardSize = { 10, 10 };
            int[] shipSizes = { 5, 4, 3, 3, 2 };

            int[,] myBoard = GenerateBoard(boardSize);
            int[,] shownBoard = GenerateBoard(boardSize);
            int[,] enemyBoard = GenerateBoard(boardSize);


            OutputBoard(shownBoard);

            myBoard = PopulateMyBoard(myBoard, shipSizes, random);
        }

        static int[,] GenerateBoard(int[] size)
        {
            int[,] board = new int[size[0], size[1]];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = 0;
                }
            }

            return board;
        }

        static int[,] PopulateMyBoard(int[,] board, int[] ships, Random random)
        {
            int x, y = 0;

            for (int i = 0; i < ships.Length; i++)
            {
                int isXAxis = random.Next(0, 2);
                bool validPlacement;
                
                do
                {
                    x = random.Next(0, board.GetLength(1));
                    y = random.Next(0, board.GetLength(0));
                    validPlacement = true;

                    if (isXAxis == 0) //Vertical
                    {
                        if (y + ships[i] >= board.GetLength(0))
                        {
                            validPlacement = false;
                        }
                        else
                        {
                            for (int j = 0; j < ships[i]; j++)
                            {
                                if (board[x, y + j] != 0)
                                {
                                    validPlacement = false;
                                    break;
                                }
                            }
                        }
                    }
                    else //Horizontal
                    {
                        if (x + ships[i] >= board.GetLength(1))
                        {
                            validPlacement = false;
                        }
                        else
                        {
                            for (int j = 0; j < ships[i]; j++)
                            {
                                if (board[x + j, y] != 0)
                                {
                                    validPlacement = false;
                                    break;
                                }
                            }
                        }
                    }

                }
                while (!validPlacement);

                Console.WriteLine($"Placed ship of size {ships[i]} at ({x}, {y}), Axis: {(isXAxis == 0 ? "Vertical" : "Horizontal")}");
                board[x, y] = 1;
                for (int j = 0; j < ships[i]; j++)
                {
                    if (isXAxis == 0)
                    {
                        board[x, y + j] = 1;
                    }
                    else
                    {
                        board[x + j, y] = 1;
                    }
                }

            }

            return board;
        }

        static void OutputBoard(int[,] board)
        {
            for (int i = -1; i < board.GetLength(1); i++)
            {
                if (i != -1)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;

                    Console.Write(i.ToString() + " ");
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;

                    Console.Write("  ");
                }

                for (int j = 0 ; j < board.GetLength(0); j++)
                {
                    if (i == -1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write($" {j} |");
                        continue;
                    }

                    if (board[j,i] != 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.Write(" " + board[j, i].ToString() + " |");
                }
                Console.WriteLine($"\n");
            }
        }

        static int[] ChooseRandomSpace(int[,] board)
        {
            int[] coords = { 0, 0 };

            return coords;
        }

        static int[] ChooseSpaceBySpaceScan(int[,] board)
        {
            int[] coords = { 0, 0 };

            return coords;
        }

        static int[] ChooseSpaceByPattern(int[,] board)
        {
            int[] coords = { 0, 0 };

            return coords;
        }

        static int[] ChooseSpaceNextToHit(int[,] board)
        {
            int[] coords = { 0, 0 };

            return coords;
        }
    }
}
