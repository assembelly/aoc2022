using System;
using System.IO; //not working/detected properly, need to use full namespace when reading from file.
using System.Collections.Generic;

namespace day_7_solution
{
    class File
    {
        public string Name { get; }
        public int Size { get; }

        public File(string name, int size)
        {            
            Name = name;
            Size = size;
        }
    }

    class Directory
    {
        public string Name { get; }
        public int Size { get; private set; }

        public Directory Parent { get; }
        Dictionary<string, Directory> subDirectories;
        Dictionary<string, File> files;

        public Directory(string name, Directory parent)
        {
            Name = name;
            Parent = parent;

            subDirectories = new Dictionary<string, Directory>();
            files = new Dictionary<string, File>();
        }

        public void AddFile(File newFile) => files.Add(newFile.Name, newFile);
        public void AddSubDirectory (Directory newSubDirectory) => subDirectories.Add(newSubDirectory.Name, newSubDirectory);
        public Directory GetSubDirectory (string name)
        {
            Directory subDirectory;
            subDirectories.TryGetValue(name, out subDirectory);

            return subDirectory;
        }
        public HashSet<Directory> GetDirectories()
        {
            HashSet<Directory> directories = new HashSet<Directory>(); 

            directories.Add(this);

            foreach (Directory subDirectory in subDirectories.Values)
            {
                directories.UnionWith(subDirectory.GetDirectories());
            }

            return directories;
        }

        public void PrintDirectoryLayout(int depth)
        {
            string depthPadding = "";
            for (int i = 0; i < depth; i++) depthPadding += " ";            

            Console.WriteLine("{0}{1} {2} (dir, size = {3})", depthPadding, depth / 2, Name, Size);

            foreach (Directory subDirectory in subDirectories.Values)
            {
                subDirectory.PrintDirectoryLayout(depth + 2);
            }

            foreach (File file in files.Values)
            {
                Console.WriteLine("{0}  {1} {2} (file, size = {3})", depthPadding, depth / 2 + 1, file.Name, file.Size);
            }
        }    
        
        public int SetDirectorySize()
        {
            int size = 0;

            foreach (Directory subDirectory in subDirectories.Values) size += subDirectory.SetDirectorySize();            

            foreach (File file in files.Values) size += file.Size;

            Size = size;
            return size;
        }
    }

    class FileSystem
    {
        
        Directory rootDirectory;
        Directory workingDirectory;
        

        public FileSystem(string fileLocation)
        {
            rootDirectory = new Directory("/", null);
            workingDirectory = rootDirectory;

            IEnumerator<string> terminalOutputIterator = System.IO.File.ReadLines(fileLocation).GetEnumerator();
            // Skip the lines creating the root directory.
            for (int i = 0 ; i < 2; i++) terminalOutputIterator.MoveNext();

            populateFileSystem(terminalOutputIterator);
            setDirectorySize();
                        
            workingDirectory = rootDirectory;
        }

        public int GetTotalSizeOfDirectoriesWithSize100kOrLess()
        {
            int totalSize = 0;

            foreach (Directory directory in getDirectories())
            {
                if (directory.Size <= 100_000) totalSize += directory.Size;
            }

            return totalSize;
        }

        public int GetSizeOfSmallestDirectoryBigEnoughToAllowUppdate ()
        {
            const int totalDiskSpace = 70_000_000;
            const int requiredDiskSpace = 30_000_000;

            int usedDiskSpace = rootDirectory.Size;
            int freeDiskSpace = totalDiskSpace - usedDiskSpace;
            
            int missingDiskSpace = requiredDiskSpace - freeDiskSpace;

            int smallestDirectorySizeAboveOrAtMissingDiskSpace = int.MaxValue;
            foreach (Directory directory in getDirectories())
            {
                if (directory.Size >= missingDiskSpace && directory.Size < smallestDirectorySizeAboveOrAtMissingDiskSpace)
                {
                    smallestDirectorySizeAboveOrAtMissingDiskSpace = directory.Size;
                }
            }

            return smallestDirectorySizeAboveOrAtMissingDiskSpace;
        }

        public void PrintFileSystem() => rootDirectory.PrintDirectoryLayout(0);

        private void addFileSystemEntry(string entryString)
        {
            // Entry string has one of following formats:
            // If file: String.Format("{0} {1}", size, name)
            // If directory: String.Format("dir {0}", name)

            string[] entryData = entryString.Split(" ");

            if (entryData[0] == "dir")
            {
                workingDirectory.AddSubDirectory(new Directory(entryData[1], workingDirectory));
            }
            else
            {
                workingDirectory.AddFile(new File(entryData[1], int.Parse(entryData[0])));
            }
        }

        private HashSet<Directory> getDirectories() => rootDirectory.GetDirectories();        

        private bool isCommand(string line) => line[0] == '$';

        private void populateFileSystem(IEnumerator<string> terminalOutputIterator)
        {
            while (terminalOutputIterator.MoveNext())
            {
                string outputLine = terminalOutputIterator.Current;

                if (isCommand(outputLine))
                {
                    processCommand(outputLine);
                }
                else
                {
                    addFileSystemEntry(outputLine);
                }
            }
        }

        private void processCommand(string commandString)
        {
            // commandLine has format: String.Format("$ {0} {1}", command, argument) , argument is missing if command is "ls".            
            string command = commandString.Split(" ")[1];

            if (command == "cd")
            {
                string argument = commandString.Split(" ")[2];

                switch (argument)
                {
                    case "/":
                        workingDirectory = rootDirectory;
                        break;

                    case "..":
                        workingDirectory = workingDirectory.Parent;
                        break;

                    default:
                        workingDirectory = workingDirectory.GetSubDirectory(argument);
                        break;
                }
            }
        }

        private void setDirectorySize() => rootDirectory.SetDirectorySize();     
    }

    class Program
    {
        static void Main(string[] args)
        {
            string fileLocation = @"..\..\..\..\input";

            FileSystem fileSystem = new FileSystem(fileLocation);

            Console.WriteLine(fileSystem.GetTotalSizeOfDirectoriesWithSize100kOrLess());
            Console.WriteLine(fileSystem.GetSizeOfSmallestDirectoryBigEnoughToAllowUppdate());
        }
    }
}