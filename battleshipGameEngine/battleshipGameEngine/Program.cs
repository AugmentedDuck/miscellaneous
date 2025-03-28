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
                                    enemyBoard = MarkSunkShip(enemyBoard, cpuHit, sunkShipSize);
                                    break;
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Invalid Number");
                            }

                        }
                        enemyBoard[cpuHit[0], cpuHit[1]] = 2;
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

        static int[,] MarkSunkShip(int[,] board, int[] startCoords, int shipSize)
        {
            HashSet<(int, int)> visited = new();
            List<(int, int)> shipParts = new();
            Queue<(int, int)> toVisit = new();

            toVisit.Enqueue((startCoords[0], startCoords[1]));

            while (toVisit.Count > 0)
            {
                var (x, y) = toVisit.Dequeue();
                if (visited.Contains((x, y)) || board[x, y] != 1)
                {
                    continue;
                }

                visited.Add((x, y));
                shipParts.Add((x, y));

                if (x > 0) toVisit.Enqueue((x - 1, y));   
                if (x < board.GetLength(0) - 1) toVisit.Enqueue((x + 1, y));
                if (y > 0) toVisit.Enqueue((x, y - 1));
                if (y < board.GetLength(1) - 1) toVisit.Enqueue((x, y + 1));
            }

            if (shipParts.Count == shipSize)
            {
                foreach (var (x, y) in shipParts)
                {
                    board[x, y] = 2; // Mark as sunk
                }
            }

            return board;

        }

        static int[] CPUTurn(int[,] enemyBoard, int randomWeight, int patternWeight, int scanWeight, Random random, int[] ships)
        {
            int nextAlgorithm = random.Next(randomWeight + patternWeight + scanWeight);
            int[] hitCoords;

            if (IsAnyLeft(enemyBoard, 1))
            {
                //return ChooseSpaceByAdjecent(enemyBoard);
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
            int[] coords = [0, 0];
            
            return coords;
        }
    }
}
