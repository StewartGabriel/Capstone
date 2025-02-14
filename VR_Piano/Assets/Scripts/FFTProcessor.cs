using MathNet.Numerics.IntegralTransforms;
using System.Numerics; // For complex numbers
using UnityEngine;

public static class FFTProcessor
{
    public static void FFT(float[] samples, float[] spectrum, int sampleRate)
    {
        int n = samples.Length;
        if ((n & (n - 1)) != 0)
        {
            Debug.LogError("FFT requires data length to be a power of 2.");
            return;
        }

        // Convert samples to complex numbers
        Complex[] complexSamples = new Complex[n];
        for (int i = 0; i < n; i++)
        {
            complexSamples[i] = new Complex(samples[i], 0.0); // Real part is the sample, imaginary part is 0
        }

        // Perform the FFT
        Fourier.Forward(complexSamples, FourierOptions.Matlab);

        // Calculate the magnitude spectrum
        int spectrumLength = spectrum.Length;
        for (int i = 0; i < spectrumLength; i++)
        {
            spectrum[i] = (float)complexSamples[i].Magnitude;
        }
    }

    public static float GetFrequencyFromIndex(int index, int sampleRate, int fftSize)
    {
        return index * (sampleRate / 2f) / (fftSize / 2f);
    }
}
