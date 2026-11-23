public struct Complex
{
    public float real;
    public float imag;

    public Complex(float real, float imag) { 
    
        this.real = real; 
        this.imag = imag;
    }


    public static Complex Multiply(Complex a, Complex b) {
        float real = a.real * b.real - a.imag * b.imag;
        float imag = a.real * b.imag + a.imag * b.real;

        return new Complex(real, imag);
    }



}