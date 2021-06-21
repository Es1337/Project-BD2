using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Project
{
    public class PersonModel
    {
        [Required]
        public int id { get; set; }

        public string names { get; set; }

        public string surname { get; set; }

        public string born { get; set; }

        public string died { get; set; }

        public int mother { get; set; }

        public int father { get; set; }

        public XElement node { get; set; }

        public PersonModel(XElement setNode, int setID, string setNames, string setSurname, string setBorn, string setDied, int setMother, int setFather)
        {
            this.node = setNode;
            this.id = setID;
            this.names = setNames;
            this.surname = setSurname;
            this.born = setBorn;
            this.died = setDied;
            this.mother = setMother;
            this.father = setFather;
        }

        public PersonModel(XElement setNode, int setID, string setNames, string setSurname)
        {
            this.node = setNode;
            this.id = setID;
            this.names = setNames;
            this.surname = setSurname;
            this.born = "";
            this.died = "";
            this.mother = -1;
            this.father = -1;
        }

        public PersonModel(XElement setNode, int setID, string setNames, string setSurname, string setBorn)
        {
            this.node = setNode;
            this.id = setID;
            this.names = setNames;
            this.surname = setSurname;
            this.born = setBorn;
            this.died = "";
            this.mother = -1;
            this.father = -1;
        }

        public PersonModel(XElement setNode, int setID, string setNames, string setSurname, string setBorn, int setMother, int setFather)
        {
            this.node = setNode;
            this.id = setID;
            this.names = setNames;
            this.surname = setSurname;
            this.born = setBorn;
            this.died = "";
            this.mother = setMother;
            this.father = setFather;
        }
    }
}
