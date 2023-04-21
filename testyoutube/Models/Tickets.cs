using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testyoutube.Models
{
    public class Tickets
    {
        public int ID_ticket { get; set; }
        public string ID_utilisateur { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public int ID_Statut { get; set; }
        public DateTime Date_creation { get; set; }
        public DateTime Date_modif_statut { get; set; }
    }
}
