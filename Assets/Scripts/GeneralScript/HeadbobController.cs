using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbobController : MonoBehaviour
{
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private CharacterController CharacterController;


    [SerializeField] private float RunBobSpeed = 14f;
    [SerializeField] private float RunBobAmount = 0.05f;

    [SerializeField] private float DashBobSpeed = 14f;
    [SerializeField] private float DashBobAmount = 0.5f;

    [SerializeField] private float DiveBobSpeed = 14f;
    [SerializeField] private float DiveBobAmount = 0.5f;

    private float defaultYPos = 0;
    private float timer = 0;

    void Awake()
    {
        defaultYPos = PlayerCamera.transform.localPosition.y;
    }


    public void HandeHeadBob(Vector3 MoveDirection)
    {
        if (!CharacterController.isGrounded)
        { return; }

        if (Mathf.Abs(MoveDirection.x) > 0.1f || Mathf.Abs(MoveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (RunBobSpeed);
            PlayerCamera.transform.localPosition = new Vector3(PlayerCamera.transform.localPosition.x,
            defaultYPos + Mathf.Sin(timer) * RunBobAmount, PlayerCamera.transform.localPosition.z);
        }
    }
}
