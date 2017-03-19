using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chunkScript : MonoBehaviour
{
    private GameObject mapGO;
    private int chunkX;
    private int chunkY;
    private int chunkZ;
    private bool update;

    private mapScript map;

    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUVs = new List<Vector2>();

    private float tUnit = 0.25f;

    private Mesh myMesh;
    private MeshCollider myMeshCol;

    private int faceCount;
    private int chunkSize = 16;

    //Setter
    public void setMapGO(GameObject newGO)
    {
        this.mapGO = newGO;
    }

    public void setChunkSize(int newSize)
    {
        this.chunkSize = newSize;
    }

    public void setChunkX(int newX)
    {
        this.chunkX = newX;
    }

    public void setChunkY(int newY)
    {
        this.chunkY = newY;
    }

    public void setChunkZ(int newZ)
    {
        this.chunkZ = newZ;
    }
    public void setUpdate(bool newVal)
    {
        this.update = newVal;
    }


    // Use this for initialization
    void Start()
    {
        map = mapGO.GetComponent<mapScript>();
        myMesh = GetComponent<MeshFilter>().mesh;
        myMeshCol = GetComponent<MeshCollider>();

        genMesh();
    }

    private void CubeTop(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x, y, z));

        Vector2 texturePos = new Vector2(0, 0);

        texturePos = selectTexture(x, y, z, block);
        buildCube(texturePos);
    }

    private Vector2 selectTexture(int x, int y, int z, byte block)
    {
        //Texturen von unten links zaehlen!
        if (Block(x, y, z) == 1)
        {
            //Purple
            return new Vector2(0, 3);
        }
        else if (Block(x, y, z) == 2)
        {
            //Water
            return new Vector2(1, 3);
        }
        else if (Block(x, y, z) == 3)
        {
            //Blue
            return new Vector2(2, 3);
        }
        else if (Block(x, y, z) == 4)
        {
            //Dark
            return new Vector2(3, 3);
        }
        else if (Block(x, y, z) == 5)
        {
            //Green
            return new Vector2(0, 2);
        }
        return new Vector2(0, 0);
    }

    private void CubeBot(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos = new Vector2(0, 0);
        texturePos = selectTexture(x, y, z, block);

        buildCube(texturePos);
    }

    private void CubeNorth(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos = new Vector2(0, 0);
        texturePos = selectTexture(x, y, z, block);

        buildCube(texturePos);
    }

    private void CubeSouth(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));

        Vector2 texturePos = new Vector2(0, 0);
        texturePos = selectTexture(x, y, z, block);

        buildCube(texturePos);
    }

    private void CubeWest(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y - 1, z));

        Vector2 texturePos = new Vector2(0, 0);
        texturePos = selectTexture(x, y, z, block);

        buildCube(texturePos);
    }

    private void CubeEast(int x, int y, int z, byte block)
    {
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));

        Vector2 texturePos = new Vector2(0, 0);
        texturePos = selectTexture(x, y, z, block);

        buildCube(texturePos);
    }

    private void UpdateMesh()
    {
        myMesh.Clear();
        myMesh.vertices = newVertices.ToArray();
        myMesh.uv = newUVs.ToArray();
        myMesh.triangles = newTriangles.ToArray();
        ;
        myMesh.RecalculateNormals();

        myMeshCol.sharedMesh = null;
        myMeshCol.sharedMesh = myMesh;

        newVertices.Clear();
        newUVs.Clear();
        newTriangles.Clear();

        faceCount = 0;
    }

    private void buildCube(Vector2 texturePos)
    {
        newTriangles.Add(faceCount * 4);
        newTriangles.Add(faceCount * 4 + 1);
        newTriangles.Add(faceCount * 4 + 2);
        newTriangles.Add(faceCount * 4);
        newTriangles.Add(faceCount * 4 + 2);
        newTriangles.Add(faceCount * 4 + 3);

        newUVs.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
        newUVs.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
        newUVs.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
        newUVs.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y));

        faceCount++;
    }

    private void genMesh()
    {
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    //Wenn du kein Luft-Block bist...
                    if (Block(x, y, z) != 0)
                    {
                        //Und der Block ueber/unter/links von/rechts von dir Luft ist...
                        if (Block(x, y + 1, z) == 0)
                        {
                            //Dann zeichne deine Top-Seite
                            CubeTop(x, y, z, Block(x, y, z));
                        }
                        if (Block(x, y - 1, z) == 0)
                        {
                            //Dann zeichne deine Bot-Seite
                            CubeBot(x, y, z, Block(x, y, z));
                        }
                        if (Block(x + 1, y, z) == 0)
                        {
                            //Dann zeichne deine Ost-Seite
                            CubeEast(x, y, z, Block(x, y, z));
                        }
                        if (Block(x - 1, y, z) == 0)
                        {
                            //Dann zeichne deine West-Seite
                            CubeWest(x, y, z, Block(x, y, z));
                        }
                        if (Block(x, y, z + 1) == 0)
                        {
                            //Dann zeichne deine Nord-Seite
                            CubeNorth(x, y, z, Block(x, y, z));
                        }
                        if (Block(x, y, z - 1) == 0)
                        {
                            //Dann zeichne deine Sued-Seite
                            CubeSouth(x, y, z, Block(x, y, z));
                        }
                    }
                }
            }
        }
        UpdateMesh();
    }

    private byte Block(int x, int y, int z)
    {
        return map.Block(x + chunkX, y + chunkY, z + chunkZ);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        if (update)
        {
            genMesh();
            update = false;
        }
    }
}
