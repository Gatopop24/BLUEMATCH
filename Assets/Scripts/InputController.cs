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

    public bool GetButton(int actionId)
    {
        return player.GetButton(actionId);
    }

    public static class InputAction
    {
        public const int MoveY = 1;
        public const int MoveX = 2;
        public const int Jump = 3;
        public const int MouseX = 4;
        public const int MouseY = 5;
        public const int Sprint = 6;
        public const int RightHook = 7;
        public const int LeftHook = 8;
        public const int Fire = 9;
        public const int ChangeGun = 10;
        public const int Reload = 11;
    }
}
