//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OpenCvSharp;
//using OpenCvSharp.CPlusPlus;

//namespace _2paskaita
//{
//    class Histograms
//    {
//        static void Main(string[] args)
//        {

//            Mat imgblue = new Mat();
//            Mat imggreen = new Mat();
//            Mat imgred = new Mat();

//            Mat img = new Mat();
//            // Mat gray = new Mat();
//            img = Cv2.ImRead(@"C:\Users\viliu\Documents\download.jpg", LoadMode.Color);
//            //gray = img.CvtColor(ColorConversion.RgbToGray);
//            Mat gray = new Mat();
//            img.CopyTo(gray);
//            Mat[] bgr = Cv2.Split(gray);
//            //var temp = bgr[0];
//            bgr[0].CopyTo(imgblue);
//            bgr[1].CopyTo(imggreen);
//            bgr[2].CopyTo(imgred);


//            float[] histvalue = new float[256];
//            float[] histvalue1 = new float[256];
//            float[] histvalue2 = new float[256];

//            MatOfByte mat = new MatOfByte(imgblue);
//            var idx = mat.GetIndexer();
//            for (int row = 0; row < imgblue.Rows; row++)
//            {

//                for (int col = 0; col < imgblue.Cols; col++)
//                {

//                    histvalue[idx[row, col]]++;

//                }
//            }
//            MatOfByte mat1 = new MatOfByte(imggreen);
//            var idx1 = mat1.GetIndexer();
//            for (int row = 0; row < imggreen.Rows; row++)
//            {

//                for (int col = 0; col < imggreen.Cols; col++)
//                {

//                    histvalue1[idx1[row, col]]++;

//                }
//            }
//            MatOfByte mat2 = new MatOfByte(imgred);
//            var idx2 = mat2.GetIndexer();
//            for (int row = 0; row < imgred.Rows; row++)
//            {

//                for (int col = 0; col < imgred.Cols; col++)
//                {

//                    histvalue2[idx2[row, col]]++;

//                }
//            }

//            float maxvalue2 = histvalue.Max();
//            float maxvalue21 = histvalue1.Max();
//            float maxvalue22 = histvalue2.Max();

//            Mat histarea = new Mat(new Size(256, 330), MatType.CV_8UC3, Scalar.White);


//            for (int i = 0; i < 256 - 1; i++)
//            {

//                //Normalizavimas
//                histvalue[i] = (histvalue[i] / maxvalue2) * 100;
//                histvalue1[i] = (histvalue1[i] / maxvalue21) * 100;
//                histvalue2[i] = (histvalue2[i] / maxvalue22) * 100;
//            }

//            for (int i = 0; i < 256 - 1; i++)
//            {
//                Cv2.Line(histarea, new Point(i, 100 - (int)histvalue[i]),
//                    new Point(i + 1, 100 - (int)histvalue[i + 1]), Scalar.Blue, 1);
//            }
//            for (int i = 0; i < 256 - 1; i++)
//            {
//                Cv2.Line(histarea, new Point(i, 100 - (int)histvalue1[i]),
//                    new Point(i + 1, 100 - (int)histvalue1[i + 1]), Scalar.Green, 1);
//            }
//            for (int i = 0; i < 256 - 1; i++)
//            {
//                Cv2.Line(histarea, new Point(i, 100 - (int)histvalue2[i]),
//                    new Point(i + 1, 100 - (int)histvalue2[i + 1]), Scalar.Red, 1);
//            }







//            Window show = new Window("Nicole", WindowMode.FreeRatio);
//            show.Image = gray;

//            Window histwindow = new Window("histogram", WindowMode.FreeRatio);
//            histwindow.Image = histarea;



//            Cv2.WaitKey();

//            show.Close();
//            histwindow.Close();

//        }
//    }
//}
