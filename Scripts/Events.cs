using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

namespace UnityEngine
{
    static public class Events
    {
        public delegate void EventHandler();
        static public event EventHandler Finish;
        static public event EventHandler BrakeSpawn;

        static public EventHandler Move;

        static public float seconds { get; set; }
        static public Image Rays { get; set; }

        static public void Cliner()
        {
            Finish = delegate () { };
            BrakeSpawn = delegate () { };
            Move = () => { };
        }
        static public void onFinish() =>
            Finish?.Invoke();

        static public void onBrakeSpawn() =>
            BrakeSpawn?.Invoke();
        
#if UNITY_EDITOR
        [MenuItem("Tools/Costom Tools/ResetSave")]
        public static void ResetSave()
        {
            PlayerPrefs.SetInt("Level", 0);
        }
#endif
    }
}
