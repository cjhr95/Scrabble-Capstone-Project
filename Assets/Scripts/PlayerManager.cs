using Assets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  [SerializeField] private Player playerPrefab;
  [SerializeField] private TextMeshProUGUI userScore;
  [SerializeField] private TextMeshProUGUI computerScore;
  // Start is called before the first frame update
  void Start()
  {
    LetterPoolManager.FillPool(PoolStyle.Scrabble);

    User.InitializeUser(playerPrefab);
    User.player.DrawToMaxHand();

    Camera.main.gameObject.transform.position = new Vector3(0, 0, -15);
  }

  // Update is called once per frame
  void Update()
  {
        
  }

  public void updateUserScore(int score)
  {
    User.player.SetScore(score);
    userScore.text = "Score: " + score.ToString();
  }

  public void UpdateComputerScore(int score)
  {
    computerScore.text = "Computer Score: " + score.ToString();
  }
}
