using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class IconInfo : ScriptableObject
    {
        [SerializeField] private string iconName;
        [SerializeField] private string prefabName;

        public string IconName => iconName;

        public string PrefabName => prefabName;
    }
}