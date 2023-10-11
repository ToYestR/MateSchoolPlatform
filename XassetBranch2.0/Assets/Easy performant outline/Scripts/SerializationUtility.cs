using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPOOutline
{
    public static class SerializationUtility
    {
        [System.Serializable]
        private class SerializedData
        {
            public string Type;
            public string Data;

            public SerializedData(object obj)
            {
                Type = obj.GetType().FullName;
                Data = JsonUtility.ToJson(obj);
            }
        }

        public static List<string> Serialize<T>(List<T> objects)
        {
            var result = new List<string>();
            foreach (var obj in objects)
                result.Add(Serialize(obj));

            return result;
        }

        public static string Serialize<T>(T item)
        {
            return JsonUtility.ToJson(new SerializedData(item));
        }
        
        public static T Deserialize<T>(string serialized)
        {
            try
            {
                var deserialized = JsonUtility.FromJson<SerializedData>(serialized);
                var item = (T)Activator.CreateInstance(Type.GetType(deserialized.Type));
                JsonUtility.FromJsonOverwrite(deserialized.Data, item);

                return item;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return default(T);
            }
        }

        public static List<T> Deserialize<T>(List<string> serialized)
        {
            var result = new List<T>();
            foreach (var data in serialized)
            {
                var deserialized = Deserialize<T>(data);
                if (deserialized != null)
                    result.Add(deserialized);
            }

            return result;
        }
    }
}