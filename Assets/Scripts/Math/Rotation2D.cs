using UnityEngine;
using System;
using UnityEditorInternal;

public static class Rotation2D {

	public static Vector2 MatrixRot(Vector2 p, float angle) { 
	
		float c = (float)Math.Cos(angle);
		float s = (float)Math.Sin(angle);

		float newX = c * p.x - s * p.y;
		float newY = s * p.x + c * p.y;

		return new Vector2(newX, newY);
	
	}


	public static Vector2 ComplexRot(Vector2 p, float angle) {

		Complex z = new Complex(p.x, p.y);

		Complex alpha = new Complex((float)Math.Cos(angle), (float)Math.Sin(angle));

		Complex result = Complex.Multiply(alpha , z);

		return new Vector2(result.real, result.imag);
	
	}


}