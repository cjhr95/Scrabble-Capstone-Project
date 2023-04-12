using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : StoreItem
{

    // Member variables
    private string new_letter;    // Letter token object for the store
    [SerializeField] private Token tokenPrefab;

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
      // TODO: fix Object not being instantiated error
      var newToken = Instantiate(tokenPrefab);
      newToken.Initialize(new_letter);
      if (player.hand.Length != Player.MAX_HAND_SIZE)
      {
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
