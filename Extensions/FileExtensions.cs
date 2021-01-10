using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Extensions
{
    public static class FileExtensions
    {
        public static void WriteAllLines(string path, IEnumerable<string> lines, string separator = "\n")
        {
            using var writer = new StreamWriter(path);
            writer.Write(lines.First());
            foreach (var line in lines.Skip(1))
            {
                writer.Write(separator);
                writer.Write(line);
            }
        }

        public static bool IsFileAccessibleForWrite(string filename)
        {
            try
            {
                using var stream = File.Open(filename, FileMode.Open, FileAccess.Write, FileShare.None);
                return stream.CanWrite;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public static bool FilesAreSame(string filename1, string filename2)
        {
            return Path.GetFullPath(filename1) == Path.GetFullPath(filename2);
        }
    }

}