using System;
using System.Globalization;
using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour
{
    public static Store Instance { get; private set; }

    public void SetValue(string key, string value, string nameSpace = null)
    {
        if (!string.IsNullOrEmpty(nameSpace))
        {
            key = string.Format("{0}.{1}", nameSpace, key);
        }
        PlayerPrefs.SetString(key, value);       
    }

    public string GetValue(string key, string nameSpace = null, string defaultValue = null)
    {
        if (!string.IsNullOrEmpty(nameSpace))
        {
            key = string.Format("{0}.{1}", nameSpace, key);
        }
        return PlayerPrefs.GetString(key, defaultValue);        
    }

    public int GetIntValue(string key, string nameSpace, int defaultValue)
    {
        var val = defaultValue;
        try
        {
            val = Int32.Parse(GetValue(key, nameSpace, defaultValue.ToString()), NumberStyles.Any);
        }
        catch (FormatException e)
        {
            Debug.LogError(e.Message);
        }
        return val;
    }

    public T GetEnumValue<T>(string key, string nameSpace, T defaultValue)
    {
        var val = defaultValue;
        try
        {
            val = (T)Enum.Parse(typeof(T), GetValue(key, nameSpace, defaultValue.ToString()));
        }
        catch (FormatException e)
        {
            Debug.LogError(e.Message);
        }
        return val;
    }

    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        this.Flush();        
    }

    public void Flush()
    {
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        Instance = this;
    }  
}
