using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.UserDbTools;
using DatabaseLayer.AdminDbTools;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace ApplicationLayer
{
    namespace UserDataTools
    {
        public class DataTools
        {
            private static Dictionary<string, byte> wrongUsers = new Dictionary<string, byte>();

            //public static object DbTools { get; private set; }

            private static string TextToMD5(string txt)
            {
                //create coding object
                var md5 = new MD5CryptoServiceProvider();
                //get byte array from text
                var inBytes = Encoding.UTF8.GetBytes(txt);
                //compute md5 hash
                var outBytes = md5.ComputeHash(inBytes);
                //convert byte to hex
                StringBuilder builder = new StringBuilder();
                foreach (var item in outBytes)
                {
                    builder.Append(item.ToString("x2")); // append = prisoedinenye
                }
                return builder.ToString();
            }

            //public static object GetFlights(string text1, string text2, DateTime value)
            //{
            //    throw new NotImplementedException();
            //} //SASHA DUREN

            /// <summary>
            /// Get arrivals or departures
            /// </summary>
            /// <param name="type">A for Arrivals, D for Departures</param>
            /// <returns>Arrivals or Departures in DataTable object</returns>
            public static DataTable GetArrivalsDepartures(string type)
            {
                try
                {
                    var data = Class1.GetArrivalsDepartures(type);
                    return data == null ? null : data;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("====Get ArrivalsDepartures(AppLayer) exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get ArrivalsDepartures(AppLayer) exception end====");
                    return null;
                }
            }


            public static bool RegisterNewUser(string user, string email, string password)
            {
                try
                {
                    return Class1.CreateNewUser(user, email, TextToMD5(password));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("====UserExists(AppLayer) exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===UserExists(AppLayer) exception end====");
                    return false;
                }
            }

            public static LoginState CheckUserExists(string user, string email)
            {
                try
                {
                    var us = Class1.CheckUserExists(user);
                    var em = Class1.CheckUserExists(email);
                    return us != true && em != true ? LoginState.NotExists : LoginState.Exists;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("====UserExists(AppLayer) exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===UserExists(AppLayer) exception end====");
                    return LoginState.Error;
                }
            }
#nullable enable
            public static List<ArrivalsDepartures>? GetArrivalsDeparturesClass(string type)
            {
                try
                {
                    List<ArrivalsDepartures> lst = new List<ArrivalsDepartures>();
                    var data = Class1.GetArrivalsDepartures(type);
                    if (data == null) return null;
                    foreach (DataRow item in data.Rows)
                    {
                        //kuriam clases objekta
                        var ad = new ArrivalsDepartures();

                        ad.FlightNr = item.ItemArray[0].ToString(); //pirmas stulpelis eiluteje //itemArray - visi stulpeliai
                        ad.From = item.ItemArray[1].ToString();
                        ad.To = item.ItemArray[2].ToString();
                        ad.Time = Convert.ToDateTime(item.ItemArray[3]);
                        lst.Add(ad);

                    }
                    return lst;
                }

                catch (Exception ex)
                {
                    Console.WriteLine("====Get ArrivalsDepartures(AppLayer) exception start====");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("===Get ArrivalsDepartures(AppLayer) exception end====");
                    return null;
                }
            }

            //public static bool BuyTickets(List<int> reservedSeats)
            //{
            //    throw new NotImplementedException();
            //}

            public static LoginState CheckUserCredentials(string user, string pass)
            {
                //check if exists
                if (!Class1.CheckUserExists(user)) return LoginState.NotExists;
                if (Class1.CheckUserBlocked(user)) return LoginState.Blocked;
                if (Class1.CheckUserCredentials(user, TextToMD5(pass))) return LoginState.Connected;
                return CheckWrongAttemps(user);
                //return LoginState.BadPassword;

            }

            private static LoginState CheckWrongAttemps(string user)
            {
                //check if user exists in the dictionary
                if (wrongUsers.ContainsKey(user))
                {
                    //get old value
                    byte val;
                    wrongUsers.TryGetValue(user, out val);
                    //increment value
                    val++;
                    //update value
                    wrongUsers[user] = val;

                    if (val >= 3)
                    {
                        //block user
                        if (Class1.BlockUser(user))
                            return LoginState.Blocked;
                        else
                            return LoginState.Error;
                    }
                    return LoginState.BadPassword;
                }
                //if first time
                wrongUsers.Add(user, 1);
                return LoginState.BadPassword;
            }

            public static List<string>? AllAirports()
            {
                return Class1.GetAllAirports();
            }
            public static DataTable? GetFlights(string from, string to, DateTime when)
            {
                var tbl = Class1.FilterFlights(from, to, new DateTime(when.Year, when.Month, when.Day, 0, 0, 0));
                if (tbl == null) return null;
                foreach (DataRow item in tbl.Rows)
                {
                    // item.ItemArray[0] = from;
                    item[1] = to;

                }

                return tbl;
            }

            public static List<string?>? GetSeats(string flightNr, DateTime flightTime)
            {
                ushort seatCount = Class1.GetSeatCount(flightNr, flightTime);
                if (seatCount == 0) return null;
                List<string?> temp = new List<string?>();
                temp.Add(seatCount.ToString());
                temp.Add(Class1.GetTakenSeats(flightNr, flightTime));
                return temp;
            }

            public static List<int> GetTakenSeats(string flightNr, DateTime flightTime)
            {
                string? taken = Class1.GetTakenSeats(flightNr, flightTime);
                if (taken == null) return new List<int>();
                return JsonConvert.DeserializeObject<List<int>>(taken);
            }

            public static bool BuyTickets(List<int> reservedSeats, string flightNr, DateTime flightTime, string user, string ticketClass)
            {
                return Class1.BuyTickets(JsonConvert.SerializeObject(reservedSeats), flightNr, flightTime, user, ticketClass);
            }

            public static List<string> GetTicketClasses()
            {
                var rez = Class1.GetSeatClassEnums();
                if (rez == string.Empty) return new List<string>();
                rez = rez.Remove(0, rez.IndexOf('(') + 1); // rez.Indexof() - neiskaitant '('
                rez = rez.Remove(rez.IndexOf(')')).Replace("'", "");

                Console.WriteLine(rez);

                var lst = new List<string>();
                lst.AddRange(rez.Split(','));
                return lst;
            }

            public static DataTable? ViewHistorty(string user)
            {
                return Class1.ViewHistory(user);
            }
        }

    }

    namespace AdminDataTools
    {
        public class AdminTools
        {
            public static object GetAirports()
            {
                DataTable? tbl = AdminToolsDb.GetAllAirports();
                if (tbl == null) return null;
                //disable ID column
                tbl.Columns[0].ReadOnly = true;
                return tbl;
            }

            public static object GetFlights()
            {
                DataTable? tbl = AdminToolsDb.GetAllFlights();
                if (tbl == null) return null;
                //disable ID column
                tbl.Columns[0].ReadOnly = true;
                return tbl;
            }

            public static DataTable? GetPlanes()
            {
                DataTable? tbl = AdminToolsDb.GetAllPlanes();
                if (tbl == null) return null;
                //disable ID column
                tbl.Columns[0].ReadOnly = true;
                return tbl;
            }

            public static DataTable? GetUsers()
            {
                DataTable? tbl = AdminToolsDb.GetAllUsers();
                if (tbl == null) return null;
                //disable ID column
                tbl.Columns[0].ReadOnly = true;
                return tbl;
            }
        }
    }

}
