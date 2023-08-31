using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.XR;
using TMPro;

public class GazeRecording : MonoBehaviour
{

    RaycastHit hit;
    Ray ray ;

    public GameObject Target ; //object used as crosshair

    public bool targetOn  =false;  // debug tool to see where the Eyegaze is hitting 
    //debug label to see hit values on screen
    public TextMeshProUGUI values_hit ;  //hit position on world
    public TextMeshProUGUI values_screen; //hit position translated to coordinates

    public Vector3 gazeHit ; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update ()
{
    // Get eye tracking data in world space
    var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);

    // Check if gaze ray is valid
    if(eyeTrackingData.GazeRay.IsValid)
    {
        // The origin of the gaze ray is a 3D point
        var rayOrigin = eyeTrackingData.GazeRay.Origin;

        // The direction of the gaze ray is a normalized direction vector
        var rayDirection = eyeTrackingData.GazeRay.Direction;

        ray = new Ray(rayOrigin, rayDirection);
        if (Physics.Raycast(ray, out hit))
        {
            // Get the world position of the hit point
            Vector3 hitPosition = hit.point;

            if (targetOn){
                Target.transform.position = hitPosition;
            }   

            // Convert the world position to screen coordinates
            gazeHit = Camera.main.WorldToScreenPoint(hitPosition);

            // Output the coordinates to the console
            values_hit.SetText("Hit Position: " + hitPosition);
            values_screen.SetText("Screen Coordinates: " + gazeHit);
        }
    }


    // // For social use cases, data in local space may be easier to work with
    // var eyeTrackingDataLocal = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.Local);

    // // The EyeBlinking bool is true when the eye is closed
    // var isLeftEyeBlinking = eyeTrackingDataLocal.IsLeftEyeBlinking;
    // var isRightEyeBlinking = eyeTrackingDataLocal.IsRightEyeBlinking;

    // // Using gaze direction in local space makes it easier to apply a local rotation
    // // to your virtual eye balls.
    // var eyesDirection = eyeTrackingDataLocal.GazeRay.Direction;


    
}
}
