using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
  public class LetterButtonScript : MonoBehaviour
  {
    [SerializeField] public Button button;
    public string userInput;

    // Start is called before the first frame update
    void Start()
    {
      Button btn = button.GetComponent<Button>();
      btn.onClick.AddListener(onClick);
    }

    // Update is called once per frame
    void Update()
    {
      userInput = Input.inputString;
    }

    void onClick()
    {
      Debug.Log("Clicked Letter Store Object");
      // SceneManager.LoadScene("GameScene");
      var LetterObj = new Letter();
      if (User.player.score > LetterObj.cost)
      {
        print("Input the letter you want to swap:");
        if (userInput.Length == 1)
        {
          LetterObj.activate(User.player, userInput);
          User.player.SetScore(-1 * LetterObj.cost);    // Subtract from score
        }
      }
      else
        Debug.Log("User was too poor to afford Letter: Score = " + User.player.score);
    }
  }
}
