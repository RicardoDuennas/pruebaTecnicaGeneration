using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;


public class Grid : MonoBehaviour
{
    public Material terrainMaterial;
    public float soilLevel = 1.0f;
    public float scale = .1f;
    public int size = 15;
    public Texture2D[] landTextures;
    public Texture2D[] soilTextures;
    

    public Cell[,] grid;

    void Start() {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        grid = new Cell[size, size];
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                float noiseValue = noiseMap[x, y];
                bool isSoil = noiseValue < soilLevel;
                Cell cell = new Cell(isSoil);
                grid[x, y] = cell;
            }
        }

        DrawTerrainMesh(grid);
        DrawTexture(grid);
        Debug.Log(getLandPercentage());    
    }

    void DrawTerrainMesh(Cell[,] grid) {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for(int y = 0; y < size; y++) {
            for(int x = 0; x < size; x++) {
                Cell cell = grid[x, y];
           /*      if(!cell.isSoil) { */
                    Vector3 a = new Vector3(x - .5f, 0, y + .5f);
                    Vector3 b = new Vector3(x + .5f, 0, y + .5f);
                    Vector3 c = new Vector3(x - .5f, 0, y - .5f);
                    Vector3 d = new Vector3(x + .5f, 0, y - .5f);
                    Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                    Vector2 uvB = new Vector2((x + 1) / (float)size, y / (float)size);
                    Vector2 uvC = new Vector2(x / (float)size, (y + 1) / (float)size);
                    Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                    Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                    Vector2[] uv = new Vector2[] { uvA, uvB, uvC, uvB, uvD, uvC };
                    for(int k = 0; k < 6; k++) {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }
           /*      } */
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
    }

/*     void DrawTexture(Cell[,] grid, Texture2D waterTexture, Texture2D landTexture)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] colorMap = new Color[size * size]; // This array is no longer needed

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                texture.SetPixel(x, y, cell.isSoil ? waterTexture.GetPixel(x, y) : landTexture.GetPixel(x, y));
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    } */


    void DrawTexture(Cell[,] grid)
    {
        int textureSize = size * 32; // Each cell is 32x32 pixels
        Texture2D texture = new Texture2D(textureSize, textureSize);
        Color[] colorMap = new Color[textureSize * textureSize];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                Texture2D cellTexture = cell.isSoil ? soilTextures[Random.Range(0, soilTextures.Length)] : landTextures[Random.Range(0, landTextures.Length)];

                for (int ty = 0; ty < 32; ty++)
                {
                    for (int tx = 0; tx < 32; tx++)
                    {
                        int pixelX = x * 32 + tx;
                        int pixelY = y * 32 + ty;
                        colorMap[pixelY * textureSize + pixelX] = cellTexture.GetPixel(tx, ty);
                    }
                }
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;
    }

    public float getLandPercentage(){
        int total = size * size;
        int sum = 0;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isSoil) sum += 1;
                //Debug.Log($"{x} {y} {cell.isSoil} {sum} {(float)sum/total*100}");
            }
        }
        return (float)sum/total;
    }
    public void swapSquare(Cell[,] grid, int x, int y, bool soil){
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        int textureSize = size * 32; // Each cell is 32x32 pixels
        Texture2D texture = (Texture2D)meshRenderer.material.mainTexture;

        Cell cell = grid[x, y];

        cell.isSoil = soil; 
        Texture2D cellTexture = cell.isSoil ? soilTextures[Random.Range(0, soilTextures.Length)] : landTextures[Random.Range(0, landTextures.Length)];

        for (int ty = 0; ty < 32; ty++)
        {
            for (int tx = 0; tx < 32; tx++)
            {
                int pixelX = x * 32 + tx;
                int pixelY = y * 32 + ty;
                texture.SetPixel(pixelX, pixelY, cellTexture.GetPixel(tx, ty));
            }
        }
        texture.Apply();
        meshRenderer.material.mainTexture = texture;
    }
}