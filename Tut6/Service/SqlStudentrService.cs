using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stut6.Models;
using Stut6.Controllers;
using Stut6.Service;
using System.Data.SqlClient;

namespace Stut6.Services
{
    public class SqlStudentrService : IStudentsDbService
    {

        public Boolean CheckIndex(string index)
        {


            using (var con = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {

                    com.Parameters.AddWithValue("Index", index);
                    com.CommandText = "SELECT * FROM STUDENTS s WHERE s.indexNumber= @Index";
                    var u = com.ExecuteReader();
                    if (u.HasRows)
                    {
                        return true;
                        
                    }
                    else
                    {
                        throw new MyException("Student with this index already exists!");
                    }
                }
            }


        }




        public Student EnrollStudent(Student student)
        {
            var s = student;
            if (s.IndexNumber == null || s.Firstname == null || s.LastName == null || s.BirthDate == null || s.StudiesName == null)
            {
                throw new MyException("Missing Information about student!");

            }
            int StudyId;
            int IdEnrollment;

            using (var con = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.Parameters.AddWithValue("StName", s.StudiesName);
                    com.CommandText = "SELECT idStudy FROM Studies  Where studyName =@StName ";
                    con.Open();
                    var transaction = con.BeginTransaction();
                    com.Transaction = transaction;



                    var dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        StudyId = (int)dr["idStudy"];
                    }
                    else
                    {
                        throw new MyException("Study with this name does not exists!");
                        transaction.Rollback();
                    }
                    dr.Close();


                    com.Parameters.AddWithValue("SId", StudyId);
                    com.CommandText = "SELECT MAX(e.StartDate) FROM Enrollment e WHERE e.idStudy=@SId AND e.Semester=4;";

                    var d = com.ExecuteReader();
                    if (d.HasRows)
                    {
                        com.CommandText = "SELECT e.idEnrollment FROM Enrollment e WHERE e.idStudy=@SId AND e.Semester=4 AND e.StartDate =(SELECT Max(m.StartDate) FROM Enrollment m);";
                        var r = com.ExecuteReader();
                        r.Read();

                        IdEnrollment = (int)d["IdEnrollment"];

                        r.Close();

                    }
                    else
                    {

                        com.CommandText = "Select max(idEnrollment)+1 'maxId' From Enrollment; ";
                        var m = com.ExecuteReader();
                        m.Read();
                        IdEnrollment = (int)m["maxId"];
                        m.Close();
                        string StartDate;
                        com.CommandText = "SELECT CONVERT(date, getdate()) 'date' ;";
                        var e = com.ExecuteReader();
                        e.Read();
                        StartDate = e["date"].ToString();
                        e.Close();
                        com.Parameters.AddWithValue("EId", IdEnrollment);
                        com.Parameters.AddWithValue("StartDate", StartDate);
                        com.CommandText = "INSERT INTO Enrollment (idEnrollment, idStudy, Semester, StartDate) VALUES( @EId, @SId, 4, @StartDate); ";
                        com.ExecuteNonQuery();

                    }
                    d.Close();


                    
                  if(CheckIndex(s.IndexNumber))
                    {
                        throw new MyException("Student with this index already exists!");
                        transaction.Rollback();
                    }
                    else
                    {
                        com.Parameters.AddWithValue("Index", s.IndexNumber);
                        com.Parameters.AddWithValue("Name", s.Firstname);
                        com.Parameters.AddWithValue("Surname", s.LastName);
                        com.Parameters.AddWithValue("Bdate", s.BirthDate);
                        com.Parameters.AddWithValue("EId", IdEnrollment);
                        com.CommandText = "INSERT INTO Students  (indexNumber, FirstName, LastName, BirthDate, idEnrollment) VALUES(@Index, @Name, @Surname, @Bdate, @EId); ";
                        com.ExecuteNonQuery();
                    }
                    
                    transaction.Commit();




                }



            }
            return s;

        }

        public Enrollment PromoteStudent(Enrollment enrollment)
        {
            var e = enrollment;
            if (e.Semester == null || e.Studies == null)
            {
                throw new MyException("Missing information!");
            }

            using (var con = new SqlConnection("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True"))
            {
                using (var com = new SqlCommand())
                {

                    com.Connection = con;
                    com.Parameters.AddWithValue("Semester", e.Semester);
                    com.Parameters.AddWithValue("Studies", e.Studies);
                    com.CommandText = "SELECT * FROM Enrollment e JOIN Studies s ON e.idStudy=s.idStudy WHERE e.Semester = @Semester AND s.studyName=@Studies; ";
                    con.Open();
                    var transaction = con.BeginTransaction();
                    com.Transaction = transaction;

                    var dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Close();
                        com.CommandText = "myproc";
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.ExecuteNonQuery();


                        return e;
                    }
                    else
                    {

                        throw new MyException("Does not exist such record in Enrollment table! ");
                    }


                }
            }



        }






    }



}
