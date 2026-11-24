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

    private float[] jointAngles = { 0f, 0f, 0f };

    private float[] minAngles = { -90f, -120f, -135f };
    private float[] maxAngles = { 90f, 120f, 135f };

    private float[] targetAngles = { 0f, 0f, 0f };

    private float interpolationSpeed = 3f;
    private float[] interpolationProgress = { 1f, 1f, 1f };

    private float[] startAngles = { 0f, 0f, 0f };
    private float[] currentAngles = { 0f, 0f, 0f };

    private void Update()
    {
        HandleInput();
        UpdateJointRotation();
        UpdateArmVisuals();
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            RotateJoint(-rotationStep * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            RotateJoint(rotationStep * Time.deltaTime);
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
        float newAngle = targetAngles[activeJoint] + angle;

        newAngle = Mathf.Clamp(
            newAngle,
            minAngles[activeJoint],
            maxAngles[activeJoint]
            );

        startAngles[activeJoint] = currentAngles[activeJoint];
        targetAngles[activeJoint] = newAngle;
        interpolationProgress[activeJoint] = 0f;
    }

    void UpdateJointRotation()
    {
        for (int i = 0; i < joints.Count - 1; i++)
        {
            if (interpolationProgress[i] >= 1f)
                continue;

            interpolationProgress[i] += Time.deltaTime * interpolationSpeed;

            float t = Mathf.Clamp01(interpolationProgress[i]);

            // Rotación inicial
            MyQuaternion startRotation = new MyQuaternion(
                rotationAxis.normalized,
                startAngles[i] * Mathf.Deg2Rad
            );

            // Rotación final
            MyQuaternion targetRotation = new MyQuaternion(
                rotationAxis.normalized,
                targetAngles[i] * Mathf.Deg2Rad
            );

            // Interpolación entre ambas rotaciones
            MyQuaternion interpolatedRotation = Interpolation.SLERP(
                startRotation,
                targetRotation,
                t
            );

            // Recuperar el ángulo interpolado
            Vector3 axis = rotationAxis.normalized;

            Vector3 vectorPart = new Vector3(
                interpolatedRotation.x,
                interpolatedRotation.y,
                interpolatedRotation.z
            );

            float angle = 2f * Mathf.Atan2(
                Vector3.Dot(vectorPart, axis),
                interpolatedRotation.w
            ) * Mathf.Rad2Deg;

            // Aplicar únicamente el cambio de ángulo
            float appliedAngle = angle - currentAngles[i];

            currentAngles[i] = angle;

            FK.ApplyFK(joints, rotationAxis, appliedAngle, i);
        }
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
