using AOC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace AOC2020
{
    class Product
    {
        public HashSet<string> Allergens;
        public HashSet<string> Ingridients;

        public Product(string text)
        {
            var sp = text.Split(new string[] { " (contains" }, StringSplitOptions.RemoveEmptyEntries);
            Ingridients = new HashSet<string>(sp[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            Allergens = new HashSet<string>(sp[1].Split(new string[] { " ", ")", "," }, StringSplitOptions.RemoveEmptyEntries));
        }

        public bool containAllergen(string a)
        {
            return Allergens.Contains(a);
        }

        public bool containIngridient(string i)
        {
            return Ingridients.Contains(i);
        }
    }

    public class Day21
    {
        public void Exec()
        {
            var productsText = File.ReadAllLines(@"Data\day21.txt");
            var allergenCounter = new Dictionary<string, int>();
            var ingridientCounter = new Dictionary<string, int>();

            var iaPairs = new Dictionary<KeyValuePair<string, string>, int>();
            var products = new List<Product>();
            for (var productIndex = 0; productIndex < productsText.Length; ++productIndex)
            {
                var p = new Product(productsText[productIndex]);
                products.Add(p);
                foreach (var allergen in p.Allergens)
                    if (allergenCounter.ContainsKey(allergen))
                        ++allergenCounter[allergen];
                    else
                        allergenCounter[allergen] = 1;
                foreach (var ingridient in p.Ingridients)
                    if (ingridientCounter.ContainsKey(ingridient))
                        ++ingridientCounter[ingridient];
                    else
                        ingridientCounter[ingridient] = 1;

                foreach (var ingridient in p.Ingridients)
                    foreach (var allergen in p.Allergens)
                    {
                        var pairIA = new KeyValuePair<string, string>(ingridient, allergen);
                        if (iaPairs.ContainsKey(pairIA))
                            ++iaPairs[pairIA];
                        else
                            iaPairs.Add(pairIA, 1);
                    }
            }

            foreach (var p in iaPairs)
                if (allergenCounter[p.Key.Value] == p.Value)
                    ingridientCounter.Remove(p.Key.Key);

            var count = 0;
            foreach (var i in ingridientCounter)
                count += i.Value;

            var one = count;

            // Везде где есть аллерген А должен быть ингридиент И, иначе пара ИА удаляется
            var keys = iaPairs.Keys.ToArray();
            foreach (var p in keys)
                if (iaPairs[p] > 0)
                    foreach (var product in products)
                        if (product.containAllergen(p.Value) && !product.containIngridient(p.Key))
                        {
                            iaPairs.Remove(p);
                            break;
                        }

            var ingridients = new Dictionary<string, List<string>>();
            foreach(var p in iaPairs)
            {
                if (ingridients.ContainsKey(p.Key.Key))
                {
                    if (!ingridients[p.Key.Key].Contains(p.Key.Value))
                        ingridients[p.Key.Key].Add(p.Key.Value);
                }else
                    ingridients.Add(p.Key.Key, new List<string>() { p.Key.Value });
            }

            var removed = true;
            while (removed)
            {
                removed = false;
                foreach(var i in ingridients)
                    if (i.Value.Count == 1)
                    {
                        var a = i.Value[0];
                        foreach (var j in ingridients)
                            if (j.Key != i.Key && j.Value.Remove(a))
                                removed = true;
                    }
            }

            count = 0;
            var pair = new List<KeyValuePair<string, string>>();
            foreach (var p in ingridients)
                if (p.Value.Count == 1)
                    pair.Add(new KeyValuePair<string, string>(p.Key,p.Value[0]));
            pair.Sort(comparePair);
            var sb = new StringBuilder();
            foreach (var p in pair)
            {
                if (sb.Length > 0)
                    sb.Append(',');
                sb.Append(p.Key);
            }
            var two = sb.ToString();
            Console.WriteLine($"partOne: {count} partTwo: {two}");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                Clipboard.SetText(two.ToString());
        }

        int comparePair(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        ulong PartOne()
        {
            ulong result = 0;

            return result;
        }

        ulong PartTwo()
        {
            ulong result = 0;
            return result;
        }
    }
}