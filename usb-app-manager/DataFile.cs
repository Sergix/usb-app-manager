using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace usb_app_manager
{
    static class DataFile
    {
        public static void LoadStoredPrograms()
        {
            FileStream fileStream = new FileStream(Program.instance.dataFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader fileReader = new StreamReader(fileStream);

            while (!fileReader.EndOfStream)
            {
                string line = fileReader.ReadLine();
                string[] split = line.Split('\t');
                if (!Program.instance.programData.ContainsKey(split[0]))
                {
                    Program.instance.programData.Add(split[0], split[1]);
                }
            }

            fileReader.Close();
            fileStream.Close();

            foreach (KeyValuePair<string, string> item in Program.instance.programData)
            {
                Program.instance.AddNewItem(item.Key, item.Value);
            }
        }

        public static void LoadLocalPrograms(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var fi in di.GetFiles("*.exe", SearchOption.AllDirectories))
            {
                if (fi.Equals("usb-app-manager.exe"))
                    continue;

                Program.instance.AddNewItem(fi.Name.Substring(0, fi.Name.Length - 4), System.IO.Path.Combine(fi.DirectoryName, fi.Name));
            }
        }

        public static void CacheProgram(string name, string path)
        {
            if (Program.instance.programData.ContainsKey(name))
            {
                return;
            }
            Program.instance.programData.Add(name, path);

            Program.instance.fileStream = File.Open(Program.instance.dataFilePath, FileMode.Append);

            string stringOutput = name + '\t' + path + '\n';
            byte[] output = System.Text.Encoding.UTF8.GetBytes(stringOutput.ToCharArray());

            Program.instance.fileStream.Write(output, 0, output.Length);
            Program.instance.fileStream.Flush();
            Program.instance.fileStream.Close();
        }

        public static void DeleteProgram(string name)
        {
            string line = name + '\t' + Program.instance.programData[name];
            Program.instance.programData.Remove(name);

            var oldLines = System.IO.File.ReadAllLines(Program.instance.dataFilePath);
            var newLines = oldLines.Where(line1 => !line1.Contains(line));
            System.IO.File.WriteAllLines(Program.instance.dataFilePath, newLines);
        }

        public static void EditProgram(string oldName, string newName)
        {
            if (oldName == newName)
                return;

            CacheProgram(newName, Program.instance.programData[oldName]);
            DeleteProgram(oldName);
        }
    }
}
