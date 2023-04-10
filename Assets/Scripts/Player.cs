using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets
{
  public enum PlayerType
  {
    Computer,
    Human
  }
  public enum MultiplierStatus
  {
    Debuff = -1,
    None = 0,
    Buff = 1
  }
  public class Player : MonoBehaviour
  {
    public const int MAX_HAND_SIZE = 7;         // The maximum number of tokens allowed
    public const float TIME_BONUS_RATE = 1.5f;
    public Token[] hand { get; private set; }   // The hand of tokens
    private int lastIndex;                      // Index to store where next empty slot is
    public int turnTimeS;                      // Turn time in milliseconds
    public int score { get; private set; }
    public bool StopTurns;
    public int numPasses;
    public bool GivenUp;
    public PlayerType playerType { get; private set; }
    public MultiplierStatus multStatus { get; set;}


    [SerializeField] private Token tokenPrefab;

    public void Initialize(PlayerType playerType = default)
    {
      hand = new Token[MAX_HAND_SIZE];
      lastIndex = 0;
      score = 0;
      turnTimeS = 0;
      StopTurns = false;
      GivenUp = false;
      this.playerType = playerType;
      multStatus = MultiplierStatus.None;
    }

    public void SetScore(int amt)
    { 
      if (multStatus == MultiplierStatus.Buff)
      {
        // Player has a buff active on themselves
        score = (int)(amt * 1.5);  
        multStatus = MultiplierStatus.None;      // Reset multiplier status
      }
      else if (multStatus == MultiplierStatus.Debuff)
      {
        // Player (most likely the computer) has a debuff active
        score = (int)(amt * 0.5); 
        multStatus = MultiplierStatus.None;      // Reset multiplier status
      }
      else
        score = amt;            // No change to the score
    }

    public int FinalizedScore()
    {
      return score += (int)( turnTimeS * TIME_BONUS_RATE);
    }

    // Description: Adds a token to the hand.
    //              Ignores token if the hand is full.
    // Return:      Returns true if adding was successful.
    //              false otherwise.
    public bool AddToHand(Token t)
    {
      if (lastIndex < MAX_HAND_SIZE)
      {
        hand[lastIndex] = t;
        int oldIndex = lastIndex;
        for (int i = 0; i < hand.Length; i++)
        {
          if (hand[i] == null)
          {
            lastIndex = i;
            break;
          }
        }
        // No new empty spots found
        if (oldIndex == lastIndex) lastIndex = MAX_HAND_SIZE;


        // Move tokens respectively.
        RepositionTokens();

        t.gameObject.SetActive(true);
        return true;
      }
      else return false;
    }

    public void RepositionTokens()
    {
      int xpos = 0;
      for (int i = 0; i < hand.Length; i++)
      {
        if (hand[i] != null)
        {
          Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
          float yVal = bottomLeft.y + hand[i].gameObject.GetComponent<SpriteRenderer>().size.y;
          float xVal = bottomLeft.x + hand[i].gameObject.GetComponent<SpriteRenderer>().size.x + xpos;
          hand[i].gameObject.transform.position = new Vector3(xVal, yVal, 0);
          xpos++;
        }
      }
    }


    // Description: Checks if the player has a token.
    //              This is not a fully deep-check,
    //              the remaining time on the token is
    //              not compared.
    public bool HasToken(Token t)
    {
      foreach (Token token in hand)
      {
        if (token == t) return true;
      }

      return false;
    }

    // Description: Checks if the player has a token
    //              of the given letter.
    public bool HasToken(string t)
    {
      foreach (Token token in hand)
      {
        if (token.tokenLetter == t) return true;
      }

      return false;
    }

    // Description: Gets the matching token from the user's hand.
    //              This does NOT remove the token from the hand.
    //              Use RetrieveToken instead for that purpose.
    public Token GetTokenFromLetter(string letter)
    {
      foreach (Token token in hand)
      {
        if (token is not null &&  token.tokenLetter == letter) return token;
      }

      return null;
    }

    public List<string> GetAvailableLetters()
    {
      List<string> list = new List<string>();
      for (int i = 0; i < hand.Length; i++)
      {
        if (hand[i].tokenLetter != "") list.Add(hand[i].tokenLetter);
      }

      return list;
    }

    // Description: Returns a random token from the hand. This
    //              will not remove it from the hand.
    public Token SelectRandomFromHand()
    {
      System.Random r = new System.Random();
      return hand[r.Next(hand.Length)];
    }

    // Description: Draws a token from the pool. Returns true
    //              if it was able to, false otherwise.
    public bool DrawAToken()
    {
      if (lastIndex < MAX_HAND_SIZE && LetterPoolManager.GetCurrentPoolSize() > 0)
      {
        var createdToken = Instantiate(tokenPrefab);
        createdToken.Initialize(LetterPoolManager.RetrieveLetterFromPool());
        AddToHand(createdToken);
        return true;
      }
      else return false;
    }

    // Description: Draws cards until the hand can no longer accept
    //              more.
    public void DrawToMaxHand()
    {
      while (DrawAToken());
    }

    // Description: Finds and retrieves a token from the hand.
    //              This will remove the token from the hand if it
    //              is found.
    // Return:      Returns true if the token was found and will place
    //              the found token in tokenFromHand. Returns false
    //              otherwise.
    public bool RetrieveToken(Token t, out Token tokenFromHand)
    {
      tokenFromHand = null;
      if (t == null || !HasToken(t)) return false;
      for (int i = 0; i < hand.Length; i++)
      {
        if (t == hand[i])
        {
          lastIndex = i;
          tokenFromHand = hand[i];
          tokenFromHand.gameObject.SetActive(false);
          hand[i] = null;
          RepositionTokens();
          return true;
        }
      }

      return false;
    }
  }
}
