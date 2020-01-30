using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DatabaseClassLibrary
{
    public class DBHandler
    {
        private static readonly string connectionString = @"Data Source=planner.aspitweb.dk;Initial Catalog=PowerEvent;User ID=aspitlab;Password=aspitlab;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public static List<object> getEvent()
        {
            List<object> retur = new List<object>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Event";
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retur.Add(
                        new { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString() }
                        );
                }
                reader.Close();
            }
            return retur;
        }

        public static List<object> getAktivitet()
        {
            List<object> retur = new List<object>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Aktivitet";
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retur.Add(
                        new { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString(), PointType = int.Parse(reader["PointType"].ToString()) }
                        );
                }
                reader.Close();
            }
            return retur;
        }

        //public static void add(User _u)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        string sql = "INSERT INTO[User] ";
        //        if (_u.ENavn == null)
        //        {
        //            sql += "(FNavn) VALUES (@FNavn)";
        //        }
        //        else
        //        {
        //            sql += "VALUES (@FNavn, @ENavn)";
        //        }

        //        SqlCommand command = new SqlCommand(sql, con);
        //        command.Parameters.AddWithValue("@FNavn", _u.FNavn);
        //        if (_u.ENavn != null)
        //        {
        //            command.Parameters.AddWithValue("@ENavn", _u.ENavn);
        //        }
        //        con.Open();
        //        command.ExecuteNonQuery();
        //    }
        //}

        //public static void deleteUser(int _Id)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        string sql = "Delete From [User] WHERE Id = @Id";

        //        SqlCommand command = new SqlCommand(sql, con);
        //        command.Parameters.AddWithValue("@Id", _Id);
        //        con.Open();
        //        command.ExecuteNonQuery();
        //    }
        //}


    }
}
