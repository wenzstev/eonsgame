using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public Animator initiallyOpen;

    private Animator m_Open;

    private int m_OpenParameterId;

    private GameObject m_PreviouslySelected;

    const string k_OpenTransitionName = "Open";
    const string k_ClosedStateName = "Closed";

    public void OnEnable()
    {
        m_OpenParameterId = Animator.StringToHash(k_OpenTransitionName);

        if (initiallyOpen == null)
            return;
        OpenPanel(initiallyOpen);
    }

    public void OpenPanel(Animator anim)
    {
        if (m_Open == anim)
            return;

        anim.gameObject.SetActive(true);
        var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;

        anim.transform.SetAsLastSibling();

        CloseCurrent();

        m_PreviouslySelected = newPreviouslySelected;

        m_Open = anim;
        m_Open.SetBool(m_OpenParameterId, true);

        GameObject go = FindFirstEnabledSelectable(anim.gameObject);
        SetSelected(go);
    }

    static GameObject FindFirstEnabledSelectable(GameObject gameObject)
    {
        GameObject go = null;
        var selectables = gameObject.GetComponentsInChildren<Selectable>(true);
        foreach(var selectable in selectables)
        {
            if(selectable.IsActive() && selectable.IsInteractable())
            {
                go = selectable.gameObject;
                break;
            }
        }
        return go;
    }

    public void CloseCurrent()
    {
        if (m_Open == null)
            return;

        m_Open.SetBool(m_OpenParameterId, false);
        SetSelected(m_PreviouslySelected);

        StartCoroutine(DisablePanelDelayed(m_Open));

        m_Open = null;
    }

    IEnumerator DisablePanelDelayed(Animator anim)
    {
        bool closedStateReached = false;
        bool wantToClose = true; 
        while(!closedStateReached && wantToClose)
        {
            if (!anim.IsInTransition(0))
                closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

            wantToClose = !anim.GetBool(m_OpenParameterId);
            yield return new WaitForEndOfFrame();
        }

        if (wantToClose)
            anim.gameObject.SetActive(false);
    }

    private void SetSelected(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(go);

        var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
        if (standaloneInputModule != null)
            return;

        EventSystem.current.SetSelectedGameObject(null);
    }
}
