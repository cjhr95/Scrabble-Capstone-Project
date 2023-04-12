using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


namespace Assets
{
    public class StoreItem : MonoBehaviour
    {
        public int cost { get; set; }
        public double storeWeight { get; set; }
        protected System.Random rand_mod = new System.Random();
        protected string[] common_letters = {"A", "E", "I", "O", "N", "R", "T", "L", "S", "U"};
        protected string[] uncommon_letters = {"D", "B", "G", "C", "M", "P", "F", "H", "W", "Y"};
        protected string[] rare_letters = {"V", "K", "J", "X", "Q", "Z"};

        //[SerializeField] protected Token tokenPrefab;

        public void Update()
        {
        }
    }

    public class Multiplier : StoreItem
    {
        /*
        Store item to multiply a score - random for a buff (1.5x) or debuff (0.5x)
        If bought, the multiplier variable is used to affect the word score
        */
        private double multiplier;
        
        // Description: Constructor for the Multiplier StoreItem. Randomly determines
        //              if the object will be a buff or debuff
        public Multiplier()
        {
            storeWeight = 0.1;      // 10% chance to appear in the store
            cost = 20;
            multiplier = 0;
        }
        // Description: Activates the multiplier bonus on player or
        //              opponent based on passed Player object.
        //              Edits Player multiplier status to effect SetScore()
        //              function
        public void activate(Player player)
        {
            if(player.playerType == PlayerType.Human)
            {
                // Buff status; apply 1.5x bonus to next play
                multiplier = 1.5;
                player.multStatus = MultiplierStatus.Buff;
            }
            else
            {
                // Debuff status; apply 0.5x bonus to opponent's next play
                multiplier = 0.5;
                player.multStatus = MultiplierStatus.Debuff;
            }
            return;
        }
    }
    
    public class TimeIncrease : StoreItem
    {
        /*
        Store item to increase turn time
        */
        private int timeAdded;
        
        // Description: Constructor for TimeIncrease StoreItem
        public TimeIncrease()
        {
            timeAdded = rand_mod.Next(1, 5);            // Returns a random integer 1-5
            cost = 5;
            storeWeight = 0.1;                          // 10% chance to appear in the store
        }
        
        // Description: Adds the timeAdded variable to a player's turn time
        public void activate(Player player)
        {
            player.turnTimeS = player.turnTimeS + timeAdded;
        }
    }
}