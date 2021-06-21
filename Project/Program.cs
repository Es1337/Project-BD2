using System;

namespace Project

{
    class Program
    {
        public static ProjectDBDataContext db;

        public static void Main(String[] args)
        {
            Console.WriteLine("Connecting...");

            db = new ProjectDBDataContext();
            if (db.DatabaseExists())
            {
                Console.WriteLine("Connection established");
                FamilyTreeController controller = new FamilyTreeController(db);
                UserInterface ui = new UserInterface(controller);

                ui.PrintConnectionInfo(db.Connection.ConnectionString);

                controller.PrintAllFamilies();
                ui.Run();
            }

            else
            {
                Console.WriteLine("Connection failed. Press any key...");
                Console.ReadKey();
                return;
            }
        }
    }
}

