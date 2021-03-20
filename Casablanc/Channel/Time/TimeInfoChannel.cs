using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "时间接口", menuName = "固化接口/时间接口")]
public class TimeInfoChannel : InfoChannel
{
    [ChannelMessage]
    public int year = 1956;
    [ChannelMessage, Range(1, 12)]
    public int month = 12;
    [ChannelMessage]
    public int day = 25;
    [ChannelMessage, Range(0, 24)]
    public int hour = 6;
    [ChannelMessage, Range(0, 60)]
    public int minute = 30;
    [ChannelMessage, Range(0, 60)]
    public int second = 20;


    public int Second { get => second;
        set {
            if (value < 60) {
                second = value;
            }
            else {
                second = 0;
                Minute++;
            }
            this.Change = true;
        }
    }
    public int Minute {
        get => minute;
        set {
            if (value < 60) {
                minute = value;
            }
            else {
                minute = 0;
                Hour++;
            }
        }
    }
    public int Hour {
        get => hour;
        set {
            if (value < 24) {
                hour = value;
            }
            else {
                hour = 0;
                Day++;
            }
        }
    }
    public int Day {
        get => day;
        set {
            if (value < ((month == 2) ? ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)) ? 29 : 28 : MonthTable[month])) {
                day = value;
            }
            else {
                day = 0;
                Month++;
            }
        }
    }

    public int Month {
        get => month;
        set {
            if (value < 13) {
                month = value;
            }
            else {
                month = 1;
                Year++;
            }
        }
    }
    public int Year {
        get => year;
        set => year = value;
    }


    private static int[] MonthTable = new int[] { 31, 0, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
}
