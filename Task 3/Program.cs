using System;
using System.Security.Cryptography;
using System.Text;

namespace Task_3
{
    class Program
    {
        static string[] StartingParams()
        {
            string[] startingParams = Console.ReadLine()?.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            while (startingParams.Length < 3 || startingParams.Length % 2 == 0)
            {
                Console.WriteLine("Error, enter an odd number of lines equal to 3 or more");
                startingParams = StartingParams();
            }

            return startingParams;
        }

        static string GeneratingSecretKey()
        {
            RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[16];
            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        static int ComputerMove(int numberMoves)
        {
            Random random = new Random();

            return random.Next(0, numberMoves);
        }

        static string GetHMAC(string value, string secretKey)
        {
            HMACSHA256 hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hmac = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(hmac);
        }

        static void AvailableMoves(string[] startingParams)
        {
            for (int i = 0; i < startingParams.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {startingParams[i]}");
            }

            Console.WriteLine("0 - Exit");
        }

        static string GameWinner(int computerMove, int playerMove, int numberMoves)
        {
            int step = numberMoves / 2 - 1;
            string winner = null;
            if (playerMove != computerMove)
            {
                for (int i = playerMove % numberMoves+1 ; i <= playerMove + step; i++)
                {
                    if (i % numberMoves == computerMove)
                    {
                        winner = "You win!";
                        break;
                    }

                    winner = "Computer win";
                }
            }
            else
            {
                winner = "No winner!";
            }
            return winner;
        }

        static void Menu(string[] startingParams,string secretKey,int computerMove,string computerMoveText,string hmac )
        {
            bool state = true;
            do
            {
                Console.WriteLine(computerMove);
                Console.WriteLine("HMAC : \n" + hmac);
                Console.WriteLine("Available moves:");
                AvailableMoves(startingParams);
                Console.WriteLine($"Enter your move : ");
                int playerMove = Convert.ToInt32(Console.ReadLine());
                if (playerMove <= startingParams.Length && playerMove > 0)
                {
                    if (playerMove == 0)
                    {
                        Environment.Exit(0);
                    }

                    Console.WriteLine($"Your move : {startingParams[playerMove - 1]}");
                    Console.WriteLine($"Computer move : {computerMoveText}");
                    Console.WriteLine(GameWinner(computerMove, playerMove-1, startingParams.Length));
                    Console.WriteLine($"HMAC key : {secretKey}");
                    state = false;
                }
                else
                {
                    Console.WriteLine("Error, repeat");
                }

            } while (state);
        }

        static void StartGame()
        {
            string[] startingParams = StartingParams();
            string secretKey = GeneratingSecretKey();
            int computerMove = ComputerMove(startingParams.Length);
            string computerMoveText = startingParams[computerMove];
            string hmac = GetHMAC(computerMoveText, secretKey);
            Menu(startingParams,secretKey,computerMove,computerMoveText,hmac);
        }

        static void Main(string[] args)
        {
            StartGame();
        }
    }
}