using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
  public class TimeIncreaseButtonScript : MonoBehaviour
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
      Debug.Log("Clicked Time Increase Store Object");
      // SceneManager.LoadScene("GameScene");
      var TimeIncreaseObj = new TimeIncrease();
      if (User.player.score > TimeIncreaseObj.cost)
      {
        User.player.SetScore(-1 * TimeIncreaseObj.cost);  // Subtract from score
        TimeIncreaseObj.activate(User.player);
      }
      else
        Debug.Log("User was too poor to afford TimeIncrease");
    }
  }
}