using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace Images
{
    public class FirstLecture
    {
        void VideoCapture()
        {
            Mat img = Cv2.ImRead(@"C:\Users\viliu\Documents\customer-portrait-human-api.png", LoadMode.Color);
            Window show = new Window("Langas", WindowMode.FreeRatio);
            //show.Image = img;
            //Cv2.WaitKey();

            //Mat atskirasImg = new Mat();
            //img.CopyTo(atskirasImg);
            //Mat[] bgr = Cv2.Split(atskirasImg);
            //var temp = bgr[0];
            //bgr[2].CopyTo(temp)

            VideoCapture cap = new VideoCapture(0);

            while (true)
            {
                cap.Read(img);
                show.Image = img;

                if (Cv2.WaitKey(10) == 'q')
                {

                    break;
                }
            }
            cap.Release();
            show.Close();
        }
    }
}
