using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorGrid : MonoBehaviour
{
    private GameObject[,] editorCellGrid;
    private GameObject[,] placedBlocksGrid;

    private GameObject editorCellPrefab;
    private GameObject blockWrapperPrefab;

    public void InitiliazeEditorGrid(Vector2Int size, GameObject editorCellPrefab, GameObject blockWrapperPrefab) {
        this.editorCellPrefab = editorCellPrefab;
        this.blockWrapperPrefab = blockWrapperPrefab;

        InitializeGrids(size);
        PopulateEditorGridWithCells();
    }

    public void DeleteBlock(int x, int y)
    {
        if (placedBlocksGrid[x, y] != null)
        {
            Destroy(placedBlocksGrid[x, y]);
            placedBlocksGrid[x, y] = null;
        }
    }

    public bool IsBlockPresent(int x, int y)
    {
        return placedBlocksGrid[x, y] != null;
    }

    public bool IsCellEmpty(int x, int y)
    {
        return placedBlocksGrid[x, y] == null;
    }

    public GameObject GetBlock(int x, int y)
    {
        return placedBlocksGrid[x, y];
    }

    public GameObject GetBlockWithoutWrapper(int x, int y)
    {
        if (placedBlocksGrid[x, y] == null) return null;

        return placedBlocksGrid[x, y].transform.GetChild(0).gameObject;
    }

    public bool MoveBlock(int initialPositionX, int initialPositionY, int newPositionX, int newPositionY)
    {
        if (placedBlocksGrid[initialPositionX, initialPositionY] == null) return false;

        if (placedBlocksGrid[newPositionX, newPositionY] != null) return false;


        placedBlocksGrid[newPositionX, newPositionY] = placedBlocksGrid[initialPositionX, initialPositionY];
        placedBlocksGrid[initialPositionX, initialPositionY] = null;

        Vector2 newGlobalPosition = editorCellGrid[newPositionX, newPositionY].transform.position;
        placedBlocksGrid[newPositionX, newPositionY].transform.position = newGlobalPosition;

        return true;
    }

    public void DeleteAllBlocks()
    {
         for (int y = 0; y < placedBlocksGrid.GetLength(1); y++)
        {
            for (int x = 0; x < placedBlocksGrid.GetLength(0); x++)
            {
                DeleteBlock(x, y);
            }
        }
    }

    public bool AddNewBlock(int x, int y, GameObject blockPrefab)
    {
        if (placedBlocksGrid[x, y] != null) return false;

        Vector2 position = editorCellGrid[x, y].transform.position;

        GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity);
        GameObject newBlockWrapper = Instantiate(blockWrapperPrefab, position, Quaternion.identity);
        newBlock.transform.parent = newBlockWrapper.transform;
        newBlockWrapper.transform.parent = transform;

        placedBlocksGrid[x, y] = newBlockWrapper;

        return true;
    }

    public bool RotateBlock(int x, int y)
    {
        if (placedBlocksGrid[x, y] == null) return false;

        GetBlockWithoutWrapper(x, y).GetComponent<Block>().Rotate();

        return true;
    }

    public int GetGridHeight() {
        return placedBlocksGrid.GetLength(1);
    }

    public int GetGridWidth() {
        return placedBlocksGrid.GetLength(0);
    }


    private void InitializeGrids(Vector2Int size)
    {
        placedBlocksGrid = new GameObject[size.x, size.y];
        editorCellGrid = new GameObject[size.x, size.y];
    }

    private void PopulateEditorGridWithCells()
    {
        VehicleEditor vehicleEditor = gameObject.GetComponent<VehicleEditor>();
        Vector3 currentPosition = gameObject.transform.position;
        float spawnStep = editorCellPrefab.GetComponent<BoxCollider2D>().size.x;


        for (int y = 0; y < editorCellGrid.GetLength(1); y++)
        {
            for (int x = 0; x < editorCellGrid.GetLength(0); x++)
            {
                CreateEditorCell(x, y, currentPosition, vehicleEditor);

                currentPosition.x += spawnStep;
            }

            currentPosition.x = gameObject.transform.position.x;
            currentPosition.y += spawnStep;
        }
    }

    private void CreateEditorCell(int xGrid, int yGrid, Vector3 position, VehicleEditor vehicleEditor)
    {
        editorCellGrid[xGrid, yGrid] = Instantiate(editorCellPrefab, position, Quaternion.identity);

        editorCellGrid[xGrid, yGrid].transform.parent = transform;
        editorCellGrid[xGrid, yGrid].GetComponent<EditorCell>().Initialize(new Vector2Int(xGrid, yGrid), vehicleEditor);
    }
}
