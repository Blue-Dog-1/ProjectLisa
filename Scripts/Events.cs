using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.Net;
using System.IO;

namespace UnityEngine
{
    static public class Events
    {
        public delegate void EventHandler();
        static public event EventHandler Finish;
        static public event EventHandler BrakeSpawn;
        static public EventHandler ShowAds;
        static public EventHandler Restart;

        static public EventHandler Move;

        static public float seconds { get; set; }
        static public Image Rays { get; set; }

        static public GameObject Player { get; set; }

        static public int QuantityObjects { get; set; }
        static public void Cliner()
        {
            Finish = delegate () { };
            BrakeSpawn = delegate () { };
            ShowAds = delegate () { };
            Move = () => { };
            Restart = () => { };
        }
        static public void onFinish() =>
            Finish?.Invoke();

        static public void onBrakeSpawn() =>
            BrakeSpawn?.Invoke();
        
#if UNITY_EDITOR
        [MenuItem("Tools/Costom Tools/ResetSave")]
#endif
        public static void ResetSave()
        {
            PlayerPrefs.DeleteAll();
        }

        static public string GetHtmlFromUri(string resource)
        {
            string html = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                    if (isSuccess)
                    {
                        using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                        {
                            //We are limiting the array to 80 so we don't have
                            //to parse the entire html document feel free to 
                            //adjust (probably stay under 300)
                            char[] cs = new char[80];
                            reader.Read(cs, 0, cs.Length);
                            foreach (char ch in cs)
                            {
                                html += ch;
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            return html;
        }
    }


    
}
