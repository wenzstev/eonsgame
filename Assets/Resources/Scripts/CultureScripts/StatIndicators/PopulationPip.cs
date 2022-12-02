using System.Collections;
using UnityEngine;


public class PopulationPip : MonoBehaviour
{
    float CurrentAngle;
    public bool WillMoveLeft;
    public float AngleAmount;
    public float Radius;

    public void Init(bool moveLeft, float angleAmount, float radius)
    {
        WillMoveLeft = moveLeft;
        AngleAmount = (float) angleAmount * Mathf.Deg2Rad;
        Radius = radius;
        CurrentAngle = Mathf.PI / 2f;
        transform.localPosition = TrigUtils.GetLocationOnCircleRadians(Radius, CurrentAngle);
    }

    public void IncrementAngle()
    {
        CurrentAngle += AngleAmount / 2 * (WillMoveLeft ? 1 : -1);
        transform.localPosition = TrigUtils.GetLocationOnCircleRadians(Radius, CurrentAngle);
    }

    public void DecrementAngle()
    {
        CurrentAngle -= AngleAmount / 2 * (WillMoveLeft ? 1 : -1);
        transform.localPosition = TrigUtils.GetLocationOnCircleRadians(Radius, CurrentAngle);
    }

   


}
