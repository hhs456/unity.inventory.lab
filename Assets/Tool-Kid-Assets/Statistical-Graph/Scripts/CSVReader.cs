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

            date = date.Trim('"');
            name = name.Trim('"');
            isHoliday = isHoliday.Trim('"');
            holidayCategory = holidayCategory.Trim('"');
            description = description.Trim('"');

            string[] time = date.Split('/'); 
            this.date = new Date(new DateTime(int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2])));
            name = GetHolidayName(this.date);           

            this.isHoliday = isHoliday == "�O" ? true : false;
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
            { 101 ,"����/�}�������" },
            { 106 ,"�~��`" },
            { 111 ,"�q�k�`" },
            { 115 ,"�Įv�`" },
            { 119 ,"�����`" },
            { 123 ,"�ۥѤ�" },
            { 204 ,"�A���`" },
            { 214 ,"���H�`" },
            { 215 ,"���@�`" },
            { 219 ,"�����`" },
            { 228 ,"�M��������" },
            { 301 ,"�L�и`" },
            { 305 ,"���l�x�`" },
            { 308 ,"���k�`" },
            { 312 ,"�Ӿ�`/����Ҥ�" },
            { 314 ,"�ϫI����" },
            { 317 ,"����`" },
            { 320 ,"�l�F�`" },
            { 321 ,"��H�`" },
            { 325 ,"���N�`" },
            { 326 ,"�s���q���`" },
            { 329 ,"�C�~�`/���P������" },
            { 401 ,"�D�p�`/�M�H�`" },
            { 404 ,"�ൣ�`" },
            { 405 ,"�M���`" },
            { 407 ,"�å͸`/���צۥѤ�" },
            { 428 ,"�u�ܤ�" },
            { 501 ,"�Ұʸ`" },
            { 504 ,"�����`/����v�`" },
            { 505 ,"�R�и`" },
            { 512 ,"�@�v�`" },
            { 603 ,"�T�ϸ`" },
            { 605 ,"���Ҥ�" },
            { 606 ,"�u�{�v�`/���Q�`" },
            { 608 ,"�@�ɮ��v��" },
            { 609 ,"�K���`" },
            { 615 ,"ĵ��`" },
            { 623 ,"���@�A�Ȥ�" },
            { 630 ,"�|�p�v�`" },            
            { 701 ,"�����`/�����`/�|�ȸ`/��F��" },            
            { 711 ,"����`" },
            { 715 ,"���Y������" },
            { 808 ,"���˸`" },            
            { 827 ,"�����p���߽�" },
            { 901 ,"�O�̸`" },
            { 903 ,"�x�H�`/�����꨾�Ш|��" },
            { 908 ,"���z�v���v�`" },
            { 909 ,"�����|�`" },
            { 921 ,"��a���a��" },
            { 928 ,"�Юv�`/�դl�߽�" },
            { 1020 ,"�p�v�`" },
            { 1021 ,"�ع��`" },            
            { 1024 ,"�x�W�p�X���" },
            { 1025 ,"���_�`/�M�䶤���`" },
            { 1027 ,"¾��v���v�`" },
            { 1031 ,"�a���`/�U�t�]" },
            { 1101 ,"�ӤH�`/�U�t�`" },
            { 1111 ,"�u�~�`/�a�F�`/���Ҹ`/����������" },
            { 1112 ,"��v�`/���ش_���`/����߽�" },
            { 1121 ,"���Ÿ`" },
            { 1210 ,"�H�v�`" },
            { 1218 ,"�����`" },
            { 1225 ,"�t�ϸ`/��ˬ�����" },
            { 1227 ,"�ؿv�v�`" },
            { 1228 ,"�q�H�`" }
        };

        private static int specialHolidayCount = 0;
        public static string GetHolidayName(Date date) {

            if (date.month == 5 && date.week == DayOfWeek.Sunday) {
                specialHolidayCount++;
                if (specialHolidayCount == 2) {
                    specialHolidayCount = 0;
                    return "���˸`";
                }
            }
            if (date.month == 8 && date.week == DayOfWeek.Sunday) {
                specialHolidayCount++;
                if (specialHolidayCount == 4) {
                    specialHolidayCount = 0;
                    return "�������`";
                }
            }
            if (date.month == 11 && date.week == DayOfWeek.Thursday) {
                specialHolidayCount++;
                if (specialHolidayCount == 4) {
                    specialHolidayCount = 0;
                    return "�P���`";
                }
            }

            holidayTable.TryGetValue(date.month * 100 + date.day, out string name);
            return name;
        }
    }
}