using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Linq;

namespace Images
{
    internal class Histograms
    {
        private static void Main(string[] args)
        {
            var image = Cv2.ImRead(@"C:\Users\viliu\Documents\download.jpg", LoadMode.Color);
            Mat histogramArea = new Mat(new Size(256, 330), MatType.CV_8UC3, Scalar.White);

            Window showImgWindow = new Window("Nicole", WindowMode.FreeRatio) { Image = image };
            Window histwindow = new Window("histogram", WindowMode.FreeRatio)
                { Image = MakeBgrHistogram(histogramArea, image) };

            VideoCapture cap = new VideoCapture(0);

            while (true)
            {
                cap.Read(image);
                showImgWindow.Image = image;
                histwindow.Image = new Mat(new Size(256, 330), MatType.CV_8UC3, Scalar.White);
                histwindow.Image = MakeBgrHistogram(histogramArea, image);

                if (Cv2.WaitKey(10) == 'q')
                {
                    break;
                }
            }
            cap.Release();
            showImgWindow.Close();


            Cv2.WaitKey();

            showImgWindow.Close();
            histwindow.Close();
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

        private static Mat MakeBgrHistogram(Mat histogramArea, Mat image)
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
                Cv2.Line(histogramArea, new Point(i, 100 - (int)histValueBlue[i]),
                    new Point(i + 1, 100 - (int)histValueBlue[i + 1]), Scalar.Blue, 1);
                Cv2.Line(histogramArea, new Point(i, 100 - (int)histValueGreen[i]),
                    new Point(i + 1, 100 - (int)histValueGreen[i + 1]), Scalar.Green, 1);
                Cv2.Line(histogramArea, new Point(i, 100 - (int)histValueRed[i]),
                    new Point(i + 1, 100 - (int)histValueRed[i + 1]), Scalar.Red, 1);
            }

            return histogramArea;
        }
    }
}
