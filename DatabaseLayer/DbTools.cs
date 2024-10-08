using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace DatabaseLayer
{
    namespace UserDbTools
    {
        //Class1 - DbTools
        public class Class1
        {
#nullable enable
            private static MySqlConnection? connection;

            private static string GetConnectionString()
            {
                var builder = new MySqlConnectionStringBuilder()
                {
                    Server = "localhost",
                    UserID = "airport_user",
                    //UserID = "root",
                    Password = "airport_password",
                    // Password = "nkkm",
                    Port = 3306,
                    ConnectionTimeout = 45,
                    Database = "airport"
                };
                return builder.ToString();
            }

            private static void CheckConnection()
            {
                if (connection == null)
                {
                    connection = new MySqlConnection();
                    connection.ConnectionString = GetConnectionString();
                    connection.StateChange += Connection_StateChange;
                    connection.InfoMessage += Connection_InfoMessage;
                }
            }

            private static void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs args)
            {
                Console.WriteLine("====Info message start====");
                foreach (var item in args.errors)
                {
                    Console.WriteLine($"Code {item.Code} Message {item.Message}");
                }
                Console.WriteLine("====Info message end====");

            }

            private static void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
            {
                Console.WriteLine("====State change start====");
                Console.WriteLine(e.CurrentState);
                Console.WriteLine("====State change end====");
            }


            private static bool CheckOpenConnection()
            {
                CheckConnection();
                if (connection.State == System.Data.ConnectionState.Open)
                    return true;
                return false;
            }


            private static bool OpenConnection()
            {
                try
                {
                    if (!CheckOpenConnection())
                        connection.Open();
                    return true;
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Open connection exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Open connection exception end====");
                    return false;
                }
            }

            private static bool SendDataToServer(string sqlText)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return false;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sqlText;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Open connection exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Open connection exception end====");
                    return false;
                }
            }

            private static MySqlDataReader GetDataFromServerReader(string sqlText)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return null;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sqlText;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    return cmd.ExecuteReader();
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Get reader exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get reader exception end====");
                    return null;
                }
            }



            private static MySqlDataAdapter GetDataFromServerAdapter(string sqlText)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return null;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sqlText;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    return new MySqlDataAdapter(cmd);
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Get adapter exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get adapter exception end====");
                    return null;
                }
            }


            private static object GetDataFromServerScalar(string sqlText)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return null;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sqlText;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    return cmd.ExecuteScalar();
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Get scalar exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get scalar exception end====");
                    return null;
                }
            }


            public static DataTable GetArrivalsDepartures(string type) //mnogo =  strukuktura list 
            {
                try
                {
                    string sqlText = $"SELECT f.FlightNr,(SELECT AirportName FROM from_to WHERE ID = f.FromID) as 'From', (SELECT AirportName FROM from_to WHERE ID = f.ToID) as 'To', ad.Time FROM arrivals_departures as ad LEFT JOIN flight as f ON ad.FlightID = f.ID where ad.Arr_dep='{type}' AND ad.Time BETWEEN NOW() and NOW()+ INTERVAL 1 DAY;";
                    //string  sqlText = $"SELECT f.FlightNr,(SELECT AirportName FROM from_to WHERE ID = f.FromID) as 'From', (SELECT AirportName FROM from_to WHERE ID = f.ToID) as 'To', ad.Time FROM arrivals_departures as ad LEFT JOIN flight as f ON ad.FlightID = f.ID where ad.Arr_dep='{type}';";

                    DataTable tbl = new DataTable();
                    var data = GetDataFromServerAdapter(sqlText);
                    data.Fill(tbl);
                    return tbl;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("====Get ArrivalsDepartures exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get ArrivalsDepartures exception end====");
                    return null;
                }
            }

            public static bool CheckUserExists(string user)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return false;
                    string sqlText = $"SELECT COUNT(*) FROM USER WHERE username='{user}' OR email='{user}';";
                    var data = GetDataFromServerScalar(sqlText);
                    return data == null || (long)data == 0 ? false : true;

                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====CheckUserExists exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===CheckUserExists exception end====");
                    return false;
                }
            }

            public static bool CheckUserBlocked(string user)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return false;
                    string sqlText = $"SELECT blocked FROM USER WHERE username='{user}' OR email='{user}';";
                    var data = GetDataFromServerScalar(sqlText);
                    return data == null || (sbyte)data == 0 ? false : true; //tinyint = byte

                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====CheckUserBlocked exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===CheckUserBlocked exception end====");
                    return false;
                }
            }



            public static bool BlockUser(string user)
            {
                try
                {
                    string sqlText = $"UPDATE user SET blocked=1 WHERE username='{user}' OR email='{user}';";
                    return SendDataToServer(sqlText);
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====BlockUser exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===BlockUser exception end====");
                    return false;
                }
            }

            public static bool CheckUserCredentials(string user, string password)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return false;
                    string sqlText = $"SELECT COUNT(*) FROM USER WHERE (username='{user}' OR email='{user}') AND password='{password}';";
                    var data = GetDataFromServerScalar(sqlText);
                    return data == null || (long)data == 0 ? false : true;

                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====CheckUserCredentials exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===CheckUserCredentials exception end====");
                    return false;
                }
            }

            public static bool CreateNewUser(string user, string email, string password)
            {
                try
                {
                    string sqlText = $"INSERT INTO user(username,email,password) VALUES('{user}','{email}','{password}');";
                    return SendDataToServer(sqlText);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("====CheckNewUser exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===CheckNewUser exception end====");
                    return false;

                }
            }

            public static List<string> GetAllAirports()
            {
                try
                {
                    string sqlText = "SELECT AirportName FROM from_to;";
                    var data = GetDataFromServerReader(sqlText);
                    if (data == null) return null;
                    List<string> lst = new List<string>();
                    while (data.Read())
                    {
                        lst.Add(data[0].ToString());
                    }
                    data.Close();
                    return lst;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====GetAllAirports exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===GetAllAirports exception end====");
                    return null;

                }
            }
            //public static DataTable? FilterFlights(string from, string to, DataSetDateTime when)
            public static DataTable? FilterFlights(string from, string to, DateTime when)
            {

                //when (kintamasis) - date and time=00:00:00
                try
                {
                    string sqlText = $"SELECT ft.AirportName as 'FROM', " +
                                       $"ft.AirportName as 'TO', " +
                                       $"f.FlightNr," +
                                       $"f.FlightTime," +
                                       $"f.FlightDuration," +
                                       $" p.Name as Plane," +
                                       $" p.Type," +
                                       $"p.Manuf" +
                                       $" FROM flight as f LEFT JOIN from_to as ft ON f.FromID=ft.ID LEFT JOIN plane as p ON f.PlaneID=p.ID " +
                                       $" WHERE" +
                                       $" f.fromID=(select ID FROM from_to where AirportName='{from}')" +
                                       $" AND " +
                                       $"f.toID=(select ID FROM from_to where AirportName='{to}') " +
                                       $"AND" +
                                       $" f.flightTime BETWEEN '{when}' AND '{when.AddHours(23).AddMinutes(59).AddSeconds(59)}' " +
                                       $"and f.planeID=p.ID;";

                    var data = GetDataFromServerAdapter(sqlText);
                    if (data == null) return null;
                    DataTable tb1 = new DataTable();
                    data.Fill(tb1);
                    return tb1;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====FilterFlights exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===FilterFlights exception end====");
                    return null;

                }

            }

            public static ushort GetSeatCount(string flightNr, DateTime flightTime)
            {
                try
                {
                    string sqlText = $"SELECT Seats FROM plane WHERE ID=(SELECT PlaneID FROM flight WHERE FlightNr='{flightNr}' AND FlightTime='{flightTime}');";
                    var data = GetDataFromServerScalar(sqlText);
                    if (data == null) return 0;
                    return (ushort)data;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====GetSeatCount exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===GetSeatCount exception end====");
                    return 0;

                }
            }

            public static string? GetTakenSeats(string flightNr, DateTime flightTime)
            {
                try
                {
                    string sqlText = $"SELECT TakenSeats FROM flight WHERE FlightNr='{flightNr}' AND FlightTime='{flightTime}';";
                    var data = GetDataFromServerScalar(sqlText);
                    if (data == null) return null;
                    return data.ToString();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====GetTakenSeats exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===GetTakenSeats exception end====");
                    return null;

                }
            }

            public static bool BuyTickets(string reserved, string flightNr, DateTime flightTime, string user, string ticketClass)
            {
                try
                {
                    string sqlText = $"Update flight SET TakenSeats='[]' where TakenSeats is null;Update flight SET TakenSeats=JSON_MERGE_PRESERVE(TakenSeats,'{reserved}') WHERE flightNr='{flightNr}' and flightTime='{flightTime}';"; //append - prisoedinits k kontsu //$.[0] - rodo, kad iterpiame i nulini masyva

                    foreach (var item in JsonConvert.DeserializeObject<List<int>>(reserved))
                    {
                        sqlText += $"Insert into ticket(UserID,FlightID,SeatNr,Class) values((SELECT ID FROM user WHERE username='{user}' OR email='{user}'),(SELECT ID FROM flight WHERE flightNr='{flightNr}' and flightTime='{flightTime}'),{item},'{ticketClass}');";
                    }

                    return SendDataToServer(sqlText);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====BuyTickets exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===BuyTickets exception end====");
                    return false;
                }
            }

            public static string GetSeatClassEnums()
            {
                try
                {
                    string sqlText = "SHOW COLUMNS FROM ticket LIKE 'Class';";
                    var data = GetDataFromServerReader(sqlText);
                    if (data == null) return string.Empty;
                    string rez = string.Empty;
                    while (data.Read())
                    {
                        rez = data[1].ToString();
                    }
                    data.Close();
                    return rez;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====GetSeatClassEnums exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===GetSeatClassEnums exception end====");
                    return string.Empty;
                }
            }

            public static DataTable? ViewHistory(string user)
            {
                try
                {

                    string sqlText = $"SELECT f.FlightNr,f.FlightTime,(Select AirportName from from_to WHERE ID = f.FromID)as 'From',(Select AirportName from from_to WHERE ID = f.ToID) as 'To',t.SeatNr,t.Create_time as 'Order time' from ticket as t LEFT JOIN flight as f ON t.FlightID=f.ID WHERE t.UserID=(SELECT ID FROM user WHERE username='{user}' OR email='{user}');";
                    var data = GetDataFromServerAdapter(sqlText);
                    DataTable tbl = new DataTable();
                    data.Fill(tbl);
                    return tbl;
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====ViewHistory exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===ViewHistory exception end====");
                    return null;
                }
            }
        }
    }

    namespace AdminDbTools
    {
        public class AdminToolsDb
        {
#nullable enable
            private static MySqlConnection? connection;

            private static string GetConnectionString()
            {
                var builder = new MySqlConnectionStringBuilder()
                {
                    Server = "localhost",
                    UserID = "admin_user",
                    //UserID = "root",
                    Password = "admin_password",
                    // Password = "nkkm",
                    Port = 3306,
                    ConnectionTimeout = 45,
                    Database = "airport"
                };
                return builder.ToString();
            }

            private static void CheckConnection()
            {
                if (connection == null)
                {
                    connection = new MySqlConnection();
                    connection.ConnectionString = GetConnectionString();
                    connection.StateChange += Connection_StateChange;
                    connection.InfoMessage += Connection_InfoMessage;
                }
            }

            private static void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs args)
            {
                Console.WriteLine("====Info message start====");
                foreach (var item in args.errors)
                {
                    Console.WriteLine($"Code {item.Code} Message {item.Message}");
                }
                Console.WriteLine("====Info message end====");

            }

            private static void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
            {
                Console.WriteLine("====State change start====");
                Console.WriteLine(e.CurrentState);
                Console.WriteLine("====State change end====");
            }


            private static bool CheckOpenConnection()
            {
                CheckConnection();
                if (connection.State == System.Data.ConnectionState.Open)
                    return true;
                return false;
            }


            private static bool OpenConnection()
            {
                try
                {
                    if (!CheckOpenConnection())
                        connection.Open();
                    return true;
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Open connection exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Open connection exception end====");
                    return false;
                }
            }

            private static bool SendDataToServer(string sqlText)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return false;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sqlText;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Open connection exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Open connection exception end====");
                    return false;
                }
            }

            private static MySqlDataReader GetDataFromServerReader(string sqlText)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return null;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sqlText;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    return cmd.ExecuteReader();
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Get reader exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get reader exception end====");
                    return null;
                }
            }



            private static MySqlDataAdapter GetDataFromServerAdapter(string sqlText)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return null;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sqlText;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    return new MySqlDataAdapter(cmd);
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Get adapter exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get adapter exception end====");
                    return null;
                }
            }


            private static object GetDataFromServerScalar(string sqlText)
            {
                try
                {
                    if (!CheckOpenConnection())
                        if (!OpenConnection()) return null;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = sqlText;
                    cmd.Connection = connection;
                    cmd.ExecuteNonQuery();
                    return cmd.ExecuteScalar();
                }
                catch (MySqlException ex)
                {

                    Console.WriteLine("====Get scalar exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get scalar exception end====");
                    return null;
                }
            }

            public static DataTable? GetAllUsers()
            {
                try
                {
                    string sqlText = "Select * from user;";
                    var data = GetDataFromServerAdapter(sqlText);
                    if (data == null) return null;
                    DataTable tbl = new DataTable();
                    data.Fill(tbl);
                    return tbl;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====ADMIN GetAllUsers exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===ADMIN GetAllUsers scalar exception end====");
                    return null; ;
                }
            }

            public static DataTable? GetAllPlanes()
            {
                try
                {
                    string sqlText = "Select * from plane;";
                    var data = GetDataFromServerAdapter(sqlText);
                    if (data == null) return null;
                    DataTable tbl = new DataTable();
                    data.Fill(tbl);
                    return tbl;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====ADMIN GetAllPlanes exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===ADMIN GetAllPlanes scalar exception end====");
                    return null; ;
                }
            }

            public static DataTable? GetAllAirports()//ne sdelano
            {
                try
                {
                    string sqlText = "Select * from plane;";
                    var data = GetDataFromServerAdapter(sqlText);
                    if (data == null) return null;
                    DataTable tbl = new DataTable();
                    data.Fill(tbl);
                    return tbl;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====ADMIN GetAllPlanes exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===ADMIN GetAllPlanes scalar exception end====");
                    return null; ;
                }
            }

            public static DataTable? GetAllFlights()
            {
                try
                {
                    string sqlText = "Select * from plane;";
                    var data = GetDataFromServerAdapter(sqlText);
                    if (data == null) return null;
                    DataTable tbl = new DataTable();
                    data.Fill(tbl);
                    return tbl;
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("====ADMIN GetAllPlanes exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===ADMIN GetAllPlanes scalar exception end====");
                    return null; ;
                }
            }
        }
    }
}
