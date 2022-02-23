using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace MovieLibrary
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddLogging(x=>x.AddConsole())
                .BuildServiceProvider();
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            try
            {
                PrintMenu();
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.Information, "Program didn't run");
            }
            
        }

        private static void PrintMenu()
        {
            
            var choice ="";
            do
            { 
                Console.WriteLine("What would you like to do?\n1. List movies\n2. Add movie\n3. Exit");
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ListMovie();
                        break;
                    case "2":
                        AddMovie();
                        break;
                    case "3":
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            } while (choice != "3");
            
        }

        public static void ListMovie()
        {
            string file = "movies.csv";
            if (File.Exists(file))
            {
                StreamReader sr = new StreamReader(file);
                
                Console.WriteLine("How many records do you want to see? Or A for All Records");
                var choice = (Console.ReadLine().ToUpper());
                sr.ReadLine();
                if (choice != "A")
                {
                    int counter = 0;
                    while (counter < Int32.Parse(choice))
                    {
                        string line = sr.ReadLine();
                        Console.WriteLine(line);
                        counter++;
                    }
                }
                else
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        Console.WriteLine(line);
                    }
                }
                
    
                sr.Close();
            }
            else
            {
                Console.WriteLine("File not found");
            }
        }
    
        public static void AddMovie()
        {
            try
            {
                string file = "movies.csv";
                using (var reader = new StreamReader(file))
                {
                    List<string> movieIds = new List<string>();
                    List<string> titles = new List<string>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        movieIds.Add(values[0]);
                        titles.Add(values[1]);
                    }

                    reader.Close();


                    StreamWriter sw = new StreamWriter(file, true);
                    string resp = "";
                    do
                    {
                        int movieId = Int32.Parse(movieIds.Last()) + 1;

                        Console.WriteLine("Enter movie title");
                        string title = Console.ReadLine();
                        if (titles.Contains(title))
                        {
                            Console.WriteLine("Movie already exists");
                            Console.WriteLine("Enter different movie title");
                            title = Console.ReadLine();
                            if (title.Contains(','))
                            {
                                title = "\"title\"";
                            }
                        }


                        var choice = "";
                        var genre = new List<string>();
                        do
                        {
                            Console.WriteLine("Enter movie genre");
                            genre.Add(Console.ReadLine());
                            Console.WriteLine("Is there another genre to add? Y/N ");
                            choice = Console.ReadLine().ToUpper();
                        } while (choice != "N");

                        string genres = string.Join("|", genre);

                        sw.WriteLine("{0},{1},{2}", movieId, title, genres);
                        Console.WriteLine("Do you want to add another movie (Y/N)?");
                        resp = Console.ReadLine().ToUpper();
                    } while (resp != "N");

                    sw.Close();

                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found");
            }
        }
        
    }
}