using System;
using System.Globalization;

namespace Memento
{
    public static class MementoSystem
    {
        private static ES3Settings settings = new ES3Settings(ES3.Location.Cache);

        public static bool isCacheInitialize;
        
        public static void CacheFile()
        {
            if (isCacheInitialize)
            {
                return;
            }

            isCacheInitialize = true;
            ES3.CacheFile();
            
            settings = new ES3Settings(ES3.Location.Cache);
        }

        public static string GetSaveFile()
        {
            return ES3.LoadRawString(settings);
        }

        public static void StoreCacheFile()
        {
            ES3.StoreCachedFile(settings);
        }
        
        public static void Save<T>(string key, T value)
        {
            ES3.Save(key, value, settings);
        }

        public static T Load<T>(string key, T defaultValue=default)
        {
            if (!ES3.KeyExists(key, settings))
            {
                return defaultValue;
            }
            return ES3.Load(key, defaultValue, settings); 
        }

        public static bool KeyExists(string key)
        {
            return ES3.KeyExists(key, settings);
        }

        public static void Delete(string key)
        {
            if (ES3.KeyExists(key))
            {
                ES3.DeleteKey(key, settings);
            }
        }
        
        public static  bool IsHasGuid()
        {
            return ES3.KeyExists("Guid", settings);
        }

        public static string GetGUID()
        {
            var guid = Load("Guid", "");
            if (string.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
                Save("Guid", guid);
            }
            return guid;
        }
        
        public static DateTime LoadSaveTime(string keyword)
        {
            string savedDate = Load<string>(keyword, DateTime.Now.ToString("o", CultureInfo.InvariantCulture));

            // Attempt to parse in the new format (ISO 8601)
            if (DateTime.TryParseExact(savedDate, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime parsedDate))
            {
                return parsedDate;
            }

            // If the new format doesn't work, try parsing in the old format (locale-dependent)
            if (DateTime.TryParse(savedDate, out parsedDate))
            {
                // Save in the new format to use only this format in the future
                Save(keyword, parsedDate.ToString("o", CultureInfo.InvariantCulture));
                StoreCacheFile();
                return parsedDate;
            }

            // If neither format works, return the current date as a fallback
            return DateTime.Now;
        }

        public static void SaveDateTime(string keyword, DateTime dateTime)
        {
            string saveDate = dateTime.ToString("o", CultureInfo.InvariantCulture); // "o" is the ISO 8601 format
            Save(keyword, saveDate);
        }
    }
}