using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SuperGame
{
    // Handle files read/write
    static class FileHandler
    {
        // Write to a file
        public static void WriteToFile(string filename, string data)
        {
            // write
            try
            {
                System.IO.StreamWriter fl = new System.IO.StreamWriter(filename);

                using (fl)
                {
                    fl.WriteLine(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to save a file, following error ocured:\n" + e.Message);
            }
        }

        // Read from a file
        public static string ReadFromFile(string filename)
        {
            string res = string.Empty;

            // read
            if (File.Exists(filename))
            {
                try
                {
                    StreamReader sr = new StreamReader(filename);
                    res = sr.ReadLine();
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to read a file, following error ocured:\n" + e.Message);
                }

            }

            return res;
        }
    }
}
