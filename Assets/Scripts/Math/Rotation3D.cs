using UnityEngine;
using System;

public static class Rotation3D {

    public static Vector3 MatrixRot(Vector3 p, float angle, string axis) {

        float c = (float)Math.Cos(angle);
        float s = (float)Math.Sin(angle);


        if(axis == "z")
        {
            float newX = c * p.x - s * p.y;
            float newY = s * p.x + c * p.y;

            return new Vector3(newX, newY, p.z);
        }

        if (axis == "x") {

            float newY = c * p.y - s * p.z;
            float newZ = s* p.y + c * p.z;

            return new Vector3(p.x, newY, newZ);
        }

        if (axis == "y")
        {
            float newX = c * p.x + s * p.z;
            float newZ = -s * p.x + c * p.z;

            return new Vector3(newX, p.y, newZ);
        }

        throw new ArgumentException("Axis must be x, y or z");

    }


    public static Vector3 QuaternionRot(Vector3 p, float angle, Vector3 axis) {


        MyQuaternion q = new MyQuaternion(0, p.x, p.y, p.z);

        axis.Normalize();

        float c = (float)Math.Cos(angle * 0.5);
        float s = (float)Math.Sin(angle * 0.5);

        MyQuaternion alpha = new MyQuaternion(c, axis.x * s, axis.y * s, axis.z * s);
        MyQuaternion inverse_alpha = alpha.Conjugate();

        MyQuaternion result = MyQuaternion.Multiply(alpha, MyQuaternion.Multiply(q, inverse_alpha));

        return new Vector3(result.x, result.y, result.z);

    
    }


    public static Vector3 QuaternionRot(Vector3 p, MyQuaternion q) {

        q = q.Normalize();
        MyQuaternion p_quat = new MyQuaternion(0.0f, p.x, p.y, p.z);
        MyQuaternion inverse_q = q.Conjugate();

        MyQuaternion result = MyQuaternion.Multiply(q, MyQuaternion.Multiply(p_quat, inverse_q));

        return new Vector3(result.x, result.y, result.z);
    }




}