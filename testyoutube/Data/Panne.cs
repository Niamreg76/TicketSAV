using System.ComponentModel.DataAnnotations;

namespace testyoutube.Data
{
    public class Panne
    {
        [Key]
        public int ID_panne { get; set; }
        public string Description { get; set; }
    }
}