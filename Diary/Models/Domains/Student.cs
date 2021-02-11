using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Domains
{
    public class Student
    {
        public Student()
        {
            Gradings = new Collection<Grading>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public bool Activities { get; set; }
        public int GroupId { get; set; }

        public Group Group { get; set; }
        public ICollection<Grading> Gradings { get; set; }
    }
}
