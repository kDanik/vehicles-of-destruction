using UnityEngine;

/// <summary>
/// This class stores editor grid blocks and methods to change it
/// </summary>
public class EditorGrid : MonoBehaviour
{
    private GameObject[,] editorCellGrid;
    private GameObject[,] placedBlocksGrid;

    private GameObject editorCellPrefab;
    private GameObject blockWrapperPrefab;
    private EditorBlockSelection editorBlockSelection;

    public void InitiliazeEditorGrid(Vector2Int size, GameObject editorCellPrefab, GameObject blockWrapperPrefab, EditorBlockSelection editorBlockSelection)
    {
        this.editorCellPrefab = editorCellPrefab;
        this.blockWrapperPrefab = blockWrapperPrefab;
        this.editorBlockSelection = editorBlockSelection;

        InitializeGrids(size);
        PopulateEditorGridWithCells();
    }


    public void AddControlBlock(Vector2Int position, GameObject controlBlockPrefab)
    {
        AddNewBlock(position.x, position.y, controlBlockPrefab, false);
    }

    /// <summary>
    /// Deletes block at given editor grid position, removing it from the scene
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void DeleteBlock(int x, int y)
    {
        if (placedBlocksGrid[x, y] != null)
        {
            editorBlockSelection.OnRemoveBlockFromEditorGrid(GetBlockWithoutWrapper(x, y));

            Destroy(placedBlocksGrid[x, y]);
            placedBlocksGrid[x, y] = null;
        }
    }

    /// <summary>
    /// Check if block exists in editor grid at given position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>true if block present, otherwise false</returns>
    public bool IsBlockPresent(int x, int y)
    {
        return placedBlocksGrid[x, y] != null;
    }

    /// <summary>
    /// Checks if cell in editorGrid doesn't have block in it
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>true if no block present, otherwise false</returns>
    public bool IsCellEmpty(int x, int y)
    {
        return placedBlocksGrid[x, y] == null;
    }

    /// <summary>
    /// Get block at given position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>return GameObject of block(with wrapper) or null if no block present</returns>
    public GameObject GetBlock(int x, int y)
    {
        return placedBlocksGrid[x, y];
    }

    /// <summary>
    /// Returns gameobject of block on given position, without wrapper object (so only block object itself)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>gameobject of block or null if no block present</returns>
    public GameObject GetBlockWithoutWrapper(int x, int y)
    {
        if (placedBlocksGrid[x, y] == null) return null;

        return placedBlocksGrid[x, y].transform.GetChild(0).gameObject;
    }


    /// <summary>
    /// Attempts to move block from initialPosition to newPosition in grid
    /// </summary>
    /// <param name="initialPositionX"></param>
    /// <param name="initialPositionY"></param>
    /// <param name="newPositionX"></param>
    /// <param name="newPositionY"></param>
    /// <returns>true if moved successfully, otherwise false</returns>
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

    /// <summary>
    /// Deletes all blocks in editorGrid
    /// </summary>
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

    /// <summary>
    /// Instantiates and adds to grid new block (wrapped into block wrapper)
    /// </summary>
    /// <param name="notifyEditorBlockSelection">Optional parameter, that controls if block selection script should be notified / changed on block add</param>
    /// <returns>newly instantiated object or null if position is busy</returns>
    public GameObject AddNewBlock(int x, int y, GameObject blockPrefab, bool notifyEditorBlockSelection = true)
    {
        if (placedBlocksGrid[x, y] != null) return null;

        if (notifyEditorBlockSelection) editorBlockSelection.OnAddBlockToEditorGrid(blockPrefab);

        Vector2 position = editorCellGrid[x, y].transform.position;

        GameObject newBlock = Instantiate(blockPrefab, position, Quaternion.identity);
        GameObject newBlockWrapper = Instantiate(blockWrapperPrefab, position, Quaternion.identity);
        newBlock.transform.parent = newBlockWrapper.transform;
        newBlockWrapper.transform.parent = transform;

        placedBlocksGrid[x, y] = newBlockWrapper;

        return placedBlocksGrid[x, y];
    }

    /// <summary>
    /// Rotates block by 90 degrees to the left direction
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>returns true if rotated, otherwise false</returns>
    public bool RotateBlock(int x, int y)
    {
        if (placedBlocksGrid[x, y] == null) return false;

        GetBlockWithoutWrapper(x, y).GetComponent<Block>().Rotate();

        return true;
    }

    public int GetGridHeight()
    {
        return placedBlocksGrid.GetLength(1);
    }

    public int GetGridWidth()
    {
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
