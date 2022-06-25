using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicohMasterDateFinder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var file in args)
            {
                // AVI: Offset 0x140, length 19, Date + Time
                // WAV: Offset 0x6A, length 10, Date only
                bool isAvi = file.ToLower().EndsWith(".avi");
                int currentOffset = isAvi ? 0x140 : 0x6A;
                int currentLength = isAvi ? 19 : 10;
                try
                {
                    byte[] dateBytes = new byte[currentLength];
                    using (FileStream fs = File.OpenRead(file))
                    {
                        fs.Position = currentOffset;
                        fs.Read(dateBytes, 0, currentLength);
                    }

                    string dateStr = ASCIIEncoding.ASCII.GetString(dateBytes);
                    DateTime dt = DateTime.ParseExact(dateStr, isAvi ? "yyyy:MM:dd HH:mm:ss" : "yyyy:MM:dd", CultureInfo.InvariantCulture);

                    File.SetCreationTime(file, dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing file {file}\n  {ex}");
                }
            }
        }
    }
}
