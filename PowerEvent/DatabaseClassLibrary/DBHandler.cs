using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using DatabaseClassLibrary.Models;
using System.Linq;

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


        public static List<object> getAktivitet(int? _eventId = null, int? _aktivitetId = null)
        {
            List<object> retur = new List<object>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT _a.Id, _a.Navn, _a.PointType ,_a.HoldSport FROM Aktivitet _a";
                if (_eventId != null)
                {
                    sql += ", EventAktivitet _ea";
                }
                if (_eventId != null || _aktivitetId != null)
                {
                    sql += " WHERE";
                }
                if (_eventId != null)
                {
                    sql += " _a.Id = _ea.AktivitetId AND _ea.EventId = @EventId";
                }
                if (_aktivitetId != null)
                {
                    if (_eventId != null)
                    {
                        sql += " AND";
                    }
                    sql += " _a.Id = @AktivitetId";
                }
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                if (_eventId != null)
                {
                    cmd.Parameters.AddWithValue("@EventId", _eventId);
                }
                if (_aktivitetId != null)
                {
                    cmd.Parameters.AddWithValue("@AktivitetId", _aktivitetId);
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

        //returnerer alle hold. hvis "_eventID" er indtastet så returnerer den alle hold fra et event.    færdigør metode!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public static List<object> getHold(int? _eventId = null, int? _holdOrder = null, int? _aktivitetId = null)
        {
            List<object> retur = new List<object>();
            List<Hold> holdList = new List<Hold>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT distinct _h.Id, _h.Navn FROM Hold _h";
                if (_eventId != null)
                {
                    sql += ", EventDeltager _ed WHERE _h.Id = _ed.HoldId AND _ed.EventId = @EventId";
                }
                if (_holdOrder != null && _aktivitetId != null)
                {
                    sql += "";
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
                    holdList.Add(new Hold() { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString() });
                }
                reader.Close();
            }

            if (_holdOrder != null && _aktivitetId != null)
            {
                List<EventAktivitetHold> holdEventAktivitetList = new List<EventAktivitetHold>();
                holdEventAktivitetList = getHoldAktivitet(_eventId, _holdOrder, _aktivitetId);
                foreach (Hold _hold in holdList)
                {
                    _hold.HoldAktiviteter = new List<EventAktivitetHold>();
                    _hold.HoldAktiviteter.AddRange(holdEventAktivitetList.Where(i => i.HoldId == _hold.Id).ToList());
                    if (_hold.HoldAktiviteter != null)
                    {
                        holdEventAktivitetList.RemoveAll(i => i.HoldId == _hold.Id);
                    }
                }
            }
            //konverterer holdet til et annonymt object
            foreach (Hold _hold in holdList)
            {
                List<object> hAktivitetList = new List<object>();
                //konverterer List<DBDeltagerScore> til List<object>
                if (_holdOrder != null || _aktivitetId != null)
                {
                    foreach (EventAktivitetHold _hAktivitet in _hold.HoldAktiviteter)
                    {
                        List<object> hAktivitetScores = new List<object>();
                        foreach (EventAktivitetHoldScore _score in _hAktivitet.HoldScores)
                        {
                            object tempScore = new { Id = _score.Id, EventAktivitetHoldId = _score.EventAktivitetHoldId, HoldScore = _score.HoldScore };
                            hAktivitetScores.Add(tempScore);
                        }
                        object tempAktivitet = new { _hAktivitet.EventAktivitet.Id, _hAktivitet.EventAktivitet.Navn, _hAktivitet.EventAktivitet.PointType, _hAktivitet.EventAktivitet.HoldSport };
                        object tempHoldAktivitet = new { _hAktivitet.Id, EventAktivitet = tempAktivitet, _hAktivitet.Point, _hAktivitet.HoldOrder, HoldScores = hAktivitetScores };
                        hAktivitetList.Add(tempHoldAktivitet);
                    }
                }
                retur.Add(new { Id = _hold.Id, Navn = _hold.Navn, HoldAktiviteter = hAktivitetList }
                    );
            }
            return retur;
        }
        //lav SQL sætning!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private static List<EventAktivitetHold> getHoldAktivitet(int? _eventId = null, int? _holdOrder = null, int? _aktivitetId = null)
        {
            List<EventAktivitetHold> retur = new List<EventAktivitetHold>();
            List<EventAktivitetHoldScore> scoreList = new List<EventAktivitetHoldScore>();
            scoreList = getHoldAktivitetScores(_eventId, _holdOrder, _aktivitetId);
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
                        new EventAktivitetHold() { Id = int.Parse(reader["Id"].ToString()), Point = int.Parse(reader["Point"].ToString()), HoldOrder = int.Parse(reader["HoldOrder"].ToString()) }
                        );
                }
                reader.Close();
            }
            return retur;
        }
        //lav SQL sætning!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private static List<EventAktivitetHoldScore> getHoldAktivitetScores(int? _eventId = null, int? _holdOrder = null, int? _aktivitetId = null)
        {
            List<EventAktivitetHoldScore> retur = new List<EventAktivitetHoldScore>();
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
                        new EventAktivitetHoldScore() { Id = int.Parse(reader["Id"].ToString()), EventAktivitetHoldId = int.Parse(reader["Point"].ToString()), HoldScore = int.Parse(reader["HoldOrder"].ToString()) }
                        );
                }
                reader.Close();
            }
            return retur;
        }


        //returnerer alle deltagere fra et event med "_eventId". hvis "_AktivitetId" er angivet returnere den også deres "Score" fra den angivne aktivitet i eventet. du kan også søge på hold med "HoldId".
        public static List<object> getDeltagere(int _eventId ,int? _aktivitetId = null, int? _holdId = null, int? _deltagerId = null)
        {
            List<object> retur = new List<object>();
            List<Deltager> deltagerList = new List<Deltager>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT Distinct _ed.Id, _ed.Navn, _ed.HoldId, _ed.EventId FROM EventDeltager _ed";

                if (_aktivitetId != null)
                {
                    sql += ", EventAktivitetDeltager _ead, EventAktivitet _ea";
                }
                sql += " WHERE";
                if (_aktivitetId != null)
                {
                    sql += " _ead.EventAktivitetId = _ea.Id AND _ead.DeltagerId = _ed.Id AND _ea.AktivitetId = @AktivitetId AND";
                }
                sql += " _ed.EventId = @EventId";

                if (_holdId != null)
                {
                    sql += " AND _ed.HoldId = @HoldId";
                }
                if (_deltagerId != null)
                {
                    if (_holdId != null)
                    {
                        sql += " AND";
                    }
                    sql += " _ed = @DeltagerId";
                }

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@EventId", _eventId);

                if (_aktivitetId != null)
                {
                    cmd.Parameters.AddWithValue("@AktivitetId", _aktivitetId);
                }

                if (_holdId != null)
                {
                    cmd.Parameters.AddWithValue("@HoldId", _holdId);
                }
                if (_deltagerId != null)
                {
                    cmd.Parameters.AddWithValue("@DeltagerId", _deltagerId);
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
            if (_aktivitetId != null)
            {
                List<DBDeltagerScore> scoreList = new List<DBDeltagerScore>();
                scoreList = getDeltagerScores( _eventId, _aktivitetId);
                foreach (Deltager _deltager in deltagerList)
                {
                    _deltager.ScoreList = new List<DBDeltagerScore>();
                    _deltager.ScoreList.AddRange(scoreList.Where(i => i.DeltagerId == _deltager.Id).ToList());
                    if (_deltager.ScoreList != null)
                    {
                        scoreList.RemoveAll(i => i.DeltagerId == _deltager.Id);
                    }
                }
            }

            //konverterer deltageren til et annonymt object
            foreach (Deltager _deltager in deltagerList)
            {
                List<object> dScoreList = new List<object>();
                //konverterer List<DBDeltagerScore> til List<object>
                if (_aktivitetId != null)
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
        private static List<DBDeltagerScore> getDeltagerScores(int _eventId , int? _aktivtetId, int? _deltagerId = null)
        {
            List<DBDeltagerScore> retur = new List<DBDeltagerScore>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM EventAktivitetDeltager _ead, EventAktivitet _ea WHERE _ead.EventAktivitetId = _ea.Id AND _ea.EventId = @EventId AND _ea.AktivitetId = @AktivitetId";
                if (_deltagerId != null)
                {
                    sql += " AND _ead.Id = @DeltagerId";
                }

                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@EventId", _eventId);
                cmd.Parameters.AddWithValue("@AktivitetId", _aktivtetId);
                if (_deltagerId != null)
                {
                    cmd.Parameters.AddWithValue("@DeltagerId", _deltagerId);
                }


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
