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
                        new { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString(), PointType = int.Parse(reader["PointType"].ToString()), HoldSport = int.Parse(reader["HoldSport"].ToString()) }
                        );
                }
                reader.Close();
            }
            return retur;
        }

        public static void addAktivitet(string _navn, int _pointType, int _holdSport)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Aktivitet Values(@Navn, @PointType, @HoldSport)";
                

                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.AddWithValue("@Navn", _navn);
                command.Parameters.AddWithValue("@PointType", _pointType);
                command.Parameters.AddWithValue("@HoldSport", _holdSport);
                con.Open();
                command.ExecuteNonQuery();
            }
        }

        public static void deleteAktivitet(int _id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "Delete From Aktivitet WHERE Id = @Id";

                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.AddWithValue("@Id", _id);
                con.Open();
                command.ExecuteNonQuery();
            }
        }

        public static List<object> getHold()
        {
            List<object> retur = new List<object>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Hold";
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

        public static List<object> getDeltagere(int? _eventAktivtetsId = null)
        {
            List<object> retur = new List<object>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT _ed.Id, _ed.Navn, _ed.HoldId, _ed.EventId";

                if (_eventAktivtetsId != null)
                {
                    sql += ", _ead.Score";
                }
                sql += " FROM EventDeltager _ed, EventAktivitetDeltager _ead, EventAktivitet _ea WHERE _ead.EventAktivitetId = _ea.Id AND _ead.DeltagerId = _ed.Id";

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (_eventAktivtetsId != null)
                    {
                        retur.Add(
                            new { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString(), HoldId = int.Parse(reader["HoldId"].ToString()), EventId = int.Parse(reader["EventId"].ToString()) }
                        );

                    }
                    else
                    {
                        retur.Add(
                            new { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString(), HoldId = int.Parse(reader["HoldId"].ToString()), EventId = int.Parse(reader["EventId"].ToString()), Score = int.Parse(reader["EventId"].ToString()) }
                        );

                    }
                }
                reader.Close();
            }
            return retur;
        }







    }
}
