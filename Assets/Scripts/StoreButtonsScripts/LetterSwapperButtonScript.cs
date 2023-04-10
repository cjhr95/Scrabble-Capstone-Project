using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
  public class LetterSwapperButtonScript : MonoBehaviour
  {
    [SerializeField] public Button button;

    // Start is called before the first frame update
    void Start()
    {
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
      var LetterSwapperObj = new LetterSwapper();
      if (User.player.score > LetterSwapperObj.cost)
      {
        User.player.SetScore(-1 * LetterSwapperObj.cost);       // Subtract from score
        LetterSwapperObj.activate(Computer.player);
      }
      else
        Debug.Log("User was too poor to afford LetterSwapper: Score = " + User.player.score);
    }
  }
}