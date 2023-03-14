using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKid {
    public class Ledger : MonoBehaviour {
        public List<Transaction> transactions = new List<Transaction>();
        public Text transactionListText;

        public void AddTransaction(Transaction transaction) {
            transactions.Add(transaction);
        }
        private void UpdateTransactionList() {
            transactionListText.text = "";

            foreach (Transaction transaction in transactions) {
                transactionListText.text += transaction.ToString() + "\n";
            }
        }
    }
}