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
    }
}
