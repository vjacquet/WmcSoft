using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WmcSoft.Canvas
{
    public class CanvasTestsBase
    {
        protected virtual GraphicsCanvas CreateCanvas(string filename) {
            var image = new Bitmap(400, 200);
            var path = Path.GetFullPath(Path.Combine(@"..\Tests", filename));
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            return new GraphicsCanvas(Graphics.FromImage(image), g => image.Save(path, ImageFormat.Png));
        }
    }
}
