using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButtonScript : MonoBehaviour
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
    Debug.Log("Clicked Play");
    SceneManager.LoadScene("GameScene");
  }
}
