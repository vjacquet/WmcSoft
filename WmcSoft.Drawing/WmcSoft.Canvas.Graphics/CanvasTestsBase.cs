using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WmcSoft.Canvas
{
    public class CanvasTestsBase
    {
        protected virtual GraphicsCanvas CreateCanvas(string filename, int width = 400, int height=200) {
            var image = new Bitmap(width, height);
            var path = Path.GetFullPath(Path.Combine(@"..\Tests", filename));
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            return new GraphicsCanvas(Graphics.FromImage(image), g => image.Save(path, ImageFormat.Png));
        }
    }
}
