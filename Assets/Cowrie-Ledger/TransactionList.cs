using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ToolKid {


    public class TransactionList : MonoBehaviour {
        public Ledger ledger;
        public Text transactionListText;

        private void Start() {
            // ��ܩҦ�������C��
            UpdateTransactionList();
        }

        private void UpdateTransactionList() {
            // �N����C���ഫ���r�Ŧ�
            string transactionListString = "";
            foreach (Transaction transaction in ledger.transactions) {
                transactionListString += transaction.Date.ToString("yyyy/MM/dd") + " " + transaction.Category + " " + transaction.Amount + " " + transaction.Comment + "\n";
            }

            // ��ܥ���C��
            transactionListText.text = transactionListString;
        }
    }
}