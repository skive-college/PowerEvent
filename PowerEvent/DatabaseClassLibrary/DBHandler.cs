﻿using System;
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

        //___________________________________________________________________________________________________________alt med Event ↓

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
        //___________________________________________________________________________________________________________alt med Event ↑

        //___________________________________________________________________________________________________________alt med Aktivitet  ↓
        public static List<object> getAktivitet(int? _eventId = null, int? _aktivitetId = null)
        {
            List<object> retur = new List<object>();
            List<Aktivitet> aktivitetList = getAktivitetIntern(_eventId, _aktivitetId);
            foreach (Aktivitet _aktivitet in aktivitetList)
            {
                retur.Add(
                        new { Id = _aktivitet.Id, Navn = _aktivitet.Navn, PointType = _aktivitet.PointType, HoldSport = _aktivitet.HoldSport }
                        );
            }
            return retur;
        }

        private static List<Aktivitet> getAktivitetIntern(int? _eventId = null, int? _aktivitetId = null)
        {
            List<Aktivitet> retur = new List<Aktivitet>();

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
                        new Aktivitet { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString(), PointType = int.Parse(reader["PointType"].ToString()), HoldSport = int.Parse(reader["HoldSport"].ToString()) }
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


        public static List<object> getEventAktivitet(int _eventId, int? _aktivitetId = null)
        {
            List<object> retur = new List<object>();
            List<EventAktivitet> aktivitetList = getEventAktivitetIntern(_eventId, _aktivitetId);
            foreach (EventAktivitet _eventAktivitet in aktivitetList)
            {
                retur.Add(
                        new { Id = _eventAktivitet.Id, EventId = _eventAktivitet.EventId, AktivitetId = _eventAktivitet.AktivitetId }
                        );
            }
            return retur;
        }

        private static List<EventAktivitet> getEventAktivitetIntern(int _eventId, int? _aktivitetId = null)
        {
            List<EventAktivitet> retur = new List<EventAktivitet>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT _ea.Id, _ea.EventId, _ea.AktivitetId FROM EventAktivitet _ea WHERE _ea.EventId = @EventId";
                if (_aktivitetId != null)
                {
                    sql += " AND _ea.AktivitetId = @AktivitetId";
                }
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@EventId", _eventId);

                if (_aktivitetId != null)
                {
                    cmd.Parameters.AddWithValue("@AktivitetId", _aktivitetId);
                }
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retur.Add(
                        new EventAktivitet { Id = int.Parse(reader["Id"].ToString()), EventId = int.Parse(reader["EventId"].ToString()), AktivitetId = int.Parse(reader["AktivitetId"].ToString()) }
                        );
                }
                reader.Close();
            }
            return retur;
        }


        //___________________________________________________________________________________________________________alt med Aktivitet ↑

        //___________________________________________________________________________________________________________alt med Hold ↓

        //konverterer holdet til et annonymt object
        public static List<object> getHold(int? _eventId = null, int? _holdOrder = null, int? _aktivitetId = null)
        {
            List<object> retur = new List<object>();
            List<Hold> holdList = getHoldIntnern(_eventId, _holdOrder, _aktivitetId);
            foreach (Hold _hold in holdList)
            {
                List<object> hAktivitetList = new List<object>();
                retur.Add(new { Id = _hold.Id, Navn = _hold.Navn, Farve = _hold.Farve, HoldAktiviteter = hAktivitetList }
                    );
            }
            return retur;
        }

        //returnerer alle hold. hvis "_eventID" er indtastet så returnerer den alle hold fra et event.
        private static List<Hold> getHoldIntnern(int? _eventId = null, int? _holdOrder = null, int? _aktivitetId = null)
        {
            List<Hold> retur = new List<Hold>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT distinct _h.Id, _h.Navn, _h.Farve";
                sql += " FROM Hold _h";
                if (_eventId != null)
                {
                    sql += ", EventDeltager _ed";
                }
                if (_holdOrder != null && _aktivitetId != null)
                {
                    sql += ", EventAktivitetHold _eah, EventAktivitet _ea";
                }
                if (_eventId != null || _holdOrder != null && _aktivitetId != null)
                {
                    sql += " WHERE";
                }
                if (_eventId != null)
                {
                    sql += " _h.Id = _ed.HoldId AND _ed.EventId = @EventId";
                }
                if (_holdOrder != null && _aktivitetId != null)
                {
                    sql += " AND _eah.HoldId = _h.Id AND _ea.Id = _eah.EventAktivitetId AND _ea.AktivitetId = @AktivitetId AND _eah.HoldOrder = @HoldOrder";
                }
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                if (_eventId != null)
                {
                    cmd.Parameters.AddWithValue("@EventId", _eventId);
                }
                if (_holdOrder != null && _aktivitetId != null)
                {
                    cmd.Parameters.AddWithValue("@HoldOrder", _holdOrder);
                    cmd.Parameters.AddWithValue("@AktivitetId", _aktivitetId);
                }
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retur.Add(new Hold() { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString(), Farve = reader["Farve"].ToString(), HoldAktiviteter = new List<EventAktivitetHold>() });
                }
                reader.Close();
            }
            return retur;
        }

        public static List<object> getHoldAktivitet(int? _eventId, int? _holdOrder = null, int? _aktivitetId = null)
        {
            List<object> retur = new List<object>();
            List<EventAktivitetHold> eventAktivitetHoldList = getHoldAktivitetIntern(_eventId, _holdOrder, _aktivitetId);
            foreach (EventAktivitetHold _eventAktivitetHold in eventAktivitetHoldList)
            {
                retur.Add(
                        new { Id = _eventAktivitetHold.Id, EventAktivitetId = _eventAktivitetHold.EventAktivitetId, HoldId = _eventAktivitetHold.HoldId, Point = _eventAktivitetHold.Point, HoldOrder = _eventAktivitetHold.HoldOrder, HoldScores = _eventAktivitetHold.HoldScores }
                        );
            }
            return retur;
        }

        //returnerer alle EventAktivitetHold (inklusiv deres "holdscores") i det angivne event med "_eventId". hvis "_holdOrder" og "_aktivitetId" er angivet; returnerer alle EventAktivitetHold med "_aktivitetId" og "_holdOrder"
        private static List<EventAktivitetHold> getHoldAktivitetIntern(int? _eventId, int? _holdOrder = null, int? _aktivitetId = null)
        {
            List<EventAktivitetHold> retur = new List<EventAktivitetHold>();
            List<Aktivitet> tempAktivitetList = new List<Aktivitet>();
            Aktivitet tempAktivitet = new Aktivitet();
            if (_aktivitetId != null)
            {
                tempAktivitetList = getAktivitetIntern(_eventId, _aktivitetId);
                if (tempAktivitetList.Count != 0)
                {
                    tempAktivitet = tempAktivitetList[0];
                }
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT _eah.Id, _eah.EventAktivitetId, _eah.HoldId, _eah.Point, _eah.HoldOrder FROM EventAktivitetHold _eah, EventAktivitet _ea WHERE _ea.Id = _eah.EventAktivitetId  AND _ea.EventId = @EventId";
                if (_holdOrder != null && _aktivitetId != null)
                {
                    sql += " AND _ea.AktivitetId = @AktivitetId AND _eah.HoldOrder = @HoldOrder";
                }
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                if (_eventId != null)
                {
                    cmd.Parameters.AddWithValue("@EventId", _eventId);
                }
                if (_holdOrder != null && _aktivitetId != null)
                {
                    cmd.Parameters.AddWithValue("@HoldOrder", _holdOrder);
                    cmd.Parameters.AddWithValue("@AktivitetId", _aktivitetId);
                }
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int? pointRead = null;
                    try
                    {
                        pointRead = int.Parse(reader["Point"].ToString());
                    }
                    catch
                    {
                    }
                    retur.Add(
                        new EventAktivitetHold() { Id = int.Parse(reader["Id"].ToString()), EventAktivitetId = int.Parse(reader["EventAktivitetId"].ToString()), HoldId = int.Parse(reader["HoldId"].ToString()), Point = pointRead, HoldOrder = int.Parse(reader["HoldOrder"].ToString()), HoldScores = new List<EventAktivitetHoldScore>() }
                        );
                }
                reader.Close();
            }
            return retur;
        }

        public static List<object> getHoldAktivitetScores(int? _eventId, int? _holdOrder = null, int? _aktivitetId = null)
        {
            List<object> retur = new List<object>();
            List<EventAktivitetHoldScore> eventAktivitetHoldList = getHoldAktivitetScoresIntern(_eventId, _holdOrder, _aktivitetId);
            foreach (EventAktivitetHoldScore _eventAktivitetHoldScore in eventAktivitetHoldList)
            {
                retur.Add(
                        new { Id = _eventAktivitetHoldScore.Id, EventAktivitetHoldId = _eventAktivitetHoldScore.EventAktivitetHoldId, HoldScore = _eventAktivitetHoldScore.HoldScore }
                        );
            }
            return retur;
        }

        //returnerer alle EventAktivitetHoldScore fra det angivne event med "_eventId". hvis "_holdOrder" og "_aktivitetId" er angivet; returnerer EventAktivitetHoldScore fra angivne "aktivitet" med den angivne holdOrder.
        private static List<EventAktivitetHoldScore> getHoldAktivitetScoresIntern(int? _eventId, int? _holdOrder = null, int? _aktivitetId = null)
        {
        List<EventAktivitetHoldScore> retur = new List<EventAktivitetHoldScore>();
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string sql = "SELECT _eahs.Id, _eahs.EventAktivitetHoldId, _eahs.HoldScore FROM EventAktivitetHoldScore _eahs, EventAktivitetHold _eah, EventAktivitet _ea WHERE _eahs.EventAktivitetHoldId = _eah.Id AND _eah.EventAktivitetId = _ea.Id AND _ea.EventId = @EventId";
            if (_holdOrder != null && _aktivitetId != null)
            {
                sql += " AND _Eah.HoldOrder = @HoldOrder AND _ea.AktivitetId = @AktivitetId";
            }
            con.Open();
            SqlCommand cmd = new SqlCommand(sql, con);
            if (_eventId != null)
            {
                cmd.Parameters.AddWithValue("@EventId", _eventId);
            }
            if (_holdOrder != null && _aktivitetId != null)
            {
                cmd.Parameters.AddWithValue("@HoldOrder", _holdOrder);
                cmd.Parameters.AddWithValue("@AktivitetId", _aktivitetId);
            }

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                retur.Add(
                    new EventAktivitetHoldScore() { Id = int.Parse(reader["Id"].ToString()), EventAktivitetHoldId = int.Parse(reader["EventAktivitetHoldId"].ToString()), HoldScore = int.Parse(reader["HoldScore"].ToString()) }
                    );
            }
            reader.Close();
        }
        return retur;
    }

        public static void addHoldScore(int _eventId, int _aktivitetId, int _holdOrder, int _holdId, int _score)
        {
            List<EventAktivitetHold> tempList = getHoldAktivitetIntern(_eventId, _holdOrder, _aktivitetId);
            EventAktivitetHold temphold = new EventAktivitetHold();
            temphold = tempList.Where(i => i.HoldId == _holdId).FirstOrDefault();
            if (temphold != null)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO EventAktivitetHoldScore Values(@EventAktivitetHoldId, @HoldScore)";
                    SqlCommand command = new SqlCommand(sql, con);
                    command.Parameters.AddWithValue("@EventAktivitetHoldId", temphold.Id);
                    command.Parameters.AddWithValue("@HoldScore", _score);
                    con.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void deleteHoldScore(int _id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "Delete From EventAktivitetHoldScore WHERE id = @Id";

                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.AddWithValue("@Id", _id);
                con.Open();
                command.ExecuteNonQuery();
            }
        }
        //___________________________________________________________________________________________________________alt med Hold ↑

        //___________________________________________________________________________________________________________alt med Deltagere ↓

        //konverterer deltageren til et annonymt object
        public static List<object> getDeltagere(int _eventId, int? _aktivitetId = null, int? _holdId = null, int? _deltagerId = null)
        {
            List<object> retur = new List<object>();
            List<Deltager> deltagerList = new List<Deltager>();
            deltagerList = getDeltagereIntern(_eventId, _aktivitetId, _holdId, _deltagerId);
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
                retur.Add(new { Id = _deltager.Id, Navn = _deltager.Navn, HoldId = _deltager.HoldId, EventId = _deltager.EventId, ScoreList = dScoreList }
                    );
            }
            return retur;
        }
        //returnerer alle deltagere fra et event med "_eventId". hvis "_AktivitetId" er angivet returnere den også deres "Score" fra den angivne aktivitet i eventet. du kan også søge på hold med "HoldId".
        private static List<Deltager> getDeltagereIntern(int _eventId, int? _aktivitetId = null, int? _holdId = null, int? _deltagerId = null)
        {
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
                    sql += " _ed.DeltagerId = @DeltagerId";
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
                    int? tempHoldId = null;
                    tempHoldId = int.Parse(reader["HoldId"].ToString());
                    deltagerList.Add(
                        new Deltager { Id = int.Parse(reader["Id"].ToString()), Navn = reader["Navn"].ToString(), HoldId = tempHoldId, EventId = int.Parse(reader["EventId"].ToString()) }
                    );
                }
                reader.Close();
            }

            //får alle scores for deltagere i den angivne "EventAktivitet" og tilføjer dem til de relevante deltagere
            if (_aktivitetId != null)
            {
                List<DBDeltagerScore> scoreList = new List<DBDeltagerScore>();
                scoreList = getDeltagerScores(_eventId, _aktivitetId);
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
            return deltagerList;
        }

        //returnerer scores
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

        public static void addDeltagerScore(int _eventId, int _aktivitetId, int _holdId, int _deltagerId, int _score)
        {
            List<Deltager> tempList = getDeltagereIntern(_eventId, _aktivitetId, _holdId);
            Deltager tempDeltager = new Deltager();
            tempDeltager = tempList.Where(i => i.Id == _deltagerId).FirstOrDefault();
            if (tempDeltager != null)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO EventAktiviteDeltager Values(@DeltagerId, @EventAktivitetId, @HoldScore)";
                    SqlCommand command = new SqlCommand(sql, con);
                    command.Parameters.AddWithValue("@DeltagerId", tempDeltager.Id);
                    command.Parameters.AddWithValue("@EventAktivitetHoldId", tempDeltager.);
                    command.Parameters.AddWithValue("@HoldScore", _score);
                    con.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void deleteDeltagerScore(int _id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "Delete From EventAktivitetDeltager WHERE id = @Id";

                SqlCommand command = new SqlCommand(sql, con);
                command.Parameters.AddWithValue("@Id", _id);
                con.Open();
                command.ExecuteNonQuery();
            }
        }
        //___________________________________________________________________________________________________________alt med Deltagere ↑

        //___________________________________________________________________________________________________________Alt med Hold Order ↓

        public static List<int> getHoldOrder(int _eventId, int? _aktivitetId = null)
        {
            List<int> retur = new List<int>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT distinct _eah.HoldOrder FROM EventAktivitetHold _eah, EventAktivitet _ea Where _ea.Id = _eah.EventAktivitetId AND _ea.EventId = @EventId";
                
                if (_aktivitetId != null)
                {                    
                    sql += " AND _ea.AktivitetId = @AktivitetId";
                }
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);                
                cmd.Parameters.AddWithValue("@EventId", _eventId);
                
                if (_aktivitetId != null)
                {
                    cmd.Parameters.AddWithValue("@AktivitetId", _aktivitetId);
                }
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    retur.Add(
                        int.Parse(reader["HoldOrder"].ToString()) 
                        );
                }
                reader.Close();
            }
            return retur;
        }

        //___________________________________________________________________________________________________________Alt med Hold Order ↑
    }
}
