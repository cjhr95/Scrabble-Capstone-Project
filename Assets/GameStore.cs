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

        // Description: Adds random StoreItems into the GameStore.
        //              Items are added based on internal store probabilities.
        public void Initialize()
        {
            Random rnd = new Random();
            private float probability;
            for(int i = 0; i < MAX_STORE_SIZE; i++)
            {
                probability = rnd.NextDouble()
                if(probability > Letter.storeWeight)    // Generated value is greater than 0.5
                    storeStock.Add(new Letter());
                else if((probability <= Letter.storeWeight) && (probability > LetterSwapper.storeWeight))   // Generated value is 0.5 >= x > 0.25
                    storeStock.Add(new LetterSwapper());
                else if((probability <= LetterSwapper.storeWeight) && (probability > TimeIncrease.storeWeight)) // Generated value is 0.25 >= x > 0.1
                {
                    //
                    probability = rnd.NextDouble()
                    if(probability >= 0.5)
                        storeStock.Add(new WordStealer());
                    else
                        storeStock.Add(new TimeIncrease());
                }
                else if(probability <= Multiplier.storeWeight)      // Generated value is less than or equal to 0.1
                    storeStock.Add(new Multiplier());
            }
        }

        // Description: Removes the StoreItem from the list and calls
        //              the activate() function for the desired item
        public void purchase(Player player, int index)
        {
            StoreItem desiredItem = storeStock[index];
            storeStock.RemoveAt(index);
        }
    }
}