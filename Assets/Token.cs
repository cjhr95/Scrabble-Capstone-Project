﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets
{
  // The letter values were taken from Scrabble.
  // Letter values may need to be reworked during playtesting.
  public enum LetterValues
  {
    A, E, I, O, U, L, N, S, T, R = 1,
    D, G = 2,
    B, C, M, P = 3, 
    F, H, V, W, Y = 4,
    K = 5,
    J, X = 8,
    Q, Z = 10
  }
  public class Token : MonoBehaviour
  {
    public int remainingTime { get; set; }              // The timer for the token
    public string tokenLetter { get; private set; }     // The letter the token represents
    public int pointValue { get; private set; }         // The value of the token
    [SerializeField] private TextMeshProUGUI text;
    
    // Description: Creates a token with the given letter.
    //              Attempts to assign the token a value based
    //              on the letter values from the enum. If
    //              it fails, then the point value will be 0.
    public void Initialize(string letter)
    {
      tokenLetter = letter.ToUpper();
      text.text = letter;

      LetterValues letterVal;
      if (Enum.TryParse(tokenLetter, out letterVal))
      {
        pointValue = (int)letterVal;
      }
      else
      {
        pointValue = 0;
      }
    }

    public void Update()
    {
      text.transform.position = gameObject.transform.position;
    }

    public static bool operator ==(Token left, Token right)
    {
      if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
      else if (ReferenceEquals(right, null)) return false;
      else return left.tokenLetter == right.tokenLetter;
    }


    public static bool operator !=(Token left, Token right)
    {

      return !(left == right);
    }

    public override bool Equals(object obj)
    {
      return obj is Token token &&
             tokenLetter == token.tokenLetter;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(tokenLetter);
    }
  }
}