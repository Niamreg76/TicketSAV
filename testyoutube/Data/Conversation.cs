using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using testyoutube.Data;

namespace testyoutube.Data
{
    public class Conversation
    {
        [Key]
        public int ID_conversation { get; set; }
        public int ID_ticket { get; set; }

        [ForeignKey("ID_ticket")]
        public Tickets Ticket { get; set; }

    }
}
