using UnityEngine;

public class HoverObject : MonoBehaviour
{
  [SerializeField] public HorizontalType horizontalButton;  // A placeholder to allow for code access to GameObject
  [SerializeField] public VerticalType verticalButton;      // See above
  public bool isActive { get; private set; }                // A flag to determine if the user is attempting to select a direction.
  public bool isTypingHorizontal { get; private set; }      // A flag to determine direction selected
  public bool isTypingVertical { get; private set; }        // See above

  // Description: Initializes a HoverObject to be
  //              invisible.
  public void Initialize()
  {
    Deactivate();
    isTypingHorizontal = false;
    isTypingVertical = false;
  }

  // Description: Used to turn on the gameObject.
  //              This turns on and "holds" it, so
  //              that the update code works.
  public void HoldActivation()
  {
    isActive = true;
  }

  private void ActivateHorizontal()
  {
    isActive = true;
    horizontalButton.gameObject.SetActive(true);
  }

  private void DeactivateHorizontal()
  {
    horizontalButton.gameObject.SetActive(false);
  }

  private void ActivateVertical() 
  {
    isActive = true;
    verticalButton.gameObject.SetActive(true);
  }

  private void DeactivateVertical()
  {
    verticalButton.gameObject.SetActive(false);
  }

  // Description: Used to deactivate the children
  //              of this gameObject. If the user
  //              has selected a direction, it will
  //              also flip the corresponding flags.
  public void Deactivate()
  {
    if (horizontalButton.gameObject.activeSelf)
    {
      isTypingHorizontal = true;
    } 
    else if (verticalButton.gameObject.activeSelf)
    {
      isTypingVertical = true;
    }

    isActive = false;
    horizontalButton.gameObject.SetActive(false);
    verticalButton.gameObject.SetActive(false);
  }

  public void ResetHover()
  {
    isTypingHorizontal = false;
    isTypingVertical = false;
    isActive = false;
    horizontalButton.gameObject.SetActive(false);
    verticalButton.gameObject.SetActive(false);
  }

  // Description: Called before the first frame update
  void Start()
  {
        
  }

  // Description: Called once per frame
  void Update()
  {
    if (isActive)
    {
      Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
      Vector3 mpos = Input.mousePosition;
      if (mpos.x > pos.x && mpos.y > pos.y - (mpos.x - pos.x)) ActivateHorizontal();
      else DeactivateHorizontal();
      if (mpos.y < pos.y && mpos.x < pos.x + (pos.y - mpos.y)) ActivateVertical();
      else DeactivateVertical();
    }
  }
}
