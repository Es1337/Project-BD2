using System;
using System.Text.RegularExpressions;

namespace Project
{
    class UserInterface
    {
        private static FamilyTreeController controller;

        public UserInterface(FamilyTreeController setController)
        {
            controller = setController;
        }

        public void PrintConnectionInfo(string connectionString)
        {
            Console.WriteLine();
            Console.WriteLine("==================================");
            string dataSource = connectionString.Substring(
                connectionString.IndexOf("Data Source=") + "Data Source=".Length,
                connectionString.IndexOf(";") - "Data Source=".Length);
            string initialCatalog = connectionString.Substring(
                connectionString.IndexOf("Initial Catalog=") + "Initial Catalog=".Length,
                connectionString.IndexOf(";") - "Initial Catalog=".Length - 1);
            Console.WriteLine("Server Instance: " + dataSource);
            Console.WriteLine("Database: " + initialCatalog);
            Console.WriteLine("==================================");
        }

        private static void PrintInfo()
        {
            Console.WriteLine();
            Console.WriteLine("1 - Add Family Tree");
            Console.WriteLine("2 - Delete Family Tree");
            Console.WriteLine("3 - Add Person To Tree");
            Console.WriteLine("4 - Delete Person From Tree");
            Console.WriteLine("5 - Update Parent");
            Console.WriteLine("6 - Print All Trees");
            Console.WriteLine("7 - Print Selected Tree");
            Console.WriteLine("Q - Exit");
            Console.WriteLine("========");
            Console.WriteLine();
        }

        private static void AddTree()
        {
            Console.WriteLine("Set Family Name: ");
            string name = Console.ReadLine();
            try
            {
                bool success = controller.AddNewFamilyTree(name);
                string output = "Family: " + name;
                if (success)
                    Console.WriteLine(output + " added successfully.");
                else
                    Console.WriteLine(output + " exists or database error.");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void DeleteTree()
        {
            Console.WriteLine("Set Family Name: ");
            string name = Console.ReadLine();

            controller.DeleteFamilyTreeByName(name);
        }
        private static int FindPerson(string familyName, string who)
        {
            Console.WriteLine(who + " First Name: ");
            string personName = Console.ReadLine();

            Console.WriteLine(who + " Last Name: ");
            string personSurname = Console.ReadLine();
            PersonModel person;
            try
            {
                person = controller.GetPersonByName(familyName, personName, personSurname);
                return person.id;
            }
            catch (Exception e)
            {
                Console.WriteLine(who + " Not found. Try again.");
                return -1;
            }
        }

        private static void AddPerson()
        {
            Console.WriteLine("Set Family Name: ");
            string familyName = Console.ReadLine();

            Console.WriteLine("Set Person First Name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Set Person Last Name: ");
            string lastName = Console.ReadLine();

            bool settingDOB = true;
            string born = "";
            while (settingDOB)
            {
                settingDOB = false;
                Console.WriteLine("Set Date Of Birth");
                Console.WriteLine("Format: yyyy-mm-dd");
                born = Console.ReadLine();
                if (born == "")
                {
                    Console.WriteLine("Date Of Birth has to be set");
                    settingDOB = true;
                }
                if (!Regex.IsMatch(born, @"^\d{4}-\d{2}-\d{2}$"))
                {
                    Console.WriteLine("Date Of Birth has to be of format yyyy-mm-dd");
                    settingDOB = true;
                }
            }

            bool settingDOD = true;
            string died = "";
            while (settingDOD)
            {
                settingDOD = false;
                Console.WriteLine("Set Date Of Death");
                Console.WriteLine("Format: yyyy-mm-dd or empty");
                died = Console.ReadLine();
                if (!Regex.IsMatch(died, @"^\d{4}-\d{2}-\d{2}$") && died != "")
                {
                    Console.WriteLine("Date Of Death has to be of format yyyy-mm-dd");
                    settingDOD = true;
                }
            }

            int motherID = -1;
            Console.WriteLine("Do you want to set person's mother? yes/no");
            string findMother = Console.ReadLine();
            if (findMother == "yes")
            {
                while (true)
                {
                    motherID = FindPerson(familyName, "Mother");
                    if (motherID != -1)
                        break;
                }
            }

            int fatherID = -1;
            Console.WriteLine("Do you want to set person's father? yes/no");
            string findFather = Console.ReadLine();
            if (findFather == "yes")
            {
                while (true)
                {
                    fatherID = FindPerson(familyName, "Father");
                    if (fatherID != -1)
                        break;
                }
            }

            bool success = controller.AddPersonToFamily(familyName, firstName, lastName, born, died, motherID, fatherID);
            string output = "Person: " + firstName + " " + lastName + " in family " + familyName;
            if (success)
                Console.WriteLine(output + " added successfully.");
            else
                Console.WriteLine(output + " already exists or database error.");
        }

        private static void DeletePerson()
        {
            Console.WriteLine("Set Family Name: ");
            string familyName = Console.ReadLine();

            Console.WriteLine("Set Person First Name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Set Person Last Name: ");
            string lastName = Console.ReadLine();

            try
            {
                controller.DeletePersonByName(familyName, firstName, lastName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Person not found.");
            }

        }

        private static void UpdateParent()
        {
            Console.WriteLine("Family Name: ");
            string familyName = Console.ReadLine();

            Console.WriteLine("SPerson First Name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Person Last Name: ");
            string lastName = Console.ReadLine();

            Console.WriteLine("Which Parent Do You Want To Set? Mother/Father");
            string who = Console.ReadLine();

            int parentID = -1;
            while (true)
            {
                parentID = FindPerson(familyName, who);
                if (parentID != -1)
                    break;
            }

            controller.SetParent(familyName, firstName, lastName, who, parentID);
        }

        private static void PrintOneTree()
        {
            Console.WriteLine("Set Family Name: ");
            string familyName = Console.ReadLine();

            controller.PrintOneFamilyByName(familyName);
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                PrintInfo();
                string input = Console.ReadKey().Key.ToString();
                Console.WriteLine();
                switch (input)
                {
                    case "D1":
                        AddTree();
                        break;
                    case "D2":
                        DeleteTree();
                        break;
                    case "D3":
                        AddPerson();
                        break;
                    case "D4":
                        DeletePerson();
                        break;
                    case "D5":
                        UpdateParent();
                        break;
                    case "D6":
                        controller.PrintAllFamilies();
                        break;
                    case "D7":
                        PrintOneTree();
                        break;
                    case "Q":
                        running = false;
                        break;
                }
            }
        }

    }
}
