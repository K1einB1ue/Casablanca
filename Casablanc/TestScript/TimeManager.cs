using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonMono<TimeManager>
{
    static int Year = 1950;
    static int Month = 10;
    static int Day = 20;
    static int Hour = 18;
    static int Minute;
    static int Second;

    static int[] MonthMap = new int[12] { 31, 0, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    static int[] SecondMonth = new int[2] { 28, 29 };


    private void Update() {
        DayUP();
    }
    public static bool Reach(int Year, int Month, int Day, int Hour, int Minute, int Second) {
        return Year >= TimeManager.Year && Month >= TimeManager.Month && Day >= TimeManager.Day && Hour >= TimeManager.Hour && Minute >= TimeManager.Minute && Second >= TimeManager.Second;
    }
    public static void SecondUP() {
        Second++;
        if (Second >= 60) { 
            Second = 0;
            MinuteUP();
        }
    }
    private static void MinuteUP() {
        Minute++;
        if (Minute >= 60) {
            Minute = 0;
            HourUP();
        }
    }
    private static void HourUP() {
        Hour++;
        if (Hour >= 24) {
            Hour = 0;
            DayUP();
        }
    }
    private static void DayUP() {
        Day++;
        if (Day > (Month!= 2?MonthMap[Month-1]:(SecondMonth[Year%4==0?0:1]))) {
            Day = 1;
            MonthUP();
        }
    }

    private static void MonthUP() {
        Month++;
        if (Month > 12) {
            Month = 1;
            YearUP();
        }
    }

    private static void YearUP() {
        Year++;
    }
}
