using Diary.Models.Domains;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Models.Configurations
{
    public class GradingConfiguration : EntityTypeConfiguration<Grading>
    {
        public GradingConfiguration()
        {
            ToTable("dbo.Gradings");

            HasKey(x => x.Id);
        }
    }
}
