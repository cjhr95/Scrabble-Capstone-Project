using System;
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
    // 1 point
    A = 1, E = 1, I = 1, O = 1, U = 1, L = 1, N = 1, S = 1, T = 1, R = 1,

    // 2 points
    D = 2, G = 2,

    // 3 points
    B = 3, C = 3, M = 3, P = 3, 

    // 4 points
    F = 4, H = 4, V = 4, W = 4, Y = 4,

    // 5 points
    K = 5,

    // 8 points
    J = 8, X = 8,

    // 10 points
    Q = 10, Z = 10
  }
  public class Token : MonoBehaviour
  {
    public const int TOKEN_BONUS_LIFETIME = 15;
    public int remainingTime { get; set; }              // The timer for the token
    public string tokenLetter { get; private set; }     // The letter the token represents
    public int pointValue { get; private set; }         // The value of the token
    private int minPoint;
    private float creationTime;
    PlayerManager playerManager;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI pointValueText;
    
    // Description: Creates a token with the given letter.
    //              Attempts to assign the token a value based
    //              on the letter values from the enum. If
    //              it fails, then the point value will be 0.
    public void Initialize(string letter)
    {
      playerManager = FindAnyObjectByType<PlayerManager>();
      tokenLetter = letter.ToUpper();
      text.text = tokenLetter;

      creationTime = Time.time;

      LetterValues letterVal;
      if (Enum.TryParse(tokenLetter, out letterVal))
      {
        pointValue = (int)letterVal;
      }
      else pointValue = 0;

      minPoint = pointValue;
    }

    public void Update()
    {
      text.transform.position = gameObject.transform.position;
      int newPointVal = minPoint + (TOKEN_BONUS_LIFETIME - deltaTime());
      if (playerManager.IsHumanPlayer()) pointValue = newPointVal >= minPoint ? newPointVal : minPoint;
      pointValueText.text = pointValue.ToString();
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

    private int deltaTime()
    {
      return (int) (Time.time - creationTime);
    }
  }
}
