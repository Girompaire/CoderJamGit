using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFinish : MonoBehaviour
{
    [System.Serializable]
    public class Player
    {
        public PlayerController playerController;
        public Camera playerCamera;
    }

    public Player[] players;
    public float speedCamera;
    public Image barre;
    private Player playerWinner;
    public float cameraX;
    public float cameraW;
    private int idWinner;
    private bool isFinish;

    private void Update()
    {
        if (isFinish)
        {
            cameraX = Mathf.Lerp(cameraX, 0, Time.deltaTime * speedCamera);
            cameraW = Mathf.Lerp(cameraW, 1, Time.deltaTime * speedCamera);
            playerWinner.playerCamera.rect = new Rect(cameraX, playerWinner.playerCamera.rect.y, cameraW, playerWinner.playerCamera.rect.height);

            if (cameraW > 0.999)
            {
                
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            idWinner = collision.gameObject.GetComponent<PlayerController>().id;
            foreach (Player player in players)
            {
                player.playerController.stop = true;
                if (player.playerController.id == idWinner)
                {
                    playerWinner = player;
                    cameraX = playerWinner.playerCamera.rect.x;
                    cameraW = playerWinner.playerCamera.rect.width;
                    isFinish = true;
                }
            }
            Debug.Log("Player " + idWinner + " win!");
        }
    }
}
