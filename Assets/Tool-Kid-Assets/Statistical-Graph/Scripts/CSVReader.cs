using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Text;
using UnityEngine.Networking;

public class CSVReader : MonoBehaviour {

    public string URI = "https://data.ntpc.gov.tw/api/datasets/308dcd75-6434-45bc-a95f-584da4fed251/csv/file";
    public string csvText;
    public string[] data;
    public Holiday[] holidays;

    void OnValidate() {
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        UnityWebRequest request = UnityWebRequest.Get(URI);                        
        yield return request.SendWebRequest();        
        csvText = request.downloadHandler.text;        
        data = csvText.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
        holidays = new Holiday[(int)(data.Length / 5f - 1f)];
        for (int i = 5; (i + 4) < data.Length; i += 5) {
            holidays[i/5 - 1] = new Holiday(data[i], data[i + 1], data[i + 2], data[i + 3], data[i + 4]);
        }
    }
}

public class FinanceNote {
    public Date date;
    public string[] tags;
    public MoneyType type;
    public float money;
    FinanceNote() {
        date = new Date(DateTime.Today);

    }
}
[Serializable]
public struct Date {
    public int year;
    public int month;
    public int day;
    public DayOfWeek week;
    public int count;

    public Date(DateTime dateTime) {
        year = dateTime.Year;
        month = dateTime.Month;
        day = dateTime.Day;
        week = dateTime.DayOfWeek;
        count = dateTime.DayOfYear;
    }
}

public enum MoneyType {
    [InspectorName("�s�x�� NTD")] NewTaiwanDollar
}

public enum HolidayCategory {
    [InspectorName("�ɰ�")] Compensatory = 2,
    /*[InspectorName("�P����")] Sunday,*/
    [InspectorName("�S�w�`��")] RoutineEvent = 4,
    [InspectorName("�վ�񰲤�")] FlexibleLeave = 5,
    [InspectorName("�ɦ�W�Z��")] FlexibleWork = 8,
    [InspectorName("������θ`��")] Festival = 6,
    [InspectorName("�P�����B�P����")] Weekend = 7,
    [InspectorName("�񰲤�������θ`��")] Holliday = 9,
    [InspectorName("��L")] Other = -1
}

[Serializable]
public struct Holiday {
    [SerializeField, Label("�W��")] private string name;
    [SerializeField] private Date date;    
    [SerializeField, Label("�O�_��")] private bool isHoliday;
    [SerializeField, Label("����O")] private HolidayCategory holidayCategory;
    [SerializeField, Label("�y�z")] private string description;

    public Date Date { get => date; set => date = value; }
    public string Name { get => name; set => name = value; }
    public bool IsHoliday { get => isHoliday; set => isHoliday = value; }
    public HolidayCategory HolidayCategory { get => holidayCategory; set => holidayCategory = value; }
    public string Description { get => description; set => description = value; }

    public Holiday(string date, string name, string isHoliday, string holidayCategory, string description) {
        
        string[] time = date.Split('/');
        this.date = new Date(new DateTime(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2])));
        if (this.date.month == 1 && this.date.day == 1) {
            name = "���إ���}�������";
        }

        this.isHoliday = isHoliday == "�O" ? true : false;
        /*
        this.holidayCategory = HolidayCategory.Other;
        for (int i = 0; i < Enum.GetValues(typeof(HolidayCategory)).Length; i++) {
            if (holidayCategory == ((HolidayCategory)i).GetInspectorName()) {
                this.holidayCategory = (HolidayCategory)i;
            }
        }
        if(this.holidayCategory == HolidayCategory.Other) {
            if(this.date.week == DayOfWeek.Saturday || this.date.week == DayOfWeek.Sunday) {
                this.holidayCategory = HolidayCategory.Weekend;
            }
        }
        */
        int Category = holidayCategory.Length;

        if (Category == 3) {
            this.holidayCategory = HolidayCategory.Weekend;
        }
        else if (Category == 5) {
            if (this.isHoliday) {
                this.holidayCategory = HolidayCategory.FlexibleLeave;
            }
            else {
                this.holidayCategory = HolidayCategory.FlexibleWork;
            }
        }
        else {
            this.holidayCategory = (HolidayCategory)holidayCategory.Length;
            //if(holidayCategory.Length == 9) {
            //    if(this.date.week == DayOfWeek.Saturday || this.date.week == DayOfWeek.Sunday) {
            //        Debug.Log(this.date.year + "." + this.date.month + "." + this.date.day + "�ݸɰ�");
            //    }
            //}            
        }
        Debug.Log(this.holidayCategory.GetInspectorName());
        //if (this.holidayCategory != HolidayCategory.Weekend) {
        //    Debug.Log(this.date.year + "." + this.date.month + "." + this.date.day + this.holidayCategory.GetInspectorName()/* + " " + this.date.week*/);
        //}
       this.name = name == "" ? this.holidayCategory.GetInspectorName() : name;
        this.description = description;
    }
}