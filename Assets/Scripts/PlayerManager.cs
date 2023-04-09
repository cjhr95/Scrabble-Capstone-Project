using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
  public const int NUM_MAX_PASSES = 1;

  [SerializeField] private Player playerPrefab;
  [SerializeField] private TextMeshProUGUI userScore;
  [SerializeField] private TextMeshProUGUI computerScore;
  [SerializeField] private TextMeshProUGUI userTime;
  [SerializeField] private TextMeshProUGUI computerTime;
  [SerializeField] private int DefaultTurnTimeS;
  [SerializeField] private TextMeshProUGUI winnerText;
  [SerializeField] private Button UserPassTurnButton;
  private Player ActivePlayer;
  private float TurnStartTimeS;
  private float TurnEndTimeS;
  public bool GameOver;
  // Start is called before the first frame update
  void Start()
  {
    LetterPoolManager.FillPool(PoolStyle.Scrabble);
    TurnEndTimeS = 0;
    TurnStartTimeS = 0;
    GameOver = false;
    winnerText.gameObject.SetActive(false);
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
        if (ActivePlayer.playerType == PlayerType.Human) ChangeActivePlayer(Computer.player);
        else ChangeActivePlayer(User.player);
      }
      if (ActivePlayer.playerType == PlayerType.Human) userTime.text = changeText;
      else computerTime.text = changeText;
    }
  }

  public void UserPassTurn()
  {
    Debug.Log(User.player.numPasses);
    if (ActivePlayer.playerType == PlayerType.Human)
    {
      User.player.numPasses++;
      if (User.player.numPasses == 2) UserPassTurnButton.GetComponentInChildren<TextMeshProUGUI>().text = "Give Up";
      if (User.player.numPasses > 2) User.player.GivenUp = true;
      ChangeActivePlayer(Computer.player);
    }
  }

  public void ComputerPassTurn()
  {
    if (ActivePlayer.playerType == PlayerType.Computer)
    {
      Computer.player.numPasses++;
      if (Computer.player.numPasses > 2) Computer.player.GivenUp = true;
      ChangeActivePlayer(User.player);
    }
  }

  public void ChangeActivePlayer(Player p)
  {
    if (ActivePlayer != null)
    {
      ActivePlayer.turnTimeS -= (int)(TurnEndTimeS - TurnStartTimeS);
      if (p.turnTimeS <= 0)
      {
        if (ActivePlayer.turnTimeS <= 0)
        {
          EndGame();
        }
        return;
      }

      if (LetterPoolManager.GetCurrentPoolSize() <= 0)
      {
        if (p.StopTurns)
        {
          EndGame();
          return;
        } else
        {
          ActivePlayer.StopTurns = true;
        }
      }

      if (p.GivenUp)
      {
        if (ActivePlayer.GivenUp) EndGame();
        else return;
      }
    }
    ActivePlayer = p;
    TurnStartTimeS = Time.time;
  }

  public void EndGame()
  {
    GameOver = true;
    userScore.text = "Score: " + User.player.FinalizedScore().ToString();
    computerScore.text = "Computer Score: " + Computer.player.FinalizedScore().ToString();
    if (User.player.FinalizedScore() > Computer.player.FinalizedScore())
    {
      winnerText.text = "You win!";
    }
    else if (User.player.FinalizedScore() == Computer.player.FinalizedScore())
    {
      winnerText.text = "~~Tie~~";
    } else
    {
      winnerText.text = "You lost bozo";
    }
    winnerText.gameObject.SetActive(true);
  }

  public bool IsHumanPlayer() { return ActivePlayer != null && ActivePlayer.playerType == PlayerType.Human; }

  public void updateUserScore(int score)
  {
    User.player.SetScore(score);
    userScore.text = "Score: " + score.ToString();
  }

  public void UpdateComputerScore(int score)
  {
    Computer.player.SetScore(score);
    computerScore.text = "Computer Score: " + score.ToString();
  }
}
