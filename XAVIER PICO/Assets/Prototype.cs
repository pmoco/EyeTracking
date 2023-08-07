
//using System.Numerics;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;




public class Prototype : MonoBehaviour
{
    public UnityEvent onBegin;
    public UnityEvent onEnd;

    public List<Image> dots = new List<Image>();
    public GameObject grelha; 

    public TextMeshProUGUI LOG;

    public Vector3 tagCoordinates;

    bool on = false;

    public void Alpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    [Range(0.1f, 10.0f)]
    public float fadeInTime = 0.5f;
    [Range(0.1f, 10.0f)]
    public float showing = 5.0f;
    [Range(0.1f, 10.0f)]
    public float fadeOutTime = 0.5f;
    [Range(0.1f, 10.0f)]
    public float timeBetweenDots = 0.5f;
    [Range(0.0f, 10.0f)]
    public float scaleTime = 0.3f;
    [Range(2,20)]
    public int scaleMax = 10;
    



    public bool isShowing = false;
    public int activeDot = 0;
    public string state="STOPPED"; 
    int lastDot = 0;
    float animationTime = 0;

    public List<Image> RandomizeList(List<Image> list)
    {
        List<Image> newList = new List<Image>();
        while (list.Count > 0)
        {
            int index = Random.Range(0, list.Count);
            newList.Add(list[index]);
            list.RemoveAt(index);
        }
        return newList;
    }


    public string getCurrentDotTag (){

        if (activeDot  < dots.Count){
            return dots[activeDot].name;
        }
        else return "NULL";
    }

    public void AnimateDot(Image dot, float time)
    {

        if (time < fadeInTime)
        {
            state =  "FadeIn";
            float progress = Mathf.Clamp01(time / fadeInTime);
            Alpha(dot, progress);
            float scaleProgress = Mathf.Clamp01(time / scaleTime);
            dot.transform.localScale = Vector3.one * (scaleMax - (scaleMax - 1) * scaleProgress);
        }

        if (time >  fadeInTime && time < fadeInTime + showing)
        {
         state =  "Showing";
        }
        if (time > fadeInTime + showing)
        {
             state =  "FadeOut";
            Alpha(dot, 1.0f - (time - fadeInTime - showing) / fadeOutTime);
        }
        if (time > fadeInTime + showing + fadeOutTime + timeBetweenDots)
        {
            isShowing = false;
            activeDot++;
        }


        
        Vector2 centerLocalPoint = new Vector2(dot.rectTransform.rect.width / 2f, dot.rectTransform.rect.height / 2f);

        // Convert the local position to world space
        Vector3 centerWorldPosition = dot.transform.TransformPoint(centerLocalPoint);

        // Convert the world position to screen coordinates
        tagCoordinates = Camera.main.WorldToScreenPoint(centerWorldPosition);
        LOG.SetText("target Position: " + tagCoordinates);

    }    

    public void ResetApplication()
    {
        foreach (var dot in dots) Alpha(dot, 0.0f);
        dots = RandomizeList(dots);
        isShowing = false;
        animationTime = 0;
        activeDot = 0;
        lastDot = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Image[]  images  = grelha.GetComponentsInChildren<Image>();

        foreach( Image i in images){
            dots.Add(i);
        }
         state="STOPPED";
        UnityEngine.Debug.LogWarning(images.Length);
        ResetApplication();
    }


    public void TurnOn(){
        on = true;
    }

    public void TurnOff(){
        on = false;
    }



    // Update is called once per frame
    void Update()
    {

        if (on){
            
            if (!isShowing && activeDot < dots.Count)
            {
                animationTime = 0;
                isShowing = true;

                if (activeDot == 0)
                {
                    onBegin.Invoke();
                }
            }


            if (activeDot == dots.Count )
            {
                onEnd.Invoke();
                state ="STOPPED";
                activeDot ++;
            }

            if (isShowing)
            {
                animationTime += Time.deltaTime;
                AnimateDot(dots[activeDot], animationTime);
            }

        }
        
    }
}
