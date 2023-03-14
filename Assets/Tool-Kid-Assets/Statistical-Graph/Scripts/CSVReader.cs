using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using ToolKid.SystemExtension;

namespace ToolKid.FileTK {

    public class CSVReader : MonoBehaviour {

        public string URI = "https://data.ntpc.gov.tw/api/datasets/308dcd75-6434-45bc-a95f-584da4fed251/csv/file";
        public string csvText;
        public string[] data;
        public Holiday[] holidays;

        async void OnValidate() {
            await Load();
        }

        async UniTask Load() {
               using (UnityWebRequest request = UnityWebRequest.Get(URI)) {
                await request.SendWebRequest();
                csvText = request.downloadHandler.text;
                data = csvText.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
                holidays = new Holiday[(int)(data.Length / 5f - 1f)];
                for (int i = 5; (i + 4) < data.Length; i += 5) {
                    holidays[i / 5 - 1] = new Holiday(data[i], data[i + 1], data[i + 2], data[i + 3], data[i + 4]);
                }
            }
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

    public enum HolidayCategory {
        [InspectorName("補假")] Compensatory = 2,
        /*[InspectorName("星期日")] Sunday,*/
        [InspectorName("特定節日")] RoutineEvent = 4,
        [InspectorName("調整放假日")] FlexibleLeave = 5,
        [InspectorName("補行上班日")] FlexibleWork = 8,
        [InspectorName("紀念日及節日")] Festival = 6,
        [InspectorName("星期六、星期日")] Weekend = 7,
        [InspectorName("放假之紀念日及節日")] Holliday = 9,
        [InspectorName("其他")] Other = -1
    }

    [Serializable]
    public struct Holiday {
        [SerializeField, Label("名稱")] private string name;
        [SerializeField] private Date date;
        [SerializeField, Label("是否放假")] private bool isHoliday;
        [SerializeField, Label("假日別")] private HolidayCategory holidayCategory;
        [SerializeField, Label("描述")] private string description;

        public Date Date { get => date; set => date = value; }
        public string Name { get => name; set => name = value; }
        public bool IsHoliday { get => isHoliday; set => isHoliday = value; }
        public HolidayCategory HolidayCategory { get => holidayCategory; set => holidayCategory = value; }
        public string Description { get => description; set => description = value; }       

        public Holiday(string date, string name, string isHoliday, string holidayCategory, string description) {

            date = date.Trim('"');
            name = name.Trim('"');
            isHoliday = isHoliday.Trim('"');
            holidayCategory = holidayCategory.Trim('"');
            description = description.Trim('"');

            string[] time = date.Split('/'); 
            this.date = new Date(new DateTime(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2])));
            name = GetHolidayName(this.date);           

            this.isHoliday = isHoliday == "是" ? true : false;
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
            }            

            this.name = name == null ? this.holidayCategory.GetInspectorName() : name;
            this.description = description;
        }

        public static Dictionary<int, string> holidayTable = new Dictionary<int, string>() {
            { 101 ,"元旦/開國紀念日" },
            { 106 ,"獸醫節" },
            { 111 ,"司法節" },
            { 115 ,"藥師節" },
            { 119 ,"消防節" },
            { 123 ,"自由日" },
            { 204 ,"農民節" },
            { 214 ,"情人節" },
            { 215 ,"戲劇節" },
            { 219 ,"炬光節" },
            { 228 ,"和平紀念日" },
            { 301 ,"兵役節" },
            { 305 ,"童子軍節" },
            { 308 ,"婦女節" },
            { 312 ,"植樹節/國父忌日" },
            { 314 ,"反侵略日" },
            { 317 ,"國醫節" },
            { 320 ,"郵政節" },
            { 321 ,"氣象節" },
            { 325 ,"美術節" },
            { 326 ,"廣播電視節" },
            { 329 ,"青年節/先烈紀念日" },
            { 401 ,"主計節/愚人節" },
            { 404 ,"兒童節" },
            { 405 ,"清明節" },
            { 407 ,"衛生節/言論自由日" },
            { 428 ,"工殤日" },
            { 501 ,"勞動節" },
            { 504 ,"文藝節/牙醫師節" },
            { 505 ,"舞蹈節" },
            { 512 ,"護師節" },
            { 603 ,"禁煙節" },
            { 605 ,"環境日" },
            { 606 ,"工程師節/水利節" },
            { 608 ,"世界海洋日" },
            { 609 ,"鐵路節" },
            { 615 ,"警察節" },
            { 623 ,"公共服務日" },
            { 630 ,"會計師節" },            
            { 701 ,"公路節/漁民節/稅務節/戶政日" },            
            { 711 ,"航海節" },
            { 715 ,"解嚴紀念日" },
            { 808 ,"父親節" },            
            { 827 ,"延平郡王冥誕" },
            { 901 ,"記者節" },
            { 903 ,"軍人節/全民國防教育日" },
            { 908 ,"物理治療師節" },
            { 909 ,"國民體育節" },
            { 921 ,"國家防災日" },
            { 928 ,"教師節/孔子冥誕" },
            { 1020 ,"廚師節" },
            { 1021 ,"華僑節" },            
            { 1024 ,"台灣聯合國日" },
            { 1025 ,"光復節/清潔隊員節" },
            { 1027 ,"職能治療師節" },
            { 1031 ,"榮民節/萬聖夜" },
            { 1101 ,"商人節/萬聖節" },
            { 1111 ,"工業節/地政節/光棍節/全民健走日" },
            { 1112 ,"醫師節/中華復興節/國父冥誕" },
            { 1121 ,"防空節" },
            { 1210 ,"人權節" },
            { 1218 ,"移民節" },
            { 1225 ,"聖誕節/行憲紀念日" },
            { 1227 ,"建築師節" },
            { 1228 ,"電信節" }
        };

        private static int specialHolidayCount = 0;
        public static string GetHolidayName(Date date) {

            if (date.month == 5 && date.week == DayOfWeek.Sunday) {
                specialHolidayCount++;
                if (specialHolidayCount == 2) {
                    specialHolidayCount = 0;
                    return "母親節";
                }
            }
            if (date.month == 8 && date.week == DayOfWeek.Sunday) {
                specialHolidayCount++;
                if (specialHolidayCount == 4) {
                    specialHolidayCount = 0;
                    return "祖父母節";
                }
            }
            if (date.month == 11 && date.week == DayOfWeek.Thursday) {
                specialHolidayCount++;
                if (specialHolidayCount == 4) {
                    specialHolidayCount = 0;
                    return "感恩節";
                }
            }

            holidayTable.TryGetValue(date.month * 100 + date.day, out string name);
            return name;
        }
    }
}