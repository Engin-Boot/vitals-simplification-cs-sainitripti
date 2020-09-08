using System;
using System.Diagnostics;

enum Vital
{
    BPM,
    SPO2,
    RESPRATE,
    VITALCOUNT
};

struct Limit
{
    int lowerLimit;
    int upperLimit;

    public Limit(int lowerLimit, int upperLimit)
    {
        this.lowerLimit = lowerLimit;
        this.upperLimit = upperLimit;
    }

    public int LowerLimit
    {
        get { return lowerLimit; }
        set { lowerLimit = value; }
    }

    public int UpperLimit
    {
        get { return upperLimit; }
        set { upperLimit = value; }
    }
}

class Checker
{
    static Limit[] vitalLimit;

    static Checker()
    {
        vitalLimit = new Limit[(int)Vital.VITALCOUNT];

        vitalLimit[(int)Vital.BPM] = new Limit(70, 150);
        vitalLimit[(int)Vital.SPO2] = new Limit(90, 100);
        vitalLimit[(int)Vital.RESPRATE] = new Limit(30, 95);
    }

    static bool vitalIsOk(float value, Limit limit)
    {
        if(value < limit.LowerLimit || value > limit.UpperLimit)
        {
            return false;
        }
        return true;
    }
    static bool vitalsAreOk(float[] vitalValue) {
        
        bool allVitalAreOk = true;

        for(int i = 0; i < vitalValue.Length; i++)
        {
            if(!vitalIsOk(vitalValue[i], vitalLimit[i]))
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
        Console.WriteLine("All ok");
        return 0;
    }
}