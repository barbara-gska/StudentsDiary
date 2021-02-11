using Diary.Models.Domains;
using Diary.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Converters
{
    public static class StudentConverter
    {
        public static StudentWrapper ToWrapper(this Student model)
        {
            return new StudentWrapper
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Comments = model.Comments,
                Activities = model.Activities,
                Group = new GroupWrapper 
                { 
                    Id = model.Group.Id, 
                    Name = model.Group.Name
                },
                Math = string.Join(", ", model.Gradings
                                        .Where(y => y.SubjectId == (int)Subject.Math)
                                        .Select(y => y.Grade)),
                Physics = string.Join(", ", model.Gradings
                                        .Where(y => y.SubjectId == (int)Subject.Physics)
                                        .Select(y => y.Grade)),
                Technology = string.Join(", ", model.Gradings
                                        .Where(y => y.SubjectId == (int)Subject.Technology)
                                        .Select(y => y.Grade)),
                PolishLang = string.Join(", ", model.Gradings
                                        .Where(y => y.SubjectId == (int)Subject.PolishLang)
                                        .Select(y => y.Grade)),
                ForeingLang = string.Join(", ", model.Gradings
                                        .Where(y => y.SubjectId == (int)Subject.ForeignLang)
                                        .Select(y => y.Grade)),
            };
        }

        public static Student ToDao(this StudentWrapper model)
        {
            return new Student
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                GroupId = model.Group.Id,
                Comments = model.Comments,
                Activities = model.Activities
            };
        }

        public static List<Grading> ToGradingDao(this StudentWrapper model)
        {
            var gradings = new List<Grading>();

            if (!string.IsNullOrWhiteSpace(model.Math))
                model.Math.Split(',').ToList().ForEach(x =>
                            gradings.Add(
                                new Grading
                                {
                                    Grade = int.Parse(x),
                                    StudentId = model.Id,
                                    SubjectId = (int)Subject.Math
                                }));

            if (!string.IsNullOrWhiteSpace(model.Physics))
                model.Physics.Split(',').ToList().ForEach(x =>
                            gradings.Add(
                                new Grading
                                {
                                    Grade = int.Parse(x),
                                    StudentId = model.Id,
                                    SubjectId = (int)Subject.Physics
                                }));

            if (!string.IsNullOrWhiteSpace(model.Technology))
                model.Technology.Split(',').ToList().ForEach(x =>
                            gradings.Add(
                                new Grading
                                {
                                    Grade = int.Parse(x),
                                    StudentId = model.Id,
                                    SubjectId = (int)Subject.Technology
                                }));

            if (!string.IsNullOrWhiteSpace(model.PolishLang))
                model.PolishLang.Split(',').ToList().ForEach(x =>
                            gradings.Add(
                                new Grading
                                {
                                    Grade = int.Parse(x),
                                    StudentId = model.Id,
                                    SubjectId = (int)Subject.PolishLang
                                }));

            if (!string.IsNullOrWhiteSpace(model.ForeingLang))
                model.ForeingLang.Split(',').ToList().ForEach(x =>
                            gradings.Add(
                                new Grading
                                {
                                    Grade = int.Parse(x),
                                    StudentId = model.Id,
                                    SubjectId = (int)Subject.ForeignLang
                                }));

            return gradings;
        }

    }
}
