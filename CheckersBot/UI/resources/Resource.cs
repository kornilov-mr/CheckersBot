using System.IO;
using CheckersBot.UI.resources.color;
using CheckersBot.utils;
using Color = System.Windows.Media.Color;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace CheckersBot.UI.resources;

/// <summary>
/// Static class, which is responsible for loading resources
/// </summary>
public class Resource
{
    private static readonly string JsonColorPath =
        PathResolver.ResolvePathFromSolutionRoot(@"UI\resources\color\NColorJson.json");

    private static readonly string JsonIconPath =
        PathResolver.ResolvePathFromSolutionRoot(@"UI\resources\icons\IconPaths.png");

    private static Dictionary<string, Object> _resources = new();

    /// <summary>
    /// Get resources by name and tries to upcast it to color 
    /// </summary>
    /// <param name="name"> string to take object for resources</param>
    /// <returns> color </returns>
    public static Color GetColor(string name)
    {
        if (_resources.Count == 0)
        {
            LoadResources();
        }

        if (!_resources.ContainsKey(name))
        {
            return Color.FromRgb(0, 0, 0);
        }

        return (Color)_resources[name];
    }

    /// <summary>
    /// Get resources by name
    /// </summary>
    /// <param name="name"> string to take object for resources</param>
    /// <returns> object from resources </returns>
    public static Object GetResource(string name)
    {
        if (_resources.Count == 0)
        {
            LoadResources();
        }

        return _resources[name];
    }

    /// <summary>
    /// Get resources by name and tries to upcast it to BitmapImage
    /// </summary>
    /// <param name="name"> string to take object for resources</param>
    /// <returns> BitmapImage from resources </returns>
    public static BitmapImage GetIcon(string name)
    {
        if (_resources.Count == 0)
        {
            LoadResources();
        }

        return (BitmapImage)_resources[name];
    }

    private static void LoadResources()
    {
        _resources = new Dictionary<string, Object>();
        List<ColorKeyValueToSerialize> o = JsonSerializer.Deserialize<List<ColorKeyValueToSerialize>>(
            File.ReadAllText(JsonColorPath))!;
        foreach (ColorKeyValueToSerialize keyValue in o)
        {
            _resources.Add(keyValue.name, keyValue.ColorRGB.ToColor());
        }

        Dictionary<string, string> keysToPaths = JsonSerializer.Deserialize<Dictionary<string, string>>(
            File.ReadAllText(JsonIconPath))!;
        foreach (var keyValue in keysToPaths)
        {
            _resources.Add(keyValue.Key, ConvertPngToIcon(keyValue.Value));
        }
    }

    static BitmapImage ConvertPngToIcon(string imagePath, int size = 80)
    {
        BitmapImage bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(imagePath);
        bitmap.DecodePixelWidth = size;
        bitmap.DecodePixelHeight = size;
        bitmap.EndInit();
        return bitmap;
    }
}