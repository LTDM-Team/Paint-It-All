using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
class PaintCanvas : MonoBehaviour
{
    public static PaintCanvas Instance { get; private set; }
    private static Color32 s_transparentColor = new Color32(0, 0, 0, 0);

    [SerializeField] private float _unitSize;
    [SerializeField] private int _textureSize;

    private SpriteRenderer _renderer;
    private Texture2D _texture;

    private float _pixelsPerUnit;
    private bool _hasChanges;

    private void Awake() => Instance = this;
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();

        InitializeCanvas();
    }
    private void LateUpdate()
    {
        UpdateCanvas();
    }

    private void InitializeCanvas()
    {
        _pixelsPerUnit = _textureSize / _unitSize;

        var size = new Vector2Int(_textureSize, _textureSize);
        var pivot = new Vector2(0.5f, 0.5f);
        var rect = new Rect(Vector2.zero, size);

        _texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            alphaIsTransparency = true
        };
        _renderer.sprite = Sprite.Create(_texture, rect, pivot, _pixelsPerUnit);

        Clear();
    }
    private void UpdateCanvas()
    {
        if (_hasChanges == false) return;
        else _hasChanges = false;

        _texture.Apply();
    }

    public int GetCountColor(Color32 color)
    {
        var count = 0;
        var colors = _texture.GetPixels32();

        for (var i = 0; i < colors.Length; i++)
        {
            if (colors[i].Equals(color))
                count++;
        }

        return count;
    }

    public void Clear()
    {
        Fill(s_transparentColor);
    }
    public void Fill(Color32 color)
    {
        var size = _texture.width * _texture.height;
        var colors = GenerateColors(color, size);

        _texture.SetPixels32(colors);
        _hasChanges = true;
    }
    public void DrawSquare(Color32 color, Vector2 worldPosition, int unitSize)
    {
        var position = WorldToPixelPosition(worldPosition);
        var size = UnitToPixelSize(unitSize);

        var colors = GenerateColors(color, size * size);
        var halfSize = size / 2;

        var positionX = position.x - halfSize;
        var positionY = position.y - halfSize;

        _texture.SetPixels32(positionX, positionY, size, size, colors);
        _hasChanges = true;
    }
    public void DrawCircle(Color32 color, Vector2 worldPosition, int unitRadius)
    {
        var position = WorldToPixelPosition(worldPosition);
        var radius = UnitToPixelSize(unitRadius);

        var radiusSquared = radius * radius;
        var leftPosition = position.x - radius;
        var rightPosition = position.x + radius;
        var downPosition = position.y - radius;
        var upPosition = position.y + radius;

        for (var u = leftPosition; u < rightPosition + 1; u++)
        {
            for (var v = downPosition; v < upPosition + 1; v++)
            {
                var shiftX = position.x - u;
                var shiftY = position.y - v;

                var shiftXSquared = shiftX * shiftX;
                var shiftYSquared = shiftY * shiftY;

                if (shiftXSquared + shiftYSquared < radiusSquared)
                    _texture.SetPixel(u, v, color);
            }
        }

        _hasChanges = true;
    }

    private int UnitToPixelSize(float unitSize)
    {
        return (int)(unitSize * _pixelsPerUnit);
    }
    private Vector2Int WorldToPixelPosition(Vector2 position)
    {
        var halfSize = _textureSize / 2;
        var pixelX = UnitToPixelSize(position.x) + halfSize;
        var pixelY = UnitToPixelSize(position.y) + halfSize;

        return new Vector2Int(pixelX, pixelY);
    }

    private Color32[] GenerateColors(Color32 color, int count)
    {
        var colors = new Color32[count];

        for (var i = 0; i < count; i++)
            colors[i] = color;

        return colors;
    }
}