using System;
using System.Linq;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

// Because of .gitignore, packages, bin files and executables are not included.
// To use:
// 1. Go to nuget package manager and and uninstall OpenCvSharp
// 2. Open nuget package manager console and use the command below 
// 3. Make sure to have a camera connected
// 4. Build and run
// Install-Package OpenCvSharp-AnyCPU -Version 2.4.10.20170306

namespace Images04
{
    public static class Histograms
    {
        private static void Main(string[] args)
        {
            var capturedImage = new Mat();
            var histogramWindow = new Window("Histogram", WindowMode.FreeRatio);
            var videoCaptureWindow = new Window("Video capture", WindowMode.FreeRatio);
            var capture = new VideoCapture();
            var boxToRecord = new Rect(50, 50, 250, 300);
            var vectorSaved = false;
            var boxImage = new Mat();
            float[] histogramTemplate = { };
            float[] rawHistogram = { };
            float imageError = 0;

            capture.Open(0);

            do
            {
                if (capture.Read(capturedImage))
                {
                    capturedImage[boxToRecord].CopyTo(boxImage);

                    // Set histogram window image to histogram and dispose
                    using (var bgrHistogram = MakeBgrHistogram(new Mat(new Size(256, 110), MatType.CV_8UC3, Scalar.White), boxImage, out var histogramMeasure, out rawHistogram))
                    {
                        if (Cv2.WaitKey(1) == 's')
                        {
                            histogramTemplate = histogramMeasure;
                            Console.WriteLine("Saving histogram...");
                            vectorSaved = true;
                        }

                        if (vectorSaved)
                        {
                            imageError = GetDistance(histogramTemplate, histogramMeasure);
                            Cv2.PutText(capturedImage, imageError.ToString(), new Point(35, 35), FontFace.HersheyPlain, 2, Scalar.Red);
                            capturedImage = PutStatisticsOnCamera(capturedImage, rawHistogram);
                        }

                        // Display rectangle
                        Cv2.Rectangle(capturedImage, boxToRecord, imageError < 100 ? Scalar.Red : Scalar.Blue, 3,
                            LineType.Link4, 0);

                        videoCaptureWindow.Image = capturedImage;
                        histogramWindow.ShowImage(bgrHistogram);
                        bgrHistogram.Release();
                        GC.Collect();
                    }
                }
            }
            while (Cv2.WaitKey(10) != 'q');
            capture.Release();
            histogramWindow.Close();
            videoCaptureWindow.Close();
        }

        private static void DoSomeCalculations(Mat img, float[] value)
        {
            MatOfByte mat = new MatOfByte(img);
            var idx = mat.GetIndexer();
            for (int row = 0; row < img.Rows; row++)
            {
                for (int col = 0; col < img.Cols; col++)
                {
                    value[idx[row, col]]++;
                }
            }
        }

        private static Mat MakeBgrHistogram(Mat histogram, Mat image, out float[] histogramMeasure, out float[] rawHistogram)
        {
            Mat blueImage = new Mat();
            Mat greenImage = new Mat();
            Mat redImage = new Mat();

            Mat gray = new Mat();
            image.CopyTo(gray);
            Mat[] bgr = Cv2.Split(gray);

            bgr[0].CopyTo(blueImage);
            bgr[1].CopyTo(greenImage);
            bgr[2].CopyTo(redImage);

            float[] histValueBlue = new float[256];
            float[] histValueGreen = new float[256];
            float[] histValueRed = new float[256];

            DoSomeCalculations(blueImage, histValueBlue);
            DoSomeCalculations(greenImage, histValueGreen);
            DoSomeCalculations(redImage, histValueRed);
            // Get raw red histogram
            rawHistogram = histValueRed;

            float maxValueBlue = histValueBlue.Max();
            float maxValueGreen = histValueGreen.Max();
            float maxValueRed = histValueRed.Max();

            for (int i = 0; i < 256 - 1; i++)
            {
                //Normalize the histogram values
                histValueBlue[i] = (histValueBlue[i] / maxValueBlue) * 100;
                histValueGreen[i] = (histValueGreen[i] / maxValueGreen) * 100;
                histValueRed[i] = (histValueRed[i] / maxValueRed) * 100;
            }

            for (int i = 0; i < 256 - 1; i++)
            {
                Cv2.Line(histogram, new Point(i, 100 - (int)histValueBlue[i]),
                    new Point(i + 1, 100 - (int)histValueBlue[i + 1]), Scalar.Blue, 1);
                Cv2.Line(histogram, new Point(i, 100 - (int)histValueGreen[i]),
                    new Point(i + 1, 100 - (int)histValueGreen[i + 1]), Scalar.Green, 1);
                Cv2.Line(histogram, new Point(i, 100 - (int)histValueRed[i]),
                    new Point(i + 1, 100 - (int)histValueRed[i + 1]), Scalar.Red, 1);
            }

            histogramMeasure = histValueBlue;

            return histogram;
        }

        private static float GetDistance(float[] vect1, float[] vect2)
        {
            float error;
            double sum = 0;

            if (vect1.Length == vect2.Length)
            {
                for (int i = 0; i < vect1.Length; i++)
                {
                    sum += Math.Pow((vect1[i] - vect2[i]), 2);
                }

                error = (float)Math.Sqrt(sum);
            }
            else
            {
                return -1;
            }
            return error;
        }

        private static Mat PutStatisticsOnCamera(Mat image, float[] histogram)
        {
            StatisticsCalculator.CalculateStatistics(histogram, out float mean, out float variance, out float skewness,
            out float kurtosis, out float energy, out float entropy);
            Cv2.PutText(image, "Mean: " + mean.ToString(), new Point(350, 45), FontFace.HersheyPlain, 1, Scalar.Red);
            Cv2.PutText(image, "Variance: " + variance.ToString(), new Point(350, 75), FontFace.HersheyPlain, 1, Scalar.Red);
            Cv2.PutText(image, "Skewness: " + skewness.ToString(), new Point(350, 105), FontFace.HersheyPlain, 1, Scalar.Red);
            Cv2.PutText(image, "Kurtosis: " + kurtosis.ToString(), new Point(350, 135), FontFace.HersheyPlain, 1, Scalar.Red);
            Cv2.PutText(image, "Energy: " + energy.ToString(), new Point(350, 165), FontFace.HersheyPlain, 1, Scalar.Red);
            Cv2.PutText(image, "Entropy: " + entropy.ToString(), new Point(350, 195), FontFace.HersheyPlain, 1, Scalar.Red);

            return image;
        }
    }
}
