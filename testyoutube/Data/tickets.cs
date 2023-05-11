using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace testyoutube.Data
{
    public class Tickets
    {
        [Key]
        public int ID_ticket { get; set; }
        public string ID_utilisateur { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public int ID_statut { get; set; }
        public int ID_panne { get; set; }
        public DateTime Date_creation { get; set; }
        public DateTime Date_modif { get; set; }
        public int ID_conversation { get; set; }

        [ForeignKey("ID_statut")]
        public Statut Statut { get; set; }

        [ForeignKey("ID_panne")]
        public Panne Panne { get; set; }

    }
}
