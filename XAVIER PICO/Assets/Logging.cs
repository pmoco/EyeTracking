using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class Logging : MonoBehaviour
{

    public Prototype prototype ; 
    public GazeRecording gazeRecording;

    public string taskNumber;
    public int UserId;

    public TextMeshPro userIdText;

    public TextMeshProUGUI LOG;




    private string fileName = "Results.csv";
    private string filePath;

    private void Awake()
    {
        // Get the persistent data path of the application
        filePath = Path.Combine(Application.persistentDataPath, fileName);
    }

    public void WriteToCSV(string data)
    {
        // Append a new line with the provided data to the file
        using (StreamWriter sw = File.AppendText(filePath))
        {
            sw.WriteLine(data);
        }
    }

    public void RetireUser()
    {
        // Append a new line with the provided data to the file
        using (StreamWriter sw = File.AppendText(filePath))
        {
            string timestamp = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            sw.WriteLine($"{timestamp}, {UserId}, RETIRE, NULL, NULL, NULL, NULL, RETIRE");
        }
    }




    private void CreateCSVFile()
    {
        // Create a new file and write the header to it
        using (StreamWriter sw = File.CreateText(filePath))
        {
            sw.WriteLine("TimeStamp, userId, TaskType, GazeDeviationX, GazeDeviationY, DistanceDeviation, tagId, state"); // Replace with your desired header format
        }
    }

    public void SetTaskNumber( string task){

        taskNumber = task;
    }

    public void IncrementUserId(){
        UserId = UserId + 1;
        PlayerPrefs.SetInt("UserId", UserId);
        userIdText.SetText("UserID : "+ UserId);
    }


    // Start is called before the first frame update
    void Start()
    {
        prototype = gameObject.GetComponent<Prototype>(); 
        gazeRecording = gameObject.GetComponent<GazeRecording>();
        
        UserId =PlayerPrefs.GetInt("UserId");
        userIdText.SetText("UserID : "+ UserId);
        
        if (!File.Exists(filePath))
        {
            CreateCSVFile();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (prototype.state !=  "STOPPED"){
            string timestamp = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");


            // Get the gaze coordinates (mouse position in screen space)
//            Vector3 gazeCoordinates = Input.mousePosition;

            Vector3 gazeCoordinates = gazeRecording.gazeHit;


            


            // Get the current dot tag from the PrototypeScript
            string dotTag = prototype.getCurrentDotTag();

            // Get the current state from the PrototypeScript
            string state = prototype.state;

            Vector2 deviation =  CalculateDeviation(gazeCoordinates, prototype.tagCoordinates);
            float distance = CalculateDistance(gazeCoordinates, prototype.tagCoordinates);


            // Format and log the data
            string logData = $"{timestamp}, {UserId}, {taskNumber}, {deviation.x}, {deviation.y}, {distance}, {dotTag}, {state}";
            Debug.Log(logData);
            WriteToCSV(logData);

            LOG.SetText(logData);
            

        }


    }


        Vector2 CalculateDeviation(Vector3 a, Vector3 b)
    {
        // Create a new Vector2 with the difference between x and y components of vector A and B
        Vector2 deviation = new Vector2(b.x - a.x, b.y - a.y);
        return deviation;
    }

    float CalculateDistance(Vector3 a, Vector3 b)
    {
        // Calculate the distance between the x and y components of vector A and B
        float distance = Vector2.Distance(new Vector2(a.x, a.y), new Vector2(b.x, b.y));
        return distance;
    }




}
