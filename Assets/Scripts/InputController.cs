using UnityEngine;
using Rewired;

public class InputController : MonoBehaviour
{
    public static InputController Instance {get; private set;}
    [SerializeField] private Player player;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        player = ReInput.players.GetPlayer(0);
    }

    public float GetAxis(int actionId)
    {
        return player.GetAxis(actionId);
    }

    public bool GetButtonDown(int actionId)
    {
        return player.GetButtonDown(actionId);
    }

    public static class InputAction
    {
        public const int MoveY = 1;
        public const int MoveX = 2;
        public const int Jump = 3;
        public const int MouseX = 4;
        public const int MouseY = 5;
    }
}
