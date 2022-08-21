using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DialController : MonoBehaviour
{
    static readonly float[] rotationPoints = new float[] { 244, 225, 207, 184 };

    int curRotationIndex = 0;
    public float transitionSpeed = .05f;

    Transform WheelTransform;

    private void Start()
    {
        WheelTransform = transform.GetChild(0);
        RotateWheel(curRotationIndex);
        EventManager.StartListening("DialRotated", OnDialRotated);
        EventManager.StartListening("DialSetTo", OnDialSetTo);
    }

    void RotateWheel(int newIndex)
    {
        if(newIndex >= 0 && newIndex < rotationPoints.Length)
        {
            curRotationIndex = newIndex;
            IEnumerator coroutine = WheelRotationCoroutine(curRotationIndex);
            StartCoroutine(coroutine);
            EventManager.TriggerEvent("SpeedChange", new Dictionary<string, object> { { "speed", curRotationIndex } });
        }
    }

    IEnumerator WheelRotationCoroutine(int newIndex)
    {
        float startAngle = WheelTransform.eulerAngles.z;    
        float endAngle = rotationPoints[newIndex];

        float curLerp = 0;
        while (curLerp < 1) 
        {
            curLerp += transitionSpeed;
            WheelTransform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(startAngle, endAngle, curLerp));
            yield return null;
        }
        WheelTransform.eulerAngles = new Vector3(0, 0, endAngle);
    }

    void OnDialRotated(Dictionary<string, object> rotDirection)
    {
        RotateWheel(curRotationIndex + (int)rotDirection["direction"]);  // value can be either +1 (positive) or -1 (negative)
    }

    void OnDialSetTo(Dictionary<string, object> setPoint)
    {
        RotateWheel((int)setPoint["index"]);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("DialRotated", OnDialRotated);
        EventManager.StopListening("DialSetTo", OnDialSetTo);
    }


}
