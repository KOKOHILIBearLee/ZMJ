using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MyEventSystem
{
    static Dictionary<string, Action<object>> eventDic = new Dictionary<string, Action<object>>();

    //
    public static void DispatchEvent(string eventName,object obj=null)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName](obj);
        }
    }

    public static void AddListenter(string eventName,Action<object> action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName] += action;
        }
        else
        {
            eventDic[eventName] = action;
        }
    }

    public static void RemoveListener(string eventName, Action<object> action)
    {
        if (eventDic.ContainsKey(eventName))
        {
            eventDic[eventName] -= action;
        }
    }
}
