using Stut6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stut6.Service
{
   public interface IStudentsDbService
    {
        Student EnrollStudent(Student student);
        Enrollment PromoteStudent(Enrollment enrollment);
        Boolean CheckIndex(string index);
    }
}
