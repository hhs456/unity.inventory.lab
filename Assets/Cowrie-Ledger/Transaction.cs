using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKid {   

    public class Transaction {
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }

        public Transaction(DateTime date, string category, decimal amount, string comment) {
            Date = date;
            Category = category;
            Amount = amount;
            Comment = comment;
        }
    }
}