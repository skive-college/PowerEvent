using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using DatabaseClassLibrary.Models;

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

        //returnerer alle hold. hvis "_eventID" er indtastet så returnerer den alle hold fra et event
        public static List<object> getHold(int? _eventId = null)
        {
            List<object> retur = new List<object>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT distinct _h.Id, _h.Navn FROM Hold _h";
                if (_eventId != null)
                {
                    sql += ", EventDeltager _ed WHERE _h.Id = _ed.HoldId AND _ed.EventId = @EventId";
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
                        new { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString() }
                        );
                }
                reader.Close();
            }
            return retur;
        }


        //returnerer alle deltagere fra et event med "_eventId". hvis "_AktivitetId" er angivet returnere den også deres "Score" fra den angivne aktivitet i eventet. du kan også søge på hold med "HoldId".
        public static List<object> getDeltagere(int _eventId ,int? _aktivtetId = null, int? _holdId = null)
        {
            List<object> retur = new List<object>();
            List<Deltager> deltagerList = new List<Deltager>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT Distinct _ed.Id, _ed.Navn, _ed.HoldId, _ed.EventId FROM EventDeltager _ed";

                if (_aktivtetId != null)
                {
                    sql += ", EventAktivitetDeltager _ead, EventAktivitet _ea";
                }
                sql += " WHERE";
                if (_aktivtetId != null)
                {
                    sql += " _ead.EventAktivitetId = _ea.Id AND _ead.DeltagerId = _ed.Id AND _ea.AktivitetId = @AktivitetId AND";
                }
                sql += " _ed.EventId = @EventId";

                if (_holdId != null)
                {
                    sql += " AND _ed.HoldId = @HoldId";
                }

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@EventId", _eventId);

                if (_aktivtetId != null)
                {
                    cmd.Parameters.AddWithValue("@AktivitetId", _aktivtetId);
                }

                if (_holdId != null)
                {
                    cmd.Parameters.AddWithValue("@HoldId", _holdId);
                }


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    deltagerList.Add(
                        new Deltager{ Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString(), HoldId = int.Parse(reader["HoldId"].ToString()), EventId = int.Parse(reader["EventId"].ToString()) }
                    );
                }
                reader.Close();
            }

            //får alle scores for deltagere i den angivne "EventAktivitet" og tilføjer dem til de relevante deltagere
            if (_aktivtetId != null)
            {
                List<DBDeltagerScore> scoreList = new List<DBDeltagerScore>();
                scoreList = getDeltagerScores( _eventId, _aktivtetId);
                foreach (Deltager _deltager in deltagerList)
                {
                    _deltager.ScoreList = new List<DBDeltagerScore>();
                    foreach (DBDeltagerScore _score in scoreList)
                    {
                        if (_deltager.Id == _score.DeltagerId)
                        {
                            _deltager.ScoreList.Add(_score);
                        }
                    }
                    if (_deltager.ScoreList != null)
                    {
                        foreach (DBDeltagerScore _dBDeltagerScore in _deltager.ScoreList)
                        {
                            scoreList.Remove(_dBDeltagerScore);
                        }
                    }
                }
            }

            //konverterer deltageren til et annonymt object
            foreach (Deltager _deltager in deltagerList)
            {
                List<object> dScoreList = new List<object>();
                //konverterer List<DBDeltagerScore> til List<object>
                if (_aktivtetId != null)
                {
                    foreach (DBDeltagerScore _dScore in _deltager.ScoreList)
                    {
                        object o = new { _dScore.Id, _dScore.Score };
                        dScoreList.Add(o);
                    }
                }
                retur.Add( new { Id = _deltager.Id, Navn = _deltager.Navn, HoldId = _deltager.HoldId, EventId = _deltager.EventId, ScoreList = dScoreList}
                    );
            }
            return retur;
        }

        //returnere scores til "getDeltagere()"
        private static List<DBDeltagerScore> getDeltagerScores(int _eventId , int? _aktivtetId)
        {
            List<DBDeltagerScore> retur = new List<DBDeltagerScore>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM EventAktivitetDeltager _ead, EventAktivitet _ea WHERE _ead.EventAktivitetId = _ea.Id AND _ea.EventId = @EventId AND _ea.AktivitetId = @AktivitetId";

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@EventId", _eventId);
                cmd.Parameters.AddWithValue("@AktivitetId", _aktivtetId);
                


                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (_aktivtetId != null)
                    {
                        retur.Add(
                            new DBDeltagerScore{ Id = int.Parse(reader["Id"].ToString()), DeltagerId = int.Parse(reader["DeltagerId"].ToString()), EventAktivitetId = int.Parse(reader["EventAktivitetId"].ToString()), Score = int.Parse(reader["Score"].ToString()) }
                        );
                    }
                }
                reader.Close();
            }
            return retur;
        }


        public static void addDeltager(string _navn, int _holdId, int _eventId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO EventDeltager (Navn, HoldId, EventId) VALUES (@Navn, @HoldId, @EventId)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Navn", _navn);
                cmd.Parameters.AddWithValue("@HoldId", _holdId);
                cmd.Parameters.AddWithValue("@EventId", _eventId);

                cmd.ExecuteNonQuery();
            }
        }





    }
}
