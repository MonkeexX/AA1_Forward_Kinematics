using System.Collections.Generic;
using UnityEngine;

public class walking : MonoBehaviour
{

    public List<Transform> leg;

    public List<LineRenderer> bones = new List<LineRenderer>();

    public float walkSpeed;

    public float hipAmplitude;
    public float kneeAmplitude;
    public float ankleAmplitude;

    private Vector3 rotationAxis = new Vector3(0.0f, 0.0f, 1.0f);


    private float previousHip = 0.0f;
    private float previousKnee = 0.0f;
    private float previousAnkle = 0.0f;

    private float time = 0.0f;
    public float dt = 0.02f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateLinks(bones, leg);
        InitializeLineRenderer(bones, leg);
    }

    // Update is called once per frame
    void Update()
    {
        float phase = time * walkSpeed;
        float hipRotation = (float)Mathf.Sin(phase) * hipAmplitude;
        float kneeRotation = (float)Mathf.Sin(-(phase + Mathf.PI / 4)) * kneeAmplitude;
        float ankleRotation = (float)Mathf.Sin(-(phase + Mathf.PI / 2)) * ankleAmplitude;

        FK.ApplyFK(leg, rotationAxis, hipRotation - previousHip, 0);
        FK.ApplyFK(leg, rotationAxis, kneeRotation - previousKnee, 1);
        FK.ApplyFK(leg, rotationAxis, ankleRotation - previousAnkle, 2);


        previousHip = hipRotation;
        previousKnee= kneeRotation;
        previousAnkle = ankleRotation;

        UpdateVisualLinks(leg, bones);

        time += dt;
        
    }

    void CreateLinks(List<LineRenderer> links, List<Transform> joints)
    {

        links.Clear();

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

            links[index].material = new Material(Shader.Find("Sprites/Default"));

            links[index].startColor = Color.white;
            links[index].endColor = Color.white;

        }
    }

    void UpdateVisualLinks(List<Transform> joints, List<LineRenderer> links)
    {
        for (int index = 0; index < joints.Count - 1; index++)
        {

            links[index].SetPosition(0, joints[index].position);
            links[index].SetPosition(1, joints[index + 1].position);

        }


    }
}
