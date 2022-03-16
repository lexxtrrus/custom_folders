using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Character", menuName = "Character/Body", order = 0)]
    public class CharacterConfig : ScriptableObject
    {
        //[SerializeField] private List<string> _torsos;

        [SerializeField] private List<BodyPartConfig> legs;
        [SerializeField] private List<BodyPartConfig> torsos;
        [SerializeField] private List<BodyPartConfig> skin;
        [SerializeField] private List<BodyPartConfig> heads;
        [SerializeField] private List<BodyPartConfig> hairs;

        public List<BodyPartConfig> Legs => legs;
        public List<BodyPartConfig> Torsos => torsos;
        
        public List<BodyPartConfig> Skin => skin;
        public List<BodyPartConfig> Heads => heads;
        public List<BodyPartConfig> Hairs => hairs;
    }

    [Serializable]
    public class BodyPartConfig
    {
        [SerializeField] private string _name;
        [SerializeField] private List<Material> _materials;

        public string Name => _name;

        public Material GetRandomMaterial()
        {
            return _materials[Random.Range(0, _materials.Count)];
        }
    }
}