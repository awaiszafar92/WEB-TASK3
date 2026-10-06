using System.Data;
using AttendanceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Data.SqlClient;

namespace AttendanceManagementSystem.Repositories;

public class StudentRepository
{
    private string connection_string = "Server=localhost;DataBase=AttendanceDB;User Id=sa;Password=Usman@08042006;TrustServerCertificate=True;";

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        SqlConnection connection = new SqlConnection(connection_string);
        string query = "SELECT * from Students";
        SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

        DataTable dataTable = new DataTable();

        await Task.Run(() =>
        {
            adapter.Fill(dataTable);
        });

        List<Student> students = new List<Student>();

        foreach (DataRow row in dataTable.Rows)
        {
            Student student = new Student();
            student.StudentID = Convert.ToInt32(row["StudentID"]);
            student.StudentName = row["StudentName"].ToString();
            student.RegistrationNumber = row["RegistrationNumber"].ToString();
            student.Department = row["Department"].ToString();
            student.AttendancePercentage = Convert.ToDecimal(row["AttendancePercentage"]);

            students.Add(student);
        }

        return students;
    }

    public async Task AddStudent(Student student)
    {
        SqlConnection connection = new SqlConnection(connection_string);
        String query1 = "INSERT INTO Students (StudentName, RegistrationNumber,Department,AttendancePercentage) VALUES (@Name,@Registration,@Department,@Attendance)";
        String query2 = "SELECT * FROM Students";
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlDataAdapter adapter2 = new SqlDataAdapter(query2, connection);

        DataTable dataTable = new DataTable();

        await Task.Run(async () =>
        {
            adapter2.Fill(dataTable);
        });

        DataRow row = dataTable.NewRow();

        row["StudentName"] = student.StudentName;
        row["RegistrationNumber"] = student.RegistrationNumber;
        row["Department"] = student.Department;
        row["AttendancePercentage"] = student.AttendancePercentage;

        dataTable.Rows.Add(row);

        adapter1.InsertCommand = new SqlCommand(query1, connection);

        adapter1.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 100, "StudentName");
        adapter1.InsertCommand.Parameters.Add("@Registration", SqlDbType.NVarChar, 30, "RegistrationNumber");
        adapter1.InsertCommand.Parameters.Add("@Department", SqlDbType.NVarChar, 50, "Department");
        SqlParameter attendanceParameter = adapter1.InsertCommand.Parameters.Add("@Attendance", SqlDbType.Decimal, 0, "AttendancePercentage");
        attendanceParameter.Precision = 5;
        attendanceParameter.Scale = 2;

    await Task.Run(() =>
        {
            adapter1.Update(dataTable);
        });
    }




    public async Task UpdateStudent(Student student)
    {
        SqlConnection connection = new SqlConnection(connection_string);

        String query1 = "SELECT * FROM Students";

        SqlDataAdapter adapter1 = new SqlDataAdapter(query1, connection);

        DataTable dataTable = new DataTable();

        await Task.Run(() =>
        {
            adapter1.Fill(dataTable);
        });

        String query2 = "UPDATE Students SET StudentName=@StudentName,RegistrationNumber=@Registration,Department= @Department,AttendancePercentage=@Attendance WHERE StudentID = @StudentID";

        SqlDataAdapter adapter2 = new SqlDataAdapter();

        dataTable.PrimaryKey = new DataColumn[]
        {
            dataTable.Columns["StudentID"]
        };

        DataRow row = dataTable.Rows.Find(student.StudentID);

        if (row != null)
        {
            row["StudentName"] = student.StudentName;
            row["RegistrationNumber"] = student.RegistrationNumber;
            row["Department"] = student.Department;
            row["AttendancePercentage"] = student.AttendancePercentage;

        }

        adapter2.UpdateCommand = new SqlCommand(query2, connection);

        adapter2.UpdateCommand.Parameters.Add("@StudentName", SqlDbType.NVarChar, 100, "StudentName");
        adapter2.UpdateCommand.Parameters.Add("@Registration", SqlDbType.NVarChar, 30, "RegistrationNumber");
        adapter2.UpdateCommand.Parameters.Add("@Department", SqlDbType.NVarChar, 50, "Department");
        SqlParameter attendanceParameter = adapter2.UpdateCommand.Parameters.Add("@Attendance", SqlDbType.Decimal, 0, "AttendancePercentage");
        attendanceParameter.Precision = 5;
        attendanceParameter.Scale = 2;
        adapter2.UpdateCommand.Parameters.Add("@StudentID", SqlDbType.Int, 0, "StudentID");
        await Task.Run(() =>
        {
            adapter2.Update(dataTable);
        });
    }



    public async Task DeleteStudent(int id)
    {
        SqlConnection connection = new SqlConnection(connection_string);
        String query1 = "SELECT * FROM Students";

        SqlDataAdapter adapter1 = new SqlDataAdapter(query1, connection);
        DataTable dataTable = new DataTable();

        await Task.Run(() =>
        {
            adapter1.Fill(dataTable);
        });
        String query2 = "DELETE FROM STUDENTS WHERE StudentId=@StudentID";

        SqlDataAdapter dataAdapter2 = new SqlDataAdapter();

        dataAdapter2.DeleteCommand = new SqlCommand(query2, connection);

        dataTable.PrimaryKey = new DataColumn[]
        {
             dataTable.Columns["StudentID"]   
        };

        DataRow row = dataTable.Rows.Find(id);

        if (row == null)
        {
            return;
        }

        row.Delete();

        dataAdapter2.DeleteCommand.Parameters.Add("@StudentID",SqlDbType.Int,0,"StudentID");

        await Task.Run(() =>
        {
            dataAdapter2.Update(dataTable);
        });

    }   

    public async Task<Student> GetStudentBYID(int id)
    {
        SqlConnection connection = new SqlConnection(connection_string);
        string query = "SELECT * FROM Students where StudentId=@id";
        SqlDataAdapter adapter = new SqlDataAdapter(query,connection);
        adapter.SelectCommand.Parameters.AddWithValue("@id", id);
        DataTable dataTable = new DataTable();

        await Task.Run(() =>
        {
            adapter.Fill(dataTable);
        });


        DataRow row = dataTable.Rows[0];
        if (row ==  null)
            return null;

        Student student = new Student();
        student.StudentID = Convert.ToInt32(row["StudentID"]);
        student.StudentName = row["StudentName"].ToString();
        student.RegistrationNumber = row["RegistrationNumber"].ToString();
        student.AttendancePercentage = Convert.ToDecimal(row["AttendancePercentage"]);
        student.Department =  row["Department"].ToString();

        return student;
    }
}
