using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stargazer.Database.Entities
{
    [Table("Stars")]
    public class Star : Entity
    {
        [Column("Name")]
        public string Name { get; set; }

        public Star() : base() { }
        public Star(string name) : base()
        {
            Name = name;
        }
    }
}
