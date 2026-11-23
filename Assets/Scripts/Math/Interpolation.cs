using System;
using UnityEngine;

public static class Interpolation
{
	public static MyQuaternion SLERP( MyQuaternion q_0 , MyQuaternion q_1, float t)
	{
		float dot = MyQuaternion.ScalarProduct(q_0, q_1);
		dot = Mathf.Clamp(dot, -1f, 1f);
		float Omega = Mathf.Acos(dot);

		float s_t = (float)Mathf.Sin(t * Omega);
		float tau = 1 - t;
		float s_tau = (float)Mathf.Sin(tau * Omega);
		float s_omega = (float)Mathf.Sin(Omega); // qué pasa si es pequeño?

		MyQuaternion q_t = MyQuaternion.Sum(MyQuaternion.ProductByScalar(q_0, s_tau /s_omega ),
			               MyQuaternion.ProductByScalar(q_1, s_t / s_omega));

		return q_t;

	}

	public static MyQuaternion SLERP(MyQuaternion q_0, float angle, Vector3 axis, float time) { 
	 
		MyQuaternion finalQuaternion = new MyQuaternion(axis, angle);

		return SLERP(q_0, finalQuaternion, time);
	
	}
}
