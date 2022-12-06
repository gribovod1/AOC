using System.Linq;
using System.Text;

public static class StringService
{
    /// <summary>
    /// Возвращает строку с отсортированными по алфавиту символами из исходной строки
    /// </summary>
    /// <param name="source">Исходная строка</param>
    /// <returns>Строка с отсортированными по алфавиту символами</returns>
    public static string Sort(this string source)
    {
        var result = new StringBuilder();
        var l = source.ToList();
        l.Sort();
        foreach (var c in l)
            result.Append(c);
        return result.ToString();
    }

    /// <summary>
    /// Переворачивает строки по диагонали
    /// Пример:
    /// из
    ///     АБВ
    ///     ГДЕ
    /// в
    ///     АГ
    ///     БД
    ///     ВЕ
    /// В случае строк разной длины, отсутствующие символы будут заменены пробелами
    ///     
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string[] Transpose(this string[] source)
    {
        var maxLength = source.Max(x => x.Length);
        var result = new string[maxLength];
        for (var i = 0; i < result.Length; i++)
        {
            var sb = new StringBuilder(maxLength);
            foreach (var c in source)
                if (c.Length < i)
                    sb.Append(c[i]);
                else
                    sb.Append(' ');
            result[i] = sb.ToString();
        }
        return result;
    }

    public static string[] Reverse(this string[] source)
    {
        var result = new string[source.Length];
        foreach (var s in source)
            result[result.Length - 1] = (string)s.Reverse();
        return result;
    }
}