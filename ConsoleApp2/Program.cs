using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
               
        var stopWatch = new Stopwatch();


        var exePath = AppDomain.CurrentDomain.BaseDirectory;
        
        stopWatch.Start();

        FileSpace.readSpaceInFile(exePath, "*.txt");

        stopWatch.Stop();

        Console.WriteLine($"Время {stopWatch.ElapsedMilliseconds} мс");
     

        Console.Read();
    }


    
}
public static class FileSpace
{    
    static int counter = 0;
    public static void readSpaceInFile(string path, string extention)
    {
        try
        {
            List<Task> tasks = new List<Task>();
            foreach (var file in Directory.EnumerateFiles(path, extention))
            {
                var readLines = new Task(() =>
                {

                    using (var reader = new StreamReader(file, detectEncodingFromByteOrderMarks: true))
                    {

                        while (reader.Read() > -1)
                        {

                            counter = 0;
                            Action<string> action = new Action<string>(countSpace);
                            Array.ForEach(reader.ReadLine().Split(' '), action);
                        }
                        Console.WriteLine(counter);


                    }
                });

                tasks.Add(readLines);
            }

            taskRun(tasks);

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }
    private static void countSpace(string value)
    {
        counter = counter + 1;
    }
    private static void taskRun(List<Task> tasks)
    {
        foreach (var task in tasks)
        {
            task.Start();
            Console.WriteLine($"task Id: {task.Id}");
            Console.WriteLine($"task is Completed: {task.IsCompleted}");
            task.Wait();
            Console.WriteLine($"task Status: {task.Status}");
        }
    }
}
