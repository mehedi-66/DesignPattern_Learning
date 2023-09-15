using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning_DesignPattern
{
         /* 
            Single Responsibility Principle says => A Class has single reason to change
         */

        public class Journal
        {
            private readonly List<string> entries = new List<string>();

            private static int count = 0;
            public int AddEntry(string text)
            {
                entries.Add($"{++count}: {text}");
                return count;  // memento pattern 
            }

            public void RemoveEntry(int index)
            {
                entries.RemoveAt(index);
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, entries);
            }

            // we can give save method other method here 
            // but it violate the single responsibility priencipal
        }
        // make make another class to keep simple ... onre reaseason to change class
        public class Persistence
        {
            public void SaveToFile(Journal j, string filename, bool overwrite = false)
            {
                if(overwrite || !File.Exists(filename)) return;
                {
                    File.WriteAllText(filename, j.ToString());  
                }
            }
        }

    public class MainClass
    {
        public static void Main(string[] args)
        {
            var j = new Journal();
            j.AddEntry("I am sad");
            j.AddEntry("I am now Good");

            Console.WriteLine(j);

            Persistence p = new Persistence();
            var filename = @"c:\temp\journal.txt";
            p.SaveToFile(j, filename, true);

           // Process.Start(filename);
        }
    }
       
    
}
