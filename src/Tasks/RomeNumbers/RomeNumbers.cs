using System;
using System.Security.Cryptography.X509Certificates;
using Tasks.Common;

namespace Tasks.RomeNumbers
{
    public class RomeNumbers : IRomeNumbersSolution
    {
        private const string ROMAN_IV = "IV";
        private const string ROMAN_IX = "IX";
        private const string ROMAN_XL = "XL";
        private const string ROMAN_XC = "XC";
        
        private const string ROMAN_V = "V";
        private const string ROMAN_L = "L";
        private const string ROMAN_C = "C";

        private const char ROMAN_CHAR_I = 'I';
        private const char ROMAN_CHAR_X = 'X';
        public void Run()
        {
            string numInput = Console.ReadLine();

            try
            {
                int num = int.Parse(numInput);
                string romeNum = Convert(num);
                Console.WriteLine(romeNum);
            }
            catch
            {
                return;
            }
        }

        public string Convert(int num)
        {
            if (num < 1 || num > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(num), "Число должно быть между 1 и 100.");
            }

            if (num == 100)
            {
                return ROMAN_C;
            }

            string romeNumber = "";
            int tenPlace = num / 10;
            int onesPlace = num % 10;

            if (tenPlace >= 1 && tenPlace <= 3)
            {
                romeNumber = new string(ROMAN_CHAR_X, tenPlace);
            }
            else if (tenPlace == 4)
            {
                romeNumber = ROMAN_XL;
            }
            else if (tenPlace == 5)
            {
                romeNumber = ROMAN_L;
            }
            else if (tenPlace >= 6 && tenPlace <= 8)
            {
                romeNumber = ROMAN_L + new string(ROMAN_CHAR_X, tenPlace - 5);
            }
            else if (tenPlace == 9)
            {
                romeNumber = ROMAN_XC;
            }

            if (onesPlace >= 1 && onesPlace <= 3)
            {
                romeNumber += new string(ROMAN_CHAR_I, onesPlace);
            }
            else if (onesPlace == 4)
            {
                romeNumber += ROMAN_IV;
            }
            else if (onesPlace == 5)
            {
                romeNumber += ROMAN_V;
            }
            else if (onesPlace >= 6 && onesPlace <= 8)
            {
                romeNumber += ROMAN_V + new string(ROMAN_CHAR_I, onesPlace - 5);
            }
            else if (onesPlace == 9)
            {
                romeNumber += ROMAN_IX;
            }

            return romeNumber;
        }
    }
}
