using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBase : Channel 
{
     
    
}

[Serializable]
public class TimePoint : IComparable
{

    public int Year;
    public int Month;
    public int Day;
    public int Minute;
    public int Second;
    

    int IComparable.CompareTo(object obj) {
        if (obj.GetType() == typeof(TimePoint)) {
            TimePoint timePoint = ((TimePoint)obj);
            if (this.Year.CompareTo(timePoint.Year) != 0) {
                return this.Year.CompareTo(timePoint.Year);
            }
            if (this.Month.CompareTo(timePoint.Month) != 0) {
                return this.Month.CompareTo(timePoint.Month);
            }
            if (this.Day.CompareTo(timePoint.Day) != 0) {
                return this.Day.CompareTo(timePoint.Day);
            }
            if (this.Minute.CompareTo(timePoint.Minute) != 0) {
                return this.Minute.CompareTo(timePoint.Minute);
            }
            if (this.Second.CompareTo(timePoint.Second) != 0) {
                return this.Second.CompareTo(timePoint.Second);
            }
            return 0;
        }
        else {
            throw new Exception("´íÎóµÄ¶Ô±È");
        }
    }

}