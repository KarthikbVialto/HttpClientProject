using System;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Spectre.Console;
    public class EmployeeResults
    {
        [JsonProperty("results")]
    public List<Employee> resultList { get; set; }
    }
    public class Name
    {
        public string First { get; set; }
        public string Last { get; set; }
    }
    public class Dob
    {
        public string date { get; set; }
        public int age { get; set; }
    }
    public class Employee
    {
        public string gender { get; set; }
        public Name name { get; set; }
        public Dob dob { get; set; }
        public string  email { get; set; }
    }
public class Program
{

    static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromSeconds(100);
            HttpResponseMessage response = await client.GetAsync("https://randomuser.me/api/?results=100&seed=59a7d962d3167864");
            if (response.IsSuccessStatusCode)
            {
                string responsebody = await response.Content.ReadAsStringAsync();
                EmployeeResults results = JsonConvert.DeserializeObject<EmployeeResults>(responsebody);

                //initial implementation

                //List<Employee> maleEmployee = new List<Employee>();
                //List<Employee> femaleEmployee = new List<Employee>();
                //foreach (var employee in results.resultList)
                //{
                //    if (employee.gender == "male" && employee.dob.age >= 45 && employee.dob.age <= 60)
                //    {
                //        maleEmployee.Add(employee);
                //    }
                //    if (employee.gender == "female" && employee.dob.age > 20)
                //    {
                //        femaleEmployee.Add(employee);
                //    }
                //}



                //linq queries implementation

                List<Employee> maleEmployee = results.resultList.Where(employee => employee.gender.Equals("male") && employee.dob.age >= 45 && employee.dob.age <= 60)
                                                                  .ToList();
                List<Employee> femaleEmployee = results.resultList.Where(employee => employee.gender.Equals("female") && employee.dob.age > 20)
                                                                  .ToList();
                #region male table printing
                Table maletable = new Table();
                maletable.AddColumn("Gender");
                maletable.AddColumn("Name");
                maletable.AddColumn("DOB");
                maletable.AddColumn("Age");
                maletable.AddColumn("Email");
                foreach(var employee in maleEmployee)
                {
                    string name = employee.name.First + " " + employee.name.Last;
                    DateTime datetime = DateTime.Parse(employee.dob.date);
                    string dob = datetime.ToString("yyyy-MM-dd");
                    string age = employee.dob.age.ToString();
                    string email = employee.email;
                    maletable.AddRow(employee.gender,name,dob,age,email);
                    maletable.AddEmptyRow();
                }
                AnsiConsole.Write(maletable);
                #endregion
                Console.WriteLine("Press Enter to view female employee table");
                Console.ReadLine();
                AnsiConsole.Clear();
                #region female table printing
                Table femaletable = new Table();
                femaletable.AddColumn("Name");
                femaletable.AddColumn("DOB");
                femaletable.AddColumn("Age");
                femaletable.AddColumn("Email");
                foreach (var employee in femaleEmployee)
                {
                    string name = employee.name.First + " " + employee.name.Last;
                    DateTime datetime = DateTime.Parse(employee.dob.date);
                    string dob = datetime.ToString("yyyy-MM-dd");
                    string age = employee.dob.age.ToString();
                    string email = employee.email;
                    femaletable.AddRow(name, dob, age, email);
                    femaletable.AddEmptyRow();
                }
                AnsiConsole.Write(femaletable);
                #endregion





            }
        }  

    }
     
}
