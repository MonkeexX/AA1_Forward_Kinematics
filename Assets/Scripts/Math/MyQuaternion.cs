using System;
using UnityEngine;
using UnityEditorInternal;
using Unity.VisualScripting;

public struct MyQuaternion {

    public float w; //scalar part
    public float x, y, z; //vectorial part


    public MyQuaternion(float w, float x, float y, float z) {

        this.w = w;
        this.x = x;
        this.y = y;
        this.z = z;

    }

    public MyQuaternion(Vector3 axis, float angle) {


        float halfAngle = angle * 0.5f;
        float s = (float)Math.Sin(halfAngle);
        float c = (float)Math.Cos(halfAngle);

        this.w = c;
        this.x = axis.x * s;
        this.y = axis.y * s;
        this.z = axis.z * s;
    }


    public static MyQuaternion IdentityQuaternion() {
        return new MyQuaternion(1.0f, 0.0f, 0.0f, 0.0f);
    }

    public float Norm() {

        return (float)Math.Sqrt(w * w + x * x + y * y + z * z);

    }

    public MyQuaternion Normalize(){

        float norm = Norm();

        if (norm < 1e-8f) {

            throw new InvalidOperationException("Cannot normalize a zero quaternion");

        }

        return new MyQuaternion(w / norm, x / norm, y / norm, z / norm);

        }



    public MyQuaternion Conjugate() { 
    
        return new MyQuaternion(w, -x, -y, -z);   
    }

    public static MyQuaternion Multiply(MyQuaternion a, MyQuaternion b) {

        return new MyQuaternion(
            a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z,
            a.w * b.x + a.x*b.w + a.y*b.z - a.z*b.y,
            a.w * b.y - a.x*b.z + a.y*b.w + a.z*b.x,
            a.w*b.z + a.x*b.y - a.y*b.x +a.z*b.w
            );

    
    }

    public static float ScalarProduct(MyQuaternion a, MyQuaternion b) {
        return a.w * b.w + a.x * b.x + a.y * b.y + a.z * b.z;
    
    }

    public static MyQuaternion ProductByScalar(MyQuaternion a, float r) {

        return new MyQuaternion(r * a.w, r * a.x, r * a.y, r * a.z);   
    
    }
    public static MyQuaternion Sum(MyQuaternion a, MyQuaternion b) {

        return new MyQuaternion(a.w + b.w, a.x + b.x, a.y + b.y, a.z + b.z);
    }

}