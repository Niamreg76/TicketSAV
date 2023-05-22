using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testyoutube.Data
{
    public class Panne
    {
        [Key]
        public int ID_panne { get; set; }
        public string Description { get; set; }
        public int ID_categPanne { get; set; }
        [ForeignKey("ID_categPanne")]
        public CategPanne CategPanne { get; set; }
    }
}