using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Expenses
{
    public struct Expense
    {
        // Tile appears in daily breakdown
        public readonly string Title;
        public readonly int Cost;

        public Expense(string title, int cost)
        {
            Title = title;
            Cost = cost;
        }
    }
}