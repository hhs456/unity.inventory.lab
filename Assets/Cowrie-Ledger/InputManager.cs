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

        public void AddTransaction() {
            // Ū����J�ؤ�����
            decimal amount = decimal.Parse(amountField.text);
            string category = categoryField.text;
            DateTime date = DateTime.Parse(dateField.text);
            string comment = commentField.text;

            // �Ыطs��Transaction����
            Transaction transaction = new Transaction(date, category, amount, comment);

            // �NTransaction����K�[��Ledger��
            ledger.AddTransaction(transaction);
            UpdateTransactionList();
            // �M�ſ�J��
            amountField.text = "";
            categoryField.text = "";
            dateField.text = "";
            commentField.text = "";
        }
    }
}