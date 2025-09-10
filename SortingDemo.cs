using System;
using System.Collections.Generic;
using System.Linq;

class SortingDemo
{
    public static void Main()
    {
        Console.WriteLine("=== HDD Free Space Sorting Demo ===\n");
        
        // Sample data that demonstrates the sorting problem
        var sampleData = new List<(string PCName, string FormattedSize, double ByteSize)>
        {
            ("PC-001", "5 GB", 5_000_000_000),
            ("PC-002", "50 GB", 50_000_000_000),
            ("PC-003", "100 GB", 100_000_000_000),
            ("PC-004", "1 TB", 1_000_000_000_000),
            ("PC-005", "500 MB", 500_000_000),
            ("PC-006", "2 GB", 2_000_000_000),
            ("PC-007", "750 GB", 750_000_000_000)
        };
        
        Console.WriteLine("Original Data:");
        foreach (var item in sampleData)
        {
            Console.WriteLine($"{item.PCName}: {item.FormattedSize} ({item.ByteSize:N0} bytes)");
        }
        
        Console.WriteLine("\n=== BEFORE FIX: String Sorting (Incorrect) ===");
        var stringSorted = sampleData.OrderBy(x => x.FormattedSize).ToList();
        foreach (var item in stringSorted)
        {
            Console.WriteLine($"{item.PCName}: {item.FormattedSize}");
        }
        
        Console.WriteLine("\n=== AFTER FIX: Numeric Sorting (Correct) ===");
        var numericSorted = sampleData.OrderBy(x => x.ByteSize).ToList();
        foreach (var item in numericSorted)
        {
            Console.WriteLine($"{item.PCName}: {item.FormattedSize}");
        }
        
        Console.WriteLine("\n=== Problem Demonstration ===");
        Console.WriteLine("Notice how string sorting puts '100 GB' before '2 GB'");
        Console.WriteLine("But numeric sorting correctly puts '2 GB' before '100 GB'");
        Console.WriteLine("\nThe fix stores raw byte values for sorting while displaying formatted strings.");
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
