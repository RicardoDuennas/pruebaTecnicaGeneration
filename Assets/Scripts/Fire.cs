using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField] private float maxGrowthTime = 6f; // Time until fire reaches maximum size
    [SerializeField] private float maxScale = 6f; // Maximum size multiplier
    [SerializeField] private Vector3 initialScale = new Vector3(0.085f, 0.15f, 0.2f); // Starting size
    
    private bool isActive = true;
    private float currentGrowthTime = 0f;
    private Vector3 targetScale;
    
    public delegate void FireMaxSizeReached();
    public event FireMaxSizeReached OnFireMaxSize;
    
    private void Start()
    {
        maxGrowthTime += Random.Range(1, 3);
//        maxGrowthTime += Random.Range(10, 30);

        transform.localScale = initialScale;
        targetScale = initialScale * maxScale;
        
        StartCoroutine(GrowFire());
    }
    
    private IEnumerator GrowFire()
    {
        while (isActive && currentGrowthTime < maxGrowthTime)
        {
            currentGrowthTime += Time.deltaTime;
            float growthProgress = currentGrowthTime / maxGrowthTime;
            
            // Use smooth interpolation for natural growth
            transform.localScale = Vector3.Lerp(initialScale, targetScale, Mathf.SmoothStep(0f, 1f, growthProgress));
            
            // Check if fire has reached maximum size
            if (currentGrowthTime >= maxGrowthTime)
            {
//                OnFireMaxSize?.Invoke();
                SpreadFire();
                StopFire();
            }
            
            yield return null;
        }
    }
    
    public void ExtinguishFire()
    {
        isActive = false;
        StopAllCoroutines();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    public void StopFire()
    {
        StopAllCoroutines();
        
    }
    
    public void SpreadFire()
    {
        int dir1 = Random.Range(1, 5);
        int dir2;

        do
        {
            dir2  = Random.Range(1, 5); 
        } while (dir2 == dir1); 

        int x = (int)transform.position.x;
        int y = (int)transform.position.z;

        Debug.Log(dir1 + " " + dir2);

        if ((dir1 == 1) || (dir2 == 1))
        {
            if ((x != 0)){
                if (!FireManager.Instance.isFireActive(x - 1, y)){
                    FireManager.Instance.addFire(x - 1, y);
                }
            }             
        }
        
        if ((dir1 == 2) || (dir2 == 2))
        {
            if ((y != 14)){
                if (!FireManager.Instance.isFireActive(x, y + 1)){
                    FireManager.Instance.addFire(x, y + 1);
                }
            } 
        }

        if ((dir1 == 3) || (dir2 == 3))
        {
            if ((x != 14)){
                if (!FireManager.Instance.isFireActive(x + 1, y)){
                    FireManager.Instance.addFire(x + 1, y);
                }
            }
        }

        if ((dir1 == 4) || (dir2 == 4))
        {
            if ((y != 0)){
                if (!FireManager.Instance.isFireActive(x, y - 1)){
                    FireManager.Instance.addFire(x, y - 1);
                }
            }
        }
    }
    
    // Call this when water hits the fire cell
    public void ApplyWater()
    {
        ExtinguishFire();
    }
    
    // Public method to check if fire is still active
    public bool IsFireActive()
    {
        return isActive;
    }
    
    // Public method to get current growth progress (0 to 1)
    public float GetGrowthProgress()
    {
        return currentGrowthTime / maxGrowthTime;
    }
}