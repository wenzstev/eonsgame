using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowScroll : MonoBehaviour
{

    public float scrollSpeed;

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
        Vector3 speed = new Vector3(scrollSpeed, 0, 0);
        while(transform.position.x > endX)
        {
            transform.Translate(-speed);
            yield return null;
        }

        
    }

}
