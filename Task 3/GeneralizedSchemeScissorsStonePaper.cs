using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Task_3
{
    public class GeneralizedSchemeScissorsStonePaper
    {
        private static string[] CheckStartingParams(string[] args)
        {
            string[] startingParams = args?.Distinct()
                .ToArray();
            if (startingParams.Length < 3 || startingParams.Length % 2 == 0)
            {
                Console.WriteLine("Error, enter an odd number of non-repeating lines equal to 3 or more");
                Environment.Exit(0);
            }

            return startingParams;
        }

        private static string GenerateSecretKey()
        {
            RandomNumberGenerator randomNumberGenerator = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[16];
            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static int ComputerMove(int numberMoves)
        {
            Random random = new Random();
            return random.Next(0, numberMoves);
        }

        private static string GetHMAC(string value, string secretKey)
        {
            HMACSHA256 hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var hmac = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(hmac);
        }

        private static void AvailableMoves(string[] startingParams)
        {
            for (int i = 0; i < startingParams.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {startingParams[i]}");
            }

            Console.WriteLine("0 - Exit");
        }

        private static string GetGameWinner(int computerMove, int playerMove, int numberMoves)
        {
            int step = (numberMoves - 1) / 2;
            string winner = null;
            if (playerMove != computerMove)
            {
                for (int i = playerMove % numberMoves + 1; i <= playerMove + step; i++)
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
                winner = "Draw!";
            }

            return winner;
        }

        private static void PrintMenu(string[] startingParams, string secretKey, int computerMove, string computerMoveText,
            string hmac)
        {
            bool state = true;
            do
            {
                Console.WriteLine($"HMAC : \n {hmac}");
                Console.WriteLine("Available moves:");
                AvailableMoves(startingParams);
                Console.WriteLine($"Enter your move : ");
                int playerMove = Convert.ToInt32(Console.ReadLine());
                if (playerMove <= startingParams.Length && playerMove >= 0)
                {
                    if (playerMove == 0)
                    {
                        Environment.Exit(0);
                    }

                    playerMove--;
                    Console.WriteLine($"Your move : {startingParams[playerMove]}");
                    Console.WriteLine($"Computer move : {computerMoveText}");
                    Console.WriteLine(GetGameWinner(computerMove, playerMove, startingParams.Length));
                    Console.WriteLine($"HMAC key : {secretKey}");
                    state = false;
                }
                else
                {
                    Console.WriteLine("Error, repeat");
                }
            } while (state);
        }
        
        public static void StartGame(string[] args)
        {
            string[] startingParams = CheckStartingParams(args);
            string secretKey = GenerateSecretKey();
            int computerMove = ComputerMove(startingParams.Length);
            string computerMoveText = startingParams[computerMove];
            string hmac = GetHMAC(computerMoveText, secretKey);
            PrintMenu(startingParams, secretKey, computerMove, computerMoveText, hmac);
        }
    }
}