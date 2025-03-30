namespace simple_Shell
{
    public struct Token
    {
        public string command;
        public string value;
        public string sec_value;
        public string third_value;
    }

    class Program
    {
        

        public static string PATH_ON_PC = "C:\\Users\\ahmed\\OneDrive\\Desktop\\lastyOS\\FAT-system-master\\miniFat.txt";
        public static Directory current;
        public static string currentPath;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcom to OS project\n\n");
            VirtualDisk.initialize(PATH_ON_PC);


            currentPath = new string(current.dir_name);
            currentPath = currentPath.Trim();




          


            Parser parser = new Parser();



            while (true)
            {
                var currentLocation = currentPath;
                Console.Write(currentLocation + "\\>");
                current.ReadDirectory();

                string input = Console.ReadLine();
                parser.parse_input(input);
            }
        }
    }
}
