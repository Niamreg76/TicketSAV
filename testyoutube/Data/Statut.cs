using System.ComponentModel.DataAnnotations;

namespace testyoutube.Data
{
    public class Statut
    {
        [Key]
        public int ID_statut { get; set; }
        public string Nom { get; set; }
    }
}