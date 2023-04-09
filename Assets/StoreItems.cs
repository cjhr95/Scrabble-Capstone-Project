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
        public int cost { get; set; }      // I don't know what to set for cost values yet
        public float storeWeight { get; set; }
        Random rand_mod = new Random();
        private char[] common_letters = {'A', 'E', 'I', 'O', 'N', 'R', 'T', 'L', 'S', 'U'};
        private char[] uncommon_letters = {'D', 'B', 'G', 'C', 'M', 'P', 'F', 'H', 'W', 'Y'};
        private char[] rare_letters = {'V', 'K', 'J', 'X', 'Q', 'Z'};
        [SerializeField] private TextMeshProUGUI text;

        public void Update()
        {
            text.transform.position = gameObject.transform.position;
        }
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
        public void Initialize()
        {
            float probability = rand_mod.NextDouble();
            if(probability > 0.4)                                      // 60% chance for common letter (A, E, I, O, N, R, T, L, S, U)
                new_letter.tokenLetter = common_letters[rand_mod.Next(0, common_letters.Length)];
            else if((probability > 0.1) && (probability <= 0.4))       // 30% chance for uncommon letter (D, B, G, C, M, P, F, H, W, Y)
                new_letter.tokenLetter = uncommon_letters[rand_mod.Next(0, common_letters.Length)];
            else                                                       // 10% chance for rare letter (V, K, J, X, Q, Z)
                new_letter.tokenLetter = rare_letters[rand_mod.Next(0, common_letters.Length)];
        }

        // Description: Replaces a token in a player's hand
        public void activate(ref Player player, Token letterToReplace)
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
        Store item to multiply a score - random for a buff (1.5x) or debuff (0.5x)
        If bought, the multiplier variable is used to affect the word score
        */
        private bool status;            // Flag to determine if this item is a debuff or a buff; default is buff
        private float multiplier;
        storeWeight = 0.1;              // 10% chance to appear in the store 
        
        // Description: Constructor for the Multiplier StoreItem. Randomly determines
        //              if the object will be a buff or debuff
        public void Initialize()
        {
            status = true;
            multiplier = 0;
            if(rand_mod.NextDouble() > 0.5)
            {
                status = true;      // Item is a buff
                multiplier = 1.5;   // When used, word score is multiplied by 1.5
            }
            else
            {
                status = false;     // Item is a debuff
                multiplier = 0.5;   // When used, word score is multiplyed by 0.5
            }
        }
        // Description: Activates the multiplier bonus on player or
        //              opponent depending on status of the object
        // Will add multiplier to Score updating function
        public void activate()
        {
            if(status)
            {
                // Buff status; apply 2x bonus to next play
            }
            else
            {
                // Debuf status; apply 0.5x bonus to opponent's next play
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
        public void Initialize()
        {
           swapToken.tokenLetter = rare_letters[rand_mod.Next(0,rare_letters.Length)]  // Select random letter from rare pool 
        }

        // Description: Swaps a random letter from the passed player's hand
        public void activate(ref Player player)
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
        public void activate(ref Player player)
        {
            player.turnTimeMS = player.turnTimeMS + timeAdded;
        }
    }
}