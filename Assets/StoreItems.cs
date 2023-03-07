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
        public int cost = 15 { get; set; }      // I don't know what to set for cost values yet
        public float storeWeight { get; set; }
        Random rand_mod = new Random();
        private char[] common_letters = {'A', 'E', 'I', 'O', 'N', 'R', 'T', 'L', 'S', 'U'};
        private char[] uncommon_letters = {'D', 'B', 'G', 'C', 'M', 'P', 'F', 'H', 'W', 'Y'};
        private char[] rare_letters = {'V', 'K', 'J', 'X', 'Q', 'Z'};

        public void Initialize();
        public void Update();

    }

    public class Letter : StoreItem
    {
        /*
        The Letter store item
        */
        // Member variables
        private Token new_letter = Instantiate(tokenPrefab);    // Letter token object for the store
        storeWeight = 0.5;                                      // 50% chance to appear in the store

        // Description: Constructor for the Letter StoreItem. Randomly selects the letter
        //              based on rarity
        public Letter()
        {
            float probability = rand_mod.NextDouble();
            if(probability > 0.4)                                      // 60% chance for common letter (A, E, I, O, N, R, T, L, S, U)
                new_letter.tokenLetter = rand_mod.Next(common_letters.Count)
            else if((probability > 0.1) && (probability <= 0.4))       // 30% chance for uncommon letter (D, B, G, C, M, P, F, H, W, Y)
                new_letter.tokenLetter = rand_mod.Next(uncommon_letters.Count)
            else                                                       // 10% chance for rare letter (V, K, J, X, Q, Z)
                new_letter.tokenLetter = rand_mod.Next(rare_letters.Count)
        }

        // Description: Replaces a token in a player's hand
        public void activate(Player player, Token letterToReplace)
        {
            if (player.hand.Length != MAX_HAND_SIZE)
                player.AddToHand(new_letter);
            else
            {
                player.RetrieveToken(letterToReplace);
                player.AddToHand(new_letter)
            }
        }
    }

    public class Multiplier : StoreItem
    {
        /*
        Store item to multiply a score - random for a buff (2x) or debuff (0.5x)
        If bought, the multiplier variable is used to affect the word score
        */
        private bool status;     // Flag to determine if this item is a debuff or a buff; default is buff
        private float multiplier;
        storeWeight = 0.1;              // 10% chance to appear in the store 
        
        // Description: Constructor for the Multiplier StoreItem. Randomly determines
        //              if the object will be a buff or debuff
        public Multiplier()
        {
            status = true;
            multiplier = 0;
            if(rand_mod.NextDouble() > 0.5)
            {
                status = true;      // Item is a buff
                multiplier = 2.0;   // When used, word score is multiplied by 2
            }
            else
            {
                status = false;     // Item is a debuff
                multiplier = 0.5;   // When used, word score is multiplyed by 0.5
            }
        }
        // Description: Activates the multiplier bonus on player or
        //              opponent depending on status of the object
        // Not sure how to implement this yet
        public void activate()
        {
            if(status)
            {
                // Buff status; apply 2x bonus to next play
            }
            else
            {
                // Debuf status; apply 0.5x bonus to opponent's netx play
            }
            return;
        }
    }

    public class LetterSwapper : StoreItem
    {
        /*
        Store item to swap a letter for a player; strictly a debuff item
        */
        public Token swapToken = Instantiate(tokenPrefab);
        storeWeight = 0.25;                                 // 25% chance to appear in the store

        // Description: Constructor for LetterSwapper debuff item. Randomly chooses
        //              a rare letter and swaps it from the opponet's hand
        public LetterSwapper()
        {
           swapToken.tokenLetter = rare_letters[rand_mod.Next(0,rare_letters.Length)]  // Select random letter from rare pool 
        }

        // Description: Swaps a random letter from the passed player's hand
        public void activate(Player player)
        {
            if(player.hand.Length != MAX_HAND_SIZE)
                player.AddToHand(swapToken);
            else
            {
                player.RetrieveToken(player.SelectRandomFromHand);
                player.AddToHand(swapToken);
            }
        }

    }
    
    public class TimeIncrease : StoreItem
    {
        /*
        Store item to increase turn time
        */
        private float timeAdded = rand_mod.Next(1, 4)*1000;     // Returns a random integer 1-3 and converts to milliseconds
        storeWeight = 0.1;                                      // 10% chance to appear in the store
        
        // Description: Adds the timeAdded variable to a player's turn time
        public void activate(Player player)
        {
            player.turnTimeMS = player.turnTimeMS + timeAdded;
        }
    }

    public class WordStealer : StoreItem
    {
        /*
        Store item to steal an opponent's word score
        */
        storeWeight = 0.1;             // 5% chance to appear in the store

        // TODO: implement when we determine how word scores are calculated
    }
}