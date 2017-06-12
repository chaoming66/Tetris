using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece : MonoBehaviour 
{
    #region Private Variables

    // Timer for moving piece down for the player
    private float m_timer;

    #endregion

    #region Unity APIs

    void Awake()
	{
        m_timer = LevelManager.instance.GetTimeInterval();
	}
	
	void Update () 
	{
        m_timer -= Time.deltaTime;

		// Logic that deals with input and movement
        Vector3 movement = Vector3.zero;
        bool movingDown = false;

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			movement = new Vector3(-1f, 0f, 0f);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
            movement = new Vector3(1f, 0f, 0f);
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
            movement = new Vector3(0f, 0f, -90f);			
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
            movingDown = true;
			movement = new Vector3(0f, -1f, 0f);			
		}

        // If detects player's input
        if (movement != Vector3.zero)
        {
            // Can't both move the piece and rotate it in one frame
            if (movement.z == 0f)
            {
                transform.position += movement;   
            }
            else
            {
                transform.Rotate(movement);   
            }
        }
        else
        {
            // If there is no input this frame, check the timer
            if (m_timer <= 0)
            {
                m_timer = LevelManager.instance.GetTimeInterval();
                movingDown = true;
                movement = new Vector3(0f, -1f, 0f);

                transform.position += movement;   
            }
        }

        if (movement != Vector3.zero)
        {
            // If the movement is not valid, move the piece or rotate back to where it was
            if (!Board.instance.IsPieceMovementValid(transform))
            {
                if (movement.z == 0f)
                {
                    transform.position -= movement;   
                }
                else
                {
                    movement = new Vector3(0f, 0f, 90f);    
                    transform.Rotate(movement);
                }

                // Piece stops either when it hits the bottom or collides with other pieces that are already on the board               
                if (movingDown)
                {                       
                    Board.instance.DeleteFullRows();
                    enabled = false; 
                    if (Board.instance.IsOverFlow())
                    {
                        LevelManager.instance.GameOver();
                    }
                    else
                    {
                        Spawner.instance.SpawnNewPiece();
                    }

                }
            }
            else
            {
                // Valid movement, update the newest position of this piece to the board
                Board.instance.UpdateGridOnBoard(transform);
            }
        }
	}

    #endregion
}
