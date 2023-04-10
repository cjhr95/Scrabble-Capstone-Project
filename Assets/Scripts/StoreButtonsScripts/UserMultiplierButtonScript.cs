using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
  public class UserMultiplierButtonScript : MonoBehaviour
  {
    [SerializeField] public Button button;

    // Start is called before the first frame update
    void Start()
    {
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
      var MultiplierObj = new Multiplier();
      if (User.player.score > MultiplierObj.cost)
      {
        User.player.SetScore(-1 * MultiplierObj.cost);  // Subtract from score
        MultiplierObj.activate(User.player);
      }
      else
        Debug.Log("User was too poor to afford User Multiplier");
    }
  }
}