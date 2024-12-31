namespace Homework;

public class HW4_1
{
    // Четный массив
    public static int[] GetFirstEvenNumbers(int count)
    {
        var array = new int[count];
        for (var i = 0; i < count; i++)
        {
            array[i] = 2 * (i + 1);
        }

        return array;
    }


    //Индекс максимума
    public static int MaxIndex(double[] array)
    {
        var index = -1;
        double maxValue = double.MinValue;

        if (array.Length > 0)
        {
            maxValue = array[0];
            index = 0;
        }

        for (var i = 1; i < array.Length; i++)
        {
            if (array[i] >= maxValue)
            {
                maxValue = array[i];
                index = i;
            }
        }

        return index;
    }

    //Подсчет
    public static int GetElementCount(int[] items, int itemToCount)
    {
        var count = 0;
        foreach (var item in items)
        {
            if (item == itemToCount)
                count++;
        }

        return count;
    }


    //Поиск массива в массиве
    public static int FindSubarrayStartIndex(int[] array, int[] subArray)
    {
        for (var i = 0; i < array.Length - subArray.Length + 1; i++)
            if (ContainsAtIndex(array, subArray, i))
                return i;
        return -1;
    }

    public static bool ContainsAtIndex(int[] array, int[] subArray, int index)
    {
        for (var i = 0; i < subArray.Length; i++)
        {
            if (array[index + i] != subArray[i])
                return false;
        }

        return true;
    }


    //Карты Таро
    public enum Suits
    {
        Wands,
        Coins,
        Cups,
        Swords
    }

    public static string GetSuit(Suits suit)
    {
        return (new[] { "жезлов", "монет", "кубков", "мечей" })[(int)suit];
    }



    //Null или не Null?
    public static bool CheckFirstElement(int[] array)
    {
        return array?.Length > 0 && array[0] == 0;
    }


    //Возвести массив в степень
    public static int[] GetPoweredArray(int[] arr, int power)
    {
        var result = new int[arr.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            var pow = Math.Pow(arr[i], power);

            if (pow > int.MaxValue)
                throw new OverflowException();

            result[i] = (int)Math.Pow(arr[i], power);
        }

        return result;
    }


    //Крестики-нолики
    public enum Mark
    {
        Empty,
        Cross,
        Circle
    }

    public enum GameResult
    {
        CrossWin,
        CircleWin,
        Draw
    }

    public static GameResult GetGameResult(Mark[,] field)
    {
        var crossWin = false;
        var circleWin = false;

        var circleWinMultiplication = (int)Math.Pow(((int)Mark.Circle), field.GetLength(0));
        var crossWinMultiplication = (int)Math.Pow(((int)Mark.Cross), field.GetLength(0));

        var multiplications = GetMultiplications(field);

        foreach (var multiplicationArray in multiplications)
        {
            foreach (var multiplication in multiplicationArray)
            {
                if (multiplication == circleWinMultiplication)
                    circleWin = true;
                else if (multiplication == crossWinMultiplication)
                    crossWin = true;
            }
        }

        var result = GameResult.Draw;

        if (crossWin && !circleWin)
            result = GameResult.CrossWin;
        else if (circleWin && !crossWin)
            result = GameResult.CircleWin;

        return result;
    }

    private static int[][] GetMultiplications(Mark[,] field)
    {
        var xDimension = field.GetLength(1);
        var yDimension = field.GetLength(0);

        if (xDimension != yDimension)
            throw new Exception("Invalid field");

        var multiplications = new int[3][]
        {
            Enumerable.Repeat(1, yDimension).ToArray(),
            Enumerable.Repeat(1, xDimension).ToArray(),
            Enumerable.Repeat(1, 2).ToArray()
        };

        for (var row = 0; row < yDimension; row++)
        {
            for (var column = 0; column < xDimension; column++)
            {
                multiplications[0][row] *= (int)field[row, column];
                multiplications[1][column] *= (int)field[row, column];

                if (row == column)
                    multiplications[2][0] *= (int)field[row, column];

                if ((row + column) == xDimension - 1)
                    multiplications[2][1] *= (int)field[row, column];
            }
        }

        return multiplications;
    }


    //Гистограмма
    internal static class HistogramTask
    {
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var xLabels = new string[31];
            for (var i = 0; i < xLabels.Length; i++)
                xLabels[i] = (i + 1).ToString();

            var yValues = new double[31];

            foreach (var nameData in names)
            {
                if (nameData.Name == name && nameData.BirthDate.Day != 1)
                    yValues[nameData.BirthDate.Day - 1]++;
            }

            return new HistogramData(
                $"Рождаемость людей с именем '{name}'",
                xLabels,
                yValues);
        }


        //Тепловая карта
        internal static class HeatmapTask
        {
            public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
            {
                var xLabels = new string[30];
                for (var i = 0; i < xLabels.Length; i++)
                    xLabels[i] = (i + 2).ToString();

                var yLabels = new string[12];
                for (var i = 0; i < yLabels.Length; i++)
                    yLabels[i] = (i + 1).ToString();

                var heat = new double[30, 12];

                foreach (var name in names)
                {
                    if (name.BirthDate.Day == 1)
                        continue;

                    heat[name.BirthDate.Day - 2, name.BirthDate.Month - 1]++;
                }

                return new HeatmapData(
                    "Пример карты интенсивностей",
                    heat,
                    xLabels,
                    yLabels);
            }
        }
    }
}
