namespace battleshipGameEngine
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

        const bool debugText = true;
        static int[] firstHit = [-1, -1];
        static int[] lastHit = [-1, -1];

        static void Main(string[] args)
        {
            Random random = new();

            int[] boardSize = [10, 10];
            int[] ships = [5, 4, 3, 3, 2];
            
            int randomWeight = 0;
            int patternWeight = 0;
            int scanWeight = 0;

            Console.WriteLine("Your turn please weights as 'rng pattern scan'");

            while (true)
            {
                try
                {
                    string[] weights = Console.ReadLine()!.Split(' ');
                    if (weights.Length != 3) throw new Exception();

                    randomWeight = int.Parse(weights[0]);
                    patternWeight = int.Parse(weights[1]);
                    scanWeight = int.Parse(weights[2]);
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid format. Please enter three numbers.");
                }
            }

            int[,] myBoard = GenerateBoard(boardSize);
            int[,] shownBoard = GenerateBoard(boardSize);
            int[,] enemyBoard = GenerateBoard(boardSize);


            OutputBoard(shownBoard);
            myBoard = PopulateMyBoard(myBoard, ships, random);

            if (debugText)
            {
                Console.WriteLine("| - - - - - - - - - - - D E B U G - - - - - - - - - - - - - - - |");
                OutputBoard(myBoard);
                Console.WriteLine("| - - - - - - - - - - - D E B U G - - - - - - - - - - - - - - - |");
            }

            /////////////////////////////////////////
            //
            // GAME LOOP
            //
            /////////////////////////////////////////

            while (IsAnyLeft(myBoard))
            {
                // PLAYER'S TURN
                int[] playerHit = PlayerTurn(boardSize);
                shownBoard = HitBoard(shownBoard, myBoard, playerHit);
                OutputBoard(shownBoard);

                //   CPU'S TURN
                int[] cpuHit = CPUTurn(enemyBoard, randomWeight, patternWeight, scanWeight, random, ships);
                Console.WriteLine($"Is ({cpuHit[0]} ; {cpuHit[1]}) a hit? (Y.es - N.o - S.unk)");

                while (true)
                {
                    string input = Console.ReadLine()!.ToLower();

                    if (input == "y")
                    {
                        enemyBoard[cpuHit[0], cpuHit[1]] = 1;
                        break;
                    }
                    else if (input == "n")
                    {
                        enemyBoard[cpuHit[0], cpuHit[1]] = 3;
                        break;
                    }
                    else if (input == "s")
                    {
                        enemyBoard[cpuHit[0], cpuHit[1]] = 1;
                        lastHit = [cpuHit[0], cpuHit[0]];

                        Console.WriteLine($"Enter sunk ship size:");
                        int sunkShipSize;
                        while (true)
                        {
                            try
                            {
                                sunkShipSize = int.Parse(Console.ReadLine()!);
                                if (ships.Contains(sunkShipSize))
                                {
                                    ships = ships.Where(s => s != sunkShipSize).ToArray();
                                    enemyBoard = MarkSunkShip(enemyBoard, sunkShipSize);
                                    firstHit = [-1, -1];
                                    break;
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Invalid Number");
                            }

                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Only 'Y', 'N' or 'S' are accepted");
                    }
                }

                if (debugText)
                {
                    Console.WriteLine("| - - - - - - - - - - - D E B U G - - - - - - - - - - - - - - - |");
                    OutputBoard(enemyBoard);
                    Console.WriteLine("| - - - - - - - - - - - D E B U G - - - - - - - - - - - - - - - |");
                }
            }
        }

        static int[,] MarkSunkShip(int[,] board, int shipSize)
        {
            int xDifference = firstHit[0] - lastHit[0];
            int yDifference = firstHit[1] - lastHit[1];

            if (xDifference != 0)
            {
                if (xDifference == shipSize)
                {
                    for (int i = 0; i < xDifference; i++)
                    {
                        board[firstHit[0] + i, firstHit[1]] = 2;
                    }
                }
                else if (-xDifference == shipSize)
                {
                    for (int i = 0; i < -xDifference; i++)
                    {
                        board[firstHit[0] - i, firstHit[1]] = 2;
                    }
                }
                else
                {
                    for (int i = 0; i < board.GetLength(0); i++)
                    {
                        if (board[i, firstHit[1]] == 1)
                        {
                            board[i, firstHit[1]] = 2;
                        }
                    }
                }
            }
            else if ( yDifference != 0)
            {
                if (yDifference == shipSize)
                {
                    for (int i = 0; i < yDifference; i++)
                    {
                        board[firstHit[0], firstHit[1] + i] = 2;
                    }
                }
                else if (-yDifference == shipSize)
                {
                    for (int i = 0; i < -yDifference; i++)
                    {
                        board[firstHit[0], firstHit[1] - i] = 2;
                    }
                }
                else
                {
                    for (int i = 0; i < board.GetLength(1); i++)
                    {
                        if (board[firstHit[0], i] == 1)
                        {
                            board[firstHit[0], i] = 2;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Last Hit and First hit are identical; Expected different values");
            }

            return board;
        }

        static int[] CPUTurn(int[,] enemyBoard, int randomWeight, int patternWeight, int scanWeight, Random random, int[] ships)
        {
            int nextAlgorithm = random.Next(randomWeight + patternWeight + scanWeight);
            int[] hitCoords;

            if (IsAnyLeft(enemyBoard, 1))
            {
                if (firstHit[0] == -1)
                {
                    firstHit = FindFirstInstance(enemyBoard, 1);
                }
                return ChooseSpaceByAdjecent(enemyBoard);
            }

            if (nextAlgorithm < randomWeight)
            {
                hitCoords = ChooseRandomSpace(enemyBoard, random);
            }
            else if (nextAlgorithm < randomWeight + patternWeight)
            {
                hitCoords = ChooseSpaceByPattern(enemyBoard, ships);
            }
            else
            {
                hitCoords = ChooseSpaceBySpaceScan(enemyBoard, ships);
            }

            return hitCoords;
        }

        static int[] FindFirstInstance(int[,] board, int target)
        {
            int[] coords = [-1, -1];
            for (int i = 0; i < board.GetLength(1); i++)
            {
                for (int j = 0; j < board.GetLength(0); j++)
                {
                    if (board[i,j] == 1)
                    {
                        coords[0] = i;
                        coords[1] = j;
                        return coords;
                    }
                }
            }

            return coords;
        }

        static int[,] HitBoard(int[,] shownBoard, int[,] myBoard, int[] playerHit)
        {
            int hitSpot = myBoard[playerHit[0], playerHit[1]];
            if (hitSpot != 0)
            {
                Console.WriteLine("HIT!");
                shownBoard[playerHit[0], playerHit[1]] = 1;
                myBoard[playerHit[0], playerHit[1]] = 0;

                if (!IsAnyLeft(myBoard, hitSpot))
                {
                    Console.WriteLine("SUNK!");
                }
            } 
            else
            {
                Console.WriteLine("MISS!");
                shownBoard[playerHit[0], playerHit[1]] = 2;
            }

            return shownBoard;
        }

        static bool IsAnyLeft(int[,] board, int target)
        {
            foreach (int spot in board)
            {
                if (spot == target)
                {
                    return true;
                }
            }

            return false;
        }

        static bool IsAnyLeft(int[,] board)
        {
            foreach (int spot in board)
            {
                if (spot != 0)
                {
                    return true;
                }
            }

            return false;
        }

        static int[] PlayerTurn(int[] boardSize)
        {
            int[] coords = new int[2];
            Console.WriteLine("Your turn please enter coords as 'x y'");
            while (true)
            {
                try
                {
                    string input = Console.ReadLine()!;
                    string[] parts = input.Split(' ');

                    if (parts.Length != 2)
                    {
                        throw new FormatException();
                    }

                    coords[0] = Convert.ToInt32(parts[0]);
                    coords[1] = Convert.ToInt32(parts[1]);

                    if (coords[0] >= boardSize[0] || coords[0] < 0 || coords[1] < 0 || coords[1] >= boardSize[1])
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    
                    return coords;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter two numbers separated by a space.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Coordinates are out of bounds. Try again.");
                }
                catch (Exception)
                {
                    Console.WriteLine("An error occurred. Please enter valid coordinates.");
                }
            }
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
            int x, y;

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
                        if (y + ships[i] < board.GetLength(0))
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
                        else
                        {
                            validPlacement = false;
                        }
                    }
                    else //Horizontal
                    {
                        if (x + ships[i] < board.GetLength(1))
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
                        else
                        {
                            validPlacement = false;
                        }
                    }

                }
                while (!validPlacement);

                if (debugText)
                {
                    Console.WriteLine($"[DEBUG] Placed ship of size {ships[i]} at ({x}, {y}), Axis: {(isXAxis == 0 ? "Vertical" : "Horizontal")}");
                }
                board[x, y] = 1;
                for (int j = 0; j < ships[i]; j++)
                {
                    if (isXAxis == 0)
                    {
                        board[x, y + j] = i + 1;
                    }
                    else
                    {
                        board[x + j, y] = i + 1;
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

                    if (board[j, i] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    }
                    else if (board[j, i] != 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    } 
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.Write(" " + board[j, i].ToString() + " |");
                    
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine($"\n");
            }
        }

        static int[] ChooseRandomSpace(int[,] board, Random random)
        {
            int x;
            int y;

            do
            {
                x = random.Next(0, board.GetLength(1));
                y = random.Next(0, board.GetLength(0));
            }
            while (board[x, y] != 0);
            int[] coords = [x, y];

            return coords;
        }

        static int[] ChooseSpaceBySpaceScan(int[,] board, int[] ships)
        {
            int[] coords = [0, 0];

            return coords;
        }

        static int[] ChooseSpaceByPattern(int[,] board, int[] ships)
        {
            int[] coords = [0, 0];

            return coords;
        }

        static int[] ChooseSpaceByAdjecent(int[,] board) 
        {
            int[] coords = [-1, -1];
            
            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (debugText)
                {
                    Console.WriteLine($"[DEBUG] LOOPING X+");
                }

                if (i + firstHit[0] >= board.GetLength(0))
                {
                    break;
                }
                else if (board[firstHit[0] + i, firstHit[1]] == 3)
                {
                    break;
                }
                else if (board[firstHit[0] + i, firstHit[1]] == 0)
                {
                    coords[0] = firstHit[0] + i;
                    coords[1] = firstHit[1];

                    lastHit = coords;

                    return coords;
                }
            }

            for (int i = 0; i < board.GetLength(0); i++)
            {
                if (debugText)
                {
                    Console.WriteLine($"[DEBUG] LOOPING X-");
                }

                if (firstHit[0] - i < 0)
                {
                    break;
                }
                else if (board[firstHit[0] - i, firstHit[1]] == 3)
                {
                    break;
                }
                else if (board[firstHit[0] - i, firstHit[1]] == 0)
                {
                    coords[0] = firstHit[0] - i;
                    coords[1] = firstHit[1];

                    lastHit = coords;

                    return coords;
                }
            }

            for (int i = 0; i < board.GetLength(1); i++)
            {
                if (debugText)
                {
                    Console.WriteLine("[DEBUG] LOOPING Y+");
                }

                if (i + firstHit[1] >= board.GetLength(1))
                {
                    break;
                }
                else if (board[firstHit[0], firstHit[1] + i] == 3)
                {
                    break;
                }
                else if (board[firstHit[0], firstHit[1] + i] == 0)
                {
                    coords[0] = firstHit[0];
                    coords[1] = firstHit[1] + i;

                    lastHit = coords;

                    return coords;
                }
            }

            for (int i = 0; i < board.GetLength(1); i++)
            {
                if (debugText)
                {
                    Console.WriteLine("[DEBUG] LOOPING Y-");
                }

                if (firstHit[1] - i < 0)
                {
                    break;
                }
                else if (board[firstHit[0] - i, firstHit[1]] == 3)
                {
                    break;
                }
                else if (board[firstHit[0], firstHit[1] - i] == 0)
                {
                    coords[0] = firstHit[0];
                    coords[1] = firstHit[1] - i;
                    
                    lastHit = coords;

                    return coords;
                }
            }
            
            if (coords[0] == -1)
            {
                throw new Exception("Couldn't Find any adjecent unhit spaces; but hit ajdecent was run!");
            }

            lastHit = coords;

            return coords;
        }
    }
}
