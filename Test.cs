using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Test
{
    public void Run()
    {
        var a = new Parent();

        a.name = "Parent";
        a.enabled = true;

        var b = Convert(a);
        var b2 = SimpleConvert(a);

        Console.WriteLine(JsonConvert.SerializeObject(b));
        Console.WriteLine(JsonConvert.SerializeObject(b2));

        var timer = DateTime.Now.Ticks;

        //-----------Initializing-----------
        var iterateCount = 1000000;

        Console.WriteLine("Iterate count: " + iterateCount);

        var parents = new Parent[iterateCount];
        var children = new Child[iterateCount];

        for (int i = 0; i < iterateCount; i++)
        {
            parents[i] = new Parent();
            parents[i].name = "name" + new Random().Next();
            parents[i].enabled = true;

            children[i] = new Child();
        }

        Console.WriteLine("Initialize time: " + ElapsedTime(ref timer));

        //-----------Check simple convert-----------
        for (int i = 0; i < iterateCount; i++)
        {
            children[i] = SimpleConvert(parents[i]);
        }

        Console.WriteLine("Simple convert time: " + ElapsedTime(ref timer));

        //-----------Check simple convert-----------
        for (int i = 0; i < iterateCount; i++)
        {
            children[i] = Convert(parents[i]);
        }

        Console.WriteLine("Right convert time: " + ElapsedTime(ref timer));

        Console.ReadKey();
    }


    private long ElapsedTime(ref long timer)
    {
        var result = DateTime.Now.Ticks - timer;
        timer = DateTime.Now.Ticks;
        return result / TimeSpan.TicksPerMillisecond;
    }

    public static Child Convert(Parent p)
    {
        var child = new Child();
        var childType = child.GetType();
        var parentType = p.GetType();
        var parentFields = parentType.GetFields();
        var childFields = childType.GetFields();

        foreach (var baseField in parentFields)
        {
            // childFields[0].SetValue(child, baseField.GetValue(p));
        }

        childFields[1].SetValue(child, parentFields[0].GetValue(p));
        childFields[2].SetValue(child, parentFields[1].GetValue(p));

        return child;
    }

    public static Child SimpleConvert(Parent p)
    {
        var serialize = JsonConvert.SerializeObject(p);
        var child = JsonConvert.DeserializeObject<Child>(serialize);

        return child;
    }
}


public class Parent
{
    public string name;
    public bool enabled;
}

public class Child : Parent
{
    public string desc;
}

public class SubClass
{
    private string subName;
}