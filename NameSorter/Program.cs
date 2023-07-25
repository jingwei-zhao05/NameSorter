using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NameSorter {
    public interface INameReader
    {
        List<Name> ReadNames(string filePath);
    }

    public interface INameWriter
    {
        void WriteNames(List<Name> names, string filePath);
    }

    public class FileNamesReader : INameReader
    {
        public List<Name> ReadNames(string filePath)
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
    }

    public class FileNamesWriter : INameWriter
    {
        public void WriteNames(List<Name> names, string filePath)
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

    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Please provide the path to the input file.");
                    return;
                }

                string inputFile = args[0];
                string outputFile = "sorted-names-list.txt";

                // Use dependency injection to instantiate the reader and writer
                INameReader nameReader = new FileNamesReader();
                INameWriter nameWriter = new FileNamesWriter();

                NameSorter nameSorter = new(nameReader, nameWriter);
                nameSorter.SortNames(inputFile, outputFile);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error: Input file not found - " + ex.Message);
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
          