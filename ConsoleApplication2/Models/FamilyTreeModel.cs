using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Project
{
    public class FamilyTreeModel
    {
        [Required]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public XElement tree { get; set; }

        public FamilyTreeModel(int setID, string setName, XElement setTree)
        {
            id = setID;
            name = setName;
            tree = setTree;
        }
    }
}
