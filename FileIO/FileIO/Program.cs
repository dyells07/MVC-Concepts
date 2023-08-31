using System;
using System.IO;

namespace FileIO
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileloc = @"G:\Fileio.txt";
            string copyTo = @"F:\sample1.txt";

            if (!File.Exists(fileloc))
            {
                Console.WriteLine("File does not exist. Creating the file...");
                using (StreamWriter sw = new StreamWriter(fileloc))
                {
                    sw.Write("Some sample text for the file");
                }
            }

            bool continueProgram = true;

            while (continueProgram)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Write to file");
                Console.WriteLine("2. Read from file");
                Console.WriteLine("3. Copy file");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter text to save in the file:");
                        string userInput = Console.ReadLine();

                        using (StreamWriter sw = new StreamWriter(fileloc))
                        {
                            sw.WriteLine(userInput);
                            Console.WriteLine("User input saved to the file.");
                        }
                        break;

                    case "2":
                        if (File.Exists(fileloc))
                        {
                            using (TextReader tr = new StreamReader(fileloc))
                            {
                                Console.WriteLine("Content of the file:");
                                Console.WriteLine(tr.ReadToEnd());
                            }
                        }
                        else
                        {
                            Console.WriteLine("File does not exist.");
                        }
                        break;

                    case "3":
                        if (File.Exists(fileloc))
                        {
                            File.Copy(fileloc, copyTo, true);
                            Console.WriteLine("File copied to the destination.");
                        }
                        else
                        {
                            Console.WriteLine("Source file does not exist.");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                Console.Write("Do you want to continue? (y/n): ");
                string continueChoice = Console.ReadLine();
                continueProgram = (continueChoice.ToLower() == "y");
            }

            Console.WriteLine("Program completed.");
            Console.ReadLine();
        }
    }
}
