using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Diagnostics.SymbolStore;
namespace simple_Shell
{
    class Parser
    {
        public static Dictionary<string, int> importedItems = new Dictionary<string, int>();
        public void parse_input(string str)
        {
            str = str.Trim();
            Token token = new Token();
            var argument = str.Split(' ');
            if (argument.Length == 1)
            {
                token.command = argument[0];
                action(token);
            }
            else if (argument.Length == 2)
            {
                token.command = argument[0];
                token.value = argument[1];
                token.third_value = str;
                action(token);
            }
            else if (argument.Length == 3)
            {
                token.command = argument[0];
                token.value = argument[1];
                token.sec_value = argument[2];
                token.third_value = str;
                action(token);
            }
            else
            {
                token.command = argument[0];
                token.third_value = str;
                action(token);
            }

        }

        void action(Token token)
        {
            switch (token.command)
            {
                case "cls":
                    if (token.value == null)
                    {
                        Console.Clear();
                    }
                    else Console.WriteLine("Error: cls command syntax is\ncls\nfunction: Clear the screen.");
                    break;
                case "quit":
                    if (token.value != null)
                    {
                        Console.WriteLine("Error: quit command syntax is \nquit");
                        Console.WriteLine("function: Quit the shell.");
                    }
                    else Environment.Exit(0);
                    break;
                case "help":
                    Help helper = new Help(token);
                    break;
                case "cd":
                    if (token.value == null || token.value == ".")
                    {
                        return;
                    }
                    else
                    {
                        cd(token.value);
                    }
                    break;

                case "md":
                    if (token.value == null)
                    {
                        Console.WriteLine("ERROR, md command syntax is\r\nmd [directory]\r\n[directory] can be a new directory name or fullpath of a new directory\r\nCreates a directory.");
                        return;
                    }
                    else
                    {
                        md(token.value);
                    }
                    break;
                case "dir":
                    string inputPath = token.value;
                    if (string.IsNullOrWhiteSpace(inputPath) || token.value == ".")
                    {
                        dir(); 
                    }
                    else
                    {
                        Directory originalDirectory = Program.current; 
                        string originalPath = Program.currentPath;     
                        dir(inputPath); 
                                                Program.current = originalDirectory;
                        Program.currentPath = originalPath;
                    }
                    break;
                case "rd":
                    if (token.third_value == null)
                    {
                        Console.WriteLine("ERROR,\n you shold specify folder name to delete\n rd[pah]Name");
                    }
                    else
                    {
                        token.third_value = token.third_value.Substring(2);
                        token.third_value = token.third_value.Trim();
                        string p = token.third_value;
                        string[] arr = p.Split(' ');
                        for (int item = 0; item < arr.Length; item++)
                        {
                            string r = arr[item];
                            rd(r);
                        }
                    }
                    break;
                case "import":
                    if (token.value == null)
                    {
                        Console.WriteLine("ERROR :\nimport text file(s) from your computer\nimport command syntax is\nimport [source]\nor\nimport [source] [destination]\n[source] can be file Name (or fullpath of file) or directory Name (or fullpath of directory) from\nyour physical disk\n[destination] can be file Name (or fullpath of file) or directory name or fullpath of a directory");
                    }
                    else
                    {
                        import(token.value, token.sec_value);
                    }
                    break;
                case "type":
                    if (token.third_value == null)
                    {
                        Console.WriteLine("ERROR\n,  it displays the filename before its content for every \r\nfile\r\n[file] can be file Name (or fullpath of file) of text file\r\n+ after [file] represent that you can pass more than file \r\nName (or fullpath of file).");
                    }
                    else
                    {
                        token.third_value = token.third_value.Substring(4);
                        token.third_value = token.third_value.Trim();
                        string p = token.third_value;
                        string[] arr = p.Split(' ');
                        for (int item = 0; item < arr.Length; item++)
                        {
                            string r = arr[item];
                            Console.Write("\n\n" + r + "\n\n");
                            type(r);
                        }

                    }
                    break;
                case "export":
                    if (token.value == null)
                    {
                        Console.WriteLine("ERROR,\n");
                        Console.WriteLine("- export text file(s) to your computer\r\nexport command syntax is\r\nexport [source]\r\nor\r\nexport [source] [destination]\r\n[source] can be file Name (or fullpath of file) or directory \r\nName (or fullpath of directory) from your virtual disk\r\n[destination] can be file Name (or fullpath of file) or \r\ndirectory name or fullpath of a directory.");
                    }
                    else
                    {
                        export(token.value, token.sec_value);
                    }
                    break;
                case "rename":
                    if (token.value == null || token.sec_value == null)
                    {
                        Console.WriteLine("ERROR,\n");
                        Console.WriteLine("- Renames a file.\r\nrename command syntax is\r\nrename [fileName] [new fileName]\r\n[fileName] can be a file name or fullpath of a filename\r\n[new fileName] can be a new file name not fullpath\n");
                    }
                    else
                    {
                        rename(token.value, token.sec_value);
                    }
                    break;
                case "del":

                    if (token.third_value == null)
                    {
                        Console.WriteLine("error,\n");
                        Console.WriteLine("deletes one or more files.\r\nnote: it confirms the user choice to delete the file before \r\ndeleting\r\ndel command syntax is\r\ndel [dirfile]+\r\n+ after [dirfile] represent that you can pass more than file \r\nname (or fullpath of file) or directory name (or fullpath of \r\ndirectory)\r\n[dirfile] can be file name (or fullpath of file) or \r\ndirectory name (or fullpath of directory).\r");
                    }
                    else
                    {
                        token.third_value = token.third_value.Substring(3);
                        token.third_value = token.third_value.Trim();
                        string p = token.third_value;
                        string[] arr = p.Split(' ');
                        for (int item = 0; item < arr.Length; item++)
                        {
                            string r = arr[item];
                            del(r);
                        }

                    }
                    break;
                case "copy":
                    if (token.value == null)
                    {
                        Console.WriteLine("ERROR,\n");
                        Console.WriteLine("Copies one or more files to another location.\r\ncopy command syntax is\r\ncopy [source]\r\nor\r\ncopy [source] [destination]\r\n[source] can be file Name (or fullpath of file) or directory \r\nName (or fullpath of directory)\r\n[destination] can be file Name (or fullpath of file) or \r\ndirectory name or fullpath of a directory");
                    }
                    else
                    {
                        copy(token.value, token.sec_value);
                    }
                    break;
                default:
                    Console.WriteLine("Unknown Command..");
                    break;



            }
        }

        public static void type(string name)
        {
            string[] path = name.Split("\\");
            if (name.Length <= 3) { Console.WriteLine("Error this is not a file name or access is denied"); return; }
            string s = name.Substring(name.Length - 3);
            if (s != "txt")
            {
                Console.WriteLine("Error this is not a file name or access is denied");
                return;
            }
            if (path.Length > 1)
            {
                Directory dir = changeMyCurrentDirectory(name, false, false);
                if (dir == null)
                    Console.WriteLine($"The Path {name} Is not exist");
                else
                {
                    name = path[path.Length - 1];
                    int j = dir.searchDirectory(name);
                    if (j != -1)
                    {
                        int fc = dir.entries[j].firs_cluster;
                        int sz = dir.entries[j].dir_fileSize;
                        string content = null;
                        FILE file = new FILE(name, 0x0, fc, dir, content, sz);
                        file.ReadFile();
                        Console.WriteLine(file.content);
                    }
                    else
                    {
                        Console.WriteLine("The System could not found the file specified");
                    }
                }
            }
            else
            {
                int j = Program.current.searchDirectory(name);
                if (j != -1)
                {
                    int fc = Program.current.entries[j].firs_cluster;
                    int sz = Program.current.entries[j].dir_fileSize;
                    string content = null;
                    FILE file = new FILE(name, 0x0, fc, Program.current, content, sz);
                    file.ReadFile();
                    Console.WriteLine(file.content);
                }
                else
                {
                    Console.WriteLine("The System could not found the file specified");
                }
            }
        }
        public static void cd(string path)
        {
            Directory dir = changeMyCurrentDirectory(path, true, false);
            if (dir != null)
            {
                dir.ReadDirectory();
                Program.current = dir;
            }
            else
            {
                Console.WriteLine($"Eroor : this path \"{path}\" is not exists!");
            }
        }
        public static void moveToDirUsedInAnother(string path)
        {
            Directory dir = changeMyCurrentDirectory(path, false, false);
            if (dir != null)
            {
                dir.ReadDirectory();
                Program.current = dir;
            }
            else
            {
                Console.WriteLine("the system cannot find the specified folder.!");
            }
        }

        private static Directory changeMyCurrentDirectory(string p, bool usedInCD, bool isUsedInRD)
        {
            Directory d = null;
            string[] arr = p.Split('\\');
            string path;
            if (arr.Length == 1)
            {
                if (arr[0] != "..")
                {
                    int i = Program.current.searchDirectory(arr[0]);
                    if (i == -1)
                        return null;
                    else
                    {
                        string nameOfDiserableFolder = new string(Program.current.entries[i].dir_name); 
                        byte attr = Program.current.entries[i].dir_attr;
                        int fisrtcluster = Program.current.entries[i].firs_cluster;
                        d = new Directory(nameOfDiserableFolder, attr, fisrtcluster, Program.current);
                        d.ReadDirectory();
                        path = Program.currentPath;
                        path += "\\" + nameOfDiserableFolder.Trim();
                        if (usedInCD)
                            Program.currentPath = path;
                    }
                }
                else
                {
                    if (Program.current.parent != null)
                    {
                        d = Program.current.parent;
                        d.ReadDirectory();
                        path = Program.currentPath;
                        path = path.Substring(0, path.LastIndexOf('\\'));
                        if (usedInCD)
                            Program.currentPath = path;
                    }
                    else
                    {
                        d = Program.current;
                        d.ReadDirectory();
                    }
                }
            }
            else if (arr.Length > 1)
            {

                List<string> ListOfHandledPath = new List<string>();
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] != " ")
                        ListOfHandledPath.Add(arr[i]);



                Directory rootDirectory = new Directory("M:", 0x10, 5, null);
                rootDirectory.ReadDirectory();


                if (ListOfHandledPath[0].Equals("m:") || ListOfHandledPath[0].Equals("M:"))
                {
                    path = "M:";
                    int howLongIsMyWay;
                    if (isUsedInRD || usedInCD)
                    {
                        howLongIsMyWay = ListOfHandledPath.Count;
                    }
                    else
                    {
                        howLongIsMyWay = ListOfHandledPath.Count - 1;
                    }
                    for (int i = 1; i < howLongIsMyWay; i++)
                    {
                        int j = rootDirectory.searchDirectory(ListOfHandledPath[i]);
                        if (j != -1)
                        {
                            Directory tempOfParent = rootDirectory;
                            string newName = new string(rootDirectory.entries[j].dir_name);
                            byte attr = rootDirectory.entries[j].dir_attr;
                            int fc = rootDirectory.entries[j].firs_cluster;
                            rootDirectory = new Directory(newName, attr, fc, tempOfParent);
                            rootDirectory.ReadDirectory();
                            path += "\\" + newName.Trim();
                        }
                        else
                        {
                            return null;
                        }
                    }
                    d = rootDirectory;
                    if (usedInCD)
                        Program.currentPath = path;
                }
                else if (ListOfHandledPath[0] == "..")
                {
                    d = Program.current;
                    for (int i = 0; i < ListOfHandledPath.Count; i++)
                    {
                        if (d.parent != null)
                        {
                            d = d.parent;
                            d.ReadDirectory();
                            path = Program.currentPath;
                            path = path.Substring(0, path.LastIndexOf('\\'));
                            if (usedInCD)
                                Program.currentPath = path;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                    return null;
            }
            return d;
        }


        public static void md(string name)
        {
            string[] arr = name.Split('\\');
            if (arr.Length == 1) 
            {
                if (Program.current.searchDirectory(arr[0]) == -1)
                {
                    DirectoryEntry d = new DirectoryEntry(arr[0], 0x10, 0, 0);

                    if (FAT.GetEmptyCulster() != -1)
                    {
                        Program.current.entries.Add(d);
                        Program.current.WriteDirectory();
                        if (Program.current.parent != null)
                        {
                            Program.current.parent.updateContent(Program.current.getDirectoryEntry());
                            Program.current.parent.WriteDirectory();
                        }
                        FAT.writeFat();
                    }
                    else
                        Console.WriteLine("The Disk is Full :(");
                }
                else
                    Console.WriteLine($"Error : {arr[0]} is aready existed ");
            }
            else if (arr.Length > 1)
            {
                Directory dir = changeMyCurrentDirectory(name, false, false); 
                if (dir == null)
                    Console.WriteLine($"The Path {name} Is not exist");
                else
                {
                    
                    if (dir.searchDirectory(arr[arr.Length - 1]) == -1) 
                    {
                        if (FAT.GetEmptyCulster() != -1)
                        {
                            DirectoryEntry d = new DirectoryEntry(arr[arr.Length - 1], 0x10, 0, 0);
                            dir.entries.Add(d);
                            dir.WriteDirectory();
                            if (dir.parent != null)
                            {
                                dir.parent.updateContent(dir.getDirectoryEntry());
                                dir.parent.WriteDirectory();
                            }
                            FAT.writeFat();
                        }
                        else
                            Console.WriteLine("The Disk is Full :");
                    }
                    else
                    {
                        Console.WriteLine($"{arr[arr.Length - 1]} is aready existed in {name.Substring(0, name.LastIndexOf('\\'))} ");
                    }
                }
            }
        }


        public static void dir(string path = null)
        {

            Directory targetDirectory = null;
            bool pathh = false, l = false;
            string[] s = { };
            string name = "";

            if (path != null)
            {
                if (path == "..")
                {
                    string[] h = Program.currentPath.Split("\\");
                    string r = string.Join("\\", h.Take(h.Length - 1));
                    path = "";
                    for (int i = 0; i < r.Length; i++) { if ((r[i] >= 'A' && r[i] <= 'z') || (r[i] >= 45 && r[i] <= 58)) path += r[i]; }
                    dir(path);
                    return;
                }
                else
                {
                    s = path.Split('\\');
                    name = s[s.Length - 1];
                    pathh = (s.Length == 1 && path.Contains("."));
                    l = path.Contains(".");
                    if (!l)
                    {
                        targetDirectory = changeMyCurrentDirectory(path, true, false);

                        if (targetDirectory == null)
                        {
                            Console.WriteLine($"Error: The path \"{path}\" does not exist!");
                            return;
                        }
                    }
                }
            }
            else
            {
                targetDirectory = Program.current;
                path = Program.currentPath;


            }

            if (!l)
            {
                int fileCount = 0, dirCount = 0, totalFileSize = 0;
                Console.WriteLine("Directory of " + path);
                Console.WriteLine();

                foreach (var entry in targetDirectory.entries)
                {
                    if (entry.dir_attr == 0x0)
                    {
                        Console.WriteLine($"\t{entry.dir_fileSize} \t {new string(entry.dir_name)}");
                        fileCount++;
                        totalFileSize += entry.dir_fileSize;
                    }
                    else if (entry.dir_attr == 0x10)
                    {
                        Console.WriteLine($"\t<DIR> {new string(entry.dir_name)}");
                        dirCount++;
                    }
                }

                Console.WriteLine($"{"\t\t"}{fileCount} File(s)    {totalFileSize} bytes");
                Console.WriteLine($"{"\t\t"}{dirCount} Dir(s)    {VirtualDisk.getFreeSpace()} bytes free");
            }
            else
            {
                if (pathh) { targetDirectory = Program.current; path = Program.currentPath; }
                else { targetDirectory = changeMyCurrentDirectory(path, false, false); path = string.Join("\\", s.Take(s.Length - 1)); }

                if (targetDirectory.searchDirectory(name) != -1)
                {
                    int fileCount = 0, dirCount = 0, totalFileSize = 0;
                    Console.WriteLine("Directory of " + path);
                    Console.WriteLine();

                    foreach (var entry in targetDirectory.entries)
                    {
                        string j = new string(entry.dir_name);
                        if (entry.dir_attr == 0x0 && j == name)                         {
                            Console.WriteLine($"\t{entry.dir_fileSize} \t {new string(entry.dir_name)}");
                            fileCount++;
                            totalFileSize += entry.dir_fileSize;
                            break;
                        }
                    }

                    Console.WriteLine($"{"\t\t"}{fileCount} File(s)    {totalFileSize} bytes");
                    Console.WriteLine($"{"\t\t"}{dirCount} Dir(s)    {VirtualDisk.getFreeSpace()} bytes free");
                }
                else
                {
                    path += "\\" + name;
                    Console.WriteLine($"Error: this path '{path}' is not exist.");
                }
            }
        }


        public static void rd(string name)
        {


            string[] arr = name.Split('\\');
            Directory dir = changeMyCurrentDirectory(name, false, true);
            if (dir != null)
            {
                while (true)
                {
                    Console.Write($"are you sure that you want to delete {arr[arr.Length-1]}, please enter y for yes or n for no:");
                    string s;
                    s = Console.ReadLine();
                    if (s == "y")
                    {
                        var l = dir.entries.Count;
                        if (l == 0) dir.deleteDirectory();
                        else Console.WriteLine($"directory \" {arr[arr.Length - 1]} \" is not Empty!");
                        break;
                    }
                    else if (s == "n")
                    {
                        break;
                    }

                }
                


            }
            else
                Console.WriteLine($"directory \" {arr[arr.Length - 1]} \" is not exists!");

        }

        public static void import(string source, string destination = null)
        {
            string[] sourceParts = source.Split("\\");
            string sourceName = sourceParts[sourceParts.Length - 1];

            if (System.IO.Directory.Exists(source))
            {
                Console.WriteLine($"Importing folder: {source}");

                DirectoryInfo dir = new DirectoryInfo(source);
                foreach (var file in dir.GetFiles())
                {
                    import(file.FullName, destination); 
                }

                                importedItems[sourceName] = 1;

                Console.WriteLine($"Folder {sourceName} imported successfully.");
                return;
            }

            if (File.Exists(source))
            {
                if (destination == null) 
                {
                    Console.WriteLine($"Importing file: {source}");

                    int j = Program.current.searchDirectory(sourceName);
                    if (j == -1)
                    {
                        string content = File.ReadAllText(source);
                        int size = content.Length;

                        int fc = size > 0 ? FAT.GetEmptyCulster() : 0;

                        FILE newFile = new FILE(sourceName, 0X0, fc, Program.current, content, size);
                        newFile.writeFile();

                        DirectoryEntry d = new DirectoryEntry(sourceName, 0X0, fc, size);
                        Program.current.entries.Add(d);
                        Program.current.WriteDirectory();

                        importedItems[sourceName] = 1;

                        Console.WriteLine($"{sourceName} file imported successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"{sourceName} already exists in the virtual disk.");
                    }
                }
                else
                {
                    string[] destParts = destination.Split("\\");
                    string destName = destParts[destParts.Length - 1];

                    if (destination.Contains("\\"))
                    {
                        DirectoryInfo destDir = new DirectoryInfo(destination);
                        if (destDir.Exists)
                        {
                            int fileIndex = Program.current.searchDirectory(sourceName);
                            if (fileIndex == -1)
                            {
                                Console.WriteLine($"Creating file {sourceName} in destination directory: {destination}");
                                string content = File.ReadAllText(source);
                                int size = content.Length;

                                int fc = size > 0 ? FAT.GetEmptyCulster() : 0;

                                FILE newFile = new FILE(sourceName, 0X0, fc, Program.current, content, size);
                                newFile.writeFile();

                                DirectoryEntry d = new DirectoryEntry(sourceName, 0X0, fc, size);
                                Program.current.entries.Add(d);
                                Program.current.WriteDirectory();

                                Console.WriteLine($"File {sourceName} imported successfully to {destination}.");
                            }
                            else
                            {
                                Console.WriteLine($"A file with the same name already exists in the destination directory: {destination}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"The destination directory {destination} does not exist.");
                        }
                    }
                    else
                    {
                        int destIndex = Program.current.searchDirectory(destName);
                        if (destIndex == -1)
                        {
                            Console.WriteLine($"Creating destination file: {destName}");
                            string content = File.ReadAllText(source);
                            int size = content.Length;

                            int fc = size > 0 ? FAT.GetEmptyCulster() : 0;

                            FILE newFile = new FILE(destName, 0X0, fc, Program.current, content, size);
                            newFile.writeFile();

                            DirectoryEntry d = new DirectoryEntry(destName, 0X0, fc, size);
                            Program.current.entries.Add(d);
                            Program.current.WriteDirectory();

                            Console.WriteLine($"File {destName} imported successfully to the virtual disk.");
                        }
                        else if (Program.current.entries[destIndex].dir_attr == 0x10)
                        {
                            Directory targetDirectory = changeMyCurrentDirectory(destination, false, true);

                            if (targetDirectory == null)
                            {
                                Console.WriteLine($"The destination directory {destination} does not exist.");
                                return;
                            }

                            int fileIndex = targetDirectory.searchDirectory(sourceName);
                            if (fileIndex == -1)
                            {
                                Console.WriteLine($"Creating file {sourceName} in destination directory: {destination}");
                                string content = File.ReadAllText(source);
                                int size = content.Length;

                                int fc = size > 0 ? FAT.GetEmptyCulster() : 0;

                                FILE newFile = new FILE(sourceName, 0X0, fc, targetDirectory, content, size);
                                newFile.writeFile();

                                DirectoryEntry d = new DirectoryEntry(sourceName, 0X0, fc, size);
                                targetDirectory.entries.Add(d);
                                targetDirectory.WriteDirectory();

                                Console.WriteLine($"File {sourceName} imported successfully to {destination}.");
                            }
                            else
                            {
                                Console.WriteLine($"A file with the same name already exists in the destination directory: {destination}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"The destination {destination} is not valid.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"The source {source} does not exist.");
            }
        }

        public static void export(string source, string dest = null)
        {
            string[] path = source.Split("\\");
            string[] pp = { };
            string lName = "";
            if (dest != null)
            {
                pp = dest.Split("\\");
                lName = pp[pp.Length - 1];
            }
            if (path.Length > 1)
            {
                Directory dir = changeMyCurrentDirectory(source, false, false);
                if (dir == null)
                    Console.WriteLine($"The Path {source} Is not exist");
                else
                {
                    source = path[path.Length - 1];

                    int j = dir.searchDirectory(source);
                    if (j != -1)
                    {

                        bool c = lName.Contains(".");
                        if (!c)
                        {
                            if (System.IO.Directory.Exists(dest))
                            {
                                int fc = dir.entries[j].firs_cluster;
                                int sz = dir.entries[j].dir_fileSize;
                                string content = null;
                                FILE file = new FILE(source, 0x0, fc, dir, content, sz);
                                file.ReadFile();
                                StreamWriter sw = new StreamWriter(dest + "\\" + source);
                                sw.Write(file.content);
                                sw.Flush();
                                sw.Close();
                                Console.WriteLine("1 file (s) exported");
                            }
                            else if (dest == null)
                            {
                                int fc = Program.current.entries[j].firs_cluster;
                                int sz = Program.current.entries[j].dir_fileSize;
                                string content = null;
                                FILE file = new FILE(source, 0x0, fc, Program.current, content, sz);
                                file.ReadFile();
                                StreamWriter sw = new StreamWriter(source);
                                sw.Write(file.content);
                                sw.Flush();
                                sw.Close();
                                Console.WriteLine("1 file (s) exported");
                            }
                            else
                            {
                                Console.WriteLine($"the path {dest} does not exist");
                            }
                        }
                        else if (c)
                        {
                            dest = "";
                            for (int o = 0; o < pp.Length - 1; o++) { dest += pp[o] + "\\"; }
                            if (System.IO.Directory.Exists(dest))
                            {
                                int fc = dir.entries[j].firs_cluster;
                                int sz = dir.entries[j].dir_fileSize;
                                string content = null;
                                FILE file = new FILE(source, 0x0, fc, dir, content, sz);
                                file.ReadFile();
                                StreamWriter sw = new StreamWriter(dest + "\\" + lName);
                                sw.Write(file.content);
                                sw.Flush();
                                sw.Close();
                                Console.WriteLine("1 file (s) exported");
                            }
                            else if (dest == null)
                            {
                                int fc = Program.current.entries[j].firs_cluster;
                                int sz = Program.current.entries[j].dir_fileSize;
                                string content = null;
                                FILE file = new FILE(source, 0x0, fc, Program.current, content, sz);
                                file.ReadFile();
                                StreamWriter sw = new StreamWriter(lName);
                                sw.Write(file.content);
                                sw.Flush();
                                sw.Close();
                                Console.WriteLine("1 file (s) exported");
                            }
                            else
                            {
                                Console.WriteLine($"the path {dest} does not exist");
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine($"This file {string.Join('\\', path)} does not exist in your disk");
                    }
                }

            }
            else
            {
                int j = Program.current.searchDirectory(source);
                if (j != -1)
                {
                    bool c = lName.Contains(".");
                    if (!c)
                    {
                        if (System.IO.Directory.Exists(dest))
                        {
                            int fc = Program.current.entries[j].firs_cluster;
                            int sz = Program.current.entries[j].dir_fileSize;
                            string content = null;
                            FILE file = new FILE(source, 0x0, fc, Program.current, content, sz);
                            file.ReadFile();
                            StreamWriter sw = new StreamWriter(dest + "\\" + source);
                            sw.Write(file.content);
                            sw.Flush();
                            sw.Close();
                            Console.WriteLine("1 file (s) exported");
                        }
                        else if (dest == null)
                        {
                            int fc = Program.current.entries[j].firs_cluster;
                            int sz = Program.current.entries[j].dir_fileSize;
                            string content = null;
                            FILE file = new FILE(source, 0x0, fc, Program.current, content, sz);
                            file.ReadFile();
                            StreamWriter sw = new StreamWriter(source);
                            sw.Write(file.content);
                            sw.Flush();
                            sw.Close();
                            Console.WriteLine("1 file (s) exported");
                        }
                        else
                        {
                            Console.WriteLine($"the path {dest} does not exist");
                        }
                    }
                    else if (c)
                    {
                        dest = null;
                        if (pp.Length > 1)
                            for (int o = 0; o < pp.Length - 1; o++) { dest += pp[o] + "\\"; }
                        if (System.IO.Directory.Exists(dest))
                        {
                            int fc = Program.current.entries[j].firs_cluster;
                            int sz = Program.current.entries[j].dir_fileSize;
                            string content = null;
                            FILE file = new FILE(source, 0x0, fc, Program.current, content, sz);
                            file.ReadFile();
                            StreamWriter sw = new StreamWriter(dest + "\\" + lName);
                            sw.Write(file.content);
                            sw.Flush();
                            sw.Close();
                            Console.WriteLine("1 file (s) exported");
                        }
                        else if (dest == null)
                        {
                            int fc = Program.current.entries[j].firs_cluster;
                            int sz = Program.current.entries[j].dir_fileSize;
                            string content = null;
                            FILE file = new FILE(source, 0x0, fc, Program.current, content, sz);
                            file.ReadFile();
                            StreamWriter sw = new StreamWriter(lName);
                            sw.Write(file.content);
                            sw.Flush();
                            sw.Close();
                            Console.WriteLine("1 file (s) exported");
                            File.Delete(source);
                        }
                        else
                        {
                            Console.WriteLine($"the path {dest} does not exist");
                        }
                    }

                }
                else
                {
                    Console.WriteLine($"This file {source} doesnt exist in your disk");
                }
            }
        }

        public static void rename(string oldName, string newName)
        {
            if (newName.Contains("\\"))
            {
                Console.WriteLine("Error: The new file name should be a file name only, you cannot provide a full path.");
                return;
            }

            string[] path = oldName.Split("\\");
            if (path.Length > 1)
            {
                Directory dir = changeMyCurrentDirectory(oldName, false, false);
                if (dir == null)
                    Console.WriteLine($"The Path {oldName} Is not exist");
                else
                {
                    oldName = path[path.Length - 1];

                    int j = dir.searchDirectory(oldName);
                    if (j != -1)
                    {
                        if (dir.searchDirectory(newName) == -1)
                        {
                            DirectoryEntry d = dir.entries[j];

                            if (d.dir_attr == 0x0)
                            {
                                string[] fileName = newName.Split('.');
                                char[] goodName = getProperFileName(fileName[0].ToCharArray(), fileName[1].ToCharArray());
                                d.dir_name = goodName;
                            }
                            else if (d.dir_attr == 0x10)
                            {
                                char[] goodName = getProperDirName(newName.ToCharArray());
                                d.dir_name = goodName;
                            }

                            dir.entries.RemoveAt(j);
                            dir.entries.Insert(j, d);
                            dir.WriteDirectory();
                        }
                        else
                        {
                            Console.WriteLine("Error:Duplicate File Name exists");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"The path {string.Join('\\', path)} does not exist");
                    }
                }
            }
            else
            {
                int j = Program.current.searchDirectory(oldName);
                if (j != -1)
                {
                    if (Program.current.searchDirectory(newName) == -1)
                    {
                        DirectoryEntry d = Program.current.entries[j];

                        if (d.dir_attr == 0x0)
                        {
                            string[] fileName = newName.Split('.');
                            char[] goodName = getProperFileName(fileName[0].ToCharArray(), fileName[1].ToCharArray());
                            d.dir_name = goodName;
                        }
                        else if (d.dir_attr == 0x10)
                        {
                            char[] goodName = getProperDirName(newName.ToCharArray());
                            d.dir_name = goodName;
                        }

                        Program.current.entries.RemoveAt(j);
                        Program.current.entries.Insert(j, d);
                        Program.current.WriteDirectory();
                    }
                    else
                    {
                        Console.WriteLine("Error:Duplicate File Name exists");
                    }
                }
                else
                {
                    Console.WriteLine($"The file {oldName} does not exist");
                }
            }
        }


        public static void del(string fileName)
        {
            string[] path = fileName.Split("\\");

            if (path.Length > 1)
            {
                Directory parentDir = changeMyCurrentDirectory(fileName, false, false);
                if (parentDir == null)
                {
                    Console.WriteLine($"Error: Path '{fileName}' does not exist.");
                    return;
                }

                string targetName = path[^1];
                int index = parentDir.searchDirectory(targetName);

                if (index != -1)
                {
                    while (true)
                    {
                        Console.Write($"are you sure that you want to delete {targetName}, please enter y for yes or n for no:");
                        string s;
                        s = Console.ReadLine();
                        if (s == "y")
                        {
                            HandleDeletion(parentDir, parentDir.entries[index], targetName);
                            break;
                        }
                        else if (s == "n")
                        {
                            break;
                        }

                    }
                   
                }
                else
                {
                    Console.WriteLine($"Error: The system cannot find the file or directory '{targetName}'.");
                }
            }
            else
            {
                int index = Program.current.searchDirectory(fileName);

                if (index != -1)
                {
                    while (true)
                    {
                        Console.Write($"are you sure that you want to delete {fileName}, please enter y for yes or n for no:");
                        string s;
                        s = Console.ReadLine();
                        if (s == "y")
                        {
                            HandleDeletion(Program.current, Program.current.entries[index], fileName);
                            break;
                        }
                        else if (s == "n")
                        {
                            break;
                        }

                    }
                    
                }
                else
                {
                    Console.WriteLine($"Error: The system cannot find the file or directory '{fileName}'.");
                }
            }
        }

        private static void HandleDeletion(Directory parentDir, DirectoryEntry entryToDelete, string targetName)
        {
            if (entryToDelete.dir_attr == 0x0)
            {
                FILE fileToDelete = new FILE(targetName, 0x0, entryToDelete.firs_cluster, parentDir, null, entryToDelete.dir_fileSize);
                fileToDelete.deleteFile();
                parentDir.ReadDirectory();
                Console.WriteLine($"File '{targetName}' has been deleted successfully.");
            }
            else if (entryToDelete.dir_attr == 0x10)
            {
                Directory dirToDelete = GetDirectoryFromCluster(entryToDelete.firs_cluster);
                if (dirToDelete != null)
                {
                    List<DirectoryEntry> filesToDelete = dirToDelete.entries.FindAll(e => e != null && e.dir_attr == 0x0).ToList();

                    foreach (var fileEntry in filesToDelete)
                    {
                        FILE fileInDir = new FILE(new string(fileEntry.dir_name).TrimEnd(' '), 0x0, fileEntry.firs_cluster, dirToDelete, null, fileEntry.dir_fileSize);
                        fileInDir.deleteFile();
                    }

                    dirToDelete.ReadDirectory();
                    Console.WriteLine($"Directory '{targetName}' and its contents have been deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: Could not access directory '{targetName}'.");
                }
            }
            else
            {
                Console.WriteLine($"Error: The system cannot identify '{targetName}' as a file or directory.");
            }
        }

        static Directory GetDirectoryFromCluster(int cluster)
        {
            if (cluster == 0)
                return null;

            Directory foundDir = new Directory("temp", 0x10, cluster, null); 
            foundDir.ReadDirectory();
            return foundDir;
        }



        public static void copy(string source, string dest = null)
        {
            source = source.Trim();
            string[] path = source.Split('\\');
            string k = "", pr, v = "";
            long o = path.Length;
            bool c = false;
            if (dest != null && dest.Contains(".")) c = true;
            if (path.Length > 1 && source.Contains("."))
            {
                for (int i = 0; i < path.Length - 1; i++) { k += path[i]; if (i < path.Length - 2) k += '\\'; }
                for (int i = 0; i < Program.currentPath.Length; i++) { if ((Program.currentPath[i] >= 'A' && Program.currentPath[i] <= 'z') || (Program.currentPath[i] >= 45 && Program.currentPath[i] <= 58)) v += Program.currentPath[i]; }
                k.Trim(); v.Trim();
            }
            pr = path[path.Length - 1];
            int j = Program.current.searchDirectory(pr);
            int fc;
            int sz;
            Directory dd = changeMyCurrentDirectory(source, false, false);
            if ((dest != null && !dest.Contains('.')) && (source == dest) || (path.Length == 1 && j != -1 && dest == null) || (k == v && dd != null && dd.searchDirectory(pr) != -1))
            {
                Console.WriteLine("the file cannot be copied onto itself");
                return;
            }
            if (path.Length > 1 && c == false)
            {
                if (dest == null)
                {
                    if (dd != null)
                    {
                        source = pr;
                        j = dd.searchDirectory(source);
                        if (j != -1)
                        {
                            fc = FAT.GetEmptyCulster();
                            sz = dd.entries[j].dir_fileSize;
                            int x = Program.current.searchDirectory(pr);
                            if (x != -1)
                            {

                                Console.Write("The File is aleary existed, Do you want to overwrite it ?, please enter Y for yes or N for no:");
                                string choice = Console.ReadLine().ToLower();
                                if (choice.Equals("y"))
                                {

                                    int f = dd.entries[j].firs_cluster;
                                    string content = null;
                                    FILE file = new FILE(source, 0x0, f, Program.current, null, sz);
                                    file.ReadFile();
                                    content = file.content;
                                    FILE newFile = new FILE(source, 0X0, fc, Program.current, content, sz);
                                    newFile.writeFile();
                                    DirectoryEntry d = new DirectoryEntry(new string(source), 0X0, fc, sz);
                                    Program.current.entries.Add(d);
                                    Program.current.WriteDirectory();
                                    file.deleteFile();
                                    return;

                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                try
                                {
                                    int f = dd.entries[j].firs_cluster;
                                    string content = null;
                                    FILE file = new FILE(pr, 0x0, f, Program.current, content, sz);
                                    file.ReadFile();
                                    content = file.content;

                                    FILE newFile = new FILE(pr, 0X0, fc, Program.current, content, sz);
                                    newFile.writeFile();

                                    DirectoryEntry d = new DirectoryEntry(new string(pr), 0X0, fc, sz);
                                    Program.current.entries.Add(d);
                                    Program.current.WriteDirectory();
                                    Console.WriteLine("1 file(s) Copied");
                                    return;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"The file ${source} Is Not Existed In your disk");
                        }

                    }

                    else
                    {
                        Console.WriteLine($"The Path ${source} Is Not Existed In your disk");
                    }
                }
                else
                {
                    Directory dir = changeMyCurrentDirectory(source, false, false);
                    if (dir != null)
                    {
                        j = dir.searchDirectory(pr);
                        source = pr;
                        if (j != -1)
                        {
                            fc = FAT.GetEmptyCulster(); 
                            sz = dir.entries[j].dir_fileSize;  

                            Directory ddd = changeMyCurrentDirectory(dest, false, true);
                            if (ddd == null)
                            {
                                Console.WriteLine($"The Path {dest} does not exist.");
                                return;
                            }

                            int x = ddd.searchDirectory(pr);
                            if (x != -1)
                            {

                                Console.Write("The File is aleary existed, Do you want to overwrite it ?, please enter Y for yes or N for no:");
                                string choice = Console.ReadLine().ToLower();
                                if (choice.Equals("y"))
                                {
                                    int f = dir.entries[j].firs_cluster; 
                                    string content = null;
                                    FILE file = new FILE(pr, 0x0, f, ddd, content, sz);
                                    file.ReadFile(); 
                                    content = file.content;

                                    FILE newFile = new FILE(pr, 0x0, fc, ddd, content, sz);
                                    newFile.writeFile();


                                    DirectoryEntry d = new DirectoryEntry(pr, 0x0, fc, sz);
                                    ddd.entries.Add(d);
                                    ddd.WriteDirectory();
                                    file.deleteFile();
                                    Console.WriteLine(dest);
                                    return;
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                int f = dir.entries[j].firs_cluster; 
                                string content = null;
                                FILE file = new FILE(pr, 0x0, f, ddd, content, sz);
                                file.ReadFile();  
                                content = file.content;
                                FILE newFile = new FILE(pr, 0x0, fc, ddd, content, sz);
                                newFile.writeFile(); 

                                DirectoryEntry d = new DirectoryEntry(pr, 0x0, fc, sz);
                                ddd.entries.Add(d);
                                ddd.WriteDirectory();   

                                Console.WriteLine($"File {pr} copied successfully to {dest}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"The file {source} is not found in the current directory.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"The path {source} is not found in the current directory.");
                    }
                }

            }
            else
            {
                if (c)
                {
                    string[] h = dest.Split("\\");
                    string pt = h[h.Length - 1];
                    if (pt == pr)
                    {
                        Console.WriteLine("The file cannot be copied onto itself");
                        return;
                    }
                    else
                    {
                        Directory dir = changeMyCurrentDirectory(source, false, false);
                        if (dir != null)
                        {
                            fc = FAT.GetEmptyCulster();
                            sz = Program.current.entries[j].dir_fileSize;
                            int f = Program.current.entries[j].firs_cluster;
                            string content = null;
                            FILE file = new FILE(pt, 0x0, f, Program.current, content, sz);
                            file.ReadFile();
                            content = file.content;


                            FILE newFile = new FILE(pt, 0X0, fc, Program.current, content, sz);
                            newFile.writeFile();

                            DirectoryEntry d = new DirectoryEntry(new string(pt), 0X0, fc, sz);
                            Program.current.entries.Add(d);
                            Program.current.WriteDirectory();
                        }
                        else
                        {
                            Console.WriteLine($"The file {pr} Is not exist");
                        }
                    }
                }
                else
                {
                    if (j != -1)
                    {

                        fc = FAT.GetEmptyCulster();
                        sz = Program.current.entries[j].dir_fileSize;

                        Directory dir = changeMyCurrentDirectory(dest, false, true);
                        if (dir == null)
                        {
                            Console.WriteLine($"The Path {dest} Is not exist");
                            return;
                        }

                        int x = dir.searchDirectory(source);
                        if (x != -1)
                        {
                            Console.Write("The File is aleary existed, Do you want to overwrite it ?, please enter Y for yes or N for no:");
                            string choice = Console.ReadLine().ToLower();
                            if (choice.Equals("y"))
                            {

                                int f = Program.current.entries[j].firs_cluster;
                                string content = null;
                                FILE file = new FILE(source, 0x0, f, dir, content, sz);
                                file.ReadFile();
                                content = file.content;

                                FILE newFile = new FILE(source, 0X0, fc, dir, content, sz);
                                newFile.writeFile();

                                DirectoryEntry d = new DirectoryEntry(new string(source), 0X0, fc, sz);
                                dir.entries.Add(d);
                                dir.WriteDirectory();
                                file.deleteFile();


                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {

                            int f = Program.current.entries[j].firs_cluster;
                            string content = null;
                            FILE file = new FILE(source, 0x0, f, dir, content, sz);
                            file.ReadFile();
                            content = file.content;


                            FILE newFile = new FILE(source, 0X0, fc, dir, content, sz);
                            newFile.writeFile();

                            DirectoryEntry d = new DirectoryEntry(new string(source), 0X0, fc, sz);
                            dir.entries.Add(d);
                            dir.WriteDirectory();

                        }
                    }
                    else
                    {
                        Console.WriteLine($"The File ${source} Is Not Existed In your disk");
                    }
                }
            }
        }


        public static char[] getProperFileName(char[] fname, char[] extension)        {
            char[] dir_name = new char[11];

            int length = fname.Length, count = 0, lenOfEx = extension.Length;
            if (fname.Length >= 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    dir_name[count] = fname[i];
                    count++;
                }
                dir_name[count] = '.';
                count++;

            }
            else if (length < 7)
            {
                for (int i = 0; i < length; i++)
                {
                    dir_name[count] = fname[i];
                    count++;
                }
                for (int i = 0; i < 7 - length; i++)
                {
                    dir_name[count] = '_';
                    count++;
                }
                dir_name[count] = '.';
                count++;
            }
            for (int i = 0; i < lenOfEx; i++)
            {
                dir_name[count] = extension[i];
                count++;
            }
            for (int i = 0; i < 3 - lenOfEx; i++)
            {
                dir_name[count] = ' ';
                count++;
            }
            return dir_name;
        }

        public static char[] getProperDirName(char[] name)
        {
            char[] dir_name = new char[11];

            if (name.Length <= 11)
            {
                int j = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    j++;
                    dir_name[i] = name[i];
                }
                for (int i = ++j; i < dir_name.Length; i++)
                {
                    dir_name[i] = ' ';
                }
            }
            else
            {
                int j = 0;
                for (int i = 0; i < 11; i++)
                {
                    j++;
                    dir_name[i] = name[i];
                }
            }
            return dir_name;
        }


    }

}
