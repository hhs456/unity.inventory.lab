using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace ToolKid {

    public class InputManager : MonoBehaviour {
        public Ledger ledger;
        public InputField amountField;
        public InputField categoryField;
        public InputField dateField;
        public InputField commentField;
        public Text transactionListText;

        private void Start() {
            // 顯示所有交易的列表
            UpdateTransactionList();
        }

        private void UpdateTransactionList() {
            // 將交易列表轉換成字符串
            string transactionListString = "";
            foreach (Transaction transaction in ledger.transactions) {
                transactionListString += transaction.Date.ToString("yyyy/MM/dd") + " " + transaction.Category + " " + transaction.Amount + " " + transaction.Comment + "\n";
            }

            // 顯示交易列表
            transactionListText.text = transactionListString;
        }

        public void AddTransaction() {
            // 讀取輸入框中的值
            decimal amount = decimal.Parse(amountField.text);
            string category = categoryField.text;
            DateTime date = DateTime.Parse(dateField.text);
            string comment = commentField.text;

            // 創建新的Transaction物件
            Transaction transaction = new Transaction(date, category, amount, comment);

            // 將Transaction物件添加到Ledger中
            ledger.AddTransaction(transaction);
            UpdateTransactionList();
            // 清空輸入框
            amountField.text = "";
            categoryField.text = "";
            dateField.text = "";
            commentField.text = "";
        }
    }
}