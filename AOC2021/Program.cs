using CurrentDay = AOC2021.Day19;

namespace AOC2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDay = new CurrentDay();
            currentDay.Exec($"Data\\{currentDay.GetType().Name}.txt");
        }
    }
}