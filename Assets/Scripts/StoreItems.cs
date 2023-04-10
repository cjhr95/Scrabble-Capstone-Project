using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


namespace Assets
{
    public class StoreItem
    {
        public int cost { get; set; }      // I don't know what to set for cost values yet
        public double storeWeight { get; set; }
        protected System.Random rand_mod = new System.Random();
        protected string[] common_letters = {"A", "E", "I", "O", "N", "R", "T", "L", "S", "U"};
        protected string[] uncommon_letters = {"D", "B", "G", "C", "M", "P", "F", "H", "W", "Y"};
        protected string[] rare_letters = {"V", "K", "J", "X", "Q", "Z"};
        [SerializeField] protected static Token tokenPrefab;
        [SerializeField] private TextMeshProUGUI text;

        public void Update()
        {
            //text.transform.position = gameObject.transform.position;
        }
    }

    public class Letter : StoreItem
    {
        /*
        The Letter store item
        */
        // Member variables
        private string new_letter;    // Letter token object for the store
        //Token new_letter = Instantiate(tokenPrefab);

        // Description: Constructor for the Letter StoreItem. Randomly selects the letter
        //              based on rarity
        public Letter()
        {
            storeWeight = 0.5;          // 50% chance to appear in the store
            cost = 5;
            double probability = rand_mod.NextDouble();
            if (probability > 0.4)                                      // 60% chance for common letter (A, E, I, O, N, R, T, L, S, U)
                new_letter = common_letters[rand_mod.Next(0, common_letters.Length)];
            else if ((probability > 0.1) && (probability <= 0.4))       // 30% chance for uncommon letter (D, B, G, C, M, P, F, H, W, Y)
                new_letter = uncommon_letters[rand_mod.Next(0, common_letters.Length)];
            else                                                       // 10% chance for rare letter (V, K, J, X, Q, Z)
                new_letter = rare_letters[rand_mod.Next(0, common_letters.Length)];
        }

        // Description: Replaces a token in a player's hand
        public void activate(Player player, string letterToReplace)
        {
            Token tokenFromHand;
            Token newToken = new Token();
            newToken.Initialize(new_letter);
            if (player.hand.Length != Player.MAX_HAND_SIZE)
            {
                //Token newToken;
                //newToken.Initialize(new_letter);
                player.AddToHand(newToken);
            }
            else
            {
                if (player.HasToken(letterToReplace))
                {
                    player.RetrieveToken(player.GetTokenFromLetter(letterToReplace), out tokenFromHand);
                    player.AddToHand(newToken);
                }
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
        private double multiplier;
        
        // Description: Constructor for the Multiplier StoreItem. Randomly determines
        //              if the object will be a buff or debuff
        public Multiplier()
        {
            storeWeight = 0.1;      // 10% chance to appear in the store
            cost = 20;
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
        //              opponent based on passed Player object.
        //              Edits Player multiplier status to effect SetScore()
        //              function
        public void activate(Player player)
        {
            if(status)
            {
                // Buff status; apply 1.5x bonus to next play
                player.multStatus = MultiplierStatus.Buff;
            }
            else
            {
                // Debuff status; apply 0.5x bonus to opponent's next play
                player.multStatus = MultiplierStatus.Debuff;
            }
            return;
        }
    }

    public class LetterSwapper : StoreItem
    {
        /*
        Store item to swap a letter for a player; strictly a debuff item
        */
        private string swapLetter;

        // Description: Constructor for LetterSwapper debuff item. Randomly chooses
        //              a rare letter and swaps it from the opponet's hand
        public LetterSwapper()
        {
           storeWeight = 0.25;      // 25% chance to appear in the store
           cost = 10;
           swapLetter = rare_letters[rand_mod.Next(0,rare_letters.Length)];  // Select random letter from rare pool 
        }

        // Description: Swaps a random letter from the passed player's hand
        public void activate(Player player)
        {
            Token tokenFromHand;
            Token swapToken = new Token();
            swapToken.Initialize(swapLetter);
            if(player.hand.Length != Player.MAX_HAND_SIZE)
                player.AddToHand(swapToken);
            else
            {
                player.RetrieveToken(player.SelectRandomFromHand(), out tokenFromHand);
                player.AddToHand(swapToken);
            }
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