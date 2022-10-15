using UnityEngine;

public class VehicleEditor : MonoBehaviour
{
    // this will be replaced with block select in future steps
    public GameObject selectedObject;

    [SerializeField]
    private GameObject editorCell;
    [SerializeField]
    private int editorWidth;
    [SerializeField]
    private int editorHeight;

    private GameObject[,] editorGrid;
    private GameObject[,] placedBlocksGrid;

    private Vector2Int focusedCellPosition;
    private EditorProcessor editorProcessor;


    void Start()
    {
        GenerateEditorGrid(new Vector2Int(editorWidth, editorHeight));
        editorProcessor = gameObject.AddComponent<EditorProcessor>();
    }

    /// <summary>
    /// Setups Editor grid and populates it with cells
    /// </summary>
    /// <param name="size">Size of editorGrid in cells</param>
    private void GenerateEditorGrid(Vector2Int size)
    {
        InitializeGrids(size);

        PopulateEditorGridWithCells();
    }

    /// <summary>
    /// Creates editorGrid and placedBlocksGrid array using given size
    /// </summary>
    /// <param name="size">Size of editorGrid in cells</param>
    private void InitializeGrids(Vector2Int size)
    {
        placedBlocksGrid = new GameObject[size.x, size.y];
        editorGrid = new GameObject[size.x, size.y];
    }

    /// <summary>
    /// Creates and places EditorCell-s objects in editorGrid
    /// </summary>
    private void PopulateEditorGridWithCells()
    {
        VehicleEditor vehicleEditor = gameObject.GetComponent<VehicleEditor>();
        Vector2 startPosition = gameObject.transform.position;
        Vector3 currentPosition = startPosition;
        float spawnStep = editorCell.GetComponent<BoxCollider2D>().size.x;

        for (int y = 0; y < editorGrid.GetLength(1); y++)
        {
            for (int x = 0; x < editorGrid.GetLength(0); x++)
            {
                CreateEditorCell(x, y, currentPosition, vehicleEditor);

                currentPosition.x += spawnStep;
            }

            currentPosition.x = startPosition.x;
            currentPosition.y += spawnStep;
        }
    }

    /// <summary>
    /// Instantiates EditorCell using provided params
    /// </summary>
    /// <param name="xGrid">position in editor grid array</param>
    /// <param name="yGrid">position in editor grid array</param>
    /// <param name="position">position on the scene</param>
    /// <param name="vehicleEditor"></param>
    private void CreateEditorCell(int xGrid, int yGrid, Vector3 position, VehicleEditor vehicleEditor)
    {
        editorGrid[xGrid, yGrid] = Instantiate(editorCell, position, Quaternion.identity);

        editorGrid[xGrid, yGrid].transform.parent = transform;
        editorGrid[xGrid, yGrid].GetComponent<EditorCell>().Initialize(new Vector2Int(xGrid, yGrid), vehicleEditor);
    }

    /// <summary>
    /// Action triggered on mouse enter on EditorCell. See <see cref="EditorCell"/>
    /// </summary>
    /// <param name="positionIntEditorGrid">position of cell in editor grid array</param>
    public void EditorCellMouseEnter(Vector2Int positionIntEditorGrid)
    {
        selectedObject.transform.position = editorGrid[positionIntEditorGrid.x, positionIntEditorGrid.y].transform.position;
        focusedCellPosition = positionIntEditorGrid;
    }

    /// <summary>
    /// Actions triggered on mouse down on EditorCell. See <see cref="EditorCell"/>
    /// </summary>
    /// <param name="positionIntEditorGrid">position of cell in editor grid array</param>
    public void EditorCellMouseDown()
    {
        if (placedBlocksGrid[focusedCellPosition.x, focusedCellPosition.y] == null)
        {
            AddBlockToFocusedCell();
        }
        else
        {
            RotateFocusedBlock();
        }
    }

    /// <summary>
    /// Instantiates currently selected block on posittion of currently focused cell. TODO should be probably refactored
    /// </summary>
    private void AddBlockToFocusedCell()
    {
        Vector2 position = editorGrid[focusedCellPosition.x, focusedCellPosition.y].transform.position;
        placedBlocksGrid[focusedCellPosition.x, focusedCellPosition.y] = Instantiate(selectedObject, position, Quaternion.identity);
    }

    /// <summary>
    /// Rotate currently focused block. TODO should be probably refactored
    /// </summary>
    private void RotateFocusedBlock()
    {
        placedBlocksGrid[focusedCellPosition.x, focusedCellPosition.y].GetComponent<Block>().Rotate();
    }

    /// <summary>
    /// Processes editor grid and generates vehicle with its data
    /// </summary>
    public void ProcessEditorGrid()
    {
        editorProcessor.ProcessEditorGrid(placedBlocksGrid);
    }

    /// <summary>
    /// Deactivates (hides) editor (and all blocks in placedBlocksGrid)
    /// </summary>
    public void DeactivateEditor()
    {
        // TODO this can be removed if placedBlocksGrid elements would be automaticly child elements of grid object
        foreach (GameObject block in placedBlocksGrid)
        {
            if (block != null) block.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Active (shows) editor (and all block in placedBlockGrid)
    /// </summary>
    public void ActivateEditor()
    {
        // TODO this also can be removed if placedBlocksGrid elements would be automaticly child elements of grid object
        foreach (GameObject block in placedBlocksGrid)
        {
            if (block != null) block.SetActive(true);
        }
        gameObject.SetActive(true);
    }
}
