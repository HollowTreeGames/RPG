﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}