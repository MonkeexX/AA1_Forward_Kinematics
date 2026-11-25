
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
    public Vector3 rotationAxisX = Vector3.right;
    public Vector3 rotationAxisY = Vector3.up;

    private int activeJoint = 0;

    private float[] minAnglesX = { -90f, -120f, -135f };
    private float[] maxAnglesX = { 90f, 120f, 135f };

    private float[] minAnglesY = { -90f, -120f, -135f };
    private float[] maxAnglesY = { 90f, 120f, 135f };

    // Angulos X
    private float[] targetAnglesX = { 0f, 0f, 0f };
    private float[] startAnglesX = { 0f, 0f, 0f };
    private float[] currentAnglesX = { 0f, 0f, 0f };

    // Angulos Y
    private float[] targetAnglesY = { 0f, 0f, 0f };
    private float[] startAnglesY = { 0f, 0f, 0f };
    private float[] currentAnglesY = { 0f, 0f, 0f };

    private float interpolationSpeed = 3f;

    private float[] interpolationProgressX = { 1f, 1f, 1f };
    private float[] interpolationProgressY = { 1f, 1f, 1f };

    private void Update()
    {
        HandleInput();
        UpdateJointRotation();
        UpdateArmVisuals();
    }

    void HandleInput()
    {
        // ROTACION X
        if (Input.GetKey(KeyCode.Q))
            RotateJoint(-rotationStep * Time.deltaTime, true);

        else if (Input.GetKey(KeyCode.E))
            RotateJoint(rotationStep * Time.deltaTime, true);

        // ROTACION Y
        if (Input.GetKey(KeyCode.R))
            RotateJoint(-rotationStep * Time.deltaTime, false);

        else if (Input.GetKey(KeyCode.F))
            RotateJoint(rotationStep * Time.deltaTime, false);

        // Seleccion de articulacion
        if (Input.GetKeyDown(KeyCode.Alpha1))
            activeJoint = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            activeJoint = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            activeJoint = 2;
    }

    void RotateJoint(float angle, bool axisX)
    {
        if (activeJoint >= joints.Count - 1)
            return;

        if (axisX)
        {
            float newAngle = targetAnglesX[activeJoint] + angle;

            newAngle = Mathf.Clamp(
                newAngle,
                minAnglesX[activeJoint],
                maxAnglesX[activeJoint]
            );

            startAnglesX[activeJoint] = currentAnglesX[activeJoint];
            targetAnglesX[activeJoint] = newAngle;

            interpolationProgressX[activeJoint] = 0f;
        }
        else
        {
            float newAngle = targetAnglesY[activeJoint] + angle;

            newAngle = Mathf.Clamp(
                newAngle,
                minAnglesY[activeJoint],
                maxAnglesY[activeJoint]
            );

            startAnglesY[activeJoint] = currentAnglesY[activeJoint];
            targetAnglesY[activeJoint] = newAngle;

            interpolationProgressY[activeJoint] = 0f;
        }
    }

    void UpdateJointRotation()
    {
        for (int i = 0; i < joints.Count - 1; i++)
        {
            // =========================
            // ROTACION X
            // =========================

            if (interpolationProgressX[i] < 1f)
            {
                interpolationProgressX[i] +=
                    Time.deltaTime * interpolationSpeed;

                float t = Mathf.Clamp01(interpolationProgressX[i]);

                MyQuaternion startRotationX = new MyQuaternion(
                    rotationAxisX.normalized,
                    startAnglesX[i] * Mathf.Deg2Rad
                );

                MyQuaternion targetRotationX = new MyQuaternion(
                    rotationAxisX.normalized,
                    targetAnglesX[i] * Mathf.Deg2Rad
                );

                MyQuaternion interpolatedRotationX = Interpolation.SLERP(
                    startRotationX,
                    targetRotationX,
                    t
                );

                Vector3 vectorPartX = new Vector3(
                    interpolatedRotationX.x,
                    interpolatedRotationX.y,
                    interpolatedRotationX.z
                );

                float angleX = 2f * Mathf.Atan2(
                    Vector3.Dot(vectorPartX, rotationAxisX.normalized),
                    interpolatedRotationX.w
                ) * Mathf.Rad2Deg;

                float appliedAngleX = angleX - currentAnglesX[i];

                currentAnglesX[i] = angleX;

                FK.ApplyFK(
                    joints,
                    rotationAxisX,
                    appliedAngleX,
                    i
                );
            }

            // =========================
            // ROTACION Y
            // =========================

            if (interpolationProgressY[i] < 1f)
            {
                interpolationProgressY[i] +=
                    Time.deltaTime * interpolationSpeed;

                float t = Mathf.Clamp01(interpolationProgressY[i]);

                MyQuaternion startRotationY = new MyQuaternion(
                    rotationAxisY.normalized,
                    startAnglesY[i] * Mathf.Deg2Rad
                );

                MyQuaternion targetRotationY = new MyQuaternion(
                    rotationAxisY.normalized,
                    targetAnglesY[i] * Mathf.Deg2Rad
                );

                MyQuaternion interpolatedRotationY = Interpolation.SLERP(
                    startRotationY,
                    targetRotationY,
                    t
                );

                Vector3 vectorPartY = new Vector3(
                    interpolatedRotationY.x,
                    interpolatedRotationY.y,
                    interpolatedRotationY.z
                );

                float angleY = 2f * Mathf.Atan2(
                    Vector3.Dot(vectorPartY, rotationAxisY.normalized),
                    interpolatedRotationY.w
                ) * Mathf.Rad2Deg;

                float appliedAngleY = angleY - currentAnglesY[i];

                currentAnglesY[i] = angleY;

                FK.ApplyFK(
                    joints,
                    rotationAxisY,
                    appliedAngleY,
                    i
                );
            }
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

