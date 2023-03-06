using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TileUnit : MonoBehaviour
{
  [SerializeField] private Color baseColor;
  [SerializeField] private new SpriteRenderer renderer;
  [SerializeField] private TextMeshProUGUI tileText;
  [SerializeField] private TextMeshProUGUI pointText;
  [SerializeField] private int pointValue;
  [SerializeField] private HoverObject hoverObject;
  public Vector2 position {  get; private set; }


  [SerializeField] private Color specialColor;
  [SerializeField] private bool isSpecial;

  public void Initialize(Vector2 pos, int pointVal = 0, string tileLetter = "", bool special = false, Color? color = null)
  {
    baseColor = Color.white;
    hoverObject = Instantiate(hoverObject);
    hoverObject.Initialize();
    hoverObject.transform.position = gameObject.transform.position;
    hoverObject.name = $"{gameObject.name} - Hover";
    hoverObject.gameObject.SetActive(false);

    position = pos;

    renderer = GetComponent<SpriteRenderer>();
    isSpecial = special;
    specialColor = color ?? baseColor;
    pointValue = pointVal;
    renderer.color = color ?? baseColor;

    tileText.text = tileLetter;
    pointText.text = pointVal.ToString();

  }

  public void OnMouseDown()
  {
    hoverObject.HoldActivation();
  }

  public void OnMouseUp()
  {
    hoverObject.Deactivate();
    hoverObject.gameObject.SetActive(false);
  }

  public void OnMouseEnter()
  {
    hoverObject.gameObject.SetActive(true);
  }

  public void OnMouseExit()
  {
    if (!hoverObject.isActive)
    {
      hoverObject.gameObject.SetActive(false);
    }
  }

  public void ChangeLetter(char letter)
  {
    tileText.text = letter.ToString();
  }

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
        
  }
}
