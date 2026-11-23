using System.Collections.Generic;
using UnityEngine;

public static class FK
{
    public static void ApplyFK(
        List<Transform> joints,
        Vector3 axis,
        float angle,
        int joint = 0 // index del pivote
        )
    {

        Vector3 pivot = joints[joint].position;

        for (int index = joint + 1; index < joints.Count; index++) {

            Vector3 pseudoLink = joints[index].position - pivot;
            Vector3 newPseudoLink = Rotation3D.QuaternionRot(pseudoLink, angle, axis);

            joints[index].position = pivot + newPseudoLink;                  

        }   
    
    }

}
