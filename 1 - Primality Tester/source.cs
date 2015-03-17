using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace FermatLittleTheorem
{
    public partial class Form1 : Form
    {
        private const string YES = "yes with probability ";
        private const string NO = "no";
        private const int NUMBER_OF_TRIES = 20;

        private BigInteger possiblePrime;
        private BigInteger randomNumber;
        private double probability;
        private BigInteger ModularExpResult;

        private int yesCounter;
        private int noCounter;

        public Form1()
        {
            InitializeComponent();
        }

        private void solve_Click(object sender, EventArgs e)
        {
            //	INITIALIZING VARIABLES
            Random randomGenerator = new Random();

            possiblePrime = BigInteger.Parse(input.Text);

            randomNumber = 0;

            probability = 1 - (1 / (Math.Pow(2, NUMBER_OF_TRIES)));

            int logLength = Convert.ToInt32(BigInteger.Log(possiblePrime, 2) + 1);

            int length = logLength / 32;



            // 	SELECTING DIFFERENT RANDOM NUMBERS IF possiblePrime WAS LESS THAN 32 BITS OR NOT
            if (length < 32)
            {
                while (randomNumber == 0)
                {
                    randomNumber = mod(randomGenerator.Next(), possiblePrime);
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    randomNumber = (randomNumber << 32) + randomGenerator.Next();
                }
            }



            // 	DOING THE FERMAT TEST 20 TIMES TO IMPROVE PROBABILITY
            for (int tries = 0; tries < NUMBER_OF_TRIES; tries++)
            {
                // 	FERMAT PRIMALITY TESTER
                ModularExpResult = ModularExponentiation(randomNumber, possiblePrime - 1, possiblePrime);

                if (ModularExpResult == 1)
                {
                    yesCounter++;
                }
                else
                {
                    noCounter++;
                }
            }



            // COMPARING COUNTER TO GIVE A SOLID ANSWER TO THE PROBLEM
            if (yesCounter > noCounter)
            {
                output.Text = YES + probability.ToString();
            }
            else
            {
                output.Text = NO;
            }

        }



        // 	MODULAR EXPONENTIATION IMPLEMENTATION
        private BigInteger ModularExponentiation(BigInteger value, BigInteger exponent, BigInteger modulo)
        {

            if (exponent == 0)
            {
                return 1;
            }
            else if (mod(exponent, 2) == 0)
            {
                BigInteger result = ModularExponentiation(value, BigInteger.Divide(exponent, 2), modulo);
                BigInteger output = mod((BigInteger.Pow(result, 2)), modulo);
                return output;
            }
            else
            {
                BigInteger solvablePart = mod(value, modulo);
                BigInteger unsolvablePart = ModularExponentiation(value, BigInteger.Subtract(exponent, 1), modulo);
                BigInteger output = mod(BigInteger.Multiply(solvablePart, unsolvablePart), modulo);
                return output;
            }

        }


        // 	MOD FUNCTION
        private BigInteger mod(BigInteger number, BigInteger modulo)
        {
            BigInteger posResult = number % modulo;
            BigInteger negResult;

            if (posResult < 0)
            {
                negResult = BigInteger.Add(posResult, modulo);
                return negResult;
            }
            else
            {
                return posResult;
            }
        }
    }
}

