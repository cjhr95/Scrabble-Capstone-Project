using Assets;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
  public const int NUM_MAX_PASSES = 2;

  [SerializeField] private Player playerPrefab;
  [SerializeField] private TextMeshProUGUI userScore;
  [SerializeField] private TextMeshProUGUI computerScore;
  [SerializeField] private TextMeshProUGUI userTime;
  [SerializeField] private TextMeshProUGUI computerTime;
  [SerializeField] private int DefaultTurnTimeS;
  [SerializeField] private TextMeshProUGUI winnerText;
  [SerializeField] private Button UserPassTurnButton;
  private GridManager gridManager;
  private Player ActivePlayer;
  private float TurnStartTimeS;
  private float TurnEndTimeS;
  public bool GameOver;
  // Start is called before the first frame update
  void Start()
  {
    LetterPoolManager.FillPool(PoolStyle.Scrabble);
    gridManager = FindAnyObjectByType<GridManager>();
    TurnEndTimeS = 0;
    TurnStartTimeS = 0;
    GameOver = false;
    UserPassTurnButton.onClick.AddListener(UserPassTurn);

    // Create user
    User.InitializeUser(playerPrefab);
    User.player.DrawToMaxHand();
    User.player.turnTimeS = DefaultTurnTimeS + 2;
    userTime.text = "Turn Time: " + TimeSpan.FromSeconds(User.player.turnTimeS).ToString(@"mm\:ss");

    // Create computer
    Computer.InitializeUser(playerPrefab);
    Computer.player.DrawToMaxHand();
    Computer.player.turnTimeS = DefaultTurnTimeS;
    computerTime.text = "Turn Time: " + TimeSpan.FromSeconds(Computer.player.turnTimeS).ToString(@"mm\:ss");

    Camera.main.gameObject.transform.position = new Vector3(0, 0, -15);
    ChangeActivePlayer(User.player);
  }

  // Update is called once per frame
  void Update()
  {
    if (ActivePlayer != null && GameOver == false)
    {
      TurnEndTimeS = Time.time;
      string changeText = "Turn Time: " + TimeSpan.FromSeconds((int)(ActivePlayer.turnTimeS - (TurnEndTimeS - TurnStartTimeS))).ToString(@"mm\:ss");
      if ((ActivePlayer.turnTimeS - (TurnEndTimeS - TurnStartTimeS)) <= 0)
      {
        Debug.Log("Game ended due to turn time out");
        ActivePlayer.turnTimeS -= (int)(TurnEndTimeS - TurnStartTimeS);
        EndGame();
      }
      if (ActivePlayer.playerType == PlayerType.Human) userTime.text = changeText;
      else computerTime.text = changeText;
    }

    userScore.text = "Score: " + User.player.score.ToString();
    computerScore.text = "Computer Score: " + Computer.player.score.ToString();
  }

  public void UserPassTurn()
  {
    if (ActivePlayer.playerType == PlayerType.Human)
    {
      User.player.numPasses++;
      if (User.player.numPasses == NUM_MAX_PASSES) UserPassTurnButton.GetComponentInChildren<TextMeshProUGUI>().text = "Give Up";
      if (User.player.numPasses > NUM_MAX_PASSES) User.player.GivenUp = true;
      ChangeActivePlayer(Computer.player);
    }
  }

  public void ComputerPassTurn()
  {
    if (ActivePlayer.playerType == PlayerType.Computer)
    {
      Computer.player.numPasses++;
      if (Computer.player.numPasses > NUM_MAX_PASSES) Computer.player.GivenUp = true;
      ChangeActivePlayer(User.player);
    }
  }

  public void ChangeActivePlayer(Player p)
  {
    if (GameOver) return;
    if (ActivePlayer != null)
    {
      ActivePlayer.turnTimeS -= (int)(TurnEndTimeS - TurnStartTimeS);

      if (LetterPoolManager.GetCurrentPoolSize() <= 0)
      {
        if (p.StopTurns)
        {
          Debug.Log("Game ended due to stop turns");
          EndGame();
          return;
        } else
        {
          ActivePlayer.StopTurns = true;
        }
      }

      if (p.GivenUp)
      {
        Debug.Log("Game ended due to give up");
        EndGame();
        return;
      }
    }
    ActivePlayer = p;
    if (IsHumanPlayer()) winnerText.text = "Your turn";
    else
    {
      winnerText.text = "Opponent's turn";
      StartCoroutine(ComputerTurn());
    }
    TurnStartTimeS = Time.time;
  }

  public void EndGame()
  {
    ActivePlayer = null;
    GameOver = true;
    userScore.text = "Score: " + User.player.FinalizedScore().ToString();
    computerScore.text = "Computer Score: " + Computer.player.FinalizedScore().ToString();
    if (User.player.FinalizedScore() > Computer.player.FinalizedScore() && !User.player.GivenUp)
    {
      winnerText.text = "You win!";
    }
    else if (User.player.FinalizedScore() == Computer.player.FinalizedScore())
    {
      winnerText.text = "~~Tie~~";
    } else
    {
      winnerText.text = "You lost :(";
    }
  }

  public bool IsHumanPlayer() { return ActivePlayer != null && ActivePlayer.playerType == PlayerType.Human; }

  IEnumerator ComputerTurn()
  {
    System.Random random = new System.Random();
    int score = 0;
    while (!GameOver)
    {
      yield return new WaitForSecondsRealtime(4.0f);
      string word = GameDictionary.GenerateWord(random.Next(3, 7));
      for (int y = 0; y < gridManager.height; y++)
      {
        for (int x = 0; x < gridManager.width; x++)
        {
          for (int i = 0; i < word.Length; i++)
          {
            if (gridManager.tiles[y, x].GetLetter() == word[i].ToString())
            {
              bool valid = true;

              if (gridManager.tiles[Math.Max(y - i, 0), x].GetLetter() != "") valid = false;
              for (int j = Math.Max(y - i, 0); j < Math.Min(y + (word.Length - i), gridManager.width-1); j++)
              {
                if (gridManager.tiles[j, x].GetLetter() != "") 
                  if (gridManager.tiles[j, x].GetLetter() != word[j - (y - i)].ToString()) valid = false;
              }
              if (gridManager.tiles[Math.Min(y + (word.Length - i + 1) + 1, gridManager.width - 1), x].GetLetter() != "") valid = false;

              if (valid)
              {
                Debug.Log(word);
                if (y - i < 0) continue;
                if (!gridManager.AddWordToGrid(new Vector2(y - i, x), word, true, Color.magenta, out score)) continue;
                Computer.player.SetScore(Computer.player.score + score);
                ChangeActivePlayer(User.player); 
                yield break;
              }

              // Vertical
              valid = true;

              if (gridManager.tiles[y, Math.Min(x + i + 1, gridManager.height-1)].GetLetter() != "") valid = false;
              for (int j = Math.Min(x + i, gridManager.height - 1); j > Math.Max(x - (word.Length - i), 0); j--)
              {
                if (gridManager.tiles[y, j].GetLetter() != "") 
                  if (gridManager.tiles[y, j].GetLetter() != word[(x + i) - j].ToString())
                    valid = false;
              }
              if (gridManager.tiles[y, Math.Max(x - (word.Length - i + 1) - 1, 0)].GetLetter() != "") valid = false;

              if (valid)
              {
                Debug.Log(word);
                if (x + i > gridManager.height) continue;
                if (!gridManager.AddWordToGrid(new Vector2(y, x + i), word, false, Color.magenta, out score)) continue;
                Computer.player.SetScore(Computer.player.score + score);
                ChangeActivePlayer(User.player);
                yield break;
              }
            }
          }
        }
      }
    }
  }
}
