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


        public static List<object> getAktivitet(int? _eventId = null)
        {
            List<object> retur = new List<object>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT _a.Id, _a.Navn, _a.PointType ,_a.HoldSport FROM Aktivitet _a";

                if (_eventId != null)
                {
                    sql += ", EventAktivitet _ea WHERE _a.Id = _ea.AktivitetId AND _ea.EventId = @EventId";
                }
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                if (_eventId != null)
                {
                    cmd.Parameters.AddWithValue("@EventId", _eventId);
                }
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

        //LAV MULIGHED FOR AT INDTASTE ET EVENT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public static List<object> getHold(/*int? _eventId = null*/)
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


        //returnerer alle deltagere fra et event med "_eventId". hvis "_eventAktivitetId" er angivet returnere den også deres "Score" fra den angivne aktivitet i eventet
        public static List<object> getDeltagere(int _eventId ,int? _eventAktivtetId = null)
        {
            List<object> retur = new List<object>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT _ed.Id, _ed.Navn, _ed.HoldId, _ed.EventId";

                if (_eventAktivtetId != null)
                {
                    sql += ", _ead.Score";
                }
                sql += " FROM EventDeltager _ed";
                if (_eventAktivtetId != null)
                {
                    sql += ", EventAktivitetDeltager _ead, EventAktivitet _ea WHERE _ead.EventAktivitetId = _ea.Id AND _ead.DeltagerId = _ed.Id AND _ea.AktivitetId = @eventAktivitetId AND";
                }
                else
                {
                    sql += " WHERE";
                }
                sql += " _ed.EventId = @EventId";

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@EventId", _eventId);

                if (_eventAktivtetId != null)
                {
                    cmd.Parameters.AddWithValue("@eventAktivitetId", _eventAktivtetId);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (_eventAktivtetId != null)
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
