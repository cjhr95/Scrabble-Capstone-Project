using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
  public class TimeIncreaseButtonScript : MonoBehaviour
  {
    [SerializeField] public Button button;
    [SerializeField] private TextMeshProUGUI costText;
    StoreItem TimeIncreaseObj;

    // Start is called before the first frame update
    void Start()
    {
      TimeIncreaseObj = new TimeIncrease();
      costText.text = TimeIncreaseObj.cost.ToString();
      button.onClick.AddListener(onClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void onClick()
    {
      Debug.Log("Clicked Time Increase Store Object");
      // SceneManager.LoadScene("GameScene");
      if (User.player.score > TimeIncreaseObj.cost)
      {
        User.player.SetScore(-1 * TimeIncreaseObj.cost);  // Subtract from score
        ((TimeIncrease)TimeIncreaseObj).activate(User.player);
      }
      else
        Debug.Log("User was too poor to afford TimeIncrease: Score = " + User.player.score);
    }
  }
}