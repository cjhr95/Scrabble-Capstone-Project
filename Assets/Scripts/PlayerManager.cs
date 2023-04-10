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
        EndGame();
      }
      if (ActivePlayer.playerType == PlayerType.Human) userTime.text = changeText;
      else computerTime.text = changeText;
    }
  }

  public void UserPassTurn()
  {
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
        
        if (ActivePlayer.GivenUp)
        {
          Debug.Log("Game ended due to give up");
          EndGame();
        }
        else return;
      }
    }
    ActivePlayer = p;
    if (IsHumanPlayer()) winnerText.text = "Your turn";
    else winnerText.text = "Opponent's turn";
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
      winnerText.text = "You lost bozo";
    }
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
