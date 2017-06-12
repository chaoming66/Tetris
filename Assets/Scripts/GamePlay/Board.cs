using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
	public static Board instance = null;

    #region Private Variables

    // Board's width and height
    private int m_boardWidth = 10;
    private int m_boardHeight = 20;

    [SerializeField]
    private GameObject m_wallBrick = null;

    // Two dimensional array that represents the board
    protected Transform[,] m_board = null;

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

        Debug.Assert(m_wallBrick != null, "Board : m_wallBrick needs to be assigned!");
	}

    #endregion

    #region Public Interface

    /// <summary>
    /// Create a new board
    /// </summary>
    public void InitializeBoard()
    {
        // Clean the old board
        if (m_board != null)
        {
            for (int i = 0; i < m_board.GetLength(0); i++)
            {
                for (int j = 0; j < m_board.GetLength(1); j++)
                {
                    Destroy(m_board[i, j].gameObject);
                    m_board[i, j] = null;
                }
            }
        }

        // Create a new one based on given width and height
        m_board = new Transform[m_boardWidth, m_boardHeight];

        // Create borders
        for (int i = 0; i < m_boardHeight - 1; i++)
        {
            // Build Left Wall
			// Build Right Wall
            Instantiate(m_wallBrick, new Vector3(-1f, i, 0), Quaternion.identity);
            Instantiate(m_wallBrick, new Vector3(m_boardWidth, i, 0), Quaternion.identity);
        }
    }
    
    /// <summary>
    /// Check if piece's next move is valid or not
    /// </summary>
    /// <param name="piece"></param>
    /// <returns></returns>
    public bool IsPieceMovementValid(Transform piece)
    {
        for (int i = 0; i < piece.childCount; i++)
        {
            int x = (int)Mathf.Round(piece.GetChild(i).position.x);
            int y = (int)Mathf.Round(piece.GetChild(i).position.y);

            // Check if the piece is within the boarders
            if ( x < 0 || x > m_boardWidth - 1 || y < 0)
            {             
                return false;
            }

            // The piece hits something
            if (y < m_boardHeight)
            {
                if (m_board[x, y] != null)
                {
                    // check if the block belongs to the piece
                    if (m_board[x, y].parent != piece)
                    {
                        return false;                        
                    }
                }
            }
        }
        return true;
    }

    /// <summary>
    /// The function that updates the data on each coordinate on the board.
    /// Only gets called when the piece is in place
    /// </summary>
    /// <param name="piece"></param>
    public void UpdateGridOnBoard(Transform piece)
    {        
        // Remove blocks that are from this piece 
        for (int i = 0; i < m_boardWidth; i++)
        {
            for (int j = 0; j < m_boardHeight; j++)
            {
                if (m_board[i, j] != null)
                {
                    if (m_board[i, j].parent == piece)
                    {
                        m_board[i, j] = null;
                    }
                }
            }
        }

        // Get blocks' current positions and update the board data structure 
        for (int i = 0; i < piece.childCount; i++)
        {
            int x = (int)Mathf.Round(piece.GetChild(i).position.x);
            int y = (int)Mathf.Round(piece.GetChild(i).position.y);

            if (y < m_boardHeight)
            {
                m_board[x, y] = piece.GetChild(i);
            }
        }

    }

    /// <summary>
    /// Check if a row is full of blocks at a certain height
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    public bool IsRowFull(int height)
    {
        for (int i = 0; i < m_boardWidth; i++)
        {
            if (m_board[i,height] == null)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Delete all the full rows on the board
    /// </summary>
    public void DeleteFullRows()
    {
        // Loop through all rows and delete rows that are full
        for (int i = 0; i < m_boardHeight; i++)
        {
            if (IsRowFull(i))
            {
                DeleteRow(i);
                LevelManager.instance.AddScore(1);

                // After delation, move rows above downward
                for(int j = i; j < m_boardHeight - 1; j++)
                {
                    MoveRowDown(j);
                }
                i--;
            }
        }

    }

    /// <summary>
    /// Delete a speicifc row based on height
    /// </summary>
    /// <param name="height"></param>
    public void DeleteRow(int height)
    {
        for (int i = 0; i < m_boardWidth; i++)
        {
            Destroy(m_board[i, height].gameObject);
            m_board[i, height] = null;
        }
    }

    /// <summary>
    /// Move a row downward that is at the a certain height
    /// </summary>
    /// <param name="height"></param>
    public void MoveRowDown(int height)
    {
        for (int i = 0; i < m_boardWidth; i++)
        {
            if (height != 0)
            {
                if (m_board[i, height] != null)
                {
                    m_board[i, height - 1] = m_board[i, height];
                    m_board[i, height] = null;
                    m_board[i, height - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    /// <summary>
    /// Check the first row of the board and see if it's empty
    /// </summary>
    /// <returns></returns>
    public bool IsOverFlow()
    {
        for (int i = 0; i < m_boardWidth; i++)
        {
            if (m_board[i, m_boardHeight - 1] != null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Get the board's width
    /// </summary>
    /// <returns></returns>
    public int GetBoardWidth()
    {
        return m_boardWidth;
    }

    /// <summary>
    /// Get the board's height
    /// </summary>
    /// <returns></returns>
    public int GetBoardHeight()
    {
        return m_boardHeight;
    }

    #endregion
}
