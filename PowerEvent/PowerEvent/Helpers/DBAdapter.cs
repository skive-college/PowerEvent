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
                temp.Id = (int)type.GetProperty("Id").GetValue(dbList[i]);
                temp.Navn = (string)type.GetProperty("Navn").GetValue(dbList[i]);
                eventList.Add(temp);
            }
            return eventList;
        }





    }
}
