using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class Test
{
    public void Run()
    {
        Console.WriteLine("Base class: " + JsonConvert.SerializeObject(new Parent()));

        var timer = DateTime.Now.Ticks;

        //-----------Initializing-----------
        var iterateCount = 1000000;

        Console.WriteLine("Iterate count: " + iterateCount);

        var parents = new Parent[iterateCount];
        var children = new Child[iterateCount];
        var indChildren = new IndependentClass[iterateCount];

        for (int i = 0; i < iterateCount; i++)
        {
            parents[i] = new Parent();
            parents[i].name = "name" + new Random().Next();
            parents[i].enabled = true;
            parents[i].publicSubClass1 = new SubClass1();

            children[i] = new Child();
            indChildren[i] = new IndependentClass();
        }

        Console.WriteLine("Initialize time: " + ElapsedTime(ref timer));

        //-----------Check simple convert-----------
        for (int i = 0; i < iterateCount; i++)
        {
            children[i] = SimpleConvert(parents[i]);
        }

        Console.WriteLine("Simple convert time: " + ElapsedTime(ref timer));

        //-----------Check right convert-----------
        for (int i = 0; i < iterateCount; i++)
        {
            children[i] = Convert(parents[i]);
        }

        Console.WriteLine("Right convert time: " + ElapsedTime(ref timer));
        
        //-----------Check right convert for independent class-----------
        for (int i = 0; i < iterateCount; i++)
        {
            indChildren[i] = Convert2(parents[i]);
        }

        Console.WriteLine("Right independent convert time: " + ElapsedTime(ref timer));

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

        var parentTypeInfo = typeof(Parent).GetTypeInfo();
        var parentFields = parentTypeInfo.DeclaredFields;

        foreach (var field in parentFields)
        {
            field.SetValue(child, field.GetValue(p));
        }

        return child;
    }
    
    public static IndependentClass Convert2(Parent p)
    {
        var child = new IndependentClass();
        var childType = child.GetType().GetTypeInfo();

        var parentTypeInfo = typeof(Parent).GetTypeInfo();
        var parentFields = parentTypeInfo.DeclaredFields;

        foreach (var field in parentFields)
        {
            if (childType.DeclaredFields.Contains(field))
                field.SetValue(child, field.GetValue(p));
        }

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
    private int number;
    public bool enabled;

    public SubClass1 publicSubClass1;
    private SubClass2 privateSubClass2;

    public Parent()
    {
        privateSubClass2 = new SubClass2();
    }

    private class SubClass2
    {
        private string subName;

        public SubClass2()
        {
            subName = "Initialized";
        }
    }
}

public class Child : Parent
{
    public string desc;
}

public class SubClass1
{
    private string subName;

    public SubClass1()
    {
        subName = "Initialized";
    }
}

public class IndependentClass
{
    public string name;
}