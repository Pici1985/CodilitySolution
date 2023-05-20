// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

Solution solution = new Solution();

string currentDirectory = Directory.GetCurrentDirectory();

string filePath = Path.Combine(currentDirectory, "..\\..\\..\\values.csv");

int n = 5;

List<TopOrder> topOrdersJson = solution.GetTopOrders(filePath, n);

class Order
{
    public string type { get; set; }
    public int orderno { get; set; }
    public string description { get; set; }
    public double value { get; set; }
    public int quantity { get; set; }
}

class TopOrder 
{
    public int Rank { get; set; }
    public int OrderNo { get; set; }
    public double Total { get; set; }
}

class Solution
{
    public List<TopOrder> GetTopOrders(string filePath, int n)
    {
        
        string[] lines = File.ReadAllLines(filePath);
                
        List<Order> orders = new List<Order>();

        List<string> sortedLines = new List<string>();


        foreach (string line in lines.Skip(1))
        {
            List<string> fields = line.Split(',').ToList();


            if (fields.Count() == 5)
            {
                sortedLines.Add(line);
            }
        }

        foreach (string line in sortedLines) 
        { 
            string[] fields = line.Split(',');

            Order order = new Order
            {
                type = fields[0].Trim(),
                orderno = int.Parse(fields[1]),
                description = fields[2].Trim(),
                value = double.Parse(fields[3]),
                quantity = int.Parse(fields[4].Trim())
            };
            orders.Add(order);
        }

        var topOrders = orders.OrderByDescending(o => o.value * o.quantity)
                              .Take(n)
                              .Select((o, index) => new TopOrder()
                              {
                                 Rank = index + 1,
                                 OrderNo = o.orderno,
                                 Total = o.value * o.quantity
                              }).ToList();
                
        
        foreach (var item in topOrders) 
        { 
            var json = JsonConvert.SerializeObject(item);
            Console.WriteLine(json);    
        }

        return topOrders;
    }
}

