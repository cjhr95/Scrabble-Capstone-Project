using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
  public class UserMultiplierButtonScript : MonoBehaviour
  {
    [SerializeField] public Button button;
    [SerializeField] private TextMeshProUGUI costText;
    StoreItem MultiplierObj;

    // Start is called before the first frame update
    void Start()
    {
      MultiplierObj = new Multiplier();
      costText.text = MultiplierObj.cost.ToString();
      Button btn = button.GetComponent<Button>();
      btn.onClick.AddListener(onClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void onClick()
    {
      Debug.Log("Clicked User Multiplier Store Object");
      // SceneManager.LoadScene("GameScene");
      if (User.player.score > MultiplierObj.cost)
      {
        User.player.SetScore(User.player.score + (-1 * MultiplierObj.cost));  // Subtract from score
        ((Multiplier)MultiplierObj).activate(User.player);
      }
      else
        Debug.Log("User was too poor to afford User Multiplier: Score = " + User.player.score);
    }
  }
}