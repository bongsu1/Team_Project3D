using UnityEngine;
using UnityEngine.InputSystem;

public class DeadUI : MonoBehaviour
{
    PlayerInput playerInput;

    private void OnEnable()
    {
        playerInput = FindObjectOfType<Player>().GetComponent<PlayerInput>();
        if (playerInput != null)
            playerInput.enabled = false;
    }

    private void OnDisable()
    {
        if (playerInput != null)
            playerInput.enabled = true;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
