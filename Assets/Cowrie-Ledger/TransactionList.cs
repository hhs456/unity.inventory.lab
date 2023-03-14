using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKid {


    public class TransactionList : MonoBehaviour {
        public Ledger ledger;
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
    }
}