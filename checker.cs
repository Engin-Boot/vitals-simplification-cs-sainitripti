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

public class AlertInSMS
{
    public void SendAlert(string message)
    {
        Console.WriteLine("SMS Alert : " + message);
    }
}

public class AlertInSound       // Task
{
    public void SendAlert(string message)
    {
        Console.WriteLine("Sound Alert : " + message);
    }
}

public delegate void Alert(string message);     // Command

public class Thread         // Source
{
    Alert _alert;

    public Thread(Alert alert)
    {
        this._alert = alert;
    }

    public void Start(string message)
    {
        this._alert.Invoke(message);
    }

    public void Stop()
    {

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

    static void vitalIsOk(Thread t, float value, Vital vital)
    {
        if(value < vital.LowerLimit)
        {
            string message = vital.Name + " is low!";
            t.Start(message);
        }
        else if (value > vital.UpperLimit)
        {
            string message = vital.Name + "is high!";
            t.Start(message);
        }
    }
    static void vitalsAreOk(Thread t, float[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            vitalIsOk(t, values[i], vitals[i]);
        }
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

        AlertInSMS smsAlert = new AlertInSMS();
        Alert _smsAlertCommand = new Alert(smsAlert.SendAlert);
        Thread _t1 = new Thread(_smsAlertCommand);

        vitalsAreOk(_t1, new float[]{ 100, 95, 60});
        vitalsAreOk(_t1, new float[]{ 40, 91, 92 });

        AlertInSound soundAlert = new AlertInSound();
        Alert _soundAlertCommand = new Alert(soundAlert.SendAlert);
        Thread _t2 = new Thread(_soundAlertCommand);
        vitalIsOk(_t2, 110, vitals[(int)VitalList.BPM]);
        vitalIsOk(_t2, 80, vitals[(int)VitalList.SPO2]);
        vitalIsOk(_t2, 100, vitals[(int)VitalList.RESPRATE]);


        Console.WriteLine("All ok");
        return 0;
    }
}