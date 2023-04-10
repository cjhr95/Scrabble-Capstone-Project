using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets
{
    public class GameStore : MonoBehaviour
    {
        public const int MAX_STORE_SIZE = 7;     // Arbitrary value; can change with playtesting
        public List<StoreItem> storeStock = new List<StoreItem>(MAX_STORE_SIZE);
        private System.Random rnd = new System.Random();

        // Description: Adds random StoreItem into the GameStore.
        //              Items are added based on internal store probabilities.
        public void AddStoreItem()
        {
            var LetterItem = new Letter();
            var LetterSwapItem = new LetterSwapper();
            var MultItem = new Multiplier();
            var TimeItem = new TimeIncrease();

            double probability = rnd.NextDouble();
            if (probability > LetterItem.storeWeight)    // Generated value is greater than 0.5
                storeStock.Add(LetterItem);
            else if ((probability <= LetterItem.storeWeight) && (probability > LetterSwapItem.storeWeight))   // Generated value is 0.5 >= x > 0.25
                storeStock.Add(LetterSwapItem);
            else if ((probability <= LetterSwapItem.storeWeight) && (probability > TimeItem.storeWeight)) // Generated value is 0.25 >= x > 0.1
                storeStock.Add(TimeItem);
            else if (probability <= MultItem.storeWeight)      // Generated value is less than or equal to 0.1
                storeStock.Add(MultItem);
        }

        public void InitStore()
        {
            for (int i = 0; i < MAX_STORE_SIZE; i++)
                AddStoreItem();
        }

        // Description: Removes the StoreItem from the list and calls
        //              the activate() function for the desired item
        public void purchase(Player player, int index)
        {
            StoreItem desiredItem = storeStock[index];
            storeStock.RemoveAt(index);
            AddStoreItem();
        }
    }
}