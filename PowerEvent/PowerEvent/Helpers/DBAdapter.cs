using DatabaseClassLibrary;
using PowerEvent.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public static void addEvent(string _navn)
        {
            DBHandler.addEvent(_navn);
        }

        public static void deleteEvent(int _id)
        {
            DBHandler.deleteEvent(_id);
        }

        public static void deleteAllEvent(int _id)
        {
            DBHandler.deleteAllEvent(_id);
        }

        public static void addEventAktivitet(int _eventId, int _aktivitetId)
        {
            DBHandler.addEventAktivitet(_eventId, _aktivitetId);
        }

        public static void deleteEventAktivitet(int _id, int _eventId)
        {
            DBHandler.deleteEventAktivitet(_id, _eventId);
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

        public static void addHold(string _holdNavn, string _farve)
        {
            DBHandler.addHold(_holdNavn, _farve);
        }

        public static void deleteHold(int _id)
        {
            DBHandler.deleteHold(_id);
        }

        public static void updateHoldScore(int _id, int _score)
        {
            DBHandler.updateHoldScore(_id, _score);
        }
        //___________________________________________________________________________________________________________Alt med Hold ↑

        public static void deleteHoldScore(int _id)
        {
            DBHandler.deleteHoldScore(_id);
        }

        public static List<Deltager> getDeltagere(int _eventId, int? _eventAktivitetId = null, int? _holdId = null, int? _deltagerId = null)
        {
            List<object> DbList = DBHandler.getDeltagere(_eventId, _eventAktivitetId, _holdId, _deltagerId);
            List<Deltager> retur = new List<Deltager>();

            foreach (object _object in DbList)
            {
                Deltager tempdeltager = new Deltager();
                tempdeltager.ScoreList = new List<DeltagerScore>();
                tempdeltager.Id = adapt<int>("Id", _object);
                tempdeltager.Navn = adapt<string>("Navn", _object);
                tempdeltager.HoldId = adapt<int?>("HoldId", _object);
                tempdeltager.EventId = adapt<int>("EventId", _object);

                List<object> o = adapt<List<object>>("ScoreList", _object);
                foreach (object _score in o)
                {
                    DeltagerScore ds = new DeltagerScore();
                    ds.Id = adapt<int>("Id", _score);
                    ds.EventAktivitetId = adapt<int>("EventAktivitetId", _score);
                    ds.Score = adapt<int>("Score", _score);
                    tempdeltager.ScoreList.Add(ds);
                }
                retur.Add(tempdeltager);
            }
            return retur;
        }

        public static void addDeltager(string _navn, int _eventId)
        {
            DBHandler.addDeltager(_navn, _eventId);
        }

        public static void updateDeltager(int _id, int? _holdid)
        {
            DBHandler.updateDeltager(_id, _holdid);
        }


        public static void addDeltagerScore(int _eventId, int _aktivitetId, int _holdId, int _deltagerId, int _score)
        {
            DBHandler.addDeltagerScore(_eventId, _aktivitetId, _holdId, _deltagerId, _score);
        }

        public static void deleteDeltagerScore(int _id)
        {
            DBHandler.deleteDeltagerScore(_id);
        }

        public static void deleteDeltager(int _id)
        {
            DBHandler.deleteDeltager(_id);
        }

        public static void updateDeltagerScore(int _id, int _score)
        {
            DBHandler.updateDeltagerScore( _id,  _score);
        }
        //___________________________________________________________________________________________________________Alt med Deltager ↑

        //___________________________________________________________________________________________________________Alt med Hold Order ↓

        public static List<int> getHoldOrder(int _eventId, int? _aktivitetId = null)
        {
           
            return DBHandler.getHoldOrder(_eventId, _aktivitetId);
        }

        public static void addEventAktivitetHold(int _eventAktivitetId, int _holdId, int _holdOrder)
        {
            DBHandler.addEventAktivitetHold(_eventAktivitetId, _holdId, _holdOrder);
        }

        public static void deleteEventAktivitetHold(int _id)
        {
            DBHandler.deleteEventAktivitetHold(_id);
        }

        //___________________________________________________________________________________________________________Alt med Hold Order ↑


        //___________________________________________________________________________________________________________Alt med Login ↓

        public static List<Login> getLogin(int? _eventId = null)
        {
            List<Login> retur = new List<Login>();
            List<object> dbLogin = DBHandler.getLogin(_eventId);
            foreach (var _object in dbLogin)
            {
                Login tempLogin = new Login();
                tempLogin.Id = adapt<int>("Id", _object);
                tempLogin.Brugernavn = adapt<string>("Brugernavn", _object);
                tempLogin.AdminType = adapt<int>("AdminType", _object);
                tempLogin.EventId = adapt<int?>("EventId", _object);
                tempLogin.HoldId = adapt<int?>("HoldId", _object);

                if (tempLogin.AdminType == 0)
                {
                    string encryptedPassword = adapt<string>("Kodeord", _object);

                    string encryptionkey = GenerateEncryptionKey(tempLogin.Brugernavn);
                    string actualPassword = Decrypt(encryptedPassword, encryptionkey);
                    tempLogin.Kodeord = actualPassword;
                }
                
                retur.Add(tempLogin);
            }
            return retur;
        }

        public static void addLogin(string _brugernavn, string _kodeord, int _adminType, int? _eventId = null, int? _holdId = null)
        {
            string encryptionkey = GenerateEncryptionKey(_brugernavn);
            string encrypedKode = Encrypt(_kodeord, encryptionkey);
            DBHandler.addLogin(_brugernavn, encrypedKode, _adminType, _eventId, _holdId);
        }

        public static void deleteLogin(int _id)
        {
            DBHandler.deleteLogin(_id);
        }

        public static Login verifyLogin(string _brugernavn, string _kodeord)
        {
            string encryptionkey = GenerateEncryptionKey(_brugernavn);
            string encrypedKode = Encrypt(_kodeord, encryptionkey);

            object dbLogin = DBHandler.verifyLogin(_brugernavn, encrypedKode);

            Login retur = new Login();

            retur.Id = adapt<int>("Id", dbLogin);
            retur.Brugernavn = adapt<string>("Brugernavn", dbLogin);
            retur.AdminType = adapt<int>("AdminType", dbLogin);
            retur.EventId = adapt<int?>("EventId", dbLogin);
            retur.HoldId = adapt<int?>("HoldId", dbLogin);


            string encryptedPassword = adapt<string>("Kodeord", dbLogin);
            if (encryptedPassword != null)
            {
                string actualPassword = Decrypt(encryptedPassword, encryptionkey);
                retur.Kodeord = actualPassword;
            }
            return retur;
        }




        public static string GenerateEncryptionKey(string key = "")
        {
            string EncryptionKey = string.Empty;
            if (key == "")
            {
                Random Robj = new Random();
                int Rnumber = Robj.Next();
                key = Convert.ToString(Rnumber);
            }
            
            EncryptionKey = "XYZ" + key;

            return EncryptionKey;
        }

        public static string Encrypt(string clearText, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText, string EncryptionKey)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        //___________________________________________________________________________________________________________Alt med Login ↑



        //_____________________________________________________________________________________________________________ikke noget under her!!!!

        //trækker en "property" ud af det "object". <T> = f.eks "string" eller int;
        private static T adapt<T>(string _property, object _object)
        {
            Type type = _object.GetType();
            return (T)type.GetProperty(_property).GetValue(_object);
        }
    }
}
