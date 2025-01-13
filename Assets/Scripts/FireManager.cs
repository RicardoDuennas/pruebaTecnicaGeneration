using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public static FireManager Instance;
     private float fireLevel = 0.25f;
     private float scale = 1f;
     private int size = 15;
    public GameObject fireToSpawn;
    public Cell[,] fireGrid;

    private int initFires = 6;
    public Dictionary<Vector3, GameObject> Fires { get; private set; } = new Dictionary<Vector3, GameObject>();

    
void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // If needed across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start() {

        // Create cells grid
        fireGrid = new Cell[size, size];
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {

                bool isActive = false;
                Cell cell = new Cell(isActive);
                fireGrid[x, y] = cell;
            }
        }

        // //Create Perlin noise map
        // float[,] noiseMap = new float[size, size];
        // (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        // for(int y = 0; y < size; y++) {
        //     for(int x = 0; x < size; x++) {
        //         float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
        //         noiseMap[x, y] = noiseValue;
        //     }
        // }

        // // Update grid state
        // for(int y = 0; y < size; y++) {
        //     for(int x = 0; x < size; x++) {
        //         float noiseValue = noiseMap[x, y];
        //         if (noiseValue < fireLevel){

        //             Cell cell = fireGrid[x, y];
        //             cell.isActive = true; 

        //             Vector3 position = new Vector3(x + 0.2f, 0, y);
        //             GameObject trench = Instantiate(fireToSpawn, position, Quaternion.Euler(0f, 90f, 0f));
        //         }
        //     }
        // }

        // Set random fires
        for (int i = 0; i < initFires; i++)
        {
            int x = Random.Range(0, size - 1);
            int y = Random.Range(0, size - 1);
            addFire(x, y);
        }
    }

    public bool isFireActive(int x, int y){
        Vector3 position = new Vector3(x, 0, y);
        return Fires.ContainsKey(position); 
    } 

    public void deleteFire(int x, int y){
        Vector3 position = new Vector3(x, 0, y);
        if (Fires.ContainsKey(position)){
            GameObject fire = Fires[position];

            if(fire.GetComponent<Fire>() != null)
                {
                    Fire fireScript = fire.GetComponent<Fire>();
                    Fires.Remove(position);
                    fireScript.ExtinguishFire();
                }
        }     
    } 

    public void addFire(int x, int y){
        Vector3 position = new Vector3(x, 0, y);
        if (!Fires.ContainsKey(position))
        {
            GameObject newFire = Instantiate(fireToSpawn, position, Quaternion.Euler(0f, 90f, 0f));
            Fires[position] = newFire;
        }
    } 

    public int countFires(){
        int count = 0;
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                    Cell cell = fireGrid[x, y];
                    if (cell.isActive) count += 1; 
            }
        }
        return count;
     
    }

}