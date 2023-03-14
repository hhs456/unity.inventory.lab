using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid.FileTK {
    public class FinanceNote {
        public Date date;
        public string[] tags;
        public MoneyType type;
        public float money;
        FinanceNote() {
            date = new Date(DateTime.Today);
        }
    }

    public enum MoneyType {
        [InspectorName("·s¥x¹ô NTD")] NewTaiwanDollar
    }
}