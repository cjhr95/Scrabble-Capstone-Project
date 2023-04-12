using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSwapper : StoreItem
{
  /*
        Store item to swap a letter for a player; strictly a debuff item
        */
  private string swapLetter;
  [SerializeField] private Token tokenPrefab;

  // Description: Constructor for LetterSwapper debuff item. Randomly chooses
  //              a rare letter and swaps it from the opponet's hand
  public LetterSwapper()
  {
    storeWeight = 0.25;      // 25% chance to appear in the store
    cost = 10;
    swapLetter = rare_letters[rand_mod.Next(0, rare_letters.Length)];  // Select random letter from rare pool 
  }

  // Description: Swaps a random letter from the passed player's hand
  public void activate(Player player)
  {
    Token tokenFromHand;
    // TODO: fix Object not being instantiated error
    var swapToken = Instantiate(tokenPrefab);
    swapToken.Initialize(swapLetter);
    if (player.hand.Length != Player.MAX_HAND_SIZE)
      player.AddToHand(swapToken);
    else
    {
      player.RetrieveToken(player.SelectRandomFromHand(), out tokenFromHand);
      player.AddToHand(swapToken);
    }
  }
}
