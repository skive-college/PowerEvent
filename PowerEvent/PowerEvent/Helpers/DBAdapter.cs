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

            for (int i = 0; i < dbList.Count; i++)
            {
                Type type = dbList[i].GetType();
                Event temp = new Event();
                temp.Id = adapt<int>("Id", type, dbList[i]);
                temp.Navn = adapt<string>("Navn", type, dbList[i]);
                eventList.Add(temp);
            }
            return eventList;
        }


        //trækker en "property" ud af det "object". <T> = f.eks "string" eller int. kræver "Type type = object.GetType()" som _type;
        private static T adapt<T>(string _property, Type _type, object _object)
        {
            return (T)_type.GetProperty(_property).GetValue(_object);
        }




    }
}
