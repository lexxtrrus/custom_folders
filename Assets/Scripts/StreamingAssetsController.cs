using System;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class StreamingAssetsController : MonoBehaviour
    {
        [SerializeField] private ImageData _imageData;

        private void Start()
        {
            _imageData.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                return;
            }

            var directoryInfo = new DirectoryInfo((Application.streamingAssetsPath));
            print($"Streaaming Assets Path: {Application.streamingAssetsPath}");
            var allFlies = directoryInfo.GetFiles("*.*");

            foreach (var file in allFlies)
            {
                print($"File name: {file.Name}");
                if(file.Name.Contains("meta")) continue;
                var imageData = Instantiate(_imageData, _imageData.transform.parent);

                var bytes = File.ReadAllBytes(file.FullName);
                var texture2d = new Texture2D(1,1);
                texture2d.LoadImage(bytes);

                var rect = new Rect(0, 0, texture2d.width, texture2d.height);
                var pivot = new Vector2(0.5f, 0.5f);

                var sprite = Sprite.Create(texture2d, rect, pivot);
                imageData.image.sprite = sprite;
                imageData.text.text = file.Name;
                
                imageData.gameObject.SetActive(true);
            }
            
            _imageData.gameObject.SetActive(false);
        }
    }
}