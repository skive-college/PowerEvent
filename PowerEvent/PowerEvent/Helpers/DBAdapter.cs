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

        public static bool checkLogin(string _username, string _password)
        {
            return true;
        }



        //trækker en "property" ud af det "object". <T> = f.eks "string" eller int;
        private static T adapt<T>(string _property, object _object)
        {
            Type type = _object.GetType();
            return (T)type.GetProperty(_property).GetValue(_object);
        }




    }
}
