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

        public static List<Aktivitet> getAktivitet(int? _eventId = null)
        {
            List<object> dbList = DBHandler.getAktivitet();
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

        public static List<Hold> getHold()
        {
            List<object> dbList = DBHandler.getHold();
            List<Hold> holdList = new List<Hold>();

            foreach (object _object in dbList)
            {
                Hold temp = new Hold();
                temp.Id = adapt<int>("Id", _object);
                temp.Navn = adapt<string>("Navn", _object);
                holdList.Add(temp);
            }
            return holdList;
        }

        public static List<Deltager> getDeltagere(int _eventId, int? _aktivtetId = null, int? _holdId = null)
        {
            List<object> DbList = DBHandler.getDeltagere(_eventId, _aktivtetId, _holdId);
            List<Deltager> retur = new List<Deltager>();

            foreach (object _object in DbList)
            {
                Deltager tempdeltager = new Deltager();
                tempdeltager.ScoreList = new List<DeltagerScore>();
                tempdeltager.Id = adapt<int>("Id", _object);
                tempdeltager.Navn = adapt<string>("Navn", _object);
                tempdeltager.HoldId = adapt<int>("HoldId", _object);
                tempdeltager.EventId = adapt<int>("EventId", _object);
                if (_aktivtetId != null)
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






        //trækker en "property" ud af det "object". <T> = f.eks "string" eller int;
        private static T adapt<T>(string _property, object _object)
        {
            Type type = _object.GetType();
            return (T)type.GetProperty(_property).GetValue(_object);
        }
    }
}
