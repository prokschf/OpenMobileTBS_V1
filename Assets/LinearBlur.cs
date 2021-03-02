using UnityEngine;

public class LinearBlur
{
    private float _rSum = 0;
    private float _gSum = 0;
    private float _bSum = 0;
     
    private Texture2D _sourceImage;
    private int _sourceWidth;
    private int _sourceHeight;
    private int _windowSize;
     
    public Texture2D Blur(Texture2D image, int radius, int iterations)
    {
        _windowSize = radius * 2 + 1;
        _sourceWidth = image.width;
        _sourceHeight = image.height;
     
        var tex = image;
     
        for (var i = 0; i < iterations; i++)
        {
            tex = OneDimensialBlur(tex, radius, true);
            tex = OneDimensialBlur(tex, radius, false);
        }
     
        return tex;
    }
     
    private Texture2D OneDimensialBlur(Texture2D image, int radius, bool horizontal)
    {
        _sourceImage = image;
     
        var blurred = new Texture2D(image.width, image.height, image.format, false);
     
        if (horizontal)
        {
            for (int imgY = 0; imgY < _sourceHeight; ++imgY)
            {
                ResetSum();
     
                for (int imgX = 0; imgX < _sourceWidth; imgX++)
                {
                    if (imgX == 0)
                        for (int x = radius * -1; x <= radius; ++x)
                            AddPixel(GetPixelWithXCheck(x, imgY));
                    else
                    {
                        var toExclude = GetPixelWithXCheck(imgX - radius - 1, imgY);
                        var toInclude = GetPixelWithXCheck(imgX + radius, imgY);
     
                        SubstPixel(toExclude);
                        AddPixel(toInclude);
                    }
     
                    blurred.SetPixel(imgX, imgY, CalcPixelFromSum());
                }
            }
        }
     
        else
        {
            for (int imgX = 0; imgX < _sourceWidth; imgX++)
            {
                ResetSum();
     
                for (int imgY = 0; imgY < _sourceHeight; ++imgY)
                {
                    if (imgY == 0)
                        for (int y = radius * -1; y <= radius; ++y)
                            AddPixel(GetPixelWithYCheck(imgX, y));
                    else
                    {
                        var toExclude = GetPixelWithYCheck(imgX, imgY - radius - 1);
                        var toInclude = GetPixelWithYCheck(imgX, imgY + radius);
     
                        SubstPixel(toExclude);
                        AddPixel(toInclude);
                    }
     
                    blurred.SetPixel(imgX, imgY, CalcPixelFromSum());
                }
            }
        }
     
        blurred.Apply();
        return blurred;
    }
     
    private Color GetPixelWithXCheck(int x, int y)
    {
        if (x <= 0) return _sourceImage.GetPixel(0, y);
        if (x >= _sourceWidth) return _sourceImage.GetPixel(_sourceWidth - 1, y);
        return _sourceImage.GetPixel(x, y);
    }
     
    private Color GetPixelWithYCheck(int x, int y)
    {
        if (y <= 0) return _sourceImage.GetPixel(x, 0);
        if (y >= _sourceHeight) return _sourceImage.GetPixel(x, _sourceHeight - 1);
        return _sourceImage.GetPixel(x, y);
    }
     
    private void AddPixel(Color pixel)
    {
        _rSum += pixel.r;
        _gSum += pixel.g;
        _bSum += pixel.b;
    }
     
    private void SubstPixel(Color pixel)
    {
        _rSum -= pixel.r;
        _gSum -= pixel.g;
        _bSum -= pixel.b;
    }
     
    private void ResetSum()
    {
        _rSum = 0.0f;
        _gSum = 0.0f;
        _bSum = 0.0f;
    }
     
    Color CalcPixelFromSum()
    {
        return new Color(_rSum / _windowSize, _gSum / _windowSize, _bSum / _windowSize);
    }
}