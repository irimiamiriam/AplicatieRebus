using AplicatieRebus.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AplicatieRebus.SqlDataAccess
{
    internal class DatabaseHelper
    {
        private static readonly string _connectionstring = DataAccess.GetConnectionString();

        public static void Initialisation()
        {
            DeleteData();
            UserModel user = new UserModel();
            user.Name = "admin";
            user.Password = "1234";
            user.Email = "oti2016@bacau.ro";
            user.Tip = 1;
            InsertNewUser(user);
            InsertAllRebusuri();
        }

        private static void InsertAllRebusuri()
        {
            string[] files = { "C:\\Users\\Miriam\\Documents\\Aplicatii C\\CSHARP Nationala\\AplicatieRebus\\Resurse\\Dorinte.txt", "C:\\Users\\Miriam\\Documents\\Aplicatii C\\CSHARP Nationala\\AplicatieRebus\\Resurse\\Scriitori.txt", "C:\\Users\\Miriam\\Documents\\Aplicatii C\\CSHARP Nationala\\AplicatieRebus\\Resurse\\Sport.txt" };
            foreach(string file in files) {
                using (SqlConnection con = new SqlConnection(_connectionstring))
                {
                    con.Open();

                    using (StreamReader reader = new StreamReader(file))
                    {
                        int idRebus = 0;
                        string cmdRebusuri = "Insert into Rebusuri values (@id, @nume,@col, @lin,@timp )";
                        string cmdRezolvari = "Insert into Rezolvari values (@id, @col, @lin,@orien, @sol, @def)";
                        using (SqlCommand cmd = new SqlCommand(cmdRebusuri, con))
                        {
                            string line = reader.ReadLine();
                            var split = line.Split('|');
                            idRebus = Convert.ToInt32(split[0]);
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(split[0]));
                            cmd.Parameters.AddWithValue("@nume", split[1]);
                            cmd.Parameters.AddWithValue("@col", Convert.ToInt32(split[2]));
                            cmd.Parameters.AddWithValue("@lin", Convert.ToInt32(split[3]));
                            cmd.Parameters.AddWithValue("@timp", Convert.ToInt32(split[4]));
                            cmd.ExecuteNonQuery();
                        }
                        while (reader.Peek() >= 0)
                        {
                            string line = reader.ReadLine();
                            var split = line.Split('|');
                            using (SqlCommand cmd = new SqlCommand(cmdRezolvari, con))
                            {
                                cmd.Parameters.AddWithValue("@id", idRebus);
                                cmd.Parameters.AddWithValue("@col", Convert.ToInt32(split[0]));
                                cmd.Parameters.AddWithValue("@lin", Convert.ToInt32(split[1]));
                                cmd.Parameters.AddWithValue("@orien", split[2]);
                                cmd.Parameters.AddWithValue("@sol", split[3]);
                                cmd.Parameters.AddWithValue("@def", split[4]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        public static void InsertRebusuri(string file)
        {
           
            using (SqlConnection con = new SqlConnection(_connectionstring))
            {
                con.Open();
              
                    using(StreamReader reader = new StreamReader(file))
                    {
                        int idRebus = 0;
                        string cmdRebusuri = "Insert into Rebusuri values (@id, @nume,@col, @lin,@timp )";
                        string cmdRezolvari = "Insert into Rezolvari values (@id, @col, @lin,@orien, @sol, @def)";
                        using(SqlCommand cmd = new SqlCommand(cmdRebusuri, con))
                        {
                            string line = reader.ReadLine();
                            var split = line.Split('|');
                            idRebus = Convert.ToInt32(split[0]);
                            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(split[0]));
                            cmd.Parameters.AddWithValue("@nume", split[1]);
                            cmd.Parameters.AddWithValue("@col", Convert.ToInt32(split[2]));
                            cmd.Parameters.AddWithValue("@lin", Convert.ToInt32(split[3]));
                            cmd.Parameters.AddWithValue("@timp", Convert.ToInt32(split[4]));
                            cmd.ExecuteNonQuery();
                        }
                        while (reader.Peek() >= 0)
                        {
                            string line = reader.ReadLine();
                            var split = line.Split('|');
                            using (SqlCommand cmd = new SqlCommand(cmdRezolvari, con))
                            {
                                cmd.Parameters.AddWithValue("@id", idRebus);
                                cmd.Parameters.AddWithValue("@col", Convert.ToInt32(split[0]));
                                cmd.Parameters.AddWithValue("@lin", Convert.ToInt32(split[1]));
                                cmd.Parameters.AddWithValue("@orien", split[2]);
                                cmd.Parameters.AddWithValue("@sol", split[3]);
                                cmd.Parameters.AddWithValue("@def", split[4]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                }
            
        }

        public static void InsertNewUser(UserModel user)
        {
            using(SqlConnection con = new SqlConnection(_connectionstring))
            {
                con.Open();
                string cmdText = "Insert into Utilizatori (Parola, NumeUtilizator, Email , TipUtilizator) values (@pass, @nume, @email, @tip)";
                using(SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@pass", user.Password);
                    cmd.Parameters.AddWithValue("@nume", user.Name);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@tip", user.Tip);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void DeleteData()
        {
            bool existsData = false;
            using (SqlConnection con = new SqlConnection(_connectionstring))
            {
                con.Open();
                using(SqlCommand cmd = new SqlCommand("Select * from Utilizatori",con))
                {
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read()) { existsData = true; }
                    }
                }
                if(existsData)
                {
                    ExecuteDelete("Utilizatori", con);
                    ExecuteDelete("Statistica", con);
                    ExecuteDelete("Rezolvari", con);
                    ExecuteDelete("Rebusuri", con);
                    

                }
            }
        }

        private static void ExecuteDelete(string table, SqlConnection con)
        {
           using(SqlCommand cmd = new SqlCommand("Delete from "+ table, con))
            {
                cmd.ExecuteNonQuery();
             }
            if (table == "Utilizatori")
            {
                using (SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT( " + table+ ", RESEED,0)", con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal static UserModel FindUser(UserModel user)
        {
           UserModel userModel = null;
            using(SqlConnection con = new SqlConnection(_connectionstring))
            {
                con.Open();
                string cmdText = "Select * from Utilizatori where Parola = @parola and NumeUtilizator = @nume";
                using(SqlCommand cmd = new SqlCommand(@cmdText,con))
                {
                    cmd.Parameters.AddWithValue("@nume", user.Name);
                    cmd.Parameters.AddWithValue("@parola", user.Password);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            userModel = new UserModel();
                            userModel.Id = Convert.ToInt32(reader[0]);
                            userModel.Email= reader[3].ToString();
                            userModel.Password= user.Password;
                            userModel.Name= user.Name;
                            userModel.Tip = Convert.ToInt32(reader[4]);
                        }
                    }
                }
            }
            return userModel;
        }

        internal static UserModel FindUserByNumeOrEmail(string nume, string email)
        {
            UserModel userModel = null;
            using (SqlConnection con = new SqlConnection(_connectionstring))
            {
                con.Open();
                string cmdText = "Select * from Utilizatori where Email = @email or NumeUtilizator = @nume";
                using (SqlCommand cmd = new SqlCommand(@cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@nume", nume);
                    cmd.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userModel = new UserModel();
                            userModel.Id = Convert.ToInt32(reader[0]);
                            userModel.Email = reader[3].ToString();
                            
                        }
                    }
                }
            }
            return userModel;
        }

        internal static List<RebusModel> GetRebusuri()
        {
           List<RebusModel> rebusuri = new List<RebusModel>();
            using(SqlConnection con = new SqlConnection(_connectionstring))
            {
                con.Open();
                string cmdText = "Select * from Rebusuri";
                using(SqlCommand cmd= new SqlCommand(@cmdText, con))
                {
                    using(SqlDataReader  reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            RebusModel rebus = new RebusModel()
                            {
                                Id= Convert.ToInt32(reader[0]),
                                Name= reader[1].ToString(),
                                NrColoane= Convert.ToInt32(reader[2]),
                                NrLinii= Convert.ToInt32(reader[3]),
                                NrSecunde= Convert.ToInt32(reader[4])
                            };
                            rebusuri.Add(rebus);  
                        }
                    }
                }
            }
            return rebusuri;
        }

        internal static List<RezolvareModel> GetRezolvari(int id)
        {
            List<RezolvareModel> rezolvari = new List<RezolvareModel>();
            using (SqlConnection con = new SqlConnection(_connectionstring))
            {
                con.Open();
                string cmdText = "Select * from Rezolvari where IdRebus= @id";
                using (SqlCommand cmd = new SqlCommand(@cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RezolvareModel rezolvare = new RezolvareModel()
                            {
                               
                                ColoanaStart = Convert.ToInt32(reader[1]),
                                LinieStart = Convert.ToInt32(reader[2]),
                                Orientare= reader[3].ToString(),
                                Solutie= reader[4].ToString(),
                                Definitie= reader[5].ToString()
                             
                            };
                            rezolvari.Add(rezolvare);
                        }
                    }
                }
            }
            return rezolvari;
        }

        internal static void InsertStatistica(int idUser, int idRebus, int nrsecunde, int nrGresite, int stare)
        {
            using(SqlConnection con = new SqlConnection(_connectionstring))
            {
                con.Open();
                string cmdText = "Insert into Statistica values (@id1, @id2, @timp, @nr, @stare)";
                using(SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@id1", idUser);
                    cmd.Parameters.AddWithValue("@id2", idRebus);
                    cmd.Parameters.AddWithValue("@timp", nrsecunde);
                    cmd.Parameters.AddWithValue("@nr", nrGresite);
                    cmd.Parameters.AddWithValue("@stare", stare);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
