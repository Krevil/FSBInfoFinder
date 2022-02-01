using System;
using System.IO;

namespace FSBInfoReader
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fileStream = new FileStream(args[0], FileMode.Open);
            Console.Write("Input FSB Info Value: ");
            uint inputInfoValue = Convert.ToUInt32(Console.ReadLine());
            byte[] fileBytes = new byte[fileStream.Length];
            fileStream.Read(fileBytes, 0, fileBytes.Length);
            int index = 0;
            bool found = false;
            long numOfFiles = fileStream.Length / 0x118;
            Console.WriteLine("Total files in FSB: {0}", numOfFiles);
            for (long i = 0; i < fileBytes.Length; i += 0x118)
            {
                index++;
                uint curInfoValue = BitConverter.ToUInt32(fileBytes, (int)i);
                byte[] stringArray = new byte[0x100];
                Array.Copy(fileBytes, i + 0x18, stringArray, 0, 0x100);
                string soundFileName = System.Text.Encoding.Default.GetString(stringArray);
                uint fileSize = BitConverter.ToUInt32(fileBytes, (int)i + 4);
                if (Convert.ToUInt32(inputInfoValue) == curInfoValue)
                {
                    Console.WriteLine("Found info value at: {0}\nFilepath: {1}\n Filesize: {2}", index, soundFileName, fileSize);
                    found = true;
                }
            }
            if (!found) Console.WriteLine("Couldn't find a matching file to the provided info value");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
