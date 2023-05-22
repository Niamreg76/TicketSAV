using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using testyoutube.Areas.Identity.Data;

namespace testyoutube.Data
{
    public class Message
    {
        [Key]
        public int ID_message { get; set; }
        public string ID_utilisateur { get; set; }
        public string Contenu { get; set; }
        public DateTime DateMessage { get; set; }
        public int ID_conversation { get; set; }

        [ForeignKey("ID_utilisateur")]
        public aspnetusers User { get; set; }

        [ForeignKey("ID_conversation")]
        public Conversation Conversation { get; set; }
    }
}
