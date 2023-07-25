namespace NameSorter {
    public class NameSorter
    {
        private readonly INameReader nameReader;
        private readonly INameWriter nameWriter;

        public NameSorter(INameReader nameReader, INameWriter nameWriter)
        {
            this.nameReader = nameReader;
            this.nameWriter = nameWriter;
        }

        public void SortNames(string inputFile, string outputFile)
        {
            // Create instances of the comparers
            INameComparer ascendingComparer = new AscendingNameComparer();
            INameComparer descendingComparer = new DescendingNameComparer();
            
            try
            {
                // Read the names from the input file
                List<Name> names = nameReader.ReadNames(inputFile);

                Console.WriteLine("Enter 'A' for ascending order or 'D' for descending order:");
                string? userInput = Console.ReadLine();

                 // Sort the names based on user input
                if (string.Equals(userInput, "A", StringComparison.OrdinalIgnoreCase))
                {
                    names.Sort(ascendingComparer.Compare);
                }
                else if (string.Equals(userInput, "D", StringComparison.OrdinalIgnoreCase))
                {
                    names.Sort(descendingComparer.Compare);
                }
                else
                {
                    Console.WriteLine("Invalid input. Sorting in ascending order by default.");
                    names.Sort(ascendingComparer.Compare);
                }

                // Display the sorted names
                Console.WriteLine("Sorted Names:");
                foreach (Name name in names)
                {
                    Console.WriteLine(name.ToString());
                }

                // Write the sorted names to the output file
                nameWriter.WriteNames(names, outputFile);

                Console.WriteLine("Sorted names have been written to " + outputFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}