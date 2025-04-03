using System.Text;

namespace CritBit;

public class BinaryStringIncrementer
{
    public static string GetNextSeniorString(string binaryString)
    {
        if (string.IsNullOrEmpty(binaryString))
        {
            return Convert.ToString(255, 2).PadLeft(8, '0');
        }

        if (binaryString.Length % 8 != 0)
        {
            throw new ArgumentException("Длина бинарной строки должна быть кратна 8.", nameof(binaryString));
        }

        string lastByte = binaryString.Substring(binaryString.Length - 8);
        if (lastByte == new string('1', 8))
        {
            //binaryString = binaryString.Substring(0, binaryString.Length - 8) + new string('1', 7)+ new string('0', 1);
            binaryString += new string('1', 8);
        }
        else
        {
            int number = Convert.ToInt32(binaryString, 2) + 1;
            binaryString = Convert.ToString(number, 2);

            // Дополнить нулями до исходной длины или кратной 8, если необходимо
            int minLength = (binaryString.Length + 7) / 8 * 8;
            binaryString = binaryString.PadLeft(minLength, '0');
        }

        return binaryString;
    }
}