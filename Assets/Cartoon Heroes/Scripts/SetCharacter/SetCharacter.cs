using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace CartoonHeroes
{
    public class SetCharacter : MonoBehaviour
    {
        [SerializeField] private CharacterConfig _config;
        
        public Transform characterRoot;
        public ItemGroup[] itemGroups;

        const string namePrefix = "Set Character_";
        const string hideString = "(Hide)";

        public GameObject disabledGraySkeleton;

        [SerializeField] private GameObject legs;
        [SerializeField] private GameObject tors;
        [SerializeField] private GameObject head;
        [SerializeField] private GameObject hair;

        // Use this for initialization
        void Start()
        {
            SetupLegs();
            SetupTorsos();
            SetupHeads();
            SetupHairs();
        }

        private void SetupHairs()
        {
            itemGroups[3].name = "Hairs";
            itemGroups[3].items = new Item[_config.Hairs.Count];
            for (int i = 0; i < itemGroups[0].items.Length; i++)
            {
                itemGroups[3].items[i] = new Item();
                itemGroups[3].items[i].prefab = Resources.Load<GameObject>(_config.Hairs[i].Name);
            }

            itemGroups[3].slots = _config.Hairs.Count;
        }

        private void SetupHeads()
        {
            itemGroups[2].name = "Heads";
            itemGroups[2].items = new Item[_config.Heads.Count];
            for (int i = 0; i < itemGroups[2].items.Length; i++)
            {
                itemGroups[2].items[i] = new Item();
                itemGroups[2].items[i].prefab = Resources.Load<GameObject>(_config.Heads[i].Name);
            }

            itemGroups[2].slots = _config.Heads.Count;
        }

        private void SetupTorsos()
        {
            itemGroups[1].name = "Torsos";
            itemGroups[1].items = new Item[_config.Torsos.Count];
            for (int i = 0; i < itemGroups[1].items.Length; i++)
            {
                itemGroups[1].items[i] = new Item();
                itemGroups[1].items[i].prefab = Resources.Load<GameObject>(_config.Torsos[i].Name);
            }

            itemGroups[1].slots = _config.Torsos.Count;
        }

        private void SetupLegs()
        {
            itemGroups[0].name = "Legs";
            itemGroups[0].items = new Item[_config.Legs.Count];
            for (int i = 0; i < itemGroups[0].items.Length; i++)
            {
                itemGroups[0].items[i] = new Item();
                itemGroups[0].items[i].prefab = Resources.Load<GameObject>(_config.Legs[i].Name);
            }

            itemGroups[0].slots = _config.Legs.Count;
        }

        [System.Serializable]
        public class ItemGroup
        {
            public string name;
            public Item[] items;
            public int slots;
        }

        [System.Serializable]
        public class Item
        {
            public GameObject prefab;
        }


        public GameObject AddItem(ItemGroup itemGroup, int itemSlot)
        {
            Item item = itemGroup.items[itemSlot];
            GameObject itemInstance = GameObject.Instantiate(item.prefab);
            itemInstance.name = itemInstance.name.Substring(0, itemInstance.name.Length - "(Clone)".Length);
            RemoveAnimator(itemInstance);
            ParentObjectAndBones(itemInstance);

            SetGraySkeletonVisibility(!VisibleItems());

            return itemInstance;
        }

        public bool VisibleItems()
        {
            for(int i = 0; i < itemGroups.Length; i++)
            {
                for(int n = 0; n < itemGroups[i].items.Length; n++)
                {
                    if(HasItem(itemGroups[i], n))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SetGraySkeletonVisibility(bool set)
        {
            if (!set)
            {
                Transform[] allCharacterChildren = GetAllCharacterChildren();
                for (int i = 0; i < allCharacterChildren.Length; i++)
                {
                    if (allCharacterChildren[i].name.Contains(hideString))
                    {
                        disabledGraySkeleton = allCharacterChildren[i].gameObject;
                        allCharacterChildren[i].gameObject.SetActive(false);
                        break;
                    }
                }
             }
            else {
                if (disabledGraySkeleton != null)
                {
                    disabledGraySkeleton.SetActive(true);
                }
            }

        }

        public bool HasItem(ItemGroup itemGroup, int itemSlot)
        {
            if (itemGroup.items[itemSlot] != null && itemGroup.items[itemSlot].prefab != null)
            {

                Transform root = GetRoot();
                Transform prefab = itemGroup.items[itemSlot].prefab.transform;
                for (int i = 0; i < root.childCount; i++)
                {
                    Transform child = root.GetChild(i); 
                    if (child.name.Contains(prefab.name) && child.name.Contains(namePrefix))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void ParentObjectAndBones(GameObject itemInstance)
        {
            Transform[] allCharacterChildren = GetAllCharacterChildren();
            Transform[] allItemChildren = itemInstance.GetComponentsInChildren<Transform>();
            itemInstance.transform.position = transform.position;
            itemInstance.transform.parent = transform;

            string[] allItemChildren_NewNames= new string[allItemChildren.Length];

            for(int i = 0; i < allItemChildren.Length; i++)
            {
                //Match and parent bones
                for (int n = 0; n < allCharacterChildren.Length; n++)
                {
                    if(allItemChildren[i].name == allCharacterChildren[n].name)
                    {
                        MatchTransform(allItemChildren[i], allCharacterChildren[n]);
                        allItemChildren[i].parent = allCharacterChildren[n];
                    }
                }

                //Rename
                allItemChildren_NewNames[i] = allItemChildren[i].name;

                if (!allItemChildren[i].name.Contains(namePrefix))
                {
                    allItemChildren_NewNames[i] = namePrefix + allItemChildren[i].name;
                }

                if (!allItemChildren[i].name.Contains(itemInstance.name))
                {
                    allItemChildren_NewNames[i] += "_" + itemInstance.name;
                }
            }

            for(int i = 0; i < allItemChildren.Length; i++)
            {
                allItemChildren[i].name = allItemChildren_NewNames[i];
            }
        }

        public Transform GetRoot()
        {
            Transform root;
            if (characterRoot == null)
            {
                root = transform;
            }
            else
            {
                root = characterRoot;
            }
            return root;
        }

        public Transform[] GetAllCharacterChildren()
        {
            Transform root = GetRoot();
            Transform[] allCharacterChildren = root.GetComponentsInChildren<Transform>();

            /*List<Transform> allCharacterChildren_List = new List<Transform>();
            
            for(int i = 0; i < allCharacterChildren.Length; i++){
                if(allCharacterChildren[i].GetComponent<SkinnedMeshRenderer>() != null || allCharacterChildren[i].GetComponent<Animator>() != null)
                {
                    continue;
                }
                allCharacterChildren_List.Add(allCharacterChildren[i]);
            }

            allCharacterChildren = allCharacterChildren_List.ToArray();*/

            return allCharacterChildren;
        }

        public bool BelongsToItem(Transform obj, ItemGroup itemGroup, int itemSlot)
        {
            if(obj == null || itemGroup.items[itemSlot].prefab == null)
            {
                return false;
            }
            return (obj.name.Contains(namePrefix) && obj.name.Contains(itemGroup.items[itemSlot].prefab.name));
        }

        public void RemoveAnimator(GameObject item)
        {
            Animator animator = item.GetComponent<Animator>();
            if(animator != null)
            {
                DestroyImmediate(animator);
            }
        }

        public void MatchTransform(Transform obj, Transform target)
        {
            obj.position = target.position;
            obj.rotation = target.rotation;
        }

        public List<GameObject> GetRemoveObjList(ItemGroup itemGroup, int itemSlot)
        {
            Transform[] allChildren = GetAllCharacterChildren();

            List<GameObject> removeList = new List<GameObject>();

            for (int i = 0; i < allChildren.Length; i++)
            {
                if(BelongsToItem(allChildren[i], itemGroup, itemSlot))
                {
                    //DestroyImmediate(allChildren[i].gameObject);
                    removeList.Add(allChildren[i].gameObject);
                }
            }

            SetGraySkeletonVisibility(!VisibleItems());
            return removeList;
        }

        public void SetLegs()
        {
            if(legs) Destroy(legs);
            var index = Random.Range(0, _config.Legs.Count);
            legs = AddItem(itemGroups[0], index);
            var materials = legs.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials;
            materials[0] = _config.Legs[index].GetRandomMaterial();

            if (materials.Length > 1) materials[1] = _config.Skin[index].GetRandomMaterial();
        }
        
        public void SetTors()
        {
            if(tors) Destroy(tors);
            var index = Random.Range(0, _config.Heads.Count);
            tors = AddItem(itemGroups[1], index);
            var materials = tors.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials;
            materials[0] = _config.Torsos[index].GetRandomMaterial();
            materials[1] = _config.Skin[index].GetRandomMaterial();
        }
        
        public void SetHead()
        {
            if(head) Destroy(head);
            var index = Random.Range(0, _config.Heads.Count);
            head = AddItem(itemGroups[2], index);
            head.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material =
                _config.Heads[index].GetRandomMaterial();
        }
        
        public void SetHair()
        {
            if(hair) Destroy(hair);
            var index = Random.Range(0, _config.Hairs.Count);
            hair = AddItem(itemGroups[3], index);
            hair.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material =
                _config.Hairs[index].GetRandomMaterial();
        }
    }
}

