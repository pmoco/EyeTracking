using UnityEngine;
using UnityEngine.XR;
using Tobii.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;


public class PicoClick : MonoBehaviour
{
    public XRNode inputSource = XRNode.RightHand; // Choose the input source (RightHand or LeftHand)
    public GameObject raycastOrigin; // The game object representing the HMD center (usually set to the HMD GameObject)

    private XRRayInteractor rayInteractor;
    private bool isRaycasting = false;

    public bool on = true;

    public TextMeshProUGUI LOG;

    public GameObject target;
    

    public Color highlightColor = Color.yellow;
    private Color originalColor;
    private Material material;

    private GameObject highlightedObject;









    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();

        TurnOn();
    }

    void Update()
    {

        if (Input.anyKeyDown){
             foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Log the pressed key to the Unity console
                    LOG.SetText("Key Pressed: " + keyCode);
                    break;
                }
            }

        }



        if (on){


            RaycastHit hit;

        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0f);

        // Cast a ray from the center of the screen
        Ray ray = Camera.main.ViewportPointToRay(screenCenter);

            if (Physics.Raycast(ray, out hit))
            {


                target.transform.position = hit.point;


                // Check if the raycast hits a UI button
               

                if (Input.GetKeyDown(KeyCode.JoystickButton0)) // Adjust the input button name to match Pico VR's side button
                {
                // Perform raycast
                    LOG.SetText("CONFIRM PRESSED");
                    PerformRaycast(hit);
                }



                //HIGHLIGHTING Object if UI and Unhighlighting old
                if (hit.collider.CompareTag("UI"))
                {
                    // Highlight the hit object
                    if (highlightedObject != hit.collider.gameObject)
                    {
                        // Unhighlight the previous object
                        if (highlightedObject != null)
                        {
                            UnhighlightObject(highlightedObject);
                        }

                        // Highlight the current object
                        highlightedObject = hit.collider.gameObject;
                        HighlightObject(highlightedObject);
                    }
                }
                else
                {
                    // Unhighlight the previous object if needed
                    if (highlightedObject != null)
                    {
                        UnhighlightObject(highlightedObject);
                        highlightedObject = null;
                    }
                }

            }
            else //if No hit registered 
            {
                // Unhighlight the previous object if needed
                if (highlightedObject != null)
                {
                    UnhighlightObject(highlightedObject);
                    highlightedObject = null;
                }
            }

           
        }
      
    }

    void PerformRaycast(RaycastHit hit)
    {



         if (hit.collider.CompareTag("UI"))
                {
                    LOG.SetText("BUTON");
                    // Perform the button click (you can change this to any other action you want when the button is pressed)
                    hit.collider.GetComponent<Button>().onClick.Invoke();

                    // Set a flag to indicate that a raycast is already being performed
                    isRaycasting = true;
                }
                else {
                    LOG.SetText("hit n Miss : " + hit.collider.tag);
                }




    }

    public void TurnOn (){
        on = true;


        }

    public void TurnOff(){
        on = false;
        gameObject.GetComponent<GazeRecording>().targetOn = false;
    }



    private void HighlightObject(GameObject obj)
    {
        originalColor =   obj.GetComponent<Renderer>().material.color;
        // Change the material color to highlight color
        obj.GetComponent<Renderer>().material.color = highlightColor;
    }

    private void UnhighlightObject(GameObject obj)
    {
        // Change the material color back to the original color
        obj.GetComponent<Renderer>().material.color = originalColor;
    }
}
