using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    List<ICameraRestriction> Restrictions;
    public Camera Camera;

    private void Awake()
    {
        Restrictions = new List<ICameraRestriction>();
        Camera = GetComponent<Camera>();
    }

    public void AddRestriction(ICameraRestriction restriction)
    {
        Debug.Log("Adding restriction: " + restriction);
        Restrictions.Add(restriction);
    }

    public void AttemptMove(CameraMovement attemptedMove)
    {
        CameraMovement modifiedMove = attemptedMove;
        foreach (ICameraRestriction restriction in Restrictions)
        {
            modifiedMove = restriction.ProvideModifiedMove(modifiedMove);
        }
        modifiedMove.ExecuteMove();
    }
    
}

public interface ICameraRestriction
{
    public CameraMovement ProvideModifiedMove(CameraMovement attemptedMove);
}

