using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#region Struct

[System.Serializable]
public class PieceDataReader
{
    public PieceJson[] pieceInfos;
    public static PieceDataReader CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PieceDataReader>(jsonString);
    }
}

[System.Serializable]
public class PieceJson
{
    public int[] BlocksX;
    public int[] BlocksY;
}

#endregion

public class Spawner : MonoBehaviour {
	public static Spawner instance = null;

    #region Private Variables

    // The list for piece patterns
    [SerializeField]
	private List<Piece> m_PieceList = new List<Piece>();
    // Block gameObject that forms a piece
    [SerializeField]
    private GameObject m_block = null;
    // The place holder that holds blocks after they get created from data
    [SerializeField]
    private Piece m_pieceObject = null;
    // Piece data that will be from json
    private PieceDataReader m_piecesInfo = null;

    #endregion

    #region Unity APIs

    void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		} 
		else if (instance != this) 
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (this);

        string filePath = Path.Combine(Application.dataPath, Path.Combine("Data", "Pieces.json"));

        // If json file exists, populate pieces' Info
        if (File.Exists(filePath))
        {
            string jsonString = File.ReadAllText(Path.Combine(Application.dataPath,
            Path.Combine("Data", "Pieces.json")));
            m_piecesInfo = PieceDataReader.CreateFromJSON(jsonString);
        }
        else
        {
            Debug.LogError("Spawner : Pieces json file doesn't exist at Data folder!");
        }

        Debug.Assert(m_block != null, "Spawner : m_block is null!");
        Debug.Assert(m_pieceObject != null, "Spawner  m_pieceObject is null");
        Debug.Assert(m_piecesInfo != null, "Spawner : m_piecesInfo is null!");
        Debug.Assert(m_piecesInfo.pieceInfos != null, "Spawner : m_piecesInfo.pieces is null!");
    }

    #endregion

    #region Public Interface

    /// <summary>
    /// Create pieces based on Json Data
    /// </summary>
    public void CreatePiecePatterns()
	{
        int pieceCount = m_piecesInfo.pieceInfos.Length;

        for (int i = 0; i < pieceCount; i++)
        {
            // Create the piece object
            Piece pieceParent = (Piece)Instantiate(m_pieceObject, new Vector3(-1000f, 1000f, 0), Quaternion.identity);
            pieceParent.gameObject.name = "Piece (" + i + ")";

            // Build the pattern based on block data from piece json file
            int blockCount = m_piecesInfo.pieceInfos[i].BlocksX.Length;

            // Has to be even number of x values to y values for block data
            Debug.Assert(blockCount == m_piecesInfo.pieceInfos[i].BlocksY.Length,
                "Spawner : Length of BLocksX differes from BlocksY");

            for (int j = 0; j < blockCount; j++)
            {
                GameObject block = Instantiate(m_block);
                block.transform.parent = pieceParent.transform;

                float x = (1f * m_piecesInfo.pieceInfos[i].BlocksX[j]);
                float y = (1f * m_piecesInfo.pieceInfos[i].BlocksY[j]);

                block.transform.localPosition = new Vector3(x, y , 0f);
            }

            // Add the pattern to the piece pattern list
            m_PieceList.Add(pieceParent);
        }
    }

    /// <summary>
    /// Randomly select a piece pattern and instantiate it
    /// </summary>
    public void SpawnNewPiece()
    {
        int count = m_PieceList.Count;

        if (count == 0)
        {
            Debug.LogError("There is no Piece Data!");
            return;
        }

        int r = Random.Range(0, count);         
        int x = (int)Mathf.Round(Board.instance.GetBoardWidth() / 2f);
        int y = (int)Mathf.Round(Board.instance.GetBoardHeight());

        Piece piece = (Piece)Instantiate(m_PieceList[r], new Vector3(x, y, 0f), Quaternion.identity);
        piece.enabled = true;
    }

    #endregion
}
