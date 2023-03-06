using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverObject : MonoBehaviour
{
  [SerializeField] public HorizontalType horizontalButton;
  [SerializeField] public VerticalType verticalButton;
  public bool isActive { get; private set; }

  public void Initialize()
  {
    Deactivate();
  }

  public void HoldActivation()
  {
    isActive = true;
  }

  public void ActivateHorizontal()
  {
    isActive = true;
    horizontalButton.gameObject.SetActive(true);
  }

  private void DeactivateHorizontal()
  {
    horizontalButton.gameObject.SetActive(false);
  }

  public void ActivateVertical() 
  {
    isActive = true;
    verticalButton.gameObject.SetActive(true);
  }

  private void DeactivateVertical()
  {
    verticalButton.gameObject.SetActive(false);
  }

  public void Deactivate()
  {
    isActive = false;
    horizontalButton.gameObject.SetActive(false);
    verticalButton.gameObject.SetActive(false);
  }

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
    if (isActive)
    {
      Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
      if (Input.mousePosition.x > pos.x) ActivateHorizontal();
      else DeactivateHorizontal();
      if (Input.mousePosition.y < pos.y) ActivateVertical();
      else DeactivateVertical();
    }
  }
}
