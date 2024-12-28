namespace ConsoleApp.Extensions;

public static class MatrixExtensions
{
    public static int FindNumbers(this int[,] array, bool positive = true)
    {
        int count = 0;

        for (var i = 0; i < array.GetLength(0); i++)
        {
            for (var j = 0; j < array.GetLength(1); j++)
            {
                if (positive)
                {
                    if (array[i, j] > 0)
                        count++;
                }
                else
                {
                    if (array[i, j] < 0)
                        count++;
                }
            }
        }

        return count;
    }

    public static void SortRows(this int[,] array, bool orderByDescending = true)
    {
        for (var i = 0; i < array.GetLength(0); i++)
        {
            SortRow(array, i, orderByDescending);
        }
    }

    private static void SortRow(int[,] array, int row, bool orderByDescending)
    {
        var sorted = true;

        for (var p = 1; p <= array.GetLength(1); p++)
        {
            for (var j = 0; j < array.GetLength(1) - p; j++)
            {
                if (orderByDescending)
                {
                    if (array[row, j] > array[row, j + 1])
                    {
                        sorted = false;
                        (array[row, j + 1], array[row, j]) = (array[row, j], array[row, j + 1]);
                    }
                }
                else
                {
                    if (array[row, j] < array[row, j + 1])
                    {
                        sorted = false;
                        (array[row, j + 1], array[row, j]) = (array[row, j], array[row, j + 1]);
                    }
                }
            }
            if (sorted)
                break;
        }
    }

    public static void InvertRows(this int[,] array)
    {
        for (var i = 0; i < array.GetLength(0); i++)
        {
            InvertRow(array, i);
        }
    }

    private static void InvertRow(int[,] array, int row)
    {
        var length = array.GetLength(1);

        if (length <= 1)
            return;

        for (int i = 0, j = length - 1; i < length && j >= 0; i++, j--)
        {
            if (i >= j)
                break;

            (array[row, i], array[row, j]) = (array[row, j], array[row, i]);
        }
    }
}