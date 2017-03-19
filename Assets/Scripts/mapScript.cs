using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class mapScript : MonoBehaviour {
    public byte[,,] data;
    public int worldX = 16;
    public int worldY = 16;
    public int worldZ = 16;

    public GameObject prefabChunk;
    public chunkScript[,,] chunks;
    private int chunkSize = 16;

    //Getter
    public int getChunkSize()
    {
        return this.chunkSize;
    }

    // Use this for initialization
    void Start () {
        data = new byte[16, 16, 16];

        data[0, 0, 0] = 1;
        data[0, 0, 1] = 1;
        data[0, 0, 2] = 1;
        data[0, 0, 3] = 1;
        data[1, 0, 0] = 1;
        data[2, 0, 0] = 1;
        data[3, 0, 0] = 1;

        chunks = new chunkScript[Mathf.FloorToInt(worldX / chunkSize), Mathf.FloorToInt(worldY / chunkSize), Mathf.FloorToInt(worldZ / chunkSize)];

        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int y = 0; y < chunks.GetLength(1); y++)
            {
                for (int z = 0; z < chunks.GetLength(2); z++)
                {
                    //Temporaeres Chunk generieren
                    GameObject newChunk = Instantiate(prefabChunk, new Vector3(x * chunkSize - 0.5f,
                     y * chunkSize + 0.5f, z * chunkSize - 0.5f), new Quaternion(0, 0, 0, 0)) as GameObject;

                    //Generiertes Chunk an die richtige Stelle speichern 
                    chunks[x, y, z] = newChunk.GetComponent<chunkScript>();
                    chunks[x, y, z].setMapGO(gameObject);
                    chunks[x, y, z].setChunkSize(chunkSize);
                    chunks[x, y, z].setChunkX(x * chunkSize);
                    chunks[x, y, z].setChunkY(y * chunkSize);
                    chunks[x, y, z].setChunkZ(z * chunkSize);
                }
            }
        }
    }

    public byte Block(int x, int y, int z)
    {
        if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0)
        {
            return (byte)0;
        }
        return data[x, y, z];
    }

    // Update is called once per frame
    void Update()
    {

    }
}