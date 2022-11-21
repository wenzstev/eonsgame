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
        transform.localPosition = GetLocationOnCircle(CurrentAngle);
    }

    public void IncrementAngle()
    {
        CurrentAngle += AngleAmount / 2 * (WillMoveLeft ? 1 : -1);
        transform.localPosition = GetLocationOnCircle(CurrentAngle);
    }

    public void DecrementAngle()
    {
        CurrentAngle -= AngleAmount / 2 * (WillMoveLeft ? 1 : -1);
        transform.localPosition = GetLocationOnCircle(CurrentAngle);

    }

    Vector2 GetLocationOnCircle(float theta)
    {
        return new Vector2(Radius * Mathf.Cos(theta), Radius * Mathf.Sin(theta));
    }


}
