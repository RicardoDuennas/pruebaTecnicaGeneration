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

    public int initFires = 25;
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

    public void deleteAdjacentFires(int x, int y) {
        // Array of adjacent positions (left, right, up, down)
        int[,] adjacentPositions = new int[,] {
            {-1, 0},  // left
            {-2, 0},  // left double
            {-3, 0},  // left triple
            {1, 0},   // right
            {2, 0},   // right double
            {3, 0},   // right triple
            {0, -1},  // down
            {0, -2},  // down double
            {0, -3},  // down triple
            {0, 1},    // up
            {0, 2},    // up double
            {0, 3}    // up triple
        };

        // Create a list to store positions to delete
        List<Vector3> positionsToDelete = new List<Vector3>();

        // First gather all positions to delete
        Vector3 originalPosition = new Vector3(x, 0, y);
        if (Fires.ContainsKey(originalPosition)) {
            positionsToDelete.Add(originalPosition);
        }

        // Check each adjacent position
        for (int i = 0; i < 12; i++) {
            int newX = x + adjacentPositions[i, 0];
            int newY = y + adjacentPositions[i, 1];

            // Check if position is within grid boundaries (0-14)
            if (newX >= 0 && newX <= 14 && newY >= 0 && newY <= 14) {
                Vector3 position = new Vector3(newX, 0, newY);
                if (Fires.ContainsKey(position)) {
                    GameObject fire = Fires[position];
                    if (fire != null && fire.GetComponent<Fire>() != null) {
                        positionsToDelete.Add(position);
                    }
                }
            }
        }

        // Now delete all fires in a separate loop
        foreach (Vector3 position in positionsToDelete) {
            if (Fires.ContainsKey(position)) {
                deleteFire((int)position.x, (int)position.z);
            }
        }
    }

    public void deleteAroundFires(int x, int y) {
        // Array of adjacent positions (left, right, up, down)
        int[,] adjacentPositions = new int[,] {
            {-1, 0},  // left
            {-1, 1},  // left up
            {0, 1},    // up
            {1, 1},   // right up
            {1, 0},   // right
            {1, -1},   // right down
            {0, -1},  // down
            {-1, -1},  // down left
            {-2, 0},  // left double
            {0, 2},    // up double
            {2, 0},   // right double
            {0, -2}  // down double
        };

        // Create a list to store positions to delete
        List<Vector3> positionsToDelete = new List<Vector3>();

        // First gather all positions to delete
        Vector3 originalPosition = new Vector3(x, 0, y);
        if (Fires.ContainsKey(originalPosition)) {
            positionsToDelete.Add(originalPosition);
        }

        // Check each adjacent position
        for (int i = 0; i < 12; i++) {
            int newX = x + adjacentPositions[i, 0];
            int newY = y + adjacentPositions[i, 1];

            // Check if position is within grid boundaries (0-14)
            if (newX >= 0 && newX <= 14 && newY >= 0 && newY <= 14) {
                Vector3 position = new Vector3(newX, 0, newY);
                if (Fires.ContainsKey(position)) {
                    GameObject fire = Fires[position];
                    if (fire != null && fire.GetComponent<Fire>() != null) {
                        positionsToDelete.Add(position);
                    }
                }
            }
        }

        // Now delete all fires in a separate loop
        foreach (Vector3 position in positionsToDelete) {
            if (Fires.ContainsKey(position)) {
                deleteFire((int)position.x, (int)position.z);
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
                Vector3 position = new Vector3(x, 0, y);
                if (Fires.ContainsKey(position))
                {
                    count += 1;
                } 
            }
        }
        return count;
    }

}