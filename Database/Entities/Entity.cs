using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stargazer.Database.Entities
{
    [Table("Entity")]
    public class Entity
    {
        [Column("Id")]
        [PrimaryKey]
        [NotNull]
        [AutoIncrement]
        public int Id { get; set; }

        public Entity() { }
    }
}
