using System;
using System.Diagnostics;

enum VitalList
{
    BPM,
    SPO2,
    RESPRATE,
    VITALCOUNT
};

struct Vital
{
    string name;
    int lowerLimit;
    int upperLimit;

    public Vital(string name, int lowerLimit, int upperLimit)
    {
        this.name = name;
        this.lowerLimit = lowerLimit;
        this.upperLimit = upperLimit;
    }

    public string Name
    {
        get { return name; }
    }

    public int LowerLimit
    {
        get { return lowerLimit; }
    }

    public int UpperLimit
    {
        get { return upperLimit; }
    }
}

class Checker
{
    static Vital[] vitals;

    static Checker()
    {
        vitals = new Vital[(int)VitalList.VITALCOUNT] {
            new Vital("BPM", 70, 150),
            new Vital("SPO2", 90, 100),
            new Vital("RESPRATE", 30, 95) };
    }

    static bool vitalIsOk(float value, Vital vital)
    {
        if(value < vital.LowerLimit)
        {
            Console.WriteLine(vital.Name + " is low!");
            return false;
        }
        else if (value > vital.UpperLimit)
        {
            Console.WriteLine(vital.Name + "is high!");
        }
        return true;
    }
    static bool vitalsAreOk(float[] values) {
        
        bool allVitalAreOk = true;

        for(int i = 0; i < values.Length; i++)
        {
            if(!vitalIsOk(values[i], vitals[i]))
            {
                allVitalAreOk = false;
            }
        }
        return allVitalAreOk;
    }
    static void ExpectTrue(bool expression) {
        if(!expression) {
            Console.WriteLine("Expected true, but got false");
            Environment.Exit(1);
        }
    }
    static void ExpectFalse(bool expression) {
        if(expression) {
            Console.WriteLine("Expected false, but got true");
            Environment.Exit(1);
        }
    }
    static int Main() {
        ExpectTrue(vitalsAreOk(new float[]{ 100, 95, 60}));
        ExpectFalse(vitalsAreOk(new float[]{ 40, 91, 92 }));

        ExpectTrue(vitalIsOk(110, vitals[(int)VitalList.BPM]));
        ExpectFalse(vitalIsOk(80, vitals[(int)VitalList.SPO2]));
        ExpectFalse(vitalIsOk(100, vitals[(int)VitalList.RESPRATE]));

        Console.WriteLine("All ok");
        return 0;
    }
}