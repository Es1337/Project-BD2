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

                string connectionString = db.Connection.ConnectionString;
                ui.PrintConnectionInfo(connectionString);

                controller.PrintAllFamilies();

                ui.Run();
            }

            else
            {
                Console.WriteLine("Connection failed. Press any key...");
                Console.ReadKey();
                return;
            }

            //controller.AddPersonToFamily("Nowak", "Łukasz Mariusz", "Nowak", "2012-01-01", "", 2, 1);
            //controller.AddPersonToFamily("Nowak", "Giulia", "Rossi", "1995-01-01", "", -1, -1);
            //controller.AddPersonToFamily("Nowak", "Brajan", "Nowak", "2018-01-01", "", 5, 1);
            //controller.DeletePersonByName("Nowak", "Łukasz Mariusz", "Nowak");
            //controller.DeletePersonByName("Nowak", "Giulia", "Rossi");
            //controller.SetParent("Nowak", "Brajan", "Nowak", "Mother", controller.GetPersonByName("Nowak", "Giulia", "Rossi").id);
            //controller.AddNewFamilyTree("Kowalski");
            //controller.AddPersonToFamily("Kowalski", "Jan", "Kowalski", "1960-01-01", "", -1, -1);
        }
    }
}

