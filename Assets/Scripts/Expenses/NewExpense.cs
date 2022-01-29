using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Expenses
{
    public struct NewExpense
    {
        public readonly Expense Expense;
        
        // Reason for the new Expense.
        public readonly string Reason;

        public NewExpense(int cost, string title, string reason)
        {
            Expense = new Expense(title, cost);
            Reason = reason;
        }
    }
}