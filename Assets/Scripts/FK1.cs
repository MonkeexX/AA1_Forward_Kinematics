using System;
using System.Collections.Generic;
using UnityEngine;

public class FK1 : MonoBehaviour
{
    [Header("Robot Joints")]
    public List<Transform> joints;

    [Header("Joint Pivots")]
    public List<Transform> jointPivots;

    [Header("Visual Links")]
    public List<LineRenderer> links;

    [Header("Hinges")]
    public List<Hinge2d> hinges = new List<Hinge2d>();

    public List<float> targetAngles = new List<float>();

    [Header("Rotations")]
    public List<MyQuaternion> initialRotations = new List<MyQuaternion>();
    public List<MyQuaternion> finalRotations = new List<MyQuaternion>();

    public List<float> previousAngles = new List<float>();

    [Header("Settings")]
    public Vector3 axis = new Vector3(0f, 0f, 1f);

    private float time = 0f;
    public float dt = 0.02f;

    void Start()
    {
        CreateLinks();
        InitializeLineRenderer(links, joints);

        initialRotations.Clear();
        finalRotations.Clear();
        previousAngles.Clear();

        for (int i = 0; i < hinges.Count; i++)
        {
            MyQuaternion q0 = MyQuaternion.IdentityQuaternion();

            float constrainedAngle =
                hinges[i].AngleConstraint(targetAngles[i]);

            MyQuaternion q1 = new MyQuaternion(
                axis.normalized,
                constrainedAngle * Mathf.Deg2Rad
            );

            initialRotations.Add(q0);
            finalRotations.Add(q1);

            previousAngles.Add(0f);
        }
    }

    void Update()
    {
        if (time < 1f)
        {
            for (int i = 0; i < hinges.Count; i++)
            {
                MyQuaternion q = Interpolation.SLERP(
                    initialRotations[i],
                    finalRotations[i],
                    time
                );

                float currentAngle =
                    2f * Mathf.Atan2(q.z, q.w);

                currentAngle *= Mathf.Rad2Deg;

                float deltaAngle =
                    currentAngle - previousAngles[i];

                FK.ApplyFK(
                    joints,
                    axis,
                    deltaAngle,
                    i
                );

                previousAngles[i] = currentAngle;
            }

            UpdateVisualLinks();

            time = Mathf.Min(time + dt, 1f);
        }
    }

    void CreateLinks()
    {
        links = new List<LineRenderer>();

        for (int index = 0; index < joints.Count - 1; index++)
        {
            GameObject obj = new GameObject("Link_" + index);

            LineRenderer line = obj.AddComponent<LineRenderer>();

            links.Add(line);
        }
    }

    void InitializeLineRenderer(
        List<LineRenderer> links,
        List<Transform> joints
    )
    {
        for (int index = 0; index < joints.Count - 1; index++)
        {
            links[index].positionCount = 2;

            links[index].SetPosition(0, joints[index].position);
            links[index].SetPosition(1, joints[index + 1].position);

            links[index].startWidth = 0.1f;
            links[index].endWidth = 0.1f;

            links[index].material =
                new Material(Shader.Find("Sprites/Default"));

            links[index].startColor = Color.white;
            links[index].endColor = Color.white;
        }
    }

    void UpdateVisualLinks()
    {
        for (int index = 0; index < joints.Count - 1; index++)
        {
            links[index].SetPosition(0, joints[index].position);
            links[index].SetPosition(1, joints[index + 1].position);
        }
    }
}
