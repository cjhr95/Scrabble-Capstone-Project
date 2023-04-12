using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static TMPro.TMP_InputField;

namespace Assets
{
  public class LetterButtonScript : MonoBehaviour
  {
    [SerializeField] public Button button;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TMP_InputField inputField;
    StoreItem LetterObj;
    //public string userInput;

    // Start is called before the first frame update
    void Start()
    {
      LetterObj = new Letter();
      Button btn = button.GetComponent<Button>();
      btn.onClick.AddListener(onClick);
      costText.text = LetterObj.cost.ToString();
      inputField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      //userInput = Input.inputString;
    }

    void onClick()
    {
      Debug.Log("Clicked Letter Store Object");
      // SceneManager.LoadScene("GameScene");

      // Create an InputField object for the user to enter a letter to swap
      inputField.gameObject.SetActive(true);

      if (User.player.score >= LetterObj.cost)
      {
        StartCoroutine(haha());
      }
      else Debug.Log("User was too poor to afford Letter: Score = " + User.player.score);
    }

    IEnumerator haha()
    {
      //Debug.Log(inputField.text);
      while (true)
      {
        inputField.onEndEdit.AddListener(SubmitName);
        
        if (inputField.text.Length == 1)
        {
          ((Letter)LetterObj).activate(User.player, inputField.text.ToUpper());
          User.player.SetScore(User.player.score + (-1 * LetterObj.cost));    // Subtract from score
          inputField.gameObject.SetActive(false);
          yield break;
        }
        yield return null;
      }
    }

    private void SubmitName(string userLetter)
    {
      Debug.Log("Entered letter was: " + userLetter);
    }
  }
}
