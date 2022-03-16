using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ResourcesLoader : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer baseSprite;
        [SerializeField] private Transform prefabRoot;

        [SerializeField] private string[] icons = new string[1];

        private GameObject obj;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetupIcon(icons[0]);
            }
        }

        private void SetupIcon(string configName)
        {
            var config = Resources.Load<IconInfo>($"Configs/{configName}");
            var sprite = Resources.Load<Sprite>(config.IconName);
            var prefab = Resources.Load<GameObject>($"Prefabs/{config.PrefabName}");

            baseSprite.sprite = sprite;

            if(obj) Destroy(obj);
            
            obj = Instantiate(prefab, prefabRoot);
        }
    }
}