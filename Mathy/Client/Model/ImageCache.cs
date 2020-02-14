using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathy.Client.Model
{
    public class ImageCache
    {
        class CacheItem
        {
            public byte[] Data { get; set; }

            public DateTime CreateTime { get; set; }
        }


        private static object locker = new object();

        private static Dictionary<string, CacheItem> images = new Dictionary<string, CacheItem>();


        public static void Set(string id, byte[] data)
        {
            RemoveItems();

            lock (locker)
            {
                if (images.ContainsKey(id))
                {
                    images.Remove(id);
                }


                images.Add(id, new CacheItem() { Data = data, CreateTime = DateTime.Now });
            }
        }

        public static byte[] Get(string id)
        {
            RemoveItems();

            lock (locker)
            {
                CacheItem item = null;

                if (images.ContainsKey(id))
                {
                    item = images[id];
                    images.Remove(id);
                }

                return item.Data;
            }
        }


        private static void RemoveItems()
        {
            lock (locker)
            {
                DateTime now = DateTime.Now;

                foreach (string id in images.Keys.ToArray())
                {
                    if ((now - images[id].CreateTime).TotalSeconds > 10)
                    {
                        images.Remove(id);
                    }
                }
            }
        }
    }
}