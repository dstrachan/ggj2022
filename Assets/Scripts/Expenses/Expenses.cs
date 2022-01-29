using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Expenses
{
    public class Expenses
    {
        // We use this to ensure that we're in sync with the game state 
        private int _day;

        private List<Expense> _recurringExpenses;

        public Expenses()
        {
            _day = 0;
        }

        // Should be called once at the end of the day.
        // Returns the new expenses for today (you should show these to the player as prompts)
        // and the full list of expenses including new expenses (you should show this to the player as a breakdown).
        public (NewExpenses, List<Expense>) Step()
        {
            // TODO
            // int gameStateDay = GameState.Instance.Day;
            // if (_day != gameStateDay)
            // {
            //     throw new Exception(
            //         $"You must call Step exactly once per day. Expected day {_day} but game is at day {gameStateDay}.");
            // }

            List<NewExpense> newRecurring = new List<NewExpense>();
            {
                // Rent and Utilities
                if (_day == 0)
                    newRecurring.Add(new NewExpense(
                        10,
                        "Rent + Utilities",
                        "Keep your family housed and warm. Despite the rising cost of living, you've found yourself an affordable house in town."));

                // Car Lease
                if (_day == 10)
                    newRecurring.Add(new NewExpense(
                        25,
                        "Car Lease",
                        "Your wife's job now requires a car. The company is like a family. As a family, corporate decided that the employees should do their part and take responsibility for their own transport costs."));

                // Medical: Eczema
                if (_day == 15)
                    newRecurring.Add(new NewExpense(
                        25,
                        "Medical",
                        "Billy develops eczema. You'll now need a steady stream of topical corticosteroids to keep the rashes at bay."));
                
                // Police Bribes
                if (_day == 20)
                    newRecurring.Add(new NewExpense(
                        50,
                        "Bribes",
                        "The police know you're up to no good. You tried to explain that impoverished conditions lead you down a spiraling descent. They decided to turn the other cheek... at a cost."));
            }

            List<NewExpense> oneOff = new List<NewExpense>();
            {
                // Poop Explosion
                if (Chance(0.3f))
                    oneOff.Add(new NewExpense(
                        4,
                        "Poop Explosion",
                        "Baby Billy pooped. Replace the rug."));

                // Water Damage
                if (_day > 20 && Chance(0.05f))
                    oneOff.Add(new NewExpense(
                        Random.Range(300, 1000),
                        "Water Damage",
                        "A pipe bursts in your ceiling. Your landlord refers to you to page 325 of your Tenant's agreement: \"The tenant is responsible to keep the apartment in working condition.\""));
            }

            // Record recurring expenses
            // TODO we probably want to group by expense title and sum up costs. And sort by title?
            _recurringExpenses.AddRange(newRecurring.Select(expense => expense.Expense));

            var allExpenses = _recurringExpenses.Concat(oneOff.Select(expense => expense.Expense)).ToList();
            _day++;
            return (new NewExpenses(oneOff, new List<NewExpense>(newRecurring)), allExpenses);
        }

        private static bool Chance(float probabilityOfTrue)
        {
            return Random.Range(0.0f, 1.0f) > probabilityOfTrue;
        }
    }
}