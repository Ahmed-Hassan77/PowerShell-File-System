﻿namespace simple_Shell
{
    class Help
    {
        private string help_cd = "cd         - Change the current default directory to .\n             If the argument is not present, report the current directory.\n             If the directory does not exist an appropriate error should be reported.\n";
        private string help_dir = "dir        - List the contents of directory.\n";
        private string help_cls = "cls        - Clear the shell content.\n";
        private string help_quit = "quit       - Quit the shell.\n";
        private string help_copy = "copy       - Copies one or more files to another location.\n";
        private string help_del = "del        - Deletes one or more files.\n";
        private string help_help = "help       - Provides Help information for commands.\n";
        private string help_md = "md         - Creates a directory.\n";
        private string help_rd = "rd         - Removes a directory.\n";
        private string help_rename = "rename     - Renames a file.\n";
        private string help_type = "type       - Displays the contents of a text file.\n";
        private string help_import = "import     - import text file(s) from your computer.\n";
        private string help_export = "export     - export text file(s) to your computer.\n";
        private string help_command;
        public Help(Token token)
        {
            help_command = help_cd + help_dir + help_cls + help_quit + help_copy + help_del + help_help + help_md + help_rd + help_rename + help_type + help_import + help_export;
            doHelp(token);
        }
        public Help() { }

        public void doHelp(Token token)
        {
            if (token.value == null)
            {
                Console.WriteLine(help_command);
                return;
            }
            switch (token.value)
            {
                case "cd":
                    Console.WriteLine("Displays the name of or changes the current directory.\n\n");
                    Console.WriteLine("CD [/D] [drive:][path]\n");
                    Console.WriteLine("CD [..]\n");
                    Console.WriteLine("  ..   Specifies that you want to change to the parent directory.\n");
                    Console.WriteLine("Type CD drive: to display the current directory in the specified drive.\n");
                    Console.WriteLine("Type CD without parameters to display the current drive and directory\n\n");
                    Console.WriteLine("If Command Extensions are enabled CHDIR changes as follows:\n");
                    Console.WriteLine("The current directory string is converted to use the same case as\n");
                    Console.WriteLine("the on disk names.  So CD C:\\TEMP would actually set the current\n");
                    Console.WriteLine("directory to C:\\Temp if that is the case on disk.\n");
                    Console.WriteLine(" For example:\n");
                    Console.WriteLine("   cd \\winnt\\profiles\\username\\programs\\start menu\n");
                    Console.WriteLine("\n");
                    break;
                case "dir":
                    Console.WriteLine("Displays a list of files and subdirectories in a directory.\n");
                    Console.WriteLine("The Syntax :\nDIR [d:][path][filename] [/A:(attributes)] [/O:(order)] [/B][/C][/CH][/L][/S][/P] [/W]\n");
                    Console.WriteLine("[/O[[:]sortorder]] [/P] [/Q] [/R] [/S] [/T[[:]timefield]] [/W] [/X] [/4]\n\n");
                    Console.WriteLine("[drive:][path][filename]\n");
                    Console.WriteLine("Specifies drive, directory, and/or files to list.\n");
                    Console.WriteLine("\n");

                    break;
                case "cls":
                    Console.WriteLine("Clears the screen.\n");
                    Console.WriteLine("CLS\n");
                    break;
                case "quit":
                    Console.WriteLine(help_quit);
                    Console.WriteLine("The Syntax :\nquit\n");
                    break;
                case "copy":
                    Console.WriteLine("Copies one or more files to another location.");
                    Console.WriteLine("copy command syntax is");
                    Console.WriteLine(" copy [source]");
                    Console.WriteLine("or");
                    Console.WriteLine(" copy [source] [destination]");
                    Console.WriteLine("[source] can be file Name (or fullpath of file) or directory Name (or fullpath of directory)");
                    Console.WriteLine("[destination] can be file Name (or fullpath of file) or directory name or fullpath of a directory");
                    break;
                case "del":
                    Console.WriteLine("Deletes one or more files.\n");
                    Console.WriteLine("DEL [/P] [/F] [/S] [/Q] [/A[[:]attributes]] names\n");
                    Console.WriteLine("If Command Extensions are enabled DEL and ERASE change as follows:\n");
                    Console.WriteLine("The display semantics of the /S switch are reversed in that it shows\n");
                    Console.WriteLine("you only the files that are deleted, not the ones it could not find.\n");

                    break;
                case "help":
                    Console.WriteLine("Provides help information for Windows commands.");
                    Console.WriteLine("help command syntax is");
                    Console.WriteLine(" help");
                    Console.WriteLine(" or");
                    Console.WriteLine(" for more information on a specific command, type help [command]");
                    Console.WriteLine("command - displays help information on that command.");
                    break;
                case "md":
                    Console.WriteLine("Creates a directory.\n");
                    Console.WriteLine("MD [drive:]path\n");
                    Console.WriteLine("\n");
                    break;
                case "rd":
                    Console.WriteLine("Removes a directory.\nNOTE: it confirms the user choice to delete the directory \nbefore deleting\nrd command syntax is\nrd [directory]+\n[directory] can be a directory name or fullpath of a \ndirectory\n+ after [directory] represent that you can pass more than \ndirectory name (or fullpath of directory)\nTest cases:");

                    break;
                case "rename":
                    Console.WriteLine("Renames a file or files.\n");
                    Console.WriteLine("RENAME [drive:][path]filename1 filename2\n");
                    Console.WriteLine("Note that you cannot specify a new drive or path for your destination file.\n");
                    break;
                case "type":
                    Console.WriteLine("Displays the contents of a text file or files.\n");
                    Console.WriteLine("TYPE [drive:][path]filename\n");
                    break;
                case "import":
                    Console.WriteLine("import text file(s) from your computer.\n");
                    Console.WriteLine("import [drive:][path]filename\n");
                    break;
                case "export":
                    Console.WriteLine("export text file(s) to your computer.\n");
                    Console.WriteLine("export [drive:][path]filename  [dest path]\n");
                    break;
                default:
                    Console.WriteLine($"{token.value}=> This Command is not supported by help utility ..");
                    break;
            }
        }
    }
}
