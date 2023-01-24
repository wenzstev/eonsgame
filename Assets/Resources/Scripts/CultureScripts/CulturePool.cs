using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Class for generating cultures from a shared objectpool
/// </summary>
public class CulturePool : MonoBehaviour
{
    public Culture _culturePrefab;
    private static ObjectPool<Culture> _pool;
    private int amountToPool = 20;

    public static Culture GetCulture()
    {
        return _pool.Get();
    }

    public static void ReleaseCulture(Culture culture)
    {
        _pool.Release(culture);
    }

    private void Awake()
    {
        _pool = new ObjectPool<Culture>( ()=>
        {
            // what to do when there isn't an available object
            return Instantiate(_culturePrefab);
        }, culture =>
        {
            // what to do when you need the object
            culture.gameObject.SetActive(true);
        }, culture =>
        {
            // what to do when returning to pool
            culture.gameObject.SetActive(false);
        }, culture => 
        {
            Destroy(culture.gameObject);
        }, false, 100, 10000); 

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
