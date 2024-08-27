using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FSBInfoReader
{
    public class Program
    {
        static void Main(string[] args)
        {
            FMODInfo info = new FMODInfo(args[0]);


            foreach (FMODInfoFile file in info.FileList)
            {
                Console.WriteLine(file.FileNameString);
            }
            Console.ReadLine();
        }

        public static void OldFileFinder(string[] args)
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

        public static string CharArrayToString(char[] chars)
        {
            if (chars == null || chars.Length == 0) throw new ArgumentNullException("chars");

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '0') continue;
                sb.Append(chars[i]);
            }
            return sb.Length > 0 ? sb.ToString() : "";
        }
    }

    public class FMODInfo
    {
        public FMODInfo(string fmodInfoPath)
        {
            FileStream fs = new FileStream(fmodInfoPath, FileMode.Open, FileAccess.Read);

            if (fs == null) throw new Exception("File was null");
            if (fs.Length % 280 != 0) throw new Exception("File length must be a multiple of 280");

            int fileCount = (int)(fs.Length / 280);
            for (int i = 0; i <= fileCount; i++)
            {
                byte[] infoBytes = new byte[280];
                fs.Read(infoBytes, 0, 280);
                FileList.Add(new FMODInfoFile(infoBytes));
            }
        }

        public List<FMODInfoFile> FileList = new List<FMODInfoFile>();
    }


    public class FMODInfoFile
    {
        public FMODInfoFile(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException("bytes");
            if (bytes.Length != 280) throw new ArgumentException("Bytes must be 280 in length");

            SoundIdHash = BitConverter.ToInt32(bytes, 0);
            SoundFileSize = BitConverter.ToUInt32(bytes, 4);
            Encoding = BitConverter.ToUInt32(bytes, 8);
            Compression = BitConverter.ToUInt32(bytes, 12);
            UnknownValue3 = BitConverter.ToUInt32(bytes, 16);
            Samples = BitConverter.ToUInt32(bytes, 20);
            FileNameChars = new char[256];
            Array.Copy(bytes, 24, FileNameChars, 0, 256);
            FileNameString = Program.CharArrayToString(FileNameChars);
        }

        public int SoundIdHash;

        public uint SoundFileSize;

        public uint Encoding;

        public enum EncodingType : int
        {
            Mono = 1,
            Stereo = 2,
            Quad = 4,
            FivePointOne = 6
        }

        public uint Compression; // Should always have a value of 2 for WAV/PCM

        public uint UnknownValue3;

        public uint Samples;

        public char[] FileNameChars;

        public string FileNameString;
    }
}
