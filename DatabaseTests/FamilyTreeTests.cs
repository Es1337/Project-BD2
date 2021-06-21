using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project;
using System;

namespace DatabaseTests
{
    [TestClass]
    public class FamilyTreeTests
    {
        static ProjectDBDataContext db = new ProjectDBDataContext();
        static FamilyTreeController controller = new FamilyTreeController(db);

        [TestMethod]
        public void Test1_AddNewFamilyTree_CreatesWithProperName()
        {
            string expectedFamilyName = "Test Family";

            bool treeAdded = controller.AddNewFamilyTree(expectedFamilyName);

            FamilyTreeModel familyTree = controller.GetFamilyByName(expectedFamilyName);
            Assert.AreEqual(true, treeAdded);
            Assert.AreEqual(expectedFamilyName, familyTree.name);
        }

        [TestMethod]
        public void Test2_AddNewFamilyTree_WithExistingName_ShouldNotAdd()
        {
            string expectedFamilyName = "Test Family";

            bool treeAdded = controller.AddNewFamilyTree(expectedFamilyName);

            Assert.AreEqual(false, treeAdded);
        }

        [TestMethod]
        public void Test3_AddPersonToFamily_WithNoParents_AddsProperly()
        {
            string familyName = "Test Family";
            string expectedName = "Test";
            string expectedSurname = "Person";
            string expectedDateOfBirth = "1000-01-01";
            string expectedDateOfDeath = "2000-12-31";

            bool personAdded = controller.AddPersonToFamily(familyName, expectedName, expectedSurname, expectedDateOfBirth, expectedDateOfDeath, -1, -1);

            PersonModel person = controller.GetPersonByName(familyName, expectedName, expectedSurname);

            Assert.AreEqual(true, personAdded);
            Assert.AreEqual(expectedName, person.names);
            Assert.AreEqual(expectedSurname, person.surname);
            Assert.AreEqual(expectedDateOfBirth, person.born);
            Assert.AreEqual(expectedDateOfDeath, person.died);
        }

        [TestMethod]
        public void Test4_AddPersonToFamily_WithParents_AddsProperly()
        {
            string familyName = "Test Family";
            string expectedName = "Test";
            string expectedSurname = "Person2";
            string expectedDateOfBirth = "1000-01-01";
            string expectedDateOfDeath = "2000-12-31";
            controller.AddPersonToFamily(familyName, "Test", "Person3", "1000-01-01", expectedDateOfDeath, -1, -1);
            PersonModel mother = controller.GetPersonByName(familyName, "Test", "Person");
            PersonModel father = controller.GetPersonByName(familyName, "Test", "Person3");

            bool personAdded = controller.AddPersonToFamily(familyName, expectedName, expectedSurname, expectedDateOfBirth, expectedDateOfDeath, mother.id, father.id);

            PersonModel person = controller.GetPersonByName(familyName, expectedName, expectedSurname);
            Assert.AreEqual(true, personAdded);
            Assert.AreEqual(expectedName, person.names);
            Assert.AreEqual(expectedSurname, person.surname);
            Assert.AreEqual(expectedDateOfBirth, person.born);
            Assert.AreEqual(expectedDateOfDeath, person.died);
            Assert.AreEqual(mother.id, person.mother);
            Assert.AreEqual(father.id, person.father);
        }

        [TestMethod]
        public void Test5_AddPersonToFamily_WithExistingNames_ShouldNotAdd()
        {
            string familyName = "Test Family";
            string expectedName = "Test";

            bool personAdded = controller.AddPersonToFamily(
                familyName,
                expectedName,
                "Person",
                "1000-01-01",
                "2000-12-31",
                -1,
                -1);

            Assert.AreEqual(false, personAdded);
        }

        [TestMethod]
        public void Test6_SetParent_SetsProperly()
        {
            string familyName = "Test Family";
            string expectedName = "Test";
            string expectedSurname = "Person4";

            controller.AddPersonToFamily(
                familyName,
                expectedName,
                expectedSurname,
                "1000-01-01",
                "2000-12-31",
                -1,
                -1);

            PersonModel parent = controller.GetPersonByName(familyName, expectedName, expectedSurname);
            bool success = controller.SetParent(familyName, expectedName, "Person", "Mother", parent.id);
            PersonModel child = controller.GetPersonByName(familyName, expectedName, "Person");

            Assert.AreEqual(true, success);
            Assert.AreEqual(parent.id, child.mother);
        }

        [TestMethod]
        public void Test7_DeletePerson_DeletesProperly()
        {
            string familyName = "Test Family";
            string expectedName = "Test";
            string expectedSurname = "Person4";

            bool success = controller.DeletePersonByName(familyName, expectedName, expectedSurname);

            Assert.AreEqual(true, success);
        }

        [TestMethod]
        public void Test8_DeletePerson_WhenDoesntExist_ShoudlThrowInvalidOperation()
        {
            string familyName = "Test Family";
            string expectedName = "Test";
            string expectedSurname = "Person4";

            Assert.ThrowsException<InvalidOperationException>(() =>
                controller.DeletePersonByName(familyName, expectedName, expectedSurname));
        }

        [TestMethod]
        public void Test9_DeleteFamilyTree_DeletesProperly()
        {
            string familyName = "Test Family";

            bool success = controller.DeleteFamilyTreeByName(familyName);

            Assert.AreEqual(true, success);
        }
    }
}
