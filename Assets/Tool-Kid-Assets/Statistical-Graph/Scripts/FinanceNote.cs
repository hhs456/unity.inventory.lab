using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Text;

public class DateLoader {
    public DateLoader() {
        var url = "http://data.ntpc.gov.tw/api/v1/rest/datastore/382000000A-000077-002";
        var request = WebRequest.Create(url);
        // 透過 Chrome 開發者工具可以取得 Method, ContentType
        request.Method = "GET";
        request.ContentType = "application/json;charset=UTF-8";
        var response = request.GetResponse() as HttpWebResponse;
        var responseStream = response.GetResponseStream();
        var reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
        var srcString = reader.ReadToEnd();
    }
}

public class JsonDate {

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
    [InspectorName("新台幣 NTD")] NewTaiwanDollar
}

public struct Holiday {
    [SerializeField, Label("日期")] private Date date;
    [SerializeField, Label("名稱")] private string name;
    [SerializeField, Label("是否放假")] private bool isHoliday;
    [SerializeField, Label("假日別")] private string holidayCategory;
    [SerializeField, Label("描述")] private string description;

    public Date Date { get => date; set => date = value; }
    public string Name { get => name; set => name = value; }
    public bool IsHoliday { get => isHoliday; set => isHoliday = value; }
    public string HolidayCategory { get => holidayCategory; set => holidayCategory = value; }
    public string Description { get => description; set => description = value; }
}