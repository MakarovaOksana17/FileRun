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

        FileSpaceSearcher.ReadSpaceInFile(exePath, "*.txt");

        stopWatch.Stop();

        Console.WriteLine($"Время {stopWatch.ElapsedMilliseconds} мс");
     

        Console.Read();
    }


    
}
public static class FileSpaceSearcher
{    
    public static void ReadSpaceInFile(string path, string extention)
    {
        try
        {
            List<Task> tasks = new List<Task>();
            foreach (var file in Directory.EnumerateFiles(path, extention))
            {
               
                var readLines =  Task.Factory.StartNew(() =>
                {
                    using (var reader = new StreamReader(file, detectEncodingFromByteOrderMarks: true))
                    {                        
                        while (reader.Read() > -1)
                        {
                            Console.WriteLine($@"Task Id: {Task.CurrentId}  
                                                File: {file.Replace(path, "")} contains {reader.ReadToEnd().Count(x => x == ' ').ToString()} spaces.");                           
                            Console.WriteLine($"Task {Task.CurrentId} is Completed: {Task.CompletedTask.IsCompleted}");
                        }
                        
                    } 
                });

                tasks.Add(readLines);
            }
            Task.WaitAll(tasks.ToArray());            
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }     
}
