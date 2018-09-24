using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Utils
{
    public static ArrayPair[] DictToArray(Dictionary<string, int> dict)
    {
        List<ArrayPair> friendshipList = new List<ArrayPair>();
        foreach (KeyValuePair<string, int> f in dict)
        {
            friendshipList.Add(new ArrayPair(f.Key, f.Value));
        }
        return friendshipList.ToArray();
    }

    public static Dictionary<string, int> ArrayToDict(ArrayPair[] friendshipArray)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        foreach (ArrayPair f in friendshipArray)
        {
            dict.Add(f.key, f.value);
        }
        return dict;
    }

    [System.Serializable]
    public class ArrayPair
    {
        public string key;
        public int value;

        public ArrayPair(string friend, int level)
        {
            this.key = friend;
            this.value = level;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", key, value);
        }
    }

    [System.Serializable] public class DictionaryStringInt : SerializableDictionary<string, int> { }

    public static string Join(params System.Object[] args)
    {
        List<string> s = new List<string>();
        foreach(System.Object arg in args)
        {
            s.Add(arg.ToString());
        }
        return string.Join(" ", s.ToArray());
    }

    public static void ParseFacing(Direction direction, out float x, out float y)
    {
        ParseFacing(direction.ToString(), out x, out y);
    }

    public static void ParseFacing(string direction, out float x, out float y)
    {
        x = 0;
        y = 0;
        switch (direction.ToLower())
        {
            case "up":
                y = 1;
                break;
            case "down":
                y = -1;
                break;
            case "left":
                x = -1;
                break;
            case "right":
                x = 1;
                break;
            default:
                Debug.LogErrorFormat("Invalid facing direction: {0}", direction);
                return;
        }
        return;
    }
}
