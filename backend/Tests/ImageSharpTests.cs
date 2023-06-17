using Microsoft.VisualStudio.TestTools.UnitTesting;
using SixLabors.ImageSharp;

namespace CookBook.Tests;

[TestClass]
public class ImageSharpTests
{

    [TestMethod]
    public void ImageToBase64StringConvertion()
    {
        Image image = Image.Load("../../../images/1.jpg");
        MemoryStream memoryStream = new MemoryStream();
        image.SaveAsJpeg(memoryStream);
        byte[] imageBytes = memoryStream.ToArray();
        String imageString = Convert.ToBase64String(imageBytes);
        //TODO:
        Assert.IsTrue(true);
    }
}
