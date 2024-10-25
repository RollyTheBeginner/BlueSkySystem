using Microsoft.AspNetCore.Mvc;
using System;

namespace BlueSkySystem.Helpers
{
    public static class NumberToWordsConverter
    {
        private static readonly string[] Units =
        {
            "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
            "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
        };

        private static readonly string[] Tens =
        {
            "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
        };

        private static readonly string[] Thousands =
        {
            "", "Thousand", "Million", "Billion"
        };

        public static string ConvertToWords(decimal amount)
        {
            if (amount < 0)
                return "Negative " + ConvertToWords(-amount);

            if (amount >= 1000000)
                return ConvertToWords((int)(amount / 1000000)) + " Million " + ConvertToWords(amount % 1000000);

            if (amount >= 1000)
                return ConvertToWords((int)(amount / 1000)) + " Thousand " + ConvertToWords(amount % 1000);

            // Convert to integer for handling in three digits
            return ConvertThreeDigitNumberToWords((int)amount);
        }

        private static string ConvertWholeNumberToWords(long number)
        {
            if (number < 0)
                return "Minus " + ConvertWholeNumberToWords(Math.Abs(number));

            var words = "";

            // Process thousands
            for (int i = 0; i < Thousands.Length; i++)
            {
                var divisor = (long)Math.Pow(1000, i);
                var quotient = number / divisor;
                if (quotient > 0)
                {
                    words = ConvertThreeDigitNumberToWords((int)(quotient)) + " " + Thousands[i] + " " + words;
                }
                number %= divisor;
            }

            return words.Trim();
        }

        private static string ConvertThreeDigitNumberToWords(int number)
        {
            if (number < 0 || number > 999)
            {
                throw new ArgumentOutOfRangeException("number", "Number must be between 0 and 999.");
            }

            var words = "";

            if (number >= 100)
            {
                words += Units[number / 100] + " Hundred ";
                number %= 100;
            }

            if (number >= 20)
            {
                words += Tens[number / 10] + " ";
                number %= 10;
            }

            if (number > 0)
            {
                words += Units[number] + " ";
            }

            return words.Trim();
        }

    }
}
