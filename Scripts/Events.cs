using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
static public class Events
{
    public delegate void EventHandler();
    static public event EventHandler Finish;

    static public EventHandler Move;

    static public float seconds { get; set; }
    static public void Cliner()
    {
        Finish = delegate() { };
        Move = () => { };
    }
    static public void onFinish()
    {
        Finish?.Invoke();
    }
    [MenuItem("Tools/Costom Tools/ResetSave")]
    public static void ResetSave()
    {
        PlayerPrefs.SetInt("Level", 0);
    }
    

}
