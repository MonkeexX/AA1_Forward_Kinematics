using System;
using UnityEngine;

public static class Interpolation
{
    public static MyQuaternion SLERP(MyQuaternion q_0, MyQuaternion q_1, float t)
    {
        float dot = MyQuaternion.ScalarProduct(q_0, q_1);
        dot = Mathf.Clamp(dot, -1f, 1f);

        // Si los cuaterniones son prácticamente iguales
        if (dot > 0.9995f)
        {
            MyQuaternion result = MyQuaternion.Sum(
                MyQuaternion.ProductByScalar(q_0, 1f - t),
                MyQuaternion.ProductByScalar(q_1, t)
            );

            return result.Normalize();
        }

        float Omega = Mathf.Acos(dot);

        float s_t = Mathf.Sin(t * Omega);
        float tau = 1f - t;
        float s_tau = Mathf.Sin(tau * Omega);
        float s_omega = Mathf.Sin(Omega);

        MyQuaternion q_t = MyQuaternion.Sum(
            MyQuaternion.ProductByScalar(q_0, s_tau / s_omega),
            MyQuaternion.ProductByScalar(q_1, s_t / s_omega)
        );

        return q_t.Normalize();
    }

    public static MyQuaternion SLERP(MyQuaternion q_0, float angle, Vector3 axis, float time) { 
	 
		MyQuaternion finalQuaternion = new MyQuaternion(axis, angle);

		return SLERP(q_0, finalQuaternion, time);
	
	}
}
