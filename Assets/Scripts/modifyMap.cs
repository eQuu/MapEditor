using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class modifyMap : MonoBehaviour
{
    mapScript mapScript;
    private byte blockType = 1;

    public void changeBlockType(Dropdown myDropdown)
    {
        Debug.Log((byte)myDropdown.value);
        blockType = (byte)(myDropdown.value + 1);
    }

    private void removeBlockCursor()
    {
        //Entfernt den Block auf den der Cursor zeigt
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            removeBlockAt(hit);
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.red, 5);
        }
    }

    private void removeBlockAt(RaycastHit hit)
    {
        //Entfernt den Block an Position hit
        Vector3 position = hit.point;
        position += (hit.normal * -0.5f);

        setBlockAt(position, 0);
    }

    private void addBlockCursor(byte block)
    {
        //Fuegt einen Block am Cursor hinzu
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            addBlockAt(hit, block);
            Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 5);
        }
    }

    private void addBlockAt(RaycastHit hit, byte block)
    {
        //Fuegt einen Block an einer Position hinzu
        Vector3 position = hit.point;
        position += (hit.normal * 0.5f);

        setBlockAt(position, block);
    }

    private void setBlockAt(Vector3 position, byte block)
    {
        //Bereitet die Position im Raum vor und setzt dann den Block
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        int z = Mathf.RoundToInt(position.z);

        setBlockAt(x, y, z, block);
    }

    private void setBlockAt(int x, int y, int z, byte block)
    {
        //Setzt einen Block im Raum und updatet den betroffenen Chunk
        try
        {
            mapScript.data[x, y, z] = block;
            updateChunkAt(x, y, z);
        }
        catch (System.IndexOutOfRangeException e)
        {

        }

    }

    private void updateChunkAt(int x, int y, int z)
    {
        //Updatet den Chunk, der den Block auf x, y und z enthaelt
        int updateX = Mathf.FloorToInt(x / mapScript.getChunkSize());
        int updateY = Mathf.FloorToInt(y / mapScript.getChunkSize());
        int updateZ = Mathf.FloorToInt(z / mapScript.getChunkSize());

        mapScript.chunks[updateX, updateY, updateZ].setUpdate(true);

        //Liegt der Block am Rand des Chunks, dann muss auch der Nachbarchunk geupdatet werden
        if (x - (mapScript.getChunkSize() * updateX) == 0 && updateX != 0)
        {
            mapScript.chunks[updateX - 1, updateY, updateZ].setUpdate(true);
        }

        if (x - (mapScript.getChunkSize() * updateX) == 15 && updateX != mapScript.chunks.GetLength(0) - 1)
        {
            mapScript.chunks[updateX + 1, updateY, updateZ].setUpdate(true);
        }

        if (y - (mapScript.getChunkSize() * updateY) == 0 && updateY != 0)
        {
            mapScript.chunks[updateX, updateY - 1, updateZ].setUpdate(true);
        }

        if (y - (mapScript.getChunkSize() * updateY) == 15 && updateY != mapScript.chunks.GetLength(1) - 1)
        {
            mapScript.chunks[updateX, updateY + 1, updateZ].setUpdate(true);
        }

        if (z - (mapScript.getChunkSize() * updateZ) == 0 && updateZ != 0)
        {
            mapScript.chunks[updateX, updateY, updateZ - 1].setUpdate(true);
        }

        if (z - (mapScript.getChunkSize() * updateZ) == 15 && updateZ != mapScript.chunks.GetLength(2) - 1)
        {
            mapScript.chunks[updateX, updateY, updateZ + 1].setUpdate(true);
        }
    }

    // Use this for initialization
    void Start()
    {
        mapScript = gameObject.GetComponent<mapScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            removeBlockCursor();
        }

        if (Input.GetMouseButtonDown(1))
        {
            addBlockCursor(blockType);
        }
    }
}
