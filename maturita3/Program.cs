using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Příklad vstupu
        var exchangeRates = new List<Tuple<string, string, double>>
        {
            Tuple.Create("CZK", "EUR", 25.3),
            Tuple.Create("CZK", "USD", 24.1),
            Tuple.Create("EUR", "USD", 0.95),
            Tuple.Create("USD", "JPY", 17.3),
            Tuple.Create("HRK", "NOK", 0.25),
            Tuple.Create("EUR", "NOK", 7.1)
        };

        string startCurrency = "CZK";
        string targetCurrency = "HRK";

        var result = FindShortestExchangeSequence(exchangeRates, startCurrency, targetCurrency);

        if (result != null)
        {
            Console.WriteLine("Shortest exchange sequence:");
            foreach (var step in result)
            {
                Console.WriteLine($"[{step.Item1}, {step.Item2}]");
            }
        }
        else
        {
            Console.WriteLine("No exchange sequence found.");
        }
    }

    static List<Tuple<string, string>> FindShortestExchangeSequence(List<Tuple<string, string, double>> exchangeRates, string start, string target)
    {
        // Vytvoření grafu z tabulky směnných kurzů
        var graph = new Dictionary<string, List<string>>();
        foreach (var rate in exchangeRates)
        {
            if (!graph.ContainsKey(rate.Item1))
            {
                graph[rate.Item1] = new List<string>();
            }
            /*if (!graph.ContainsKey(rate.Item2))
            {
                graph[rate.Item2] = new List<string>();
            }*/
            graph[rate.Item1].Add(rate.Item2);
            //graph[rate.Item2].Add(rate.Item1);
        }

        // Použití BFS pro hledání nejkratší cesty
        var queue = new Queue<List<string>>();
        var visited = new HashSet<string>();
        queue.Enqueue(new List<string> { start });
        visited.Add(start);

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            string lastNode = path[path.Count - 1];

            if (lastNode == target)
            {
                var result = new List<Tuple<string, string>>();
                for (int i = 0; i < path.Count - 1; i++)
                {
                    result.Add(Tuple.Create(path[i], path[i + 1]));
                }
                return result;
            }

            foreach (var neighbor in graph[lastNode])
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    var newPath = new List<string>(path) { neighbor };
                    queue.Enqueue(newPath);
                }
            }
        }

        // Pokud neexistuje žádná cesta
        return null;
    }
}

