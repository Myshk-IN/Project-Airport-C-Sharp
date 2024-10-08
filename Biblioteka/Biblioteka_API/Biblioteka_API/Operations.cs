using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using unirest_net;
using unirest_net.http;
using unirest_net.request;


namespace Biblioteka_API
{
    class Operations
    {
        private static string ServerUrl = "http://localhost/api";
        public static async Task<bool> CheckLogin(string user, string pass)
        {
            try
            {
                var userObj = new User() { UserName = user, Password = pass };
                var response = await Unirest.post($"{ServerUrl}/user/login.php")
                    .header("Accept", "application/json")
                    .body(JsonConvert.SerializeObject(userObj))
                    .asJsonAsync<string>();

                Console.WriteLine("==========================");
                Console.WriteLine(response.Body);
                Console.WriteLine("==========================");
                if (response.Code == 200)
                    return true;
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static async Task<UserInfo> GetUserInfo(string userName)
        {
            try
            {
                var response = await Unirest.get($"{ServerUrl}/user/userInfo.php?username={userName}")
                    .header("Accept", "application/json")
                    .asJsonAsync<string>();

                Console.WriteLine("==========================");
                Console.WriteLine(response.Body);
                Console.WriteLine("==========================");
                if (response.Code == 200)
                    return JsonConvert.DeserializeObject<UserInfo>(response.Body);
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static async Task<bool> UpdateMyInfo(UserInfo info)
        {
            try
            {
                var response = await Unirest.post($"{ServerUrl}/user/updateinfo.php")
                    .header("Accept", "application/json")
                    .body(JsonConvert.SerializeObject(info))
                    .asJsonAsync<string>();

                Console.WriteLine("==========================");
                Console.WriteLine(response.Body);
                Console.WriteLine("==========================");
                if (response.Code == 200)
                    return true;
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

       // public static async Task<List<TakenBooks>> GetTakenBook(string userName)
        public static async Task<DataTable> GetTakenBook(string userName)
        {
            try
            {
                var response = await Unirest.get($"{ServerUrl}/book/takenbooks.php?username={userName}")
                    .header("Accept", "application/json")
                    .asJsonAsync<string>();

                Console.WriteLine("==========================");
                Console.WriteLine(response.Body);
                Console.WriteLine("==========================");
                if (response.Code == 200)
                {
                    var lst = JsonConvert.DeserializeObject<List<TakenBooks>>(response.Body);
                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("Title");
                    tbl.Columns.Add("Author");
                    tbl.Columns.Add("Taken From");
                    tbl.Columns.Add("Taken Until");
                    tbl.Columns.Add("Qnt");
                    //fill rows
                    for (int i = 0; i < lst.Count; i++)
                    {
                        DataRow row = tbl.NewRow();
                        row["Title"] = lst[i].Title;
                        row["Author"] = lst[i].Author;
                        row["Taken From"] = lst[i].TakenFrom;
                        row["Taken Until"] = lst[i].TakenUntil;
                        row["Qnt"] = lst[i].Qnt;
                        tbl.Rows.Add(row);
                        
                    }
                    return tbl;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static async Task<DataTable> GetBooks()
        {
            try
            {
                var response = await Unirest.get($"{ServerUrl}/book/allbooks.php")
                    .header("Accept", "application/json")
                    .asJsonAsync<string>();

                Console.WriteLine("==========================");
                Console.WriteLine(response.Body);
                Console.WriteLine("==========================");
                if (response.Code == 200)
                {
                    var lst = JsonConvert.DeserializeObject<List<Books>>(response.Body);
                    DataTable tbl = new DataTable();
                    tbl.Columns.Add("Title");
                    tbl.Columns.Add("Author");
                    tbl.Columns.Add("ReleaseDate");
                    tbl.Columns.Add("Publisher");
                    tbl.Columns.Add("Pages");
                    tbl.Columns.Add("ISBN");
                    //fill rows
                    for (int i = 0; i < lst.Count; i++)
                    {
                        DataRow row = tbl.NewRow();
                        row["Title"] = lst[i].Title;
                        row["Author"] = lst[i].Author;
                        row["ReleaseDate"] = lst[i].ReleaseDate;
                        row["Publisher"] = lst[i].Publisher;
                        row["Pages"] = lst[i].Pages;
                        row["ISBN"] = lst[i].ISBN;
                        tbl.Rows.Add(row);

                    }
                    return tbl;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
        
    }
}
