using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MiniStore.Data.Entities
{
    public class LocalGovernment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int StateId { get; set; }

        [ForeignKey("StateId")]
        public State State { get; set; }
    }
}
