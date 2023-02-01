using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureDeleter : MonoBehaviour
{
    public MouseActionsController MouseActionsController;

    // Start is called before the first frame update
    void Awake()
    {
        MouseActionsController.MouseUpAction += MouseActionsController_MouseUpAction;
    }

    
    void DeleteCulture(GameObject CultureToDelete)
    {
        CultureToDelete.GetComponent<Culture>().DestroyCulture();
    }

    private void MouseActionsController_MouseUpAction(object sender, MouseActionsController.MouseActionEventArgs e)
    {
        if (Input.GetKey(KeyCode.D) && e.GetFirstThatContains<Culture>() != null)
        {
            DeleteCulture(e.GetFirstThatContains<Culture>());
        }
    }
}
