using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Diary.Models.Converters;
using Diary.Models;

namespace Diary
{
    public class Repository
    {
        public List<Group> GetGroups()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Groups.ToList();
            }
        }

        public List<StudentWrapper> GetStudents(int groupId)
        {
            using (var context = new ApplicationDbContext())
            {
                var students = context
                    .Students
                    .Include(x => x.Group)
                    .Include(x => x.Gradings)
                    .AsQueryable();

                if (groupId != 0)
                    students = students.Where(x => x.GroupId == groupId);

                

                return students
                        .ToList()
                        .Select(x => x.ToWrapper())
                        .ToList();

            }
        }

        public void DeleteStudent(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var studentToDelete = context.Students.Find(id);
                context.Students.Remove(studentToDelete);
                context.SaveChanges();
            }
        }

        public void UpdateStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.ToDao();
            var grading = studentWrapper.ToGradingDao();

            using (var context = new ApplicationDbContext())
            {
                UpdateStudentProperties(student, context);

                var studentsGrading = GetStudentsGrading(context, student);


                UpdateGrade(student, grading, context, studentsGrading, Subject.Math);
                UpdateGrade(student, grading, context, studentsGrading, Subject.ForeignLang);
                UpdateGrade(student, grading, context, studentsGrading, Subject.Physics);
                UpdateGrade(student, grading, context, studentsGrading, Subject.PolishLang);
                UpdateGrade(student, grading, context, studentsGrading, Subject.Technology);


                context.SaveChanges();
            }
        }

        private static List<Grading> GetStudentsGrading(ApplicationDbContext context, Student student)
        {
           return context.Gradings
                    .Where(x => x.StudentId == student.Id)
                    .ToList();
        }

        private static void UpdateStudentProperties(Student student, ApplicationDbContext context)
        {
            var studentToUpdate = context.Students.Find(student.Id);
            studentToUpdate.Activities = student.Activities;
            studentToUpdate.Comments = student.Comments;
            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.LastName = student.LastName;
            studentToUpdate.GroupId = student.GroupId;
        }

        private static void UpdateGrade(Student student, List<Grading> newGrading,
            ApplicationDbContext context, List<Grading> studentsGrading, Subject subject)
        {
            var subjectGrading = studentsGrading
                                .Where(x => x.SubjectId == (int)subject)
                                .Select(x => x.Grade);

            var newSubjectGrading = newGrading
                .Where(x => x.SubjectId == (int)subject)
                .Select(x => x.Grade);

            var subjectGradingToDelete = subjectGrading.Except(newSubjectGrading).ToList();
            var subjectGradingToAdd = newSubjectGrading.Except(subjectGrading).ToList();

            subjectGradingToDelete.ForEach(x =>
            {
                var gradeToDelete = context.Gradings.First(y => y.Grade == x
                && y.StudentId == student.Id && y.SubjectId == (int)subject);
                context.Gradings.Remove(gradeToDelete);
            });

            subjectGradingToAdd.ForEach(x =>
            {
                var gradeToAdd = new Grading
                {
                    Grade = x,
                    StudentId = student.Id,
                    SubjectId = (int)subject
                };
                context.Gradings.Add(gradeToAdd);
            });
        }

        public void AddStudent(StudentWrapper studentWrapper)
        {
            var student = studentWrapper.ToDao();
            var grading = studentWrapper.ToGradingDao();

            using (var context = new ApplicationDbContext())
            {
                var dbStudent = context.Students.Add(student);

                grading.ForEach(x =>
                {
                    x.StudentId = dbStudent.Id;
                    context.Gradings.Add(x);
                });

                context.SaveChanges();

            }
            
        }
    }
}
