using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Domains
{
    public class Grading
    {
        public int Id { get; set; }
        public int Grade { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }


        public Student Student { get; set; }
    }
}
