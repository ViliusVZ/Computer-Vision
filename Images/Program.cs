using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Linq;

// Because of .gitignore, packages, bin files and executables are not included.
// To use:
// 1. Go to nuget package manager and and uninstall OpenCvSharp
// 2. Open nuget package manager console and use the command below 
// 3. Make sure to have a camera connected
// 4. Build and run
// Install-Package OpenCvSharp-AnyCPU -Version 2.4.10.20170306

namespace Images
{
    public static class Histograms
    {
        private static void Main(string[] args)
        {
            Mat capturedImage = new Mat();
            Mat histogramArea = new Mat(new Size(150, 200), MatType.CV_8UC3, Scalar.White);
            Window histogramWindow = new Window("Histogram", WindowMode.FreeRatio);
            Window videoCaptureWindow = new Window("Video capture", WindowMode.FreeRatio);
            VideoCapture capture = new VideoCapture();

            capture.Open(0);

            do
            {
                if (capture.Read(capturedImage))
                {
                    // Clear background
                    histogramArea = new Mat(new Size(256, 330), MatType.CV_8UC3, Scalar.White);
                    // Show the video
                    videoCaptureWindow.Image = capturedImage;
                    // Set histogram window image to histogram
                    histogramWindow.ShowImage(MakeBgrHistogram(histogramArea, capturedImage));
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

        private static Mat MakeBgrHistogram(Mat drawnHistogram, Mat image)
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
                Cv2.Line(drawnHistogram, new Point(i, 100 - (int)histValueBlue[i]),
                    new Point(i + 1, 100 - (int)histValueBlue[i + 1]), Scalar.Blue, 1);
                Cv2.Line(drawnHistogram, new Point(i, 100 - (int)histValueGreen[i]),
                    new Point(i + 1, 100 - (int)histValueGreen[i + 1]), Scalar.Green, 1);
                Cv2.Line(drawnHistogram, new Point(i, 100 - (int)histValueRed[i]),
                    new Point(i + 1, 100 - (int)histValueRed[i + 1]), Scalar.Red, 1);
            }

            return drawnHistogram;
        }
    }
}
