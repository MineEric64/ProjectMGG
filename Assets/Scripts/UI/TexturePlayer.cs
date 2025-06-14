using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectMGG.Ingame;

namespace ProjectMGG.UI
{
    public enum TextureKind
    {
        None = 0,
        Custom = 1,
        Noise = 2
    }

    public class TexturePlayer : MonoBehaviour
    {
        public float Fps = 2.0f;
        public TextureKind Kind = TextureKind.None;
        public List<Texture2D> CustomImages = new List<Texture2D>();

        private RawImage _image;
        private List<Texture2D> _images = new List<Texture2D>();
        private TextureKind _previousKind = TextureKind.None;
        private int _index = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _image = GetComponent<RawImage>();

            StartCoroutine(OnUpdate());
        }

        void UpdateToNewSprites()
        {
            _images.Clear();
            _index = 0;

            switch (Kind)
            {
                case TextureKind.None:
                    break;
                case TextureKind.Custom:
                    {
                        _images.AddRange(CustomImages);
                        break;
                    }
                case TextureKind.Noise:
                    {
                        var paths = new List<string>();
                        for (int i = 1; i <= 20; i++) paths.Add(string.Concat("$/images/texture/noise/", i.ToString(), ".png"));
                        foreach (string path in paths)
                        {
                            _images.Add(IngameManagerV2.LoadResource<Texture2D>(path));
                        }
                        break;
                    }
            }
        }

        IEnumerator OnUpdate()
        {
            while (true)
            {
                if (_previousKind != Kind)
                {
                    UpdateToNewSprites();

                    if (Kind == TextureKind.None)
                    {
                        _image.texture = null;
                        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
                    }
                    else if (_previousKind == TextureKind.None)
                    {
                        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1f);
                    }
                    _previousKind = Kind;
                }
                if (_images.Count > 0)
                {
                    _image.texture = _images[_index];
                    _index++;
                    if (_index >= _images.Count) _index = 0;
                }

                yield return new WaitForSeconds(1 / Fps);
            }
        }
    }
}
