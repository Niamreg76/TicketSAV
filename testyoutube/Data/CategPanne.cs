using System.ComponentModel.DataAnnotations;

namespace testyoutube.Data
{
    public class CategPanne
    {
        [Key]
        public int ID_CategPanne { get; set; }
        public string Nom { get; set; }
    }
}