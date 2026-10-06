namespace AttendanceManagementSystem.Models;

public class Student
{
    public string StudentName {
        get; set;
    }

    public int StudentID
    {
        get;set;
    }

    public string RegistrationNumber
    {
        set;get;
    }

    public string Department
    {
        set; get;
    }
    public decimal AttendancePercentage
    {
        set; get;
    }
}