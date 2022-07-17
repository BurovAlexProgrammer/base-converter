# base-converter

**Reasons**

I didn't find any solution for copy data from base class to inherited. So I decided to implement it myself.

**Test 1:**
Implemented simple test which show elapsed time to convert data from base class to inherited.
Simple convert - serialize and deserialize via JSON
Right convert - via FieldInfo
```
public class Parent
{
    public string name;
    public bool enabled;
}

public class Child : Parent
{
    public string desc;
}
```

**Result:**
```
{"desc":null,"name":"Parent","enabled":true}
{"desc":null,"name":"Parent","enabled":true}

Iterate count: 1000000
Initialize time: 876
Simple convert time: 3560
Right convert time: 565
```
