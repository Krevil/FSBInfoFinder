using System;
using System.IO;

namespace FSBInfoReader
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fileStream = new FileStream(args[0], FileMode.Open);
            uint InfoValue = Convert.ToUInt32(Console.ReadLine());
            byte[] fileBytes = new byte[fileStream.Length];
            fileStream.Read(fileBytes, 0, fileBytes.Length);
            long numOfFiles = fileStream.Length / 0x118;
            int index = 0;
            for (long i = 0; i < fileBytes.Length; i += 0x118)
            {
                index++;
                uint tempVal = BitConverter.ToUInt32(fileBytes, (int)i);
                if (Convert.ToUInt32(InfoValue) == tempVal)
                {
                    Console.WriteLine(index);
                }
            }
            Console.ReadLine();
        }
    }
}
