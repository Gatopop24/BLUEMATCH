using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [SerializeField] private Vector3 kickbackPosition = new Vector3(0f, 0f, -0.1f);
    [SerializeField] private Vector3 kickbackRotation = new Vector3(-5f, 0f, 0f);
    [SerializeField] private float kickbackReturnSpeed = 6f;
    private Vector3 originalWeaponPos;
    private Quaternion originalWeaponRot;
    private Vector3 targetKickPos;
    private Quaternion targetKickRot;
    private Vector3 aimTargetPos;

    private void Awake()
    {
        originalWeaponPos = transform.localPosition;
        originalWeaponRot = transform.localRotation;
        targetKickPos = originalWeaponPos;
        targetKickRot = originalWeaponRot;
        aimTargetPos = originalWeaponPos;
    }

    public void SetAimTargetPosition(Vector3 position)
    {
        aimTargetPos = position;
    }

    public Vector3 OriginalWeaponPosition
    {
        get { return originalWeaponPos; }
    }

    public void TriggerKickback()
    {
        targetKickPos = aimTargetPos + kickbackPosition;
        targetKickRot = originalWeaponRot * Quaternion.Euler(kickbackRotation);
    }

    public void ApplyWeaponRecoilVisual()
    {
        targetKickPos = Vector3.Lerp(targetKickPos, aimTargetPos, kickbackReturnSpeed * Time.deltaTime);
        targetKickRot = Quaternion.Lerp(targetKickRot, originalWeaponRot, kickbackReturnSpeed * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetKickPos, kickbackReturnSpeed * Time.deltaTime);
        transform.localRotation = targetKickRot;
    }
}
