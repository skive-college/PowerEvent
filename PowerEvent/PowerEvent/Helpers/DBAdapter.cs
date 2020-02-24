using DatabaseClassLibrary;
using PowerEvent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerEvent.Helpers
{
    public class DBAdapter
    {
        //___________________________________________________________________________________________________________Alt med Event ↓

        public static List<Event> getEvent()
        {
            List<object> dbList = DBHandler.getEvent();
            List<Event> eventList = new List<Event>();

            foreach (object _object in dbList)
            {
                Event temp = new Event();
                temp.Id = adapt<int>("Id", _object);
                temp.Navn = adapt<string>("Navn", _object);
                eventList.Add(temp);
            }
            return eventList;
        }

        //___________________________________________________________________________________________________________Alt med Event ↑

        //___________________________________________________________________________________________________________Alt med Aktivitet ↓

        public static List<Aktivitet> getAktivitet(int? _eventId = null, int? _aktivitetId = null)
        {
            List<object> dbList = DBHandler.getAktivitet(_eventId, _aktivitetId);
            List<Aktivitet> AktivitetList = new List<Aktivitet>();

            foreach (object _object in dbList)
            {
                Aktivitet temp = new Aktivitet();
                temp.Id = adapt<int>("Id", _object);
                temp.Navn = adapt<string>("Navn", _object);
                temp.PointType = adapt<int>("PointType", _object);
                temp.HoldSport = adapt<int>("HoldSport", _object);
                AktivitetList.Add(temp);
            }
            return AktivitetList;
        }

        public static void deleteAktivitet(int _id)
        {
            DBHandler.deleteAktivitet(_id);
        }
        
        public static void addAktivitet(string _navn, int _pointType, int _holdSport)
        {
            DBHandler.addAktivitet(_navn ,_pointType, _holdSport);
        }


        public static List<EventAktivitet> getEventAktivitet(int _eventId, int? _aktivitetId = null)
        {
            List<object> dbList = DBHandler.getEventAktivitet(_eventId, _aktivitetId);
            List<EventAktivitet> AktivitetList = new List<EventAktivitet>();

            foreach (object _object in dbList)
            {
                EventAktivitet temp = new EventAktivitet();
                temp.Id = adapt<int>("Id", _object);
                temp.EventId = adapt<int>("EventId", _object);
                temp.AktivitetId = adapt<int>("AktivitetId", _object);
                AktivitetList.Add(temp);
            }
            return AktivitetList;
        }

        //___________________________________________________________________________________________________________Alt med Aktivitet ↑

        //___________________________________________________________________________________________________________Alt med Hold ↓

        public static List<Hold> getHold(int? _eventId = null, int? _holdOrder = null, int? _eventAktivitetId = null)
        {
            List<object> dbList = DBHandler.getHold(_eventId, _holdOrder, _eventAktivitetId);
            List<Hold> holdList = new List<Hold>();

            foreach (object _object in dbList)
            {
                Hold tempHold = new Hold();
                tempHold.HoldAktiviteter = new List<EventAktivitetHold>();
                tempHold.Id = adapt<int>("Id", _object);
                tempHold.Navn = adapt<string>("Navn", _object);
                tempHold.Farve = adapt<string>("Farve", _object);
                
                holdList.Add(tempHold);
            }
            return holdList;
        }
        public static List<Hold> getHoldAktivitet(List<Hold> _holdList, int? _eventId = null, int? _holdOrder = null, int? _eventAktivitetId = null)
        {
            List<object> tempAktivitetHoldList = DBHandler.getHoldAktivitet(_eventId, _holdOrder, _eventAktivitetId);
            List<EventAktivitetHold> eventAktivitetHoldList = new List<EventAktivitetHold>();
            foreach (object _aktivitetHold in tempAktivitetHoldList)
            {
                EventAktivitetHold eah = new EventAktivitetHold();
                eah.HoldScores = new List<EventAktivitetHoldScore>();
                eah.Id = adapt<int>("Id", _aktivitetHold);
                eah.EventAktivitetId = adapt<int>("EventAktivitetId", _aktivitetHold);
                eah.HoldId = adapt<int>("HoldId", _aktivitetHold);
                eah.Point = adapt<int?>("Point", _aktivitetHold);
                eah.HoldOrder = adapt<int>("HoldOrder", _aktivitetHold);
                
                eventAktivitetHoldList.Add(eah);
            }
            foreach (Hold _hold in _holdList)
            {
                _hold.HoldAktiviteter = new List<EventAktivitetHold>();
                _hold.HoldAktiviteter.AddRange(eventAktivitetHoldList.Where(i => i.HoldId == _hold.Id).ToList());
                if (_hold.HoldAktiviteter != null)
                {
                    eventAktivitetHoldList.RemoveAll(i => i.HoldId == _hold.Id);
                }
            }
            return _holdList;

        }

        public static List<Hold> getHoldAktivitetScores(List<Hold> _holdList, int? _eventId = null, int? _holdOrder = null, int? _eventAktivitetId = null)
        {
            List<object> scores = DBHandler.getHoldAktivitetScores(_eventId, _holdOrder, _eventAktivitetId);
            List<EventAktivitetHoldScore> scoreList = new List<EventAktivitetHoldScore>();
            foreach (object _score in scores)
            {
                EventAktivitetHoldScore tempHoldScores = new EventAktivitetHoldScore();
                tempHoldScores.Id = adapt<int>("Id", _score);
                tempHoldScores.EventAktivitetHoldId = adapt<int>("EventAktivitetHoldId", _score);
                tempHoldScores.HoldScore = adapt<int>("HoldScore", _score);
                scoreList.Add(tempHoldScores);
            }

            foreach (Hold _hold in _holdList)
            {
                foreach (EventAktivitetHold _aktivitetHold in _hold.HoldAktiviteter)
                {
                    _aktivitetHold.HoldScores = new List<EventAktivitetHoldScore>();
                    _aktivitetHold.HoldScores.AddRange(scoreList.Where(i => i.EventAktivitetHoldId == _aktivitetHold.Id).ToList());
                    if (_aktivitetHold.HoldScores != null)
                    {
                        scoreList.RemoveAll(i => i.EventAktivitetHoldId == _hold.Id);
                    }
                }
            }
            return _holdList;
            
        }

            public static void addHoldScore(int _eventId, int _eventAktivitetId, int _holdOrder, int _holdId, int _score)
        {
            DBHandler.addHoldScore(_eventId, _eventAktivitetId, _holdOrder, _holdId, _score);
        }

        //___________________________________________________________________________________________________________Alt med Hold ↑

        //___________________________________________________________________________________________________________Alt med Hold Order ↓

        public static void deleteHoldScore(int _id)
        {
            DBHandler.deleteHoldScore(_id);
        }

        public static List<Deltager> getDeltagere(int _eventId, int? _aktivitetId = null, int? _holdId = null, int? _deltagerId = null)
        {
            List<object> DbList = DBHandler.getDeltagere(_eventId, _aktivitetId, _holdId, _deltagerId);
            List<Deltager> retur = new List<Deltager>();

            foreach (object _object in DbList)
            {
                Deltager tempdeltager = new Deltager();
                tempdeltager.ScoreList = new List<DeltagerScore>();
                tempdeltager.Id = adapt<int>("Id", _object);
                tempdeltager.Navn = adapt<string>("Navn", _object);
                tempdeltager.HoldId = adapt<int?>("HoldId", _object);
                tempdeltager.EventId = adapt<int>("EventId", _object);
                if (_aktivitetId != null)
                {
                    List<object> o = adapt<List<object>>("ScoreList", _object);
                    foreach (object _score in o)
                    {
                        DeltagerScore ds = new DeltagerScore();
                        ds.Id = adapt<int>("Id", _score);
                        ds.Score = adapt<int>("Score", _score);
                        tempdeltager.ScoreList.Add(ds);
                    }
                }
                retur.Add(tempdeltager);
            }

            return retur;
        }

        public static void addDeltager(string _navn, int _eventId)
        {
            DBHandler.addDeltager(_navn, _eventId);
        }


        public static void addDeltagerScore(int _eventId, int _aktivitetId, int _holdId, int _deltagerId, int _score)
        {
            DBHandler.addDeltagerScore(_eventId, _aktivitetId, _holdId, _deltagerId, _score);
        }

        public static void deleteDeltagerScore(int _id)
        {
            DBHandler.deleteDeltagerScore(_id);
        }

        //___________________________________________________________________________________________________________Alt med Deltager ↑

        //___________________________________________________________________________________________________________Alt med Hold Order ↓

        public static List<int> getHoldOrder(int _eventId, int? _aktivitetId = null)
        {
           
            return DBHandler.getHoldOrder(_eventId, _aktivitetId);
        }

        //___________________________________________________________________________________________________________Alt med Hold Order ↑





        //_____________________________________________________________________________________________________________ikke noget under her!!!!

        //trækker en "property" ud af det "object". <T> = f.eks "string" eller int;
        private static T adapt<T>(string _property, object _object)
        {
            Type type = _object.GetType();
            return (T)type.GetProperty(_property).GetValue(_object);
        }
    }
}
