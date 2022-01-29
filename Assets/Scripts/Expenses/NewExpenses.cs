using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Expenses
{
    public struct NewExpenses
    {
        public readonly List<NewExpense> OneOff;
        public readonly List<NewExpense> Recurring;

        public NewExpenses(List<NewExpense> oneOff, List<NewExpense> recurring)
        {
            OneOff = oneOff;
            Recurring = recurring;
        }
    }
}