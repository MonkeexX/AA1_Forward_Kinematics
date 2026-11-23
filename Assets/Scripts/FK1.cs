using System;
using System.Collections.Generic;
using UnityEngine;

public class FK1 : MonoBehaviour
{
    public List<Transform> joints;
    public List<LineRenderer> links;

    public List<Hinge2d> hinges = new List<Hinge2d>();
    public List<float> targetAngles = new List<float>();

    public List<MyQuaternion> initialRotations = new List<MyQuaternion>();
    public List<MyQuaternion> finalRotations = new List<MyQuaternion>();

    public List<float> previousAngles = new List<float>();

    Vector3 axis = new Vector3(0.0f, 0.0f, 1.0f);

    private float time = 0.0f;
    public float dt = 0.02f;

  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateLinks();
        InitializeLineRenderer(links, joints);

        for (int i = 0; i < links.Count; i++) {

            MyQuaternion q0 = MyQuaternion.IdentityQuaternion();

            float targetAngle = hinges[i].AngleConstraint(targetAngles[i]);
            MyQuaternion q1 = new MyQuaternion(axis, targetAngle);

            initialRotations.Add(q0);
            finalRotations.Add(q1);

            previousAngles.Add(0f);
        
        
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 1.0f) {

            for (int i = 0; i < hinges.Count; i++)
            {


                MyQuaternion q = Interpolation.SLERP(initialRotations[i], finalRotations[i], time);

                float currentAngle = 2.0f * Mathf.Atan2(q.z, q.w);

                float deltaAngle = currentAngle - previousAngles[i];
                FK.ApplyFK(joints, axis, deltaAngle, i);


                previousAngles[i] = currentAngle;
                
            }
            UpdateVisualLinks();
            time = Math.Min(time + dt, 1.0f);

        }

    }
   
    void CreateLinks() { 
        
        links = new List<LineRenderer>();
        for (int index = 0; index < joints.Count - 1; index++) {
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

    void UpdateVisualLinks()
    {
        for (int index = 0; index < joints.Count - 1; index++)
        {

            links[index].SetPosition(0, joints[index].position);
            links[index].SetPosition(1, joints[index + 1].position);

        }
     
    
    }


}
