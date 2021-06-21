using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Project
{
    public class FamilyTreeController
    {
        private ProjectDBDataContext db { get; set; }
        public FamilyTreeController(ProjectDBDataContext setDB)
        {
            db = setDB;
        }

        private bool FamilyExists(string familyName)
        {
            var families = from family in db.FamilyTrees
                           where family.FamilyName.Equals(familyName)
                           select family;

            if (families.Count() != 0)
                return true;

            return false;
        }

        private bool PersonExists(string familyName, string name, string surname)
        {
            FamilyTreeModel targetFamily = GetFamilyByName(familyName);
            List<PersonModel> familyMembers = GetAllPeopleInFamily(targetFamily);
            if (familyMembers.Where(member =>
                member.names == name
                && member.surname == surname)
                .Count() != 0)
                return true;

            return false;
        }

        private static bool ElementExists(XElement source, string targetName)
        {
            var foundEl = source.Element(targetName);
            if (foundEl != null)
                return true;

            return false;
        }

        /// <summary>
        /// Method getting all family trees from database
        /// </summary>
        /// <returns>
        /// List of <typeparamref name="FamilyTreeModel"/>s of all the families
        /// </returns>
        public List<FamilyTreeModel> GetFamilies()
        {
            List<FamilyTreeModel> result = new List<FamilyTreeModel>();

            var families = from family in this.db.FamilyTrees
                           select new
                           {
                               id = family.ID,
                               name = family.FamilyName,
                               tree = family.FamilyTree
                           };

            foreach (var family in families)
            {
                result.Add(new FamilyTreeModel(family.id, family.name, family.tree));
            }

            return result;
        }

        /// <summary>
        /// Method finds a family by it's name
        /// </summary>
        /// <param name="targetFamilyName"></param>
        /// <returns>
        /// <typeparamref name="FamilyTreeModel"/> of the family
        /// </returns>
        public FamilyTreeModel GetFamilyByName(string targetFamilyName)
        {
            try
            {
                var families = from family in this.db.FamilyTrees
                               where family.FamilyName.Equals(targetFamilyName)
                               select new
                               {
                                   id = family.ID,
                                   name = family.FamilyName,
                                   tree = family.FamilyTree
                               };
                var targetFamily = families.Single();
                return new FamilyTreeModel(targetFamily.id, targetFamily.name, targetFamily.tree);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new FamilyTreeModel(-1, "", new XElement("Error"));
            }
        }

        /// <summary>
        /// This method gets all people in a given family tree
        /// </summary>
        /// <param name="family"><typeparamref name="FamilyTreeModel"/> of a family</param>
        /// <returns>List of <typeparamref name="PersonModel"/>s containing all of the people inside a family</returns>
        public List<PersonModel> GetAllPeopleInFamily(FamilyTreeModel family)
        {
            List<PersonModel> result = new List<PersonModel>();
            var familyMembers = from person in family.tree.Element("People").Elements("Person")
                                select new
                                {
                                    person = person,
                                    id = person.Attribute("id").Value,
                                    names = person.Element("Names").Value.Trim(),
                                    surname = person.Element("Surname").Value.Trim(),

                                    born = ElementExists(person, "Born") ?
                                           person.Element("Born").Value.Trim() : "",

                                    died = ElementExists(person, "Died") ?
                                           person.Element("Died").Value.Trim() : "",

                                    mother = ElementExists(person, "Mother") ?
                                         (string)person.Element("Mother").Attribute("id") : "-1",

                                    father = ElementExists(person, "Father") ?
                                         (string)person.Element("Father").Attribute("id") : "-1"
                                };

            foreach (var person in familyMembers)
            {
                result.Add(new PersonModel(person.person, int.Parse(person.id), person.names, person.surname, person.born, person.died, int.Parse(person.mother), int.Parse(person.father)));
            }

            return result;
        }

        /// <summary>
        /// This method gets a person by name in a given family
        /// </summary>
        /// <param name="targetFamilyName"></param>
        /// <param name="names">name of the person</param>
        /// <param name="surname">surname of the person</param>
        /// <returns><typeparamref name="PersonModel"/> of the found person</returns>
        public PersonModel GetPersonByName(string targetFamilyName, string names, string surname)
        {
            FamilyTreeModel targetFamily = GetFamilyByName(targetFamilyName);
            List<PersonModel> familyMembers = GetAllPeopleInFamily(targetFamily);

            return familyMembers.Where(member => member.names == names && member.surname == surname).Single();
        }

        private static void PrintPretty(XElement tree, XElement rootPerson, string indent, bool last)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            string output = "";

            output += rootPerson.Element("Names").Value.Trim()
                        + " " + rootPerson.Element("Surname").Value.Trim()
                        + " ( *| " + rootPerson.Element("Born").Value.Trim()
                        + " +| ";

            if (ElementExists(rootPerson, "Died"))
                output += rootPerson.Element("Died").Value.Trim();
            else
                output += "-";

            output += " )";

            Console.WriteLine(output);

            var qGetChildren = from person in tree.Descendants("Person")
                               where ElementExists(person, "Father") || ElementExists(person, "Mother")
                               select new
                               {
                                   data = (XElement)person
                               };

            List<XElement> Children = new List<XElement>();
            foreach (var person in qGetChildren)
            {
                if (ElementExists(person.data, "Father"))
                {
                    if (person.data.Element("Father").Attribute("id").Value == rootPerson.Attribute("id").Value)
                    {
                        Children.Add(person.data);
                    }
                }
                if (ElementExists(person.data, "Mother"))
                {
                    if (person.data.Element("Mother").Attribute("id").Value == rootPerson.Attribute("id").Value)
                    {
                        Children.Add(person.data);
                    }
                }
            }

            int i = 0;
            foreach (var child in Children)
            {
                PrintPretty(tree, child, indent, i == Children.Count() - 1);
                i++;
            }
        }

        /// <summary>
        /// Method printing a given family in a tree fashion
        /// </summary>
        /// <param name="familyName"></param>
        public void PrintOneFamilyByName(string familyName)
        {
            FamilyTreeModel family = GetFamilyByName(familyName);
            List<PersonModel> familyMembers = GetAllPeopleInFamily(family);
            Console.WriteLine("\n" + "--- " + family.name + " ---");
            foreach (var person in familyMembers)
            {
                PrintPretty(family.tree, person.node, "", true);
            }
        }

        /// <summary>
        /// Method printing all families in a tree fashion
        /// </summary>
        public void PrintAllFamilies()
        {
            List<FamilyTreeModel> families = GetFamilies();

            foreach (var family in families)
            {
                PrintOneFamilyByName(family.name);
            }
        }

        private int GetNewID(FamilyTreeModel family)
        {
            List<PersonModel> familyMembers = this.GetAllPeopleInFamily(family);
            HashSet<int> memberIDs = new HashSet<int>();

            foreach (var person in familyMembers)
            {
                memberIDs.Add(person.id);
            }

            if (memberIDs.Contains(familyMembers.Count() + 1))
            {
                for (int i = 1; i < familyMembers.Count() + 1; i++)
                {
                    if (!memberIDs.Contains(i))
                        return i;
                }
            }

            return familyMembers.Count() + 1;
        }

        private static void AddXmlElementWithValue(XElement target, string elementName, object value)
        {
            XElement newElement = new XElement(elementName);
            newElement.SetValue(value);
            target.Add(newElement);
        }

        private XElement CreateNewPerson(FamilyTreeModel targetFamily, string names, string surname, string born, string died, int mother, int father)
        {
            XElement newPerson = new XElement("Person", new XAttribute("id", GetNewID(targetFamily)));
            AddXmlElementWithValue(newPerson, "Names", names);
            AddXmlElementWithValue(newPerson, "Surname", surname);
            AddXmlElementWithValue(newPerson, "Born", born);
            if (!died.Equals(""))
                AddXmlElementWithValue(newPerson, "Died", died);
            if (!mother.Equals(-1))
                newPerson.Add(new XElement("Mother", new XAttribute("id", mother)));
            if (!father.Equals(-1))
                newPerson.Add(new XElement("Father", new XAttribute("id", father)));

            return newPerson;
        }

        /// <summary>
        /// This method adds a family tree with a given name to the database.
        /// Family with this name must not exist.
        /// </summary>
        /// <param name="familyName">Name of the family</param>
        /// <returns>bool saying if the operation was performed successfully</returns>
        public bool AddNewFamilyTree(string familyName)
        {
            bool success = false;
            if (!FamilyExists(familyName))
            {
                FamilyTrees newTreeRecord = new FamilyTrees();
                newTreeRecord.FamilyName = familyName;
                XElement newFamilyTree = new XElement("Family",
                                                        new XElement("People"),
                                                        new XElement("Marriages"));
                newTreeRecord.FamilyTree = newFamilyTree;

                db.FamilyTrees.InsertOnSubmit(newTreeRecord);
                try
                {
                    db.SubmitChanges();
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }

        /// <summary>
        /// This method deletes a family tree with the given name from the database
        /// </summary>
        /// <param name="familyName"></param>
        /// <returns>bool saying if the operation was performed successfully</returns>
        public bool DeleteFamilyTreeByName(string familyName)
        {
            bool success = false;
            var families = from family in this.db.FamilyTrees
                           where family.FamilyName.Equals(familyName)
                           select family;
            var targetFamily = families.Single();

            db.FamilyTrees.DeleteOnSubmit(targetFamily);

            try
            {
                db.SubmitChanges();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return success;
        }

        /// <summary>
        /// This method adds a person to the database in a given family tree
        /// </summary>
        /// <param name="targetFamilyName">name of the family where the person is added</param>
        /// <param name="names">name of the person</param>
        /// <param name="surname">surname of the person</param>
        /// <param name="born">date of birth</param>
        /// <param name="died">date of death</param>
        /// <param name="mother">database id of person's mother</param>
        /// <param name="father">database id of person's father</param>
        /// <returns>bool saying if the operation was performed successfully</returns>
        public bool AddPersonToFamily(string targetFamilyName, string names, string surname, string born, string died, int mother, int father)
        {
            bool success = false;
            if (!PersonExists(targetFamilyName, names, surname))
            {
                FamilyTreeModel targetFamily = GetFamilyByName(targetFamilyName);

                XElement newPerson = this.CreateNewPerson(targetFamily, names, surname, born, died, mother, father);
                targetFamily.tree.Descendants("People").Single().Add(newPerson);

                var qUpdatedTree = (from family in db.FamilyTrees
                                    where family.ID == targetFamily.id
                                    select family).Single();

                qUpdatedTree.FamilyTree = targetFamily.tree;
                try
                {
                    db.SubmitChanges();
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }

        /// <summary>
        /// This method sets parent id of a given person of a given parent.
        /// </summary>
        /// <param name="targetFamilyName">given family name</param>
        /// <param name="names">name of a person</param>
        /// <param name="surname">surname of a person</param>
        /// <param name="whichParent">Mother/Father</param>
        /// <param name="parentID">person database id to set as a parent</param>
        /// <returns>bool saying if the operation was performed successfully</returns>
        public bool SetParent(string targetFamilyName, string names, string surname, string whichParent, int parentID)
        {
            FamilyTreeModel targetFamily = GetFamilyByName(targetFamilyName);
            PersonModel targetPerson = GetPersonByName(targetFamilyName, names, surname);

            XElement newPersonData;
            if (whichParent.Equals("Mother"))
            {
                newPersonData =
                CreateNewPerson(
                    targetFamily,
                    targetPerson.names,
                    targetPerson.surname,
                    targetPerson.born,
                    targetPerson.died,
                    parentID,
                    targetPerson.father);
            }
            else
            {
                newPersonData =
                CreateNewPerson(
                    targetFamily,
                    targetPerson.names,
                    targetPerson.surname,
                    targetPerson.born,
                    targetPerson.died,
                    targetPerson.mother,
                    parentID);
            }
            newPersonData.Attribute("id").SetValue(targetPerson.id);

            targetFamily.tree
                .Descendants("Person")
                .Where(person => (int)person.Attribute("id") == targetPerson.id)
                .Single()
                .Remove();

            targetFamily.tree
                .Descendants("People")
                .Single()
                .Add(newPersonData);

            var qUpdatedTree = (from family in db.FamilyTrees
                                where family.ID == targetFamily.id
                                select family).Single();
            qUpdatedTree.FamilyTree = targetFamily.tree;

            bool success = false;
            try
            {
                db.SubmitChanges();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return success;
        }

        /// <summary>
        /// This method deletes a person from the database
        /// </summary>
        /// <param name="targetFamilyName">family name</param>
        /// <param name="names">name of the person</param>
        /// <param name="surname">surname of the person</param>
        /// <returns>bool saying if the operation was performed successfully</returns>
        public bool DeletePersonByName(string targetFamilyName, string names, string surname)
        {
            FamilyTreeModel targetFamily = GetFamilyByName(targetFamilyName);
            PersonModel targetPerson = GetPersonByName(targetFamilyName, names, surname);

            List<PersonModel> familyMembers = GetAllPeopleInFamily(targetFamily);

            foreach (var member in familyMembers)
            {
                if (member.mother == targetPerson.id)
                {
                    targetFamily.tree
                        .Descendants("Person")
                        .Where(person => (int)person.Attribute("id") == member.id)
                        .Single()
                        .Element("Mother")
                        .Remove();
                }
                if (member.father == targetPerson.id)
                {
                    targetFamily.tree
                        .Descendants("Person")
                        .Where(person => (int)person.Attribute("id") == member.id)
                        .Single()
                        .Element("Father")
                        .Remove();
                }
            }

            targetFamily.tree
                .Descendants("Person")
                .Where(person => (int)person.Attribute("id") == targetPerson.id)
                .Remove();


            var qUpdatedTree = (from family in db.FamilyTrees
                                where family.ID == targetFamily.id
                                select family).Single();
            qUpdatedTree.FamilyTree = targetFamily.tree;

            bool success = false;
            try
            {
                db.SubmitChanges();
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return success;
        }
    }
}
