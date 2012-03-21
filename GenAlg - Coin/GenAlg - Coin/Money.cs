using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenAlg___Coin
{
    class Money
    {
        static double penny = 0.01;
        static double nickel = 0.05;
        static double dime = 0.10;
        static double quarter = 0.25;
        static double halfDollar = 0.50;
        static double dollar = 1.00;
        static Random rand = new Random();

        /*
         * Return a value that is what the string contains added up.  It switches on
         * a character, and adds up constants that represent each of the characters.
         */
        public double getValue(string coinList)
        {
            double value = 0;
            for (int index = 0; index < (int)coinList.Length; index++)
            {
                switch (coinList[index])
                {
                    case 'p':
                        value += penny;
                        break;
                    case 'n':
                        value += nickel;
                        break;
                    case 'd':
                        value += dime;
                        break;
                    case 'q':
                        value += quarter;
                        break;
                    case 'h':
                        value += halfDollar;
                        break;
                    case 'o':
                        value += dollar;
                        break;
                }
            }
            return value;
        }

        /*
         * Make a new random string of a random value.  It picks a random character
         * and adds it to the end of the string.  It then returns the random string.
         */
        public string generateRandomCoinList(int numOfCoins)
        {
            int temp = 0;
            string coinList = "";
            for (int i = 0; i < numOfCoins; i++)
            {
                temp = rand.Next() % 6;
                switch (temp)
                {
                    case 0:
                        coinList += "p";
                        break;
                    case 1:
                        coinList += "n";
                        break;
                    case 2:
                        coinList += "d";
                        break;
                    case 3:
                        coinList += "q";
                        break;
                    case 4:
                        coinList += "h";
                        break;
                    case 5:
                        coinList += "o";
                        break;
                }
            }
            return coinList;
        }
    }
}
