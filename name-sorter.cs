class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Please provide the path to the input file.");
            return;
        }

        string inputFile = args[0];
        string outputFile = "sorted-names-list.txt";

        // Read the names from the input file
        List<Name> names = ReadNamesFromFile(inputFile);

        // Sort the names
        names.Sort();

        // Display the sorted names
        Console.WriteLine("Sorted Names:");
        foreach (Name name in names)
        {
            Console.WriteLine(name.ToString());
        }

        // Write the sorted names to the output file
        WriteNamesToFile(names, outputFile);

        Console.WriteLine("Sorted names have been written to {0}.", outputFile);
    }

    static List<Name> ReadNamesFromFile(string filePath)
    {
        List<Name> names = new();

        //check if file is txt file or not
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Input file not found.", filePath);
        }

        string[] lines = File.ReadAllLines(filePath);

        //check if file is empty or not
        if (lines.Length == 0)
        {
            throw new InvalidDataException("Input file is empty.");
        }

        try
        {
            foreach (string line in lines)
            {
                Name name = new(line);
                names.Add(name);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading names from file: " + ex.Message);
        }

        return names;
    }

    static void WriteNamesToFile(List<Name> names, string filePath)
    {
        try
        {
            using StreamWriter writer = new(filePath);
            foreach (Name name in names)
            {
                writer.WriteLine(name.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing names to file: " + ex.Message);
        }
    }
}

class Name : IComparable<Name>
{
    public string[] GivenNames { get; }
    public string LastName { get; }

    public Name(string fullName)
    {
        var nameParts = fullName.Split(' ');
        //check number of given names
        if (nameParts.Length < 2 || nameParts.Length > 4)
        {
            throw new ArgumentException("A name must have between 2 and 4 parts: 1 or 3 given names and 1 last name.");
        }

        //extract all elements except the last one, and assign to given names
        GivenNames = nameParts[..^1];
        //assign the last element of the nameParts array to last name
        LastName = nameParts[^1];
    }

    public override string ToString()
    {
        return string.Join(" ", GivenNames) + " " + LastName;
    }

    public int CompareTo(Name? other)
    {
        if (other is null)
        {
            return 1; 
        }
        
        int result = string.Compare(LastName, other.LastName, StringComparison.OrdinalIgnoreCase);

        if (result == 0)
        {
            int givenNameCount = Math.Max(GivenNames.Length, other.GivenNames.Length);

            for (int i = 0; i < givenNameCount; i++)
            {
                string? thisGivenName = GivenNames.ElementAtOrDefault(i);
                string? otherGivenName = other.GivenNames.ElementAtOrDefault(i);

                result = string.Compare(thisGivenName, otherGivenName, StringComparison.OrdinalIgnoreCase);

                if (result != 0)
                {
                    break;
                }
            }
        }

        return result;
    }
}

