using System.Collections.Generic;
using UnityEngine;

public class MyRobotController : MonoBehaviour
{
    [Header("Robot Joints")]
    public List<Transform> joints;

    [Header("Arm Visuals")]
    public List<LineRenderer> armLines;

    [Header("Movement")]
    public float rotationStep = 10f;
    public Vector3 rotationAxis = Vector3.forward;

    private int activeJoint = 0;

    private void Update()
    {
        HandleInput();
        UpdateArmVisuals();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateJoint(-rotationStep);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateJoint(rotationStep);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            activeJoint = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            activeJoint = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            activeJoint = 2;
    }

    void RotateJoint(float angle)
    {
        FK.ApplyFK(joints, rotationAxis, angle, activeJoint);
    }

    void UpdateArmVisuals()
    {
        for (int i = 0; i < armLines.Count; i++)
        {
            armLines[i].SetPosition(0, joints[i].position);
            armLines[i].SetPosition(1, joints[i + 1].position);
        }
    }
}
