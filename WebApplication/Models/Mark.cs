using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entities;

namespace WebApplication.Models
{
    [Table("marks")]
    public class Mark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public int TeacherSubjectId { get; set; }
        public virtual TeacherSubject TeacherSubject { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }


        [Column("value")]
        public int Value { get; set; }
    }
}
