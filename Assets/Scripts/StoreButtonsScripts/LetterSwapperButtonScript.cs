using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
  public class LetterSwapperButtonScript : MonoBehaviour
  {
    [SerializeField] public Button button;
    [SerializeField] private TextMeshProUGUI costText;
    StoreItem LetterSwapperObj;

    // Start is called before the first frame update
    void Start()
    {
      LetterSwapperObj = new LetterSwapper();
      costText.text = LetterSwapperObj.cost.ToString();
      button.onClick.AddListener(onClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void onClick()
    {
      Debug.Log("Clicked Letter Swapper Store Object");
      // SceneManager.LoadScene("GameScene");
      if (User.player.score >= LetterSwapperObj.cost)
      {
        User.player.SetScore(User.player.score + (-1 * LetterSwapperObj.cost));       // Subtract from score
        ((LetterSwapper)LetterSwapperObj).activate(Computer.player);
      }
      else
        Debug.Log("User was too poor to afford LetterSwapper: Score = " + User.player.score);
    }
  }
}