using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantObjects : MonoBehaviour
{
    public static PersistantObjects Instance;
    [SerializeField] SceneField _bootstrapScene;
    [SerializeField] GameObject[] _allItems;
    [SerializeField] AnimationCurve _slowEndCurve;

    private Dictionary<Item, ItemDataSO> currentItemsDictionary = new Dictionary<Item, ItemDataSO>();
    private List<ItemDataSO> _interDimensionalItems = new();
    private List<DialoqueSO> _dialogues = new();

    [SerializeField, ReadOnly] ItemDataSO[] _allItemDataSO;

    private void Awake()
    {
        _allItems = Resources.LoadAll<GameObject>("Items");
        _allItemDataSO = Resources.LoadAll<ItemDataSO>("ItemData");
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        GameState.ResetGameState();
    }
    private void Start()
    {
        SceneLoader.Instance.OnSceneIsActivated += OnSceneActivated;

        Debug.Log(GameState.HasSuit);
    }

    private void OnSceneActivated(Dimension dimension)
    {
        CheckAllItemStatuses(dimension);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CheckAllItemStatuses(DimensionManager.Instance.CurrentDimension);
        }
    }

    private void CheckAllItemStatuses(Dimension dimension)
    {
        Debug.Log("Cheking all item statutes");
        foreach (var item in currentItemsDictionary)
        {
            CheckCurrentItemStatus(item.Key);
        }

        HandleInterDimensionalItems();
    }

    private void HandleInterDimensionalItems()
    {
        bool skipItem = false;
        foreach (var itemdata in _interDimensionalItems)
        {
            foreach (var item in currentItemsDictionary)
            {
                if (item.Value == itemdata)
                {
                    Debug.Log("Object already exists in scene " + itemdata.name);
                    item.Key.SetPosition();
                    skipItem = true;
                    break;
                }
            }

            if (skipItem == false)
            {
                if (itemdata.CurrentDimension == DimensionManager.Instance.CurrentDimension)
                {
                    var GO = GetItemFromAllItemsWithObjectData(itemdata);
                    var newItem = Instantiate(GO);
                    SwitchObjectToActiveDimensionScene(newItem);
                    newItem.GetComponent<Item>().SetPosition();
                }
            }

            skipItem = false;
        }
    }

    public void SwitchObjectToBootStrapScene(Item item)
    {
        item.transform.SetParent(null, true);
        Scene bootstrapScene = SceneManager.GetSceneByName(_bootstrapScene);
        SceneManager.MoveGameObjectToScene(item.gameObject, bootstrapScene);
    }

    public void SwitchObjectToActiveDimensionScene(Item item)
    {
        item.transform.SetParent(null, true);
        Scene currentScene = SceneLoader.Instance.GetCurrentDimensionScene();
        if (currentScene.isLoaded == false)
        {
            Debug.Log("<color=#ff0000> Attempted to switch object to unloaded scene </color>");
        }

        SceneManager.MoveGameObjectToScene(item.gameObject, currentScene);
    }

    public void SwitchObjectToActiveDimensionScene(GameObject item)
    {
        item.transform.SetParent(null, true);
        Scene currentScene = SceneLoader.Instance.GetCurrentDimensionScene();
        if (currentScene.isLoaded == false)
        {
            Debug.Log("<color=#ff0000> Attempted to switch object to unloaded scene </color>");
        }

        SceneManager.MoveGameObjectToScene(item.gameObject, currentScene);
    }

    [ContextMenu("Check Item Statutues")]
    public void CheckCurrentItemStatus(Item item)
    {
        if (item.GetItemDataSO().IsHeldByPlayer == true)
        {
            // Player is currently holding item. Ignore
            return;
        }
        if (currentItemsDictionary.ContainsKey(item) == false) return;
            
        // Are we destroyd or not
        ItemDataSO currentItemDataSO = currentItemsDictionary[item];
        if (currentItemDataSO.isDestroyed == true)
        {
           // Debug.Log("Item is destroyed" + item.gameObject.name);
            item.gameObject.SetActive(false);
            return;
        }
        else
        {
            item.gameObject.SetActive(true);
        }

        // Should we be visible in this dimension;
        if (currentItemDataSO.CurrentDimension != DimensionManager.Instance.CurrentDimension)
        {
            // Item should not be active in this dimension
            //Debug.Log(item.gameObject.name + "Does not belong in this dimension: Disabled", item.gameObject);
            item.gameObject.SetActive(false);
            return;
        }

        // Have we been moved to another dimension?
        foreach (var item1 in _allItems)
        {
            var obj = item1.GetComponent<Item>();
            var data = obj.GetItemDataSO();
            if (data.StartingDimension != data.CurrentDimension)
            {
                if (_interDimensionalItems.Contains(data) == false)
                {
                    _interDimensionalItems.Add(data);
                    //Debug.Log("Added " + data.name + " to interdimensional objects");
                }
            }
        }
        item.SetPosition();

    }

    public float EvaluateSlowEndCurve(float value)
    {
        return _slowEndCurve.Evaluate(value);
    }

    private GameObject GetItemFromAllItemsWithObjectData(ItemDataSO itemdata)
    {
        for (int i = 0; i < _allItems.Length; i++)
        {
            if (_allItems[i].GetComponent<Item>().GetItemDataSO() == itemdata)
            {
                return _allItems[i];
            }
        }
        return null;
    }

    public void AddItemToDictionary(Item item)
    {
        currentItemsDictionary.Add(item, item.GetItemDataSO());
        //Debug.Log("Adding to dictionary " + item.gameObject.name);
    }
    public void RemoveItemFromDictionary(Item item)
    {
        currentItemsDictionary.Remove(item);
    }

    public void RegisterDialoqueTrigger(DialoqueSO dialoqueSO)
    {
        if (_dialogues.Contains(dialoqueSO) == false)
        {
            _dialogues.Add(dialoqueSO);
        }
    }

    public void HandleDuplicateDialoque(DialoqueSO dialoqueSO, GameObject trigger)
    {
        if (_dialogues.Contains(dialoqueSO) == true)
        {
            trigger.SetActive(false);
        }
    }

    public class GameState
    {
        public static void ResetGameState()
        {
            HasSuit = false;
            PowerOnline = false;
        }

        // DOES THE PLAYER HAVE THE SUIT.
        public static bool HasSuit { get; private set; }
        public static bool PowerOnline { get; private set; }
       

        public static void SetPlayerHasSuit(bool hasSuit)
        {
            HasSuit = hasSuit;
        }
        public static void SetPowerOnline(bool powerOnline) {
            PowerOnline = powerOnline;
        }
    }
}
