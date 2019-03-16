﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    [Table("teachers_subjects")]
    public class TeacherSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        public Guid SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        // TODO TeacherSubject #0
    }
}
