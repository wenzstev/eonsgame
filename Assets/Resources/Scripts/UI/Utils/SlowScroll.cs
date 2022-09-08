using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowScroll : MonoBehaviour
{

    public float scrollSpeed;
    public float slowdownTime;

    public float startX;
    public float endX;

    


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Scroll());
    }

    public IEnumerator Scroll()
    {
        transform.position = new Vector3(startX, 0, 0);
        Vector3 topSpeed = new Vector3(scrollSpeed, 0, 0);

        for(int i = 0; i < 10000; i++)
        {
            // speed up phase
            yield return StartCoroutine(ChangeSpeed(slowdownTime, Vector3.zero, topSpeed));
            // main phase
            yield return StartCoroutine(MainScroll(topSpeed, endX));
            // slowdown phase
            yield return StartCoroutine(ChangeSpeed(slowdownTime, topSpeed, Vector3.zero));

            // reverse direction

            // speed up phase
            yield return StartCoroutine(ChangeSpeed(slowdownTime, Vector3.zero, -topSpeed));
            // main phase
            yield return StartCoroutine(MainScroll(-topSpeed, startX));
            // slowdown phase
            yield return StartCoroutine(ChangeSpeed(slowdownTime, -topSpeed, Vector3.zero));
        }

    }

    public IEnumerator ChangeSpeed(float changeTime, Vector3 startSpeed, Vector3 endSpeed)
    {
        float curTime = 0;
        while (curTime < changeTime)
        {
            curTime += Time.deltaTime;
            float curLerp = Mathf.InverseLerp(0, changeTime, curTime);
            Vector3 curSpeed = Vector3.Lerp(startSpeed, endSpeed, curLerp);
            transform.Translate(curSpeed);
            yield return null;
        }
        yield return null;
    }

    public IEnumerator MainScroll(Vector3 speed, float end)
    {
        while (Mathf.Abs(transform.position.x - end) > .005f) 
        {
            transform.Translate(speed);
            yield return null;
        }
        yield return null;

    }

}
