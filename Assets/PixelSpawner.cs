using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Card;
using Unity.VisualScripting;
using System.Collections;
using System.Threading.Tasks;


public class PixelSpawner : MonoBehaviour
{
    public GameObject pixelPrefab;
    public GameObject editorPrefab;
    public GameObject activPixelColor;
    public GameObject polzunok;
    public GameObject pereponka;
    public GameObject exampleColor;
    public GameObject ColliderCard;
    public GameObject CenterCard;
    public GameObject Border;
    public Camera Camera;

    public static EditorSettings MySettings;
    public static ConsoleText text = new ConsoleText();
    public static Inventory Inventory;
    private static Card NullCard;
    private static Case NullCase;
    public static Card selectedCard;
    public static EditorCard MyEditorCard;
    public static Statistics stats = new Statistics();
    public static Coin coin;
    public static AudioManager audioManager;
    Transform transformPereponka = null;

    public static Dictionary<string, Card> Cards = new Dictionary<string, Card>();
    public static List<Card> OnlyCards = new List<Card>();
    public static Dictionary<Card, GameObject> Centers = new Dictionary<Card, GameObject>();

    static public List<string> listOfHpSprits = new List<string>(Directory.GetFiles("Old_Sptites/sprites/Hp", "*.txt"));
    public static List<string> listOfArrmySpritsPower1 = new List<string>(Directory.GetFiles("Old_Sptites/sprites/army/power1", "*.txt"));
    public static List<string> listOfArrmySpritsPower2 = new List<string>(Directory.GetFiles("Old_Sptites/sprites/army/power2", "*.txt"));
    public static Dictionary<string, List<string>> EnemyLists = new Dictionary<string, List<string>>()
{
    {"slimes", new List<string>(Directory.GetFiles("Old_Sptites/sprites/enemy/slimes", "*.txt"))},
    {"undeads", new List<string>(Directory.GetFiles("Old_Sptites/sprites/enemy/undead", "*.txt"))},
    {"goblins", new List<string>(Directory.GetFiles("Old_Sptites/sprites/enemy/goblins", "*.txt"))},
    {"mags", new List<string>(Directory.GetFiles("Old_Sptites/sprites/enemy/mags", "*.txt"))},
    {"piphiis", new List<string>(Directory.GetFiles("Old_Sptites/sprites/enemy/piphii", "*.txt"))},
};
    public Dictionary<string, Color[,]> AllSprites = new Dictionary<string, Color[,]>();

    public static int dungenLevel = 0;
    public static int countDcard = 0;
    public static int countKillcard = 10;
    public static bool stop = false;
    private static int localX = 80;
    private static int localY = 20;
    private static bool start = true;
    public static double volume = 14;
    public static Color selectColor = Color.red;
    public static Color unSelectColor = Color.gray;
    public bool EnableControl = true;
    public static bool animationEnable = true;
    public static Dictionary<int, List<int>> ChestLins = new Dictionary<int, List<int>>();

    public enum location
    {
        Dungon,
        Chest,
        Shop,
        Menu,
        Settings,
        GameOver,
        Ststistics,
        LoadSave,
        Editor
    }
    public enum axes
    {
        Right,
        Left,
        Up,
        Down
    }
    public enum controlMod
    {
        Keyboard,
        Mouse,
        GamePad,
        SilaMisli
    }
    public static location nowLocation = location.Menu;
    public static controlMod controlModNow = controlMod.Mouse;

    public static Dictionary<axes, Func<int, bool>> expressions = new Dictionary<axes, Func<int, bool>>
        { { axes.Up, (i) => (OnlyCards[i].Ymain + 4) < localY },
          { axes.Down, (i) => (OnlyCards[i].Ymain + 4) > localY },
          { axes.Right, (i) => (OnlyCards[i].Xmain + 2) > localX },
          { axes.Left, (i) => (OnlyCards[i].Xmain + 2) < localX }
        };
    public static List<AnimationManager> StackAnime = new List<AnimationManager>();
    public static System.Random random = new System.Random();
    public static Dictionary<string, object> save;
    
    public HashSet<Pixel> AllText = new HashSet<Pixel>();
    public HashSet<Pixel> BorderHashSet = new HashSet<Pixel>();
    public HashSet<Collider> colliders = new HashSet<Collider>();
    public static HashSet<Pixel> deletedPixels = new HashSet<Pixel>();
    public static List<HashSet<Pixel>> valumeText = new List<HashSet<Pixel>>();
            
    private GamePade_KeyBoard controls;
    private float _moveInputUpDown;
    private float _moveInputRL;
    bool moveEnable;
    bool selectDraw = false;
    bool follow = false;
    bool saveMove = false;

    private void Awake()
    {
        audioManager = new AudioManager();
        controls = new GamePade_KeyBoard();
        NullCard = new Card(this, 0, 0);       
        Inventory = new Inventory(this);
        coin = new Coin(this, 6, 114, 30);
        NullCase = new Case(this, 0, 0);
        for (int i = 0; i < Inventory.CardInventory.Length; i++)
        {
            Inventory.CardInventory[i] = new List<Card>();
        }
        Inventory.ActivSlot = Inventory.CardInventory[1];

        stats.startData = DateTime.Now;
        save = new Dictionary<string, object>
        {
            {"dungenLevel", dungenLevel},
            {"volume", volume},
            {"localX", localX},
            {"localY", localY},
            {"nowLocation", nowLocation},
            {"countDcard", countDcard},
            {"stop", stop},
            {"start", start},
            {"selectedCard", selectedCard},
            {"Cards", Cards},
            {"OnlyCards", OnlyCards},
            {"coin", coin},
            {"CardInventory", Inventory.CardInventory},
            {"ChestLins", ChestLins},
        };
    }
    private async void Start()
    {
        NewBorder(Color.black);
        await NextLevl();
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    void Update()
    {
        if (EnableControl)
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(ray.origin, ray.direction * 150f, Color.green);

            RaycastHit hit;
            
            if (controlModNow == controlMod.Mouse && Physics.Raycast(ray, out hit, 150f))
            {
                if (nowLocation == location.Editor)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (hit.collider.transform.name == "EditorPixel(Clone)")
                        {
                            switch (MySettings.editorMod)
                            {
                                case EditorSettings.ToolMods.defaulBrash:
                                    EditorCard.DrawSinglPixel(hit.collider.transform.GetComponent<Pixel>());
                                    break;
                                case EditorSettings.ToolMods.filling:
                                    EditorPixelInfo editorPixelInfo = hit.collider.transform.gameObject.GetComponent<EditorPixelInfo>();
                                    MyEditorCard.Filling(editorPixelInfo.Xcolors, editorPixelInfo.Ycolors);
                                    break;
                                case EditorSettings.ToolMods.pipette:
                                    MySettings.activPixelColor.GetComponent<Renderer>().material.color = hit.collider.transform.gameObject.GetComponent<Renderer>().material.color;
                                    MySettings.Synchronize();
                                    break;
                            }
                            
                        }
                       
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        

                        switch (hit.collider.transform.name)
                        {                           
                            case "pereponka(Clone)":
                                transformPereponka = hit.collider.transform.gameObject.transform;
                                follow = true;
                                //MySettings.cursorPosition = hit.collider.transform.position;
                                break;
                            case "ExampleColor(Clone)":
                                MySettings.activPixelColor.GetComponent<Renderer>().material.color = hit.collider.transform.gameObject.GetComponent<Renderer>().material.color;
                                MySettings.Synchronize();
                                break;
                            case "EditorPixel(Clone)":
                                if (MyEditorCard.saveIndex > 0) MyEditorCard.ClearCanceledSaves();
                                saveMove = true;
                                break;
                        }
                    }
                    if (saveMove && Input.GetMouseButtonUp(0))
                    {
                        saveMove = false;
                        MyEditorCard.UpdateField();
                        MyEditorCard.AddSave();
                    }
                }
                if (follow && hit.point.y < (polzunok.transform.position.y + polzunok.transform.localScale.y / 2) && hit.point.y > (polzunok.transform.position.y - polzunok.transform.localScale.y / 2))
                {
                    transformPereponka.position = new Vector3(transformPereponka.position.x, hit.point.y, transformPereponka.position.z);
                    MySettings.CheckRGB();
                }
                if (hit.collider.name == "HitBox")
                {
                    Transform parentTransform = hit.transform.parent;
                    Collider colliderScript = parentTransform.GetComponent<Collider>();
                    if (colliderScript != null)
                    {
                        Card card = colliderScript.MyCard;
                        localX = card.Xmain + 2;
                        localY = card.Ymain + 4;
                        ChekeSelect(selectDraw);                        
                    }
                }
                else if (selectedCard != null)
                {
                    selectedCard.UnSelect(unSelectColor);
                    selectedCard = null;
                    for (int i = 0; i < OnlyCards.Count; i++)
                    {
                        if (OnlyCards[i].enabletToSelect)
                        {
                            OnlyCards[i].UnSelect(unSelectColor);
                        }
                    }
                }
            }

            _moveInputUpDown = controls.Move.UpDown.ReadValue<float>();
            _moveInputRL = controls.Move.RL.ReadValue<float>();
            //Debug.Log(moveEnable);

            switch (controlModNow)
            {
                case controlMod.GamePad:
                case controlMod.Keyboard:
                    if (_moveInputUpDown == 0 && _moveInputRL == 0) moveEnable = true;
                    if (moveEnable)
                    {
                        if (_moveInputUpDown != 0 || _moveInputRL != 0) moveEnable = false;

                        if (_moveInputRL < 0) FindCard(axes.Left);
                        if (_moveInputRL > 0) FindCard(axes.Right);
                        if (_moveInputUpDown < 0) FindCard(axes.Up);
                        if (_moveInputUpDown > 0) FindCard(axes.Down);
                    }

                    if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
                    {
                        InputInteractions();
                    }
                    if (Input.GetKeyDown(KeyCode.L) || Input.GetButtonDown("Fire2"))
                    {
                        InputLock();
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        InputSwapActivSlot();
                    }
                    break;
                case controlMod.Mouse:
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (selectedCard != null && selectedCard.Enabled)
                        {
                            selectDraw = true;
                            ChekeSelect();
                        }
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (selectDraw) InputInteractions();
                        follow = false;
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                        if (selectedCard != null) InputLock();
                    }
                    if (Input.GetMouseButtonUp(2))
                    {
                        InputSwapActivSlot();
                    }

                    break;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (OnlyCards.Count != 0 && (nowLocation == location.Dungon || nowLocation == location.Chest || nowLocation == location.Shop))
                {
                    SaveProgres();
                    stats.TimeCounter();
                    stats.SaveStatistic();
                }
                nowLocation = location.Menu;
                coin.ClaerHashSet(coin.moneyText);
                coin.ClaerHashSet(coin.moneyImage);
                Inventory.CleenAll();
                NextLevl();
            }                       
        }

    }
    public static void QuitGame()
    {
        Debug.Log("Exiting game");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        if (selectedCard != null && (nowLocation == location.Dungon || nowLocation == location.Chest || nowLocation == location.Shop))
        {
            SaveProgres();
            stats.TimeCounter();
            stats.SaveStatistic();
        }
    }      

    public Pixel CreatePixel(int x, int y, Color color, HashSet<Pixel> pixels = null, int z = -1, GameObject colliderCard = null, GameObject pixelPref = null)
    {
        if (pixelPref == null) pixelPref = pixelPrefab;

        GameObject pixelObject = Instantiate(pixelPref, new Vector3(x, y, 0), Quaternion.identity);
        Pixel pixelComponent = pixelObject.GetComponent<Pixel>();
        if (pixelComponent != null)
        {
            pixelComponent.Setup(new Vector3(x, y, z), color);
        }
        if (pixels != null) pixels.Add(pixelComponent);
        if (colliderCard != null)
        {
            Transform gameObjectCollider = colliderCard.transform;
            pixelComponent.transform.SetParent(gameObjectCollider);
        }
        return pixelComponent;
    }
    public static void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject.gameObject);
    }
    public static GameObject InstantiateObject(GameObject gameObject, Vector3 vector3, Quaternion quaternion, HashSet<GameObject> objects = null)
    {
        GameObject instantiateObject = Instantiate(gameObject, vector3, quaternion);
        if (objects != null) objects.Add(instantiateObject);
        return instantiateObject;
    }
    public Collider CreateCollider(Card card)
    {
        GameObject colliderObject = Instantiate(ColliderCard, new Vector3(card.Xmain, card.Ymain, card.Zmain), Quaternion.identity);
        Collider colliderComponent = colliderObject.GetComponent<Collider>();
        if (colliderComponent != null)
        {
            colliderComponent.Setup(new Vector3(card.Xmain - 0.5f, card.Ymain - 0.5f, card.Zmain));
            colliderComponent.transform.localScale = new Vector3(card.Weight + 1, card.Hight + 1, 1);
        }
        card.Collider = colliderObject;
        colliderComponent.MyCard = card;
        colliders.Add(colliderComponent);
        return colliderComponent;
    }
    public void SetCenter(Card card)
    {
        card.CenterPoint = Instantiate(CenterCard, new Vector3(card.Xmain + card.Weight / 2, card.Ymain + card.Hight / 2, -1), Quaternion.identity);
        Centers.Add(card, card.CenterPoint);
    }
    public static void ChangeParentPosition(GameObject parent, Vector3 newPosition)
    {

        Dictionary<Transform, Vector3> childPositions = new Dictionary<Transform, Vector3>();

        foreach (Transform child in parent.transform)
        {
            childPositions[child] = child.position;
        }

        parent.transform.position = newPosition;

        foreach (Transform child in parent.transform)
        {
            child.position = childPositions[child];
        }
    }

    public void NewBorder(Color BoardColor, int ConsolWidth = 200, int ConsolHeight = 124, bool start = true, int offset = 2, int offsetY = 0)
    {
        if (BorderHashSet.Count == 0)
        {
            for (int i = offset; i < ConsolWidth - offset; i++)
            {
                CreatePixel(i, offset + offsetY, BoardColor, BorderHashSet, -1, Border);
                CreatePixel(i, ConsolHeight - offset + offsetY, BoardColor, BorderHashSet, -1, Border);
                CreatePixel(i, offset + 20 + offsetY, BoardColor, BorderHashSet, -1, Border);
            }

            for (int i = offset; i < ConsolHeight - offset + 1; i++)
            {
                CreatePixel(offset, i + offsetY, BoardColor, BorderHashSet, -1, Border);
                CreatePixel(offset - 1, i + offsetY, BoardColor, BorderHashSet, -1, Border);

                CreatePixel(ConsolWidth - offset - 1, i + offsetY, BoardColor, BorderHashSet, -1, Border);
                CreatePixel(ConsolWidth - offset, i + offsetY, BoardColor, BorderHashSet, -1, Border);

                if (i < 21)
                {
                    CreatePixel(ConsolWidth - offset - 66, i + offsetY + 1, BoardColor, BorderHashSet, -1, Border);
                    CreatePixel(ConsolWidth - offset - 131, i + offsetY + 1, BoardColor, BorderHashSet, -1, Border);
                }
            }
        }
        else RepaintNewBorder(BorderHashSet, BoardColor);


    }
    public void RepaintNewBorder(HashSet<Pixel> hashSet, Color color)
    {
        foreach (Pixel pixel in hashSet)
        {
            pixel.RendererPixel.material.color = color;
        }
    }

    void ClearCenters(Dictionary<Card, GameObject> Centers)
    {
        foreach (GameObject CenterObject in Centers.Values)
        {
            if (CenterObject != null && (CenterObject.GetComponent<AnimationScript>().Card.actionsForCard != actions.InInventory || CenterObject.GetComponent<AnimationScript>().Card.actionsForCard == actions.Deleted))
                Destroy(CenterObject.gameObject);
        }
        Centers.Clear();
    }
    void ClearText()
    {
        foreach (Pixel pixel in AllText)
        {
            if (pixel != null) Destroy(pixel.gameObject);
            else deletedPixels.Add(pixel);
        }
        AllText.Clear();
        StackAnime.Clear();
    }
    void ClearTextAnimation()
    {
        foreach (Pixel pixel in AllText)
        {
            if (pixel == null) continue;
            GameObject PixelObject = pixel.transform.gameObject;
            AnimationManager anime = pixel.GetComponent<AnimationManager>();
            if (anime == null) anime = pixel.AddComponent<AnimationManager>();
            anime.enabled = true;
            ////anime.Mod = MyMod;

            anime.Initialize(PixelObject, new Vector3(pixel.transform.position.x, pixel.transform.position.y, pixel.transform.position.z + 1.5f), Quaternion.identity, 0.5f);
            anime.StartInterpolation();
        }
        foreach (HashSet<Pixel> pixelSet in valumeText)
        {
            foreach (Pixel pixel in pixelSet)
            {
                if (pixel == null) continue;
                GameObject PixelObject = pixel.transform.gameObject;
                AnimationManager anime = pixel.GetComponent<AnimationManager>();
                if (anime == null) anime = pixel.AddComponent<AnimationManager>();
                anime.enabled = true;
                ////anime.Mod = MyMod;

                anime.Initialize(PixelObject, new Vector3(pixel.transform.position.x, pixel.transform.position.y, pixel.transform.position.z + 1.5f), Quaternion.identity, 0.5f);
                anime.StartInterpolation();
            }
        }
    }
    void ClearColliders()
    {
        foreach (Collider collider in colliders)
        {
            if (collider != null) Destroy(collider.gameObject);
        }
        colliders.Clear();
    }
    void ClaerValumeText()
    {
        foreach (HashSet<Pixel> pixelSet in valumeText)
        {
            ClaerHashSet(pixelSet);
        }
    }
    void ClaerHashSet(HashSet<Pixel> hashSet)
    {
        foreach (Pixel pixel in hashSet)
        {
            DestroyObject(pixel.gameObject);
        }
        hashSet.Clear();
    }

    public static void StartNew()
    {
        selectedCard = null;
        start = true;

        localX = 100;
        localY = 48;

        string startKey = FindCard(axes.Up, false);
        selectedCard = Cards[startKey];
        selectedCard.Enabled = true;

    }
    public async Task<int[]> NewRender(int[] numOfCard = null, string mod = "Normal", int? Xsize = null, int? Ysize = null, int offsetX = 0, int offsetY = 3)
    {
        Dictionary<Type, int> enemyChances = new Dictionary<Type, int>()
{
    { typeof(Slime), 25 }, 
    { typeof(Undead), 25 },
    { typeof(Mag), 20 },
    { typeof(Goblin), 20 },
    { typeof(Piphiy), 10 },

};

        if (Xsize == null || Ysize == null)
        {
            Xsize = 200 - 6;
            Ysize = 140 - 16;
        }

        int countOfLins = 0;
        foreach (int i in numOfCard)
        {
            if (i > 0) countOfLins++;
            else numOfCard = RemoveElementFromArray(numOfCard, Array.IndexOf(numOfCard, 0));
        }

        int lastY = (int)((Ysize - countOfLins * 10) % (countOfLins + 1));
        int spaceY = (int)((Ysize - countOfLins * 10) / (countOfLins + 1));

        for (int i = (int)Ysize - spaceY - (int)(lastY / 2) - 7, numOfLine = 0; i >= 2; i -= 10 + spaceY, numOfLine++)
        {

            int lastX = (int)((Xsize - numOfCard[numOfLine] * 12) % (numOfCard[numOfLine] + 1));
            int spaceX = (int)((Xsize - numOfCard[numOfLine] * 12) / (numOfCard[numOfLine] + 1));

            for (int j = spaceX + (int)(lastX / 2) + 5; j <= Xsize - 8; j += 12 + spaceX)
            {

                int randomNumber;
                if (mod == "Chest" || mod == "Shop_defaulCase") randomNumber = random.Next(0, 10);
                else if (mod == "Shop_HpCase") randomNumber = random.Next(0, 5);
                else if (mod == "Shop_ArrmyCase") randomNumber = random.Next(5, 11);
                else randomNumber = random.Next(0, 100);

                string value;
                Card card = new Card(this, 0, 0);

                if (randomNumber < 5)
                {
                    value = random.Next(1, 5 + dungenLevel).ToString();
                    card = new HP(this, j + offsetX, i + offsetY, value);
                }
                if (randomNumber >= 5 && randomNumber <= 10)
                {
                    value = random.Next(1, 5 + dungenLevel).ToString();
                    card = new Arrmy(this, j + offsetX, i + offsetY, value);
                }
                if (randomNumber > 10)
                {
                    value = random.Next(1 + dungenLevel, 5 + dungenLevel).ToString();
                    //Enemy newEnemy = new Enemy(this, j + offsetX, i + offsetY, value);

                    randomNumber = random.Next(0, 100);
                    Type enemyType = null;
                    int cumulativeChance = 0;
                    foreach (var kvp in enemyChances)
                    {
                        cumulativeChance += kvp.Value;
                        if (randomNumber < cumulativeChance)
                        {
                            enemyType = kvp.Key;
                            break;
                        }
                    }
                    if (enemyType != null)
                        card = (Enemy)Activator.CreateInstance(enemyType, this, j + offsetX, i + offsetY, value);
                    else Debug.Log("null enemy");


                }

                //card.Zmain = (int)card.Zmain + 5;
                card.DrawCard();
                //Transform CenterCard = card.CenterPoint.transform.Find("CenterPoint");
                
                card.CenterPoint.transform.position = new Vector3(card.CenterPoint.transform.position.x, card.CenterPoint.transform.position.y, card.CenterPoint.transform.position.z + 5);

                if (mod == "Shop_defaulCase" || mod == "Shop_HpCase" || mod == "Shop_ArrmyCase")
                {

                    card.actionsForCard = actions.Visible;
                    card.Enabled = true;
                    card.DrawCard();
                    card.VisibleRender();
                    card.InCace = true;
                }
                string vector = (card.Xmain + 2).ToString() + "; " + (card.Ymain + 4).ToString();
                Cards.Add(vector, card);
                OnlyCards.Add(card);
            }

        }
        if (mod == "Normal")
        {
            int randomCard = random.Next(0, OnlyCards.Count);
            OnlyCards[randomCard].ClaerHashSet(OnlyCards[randomCard].BorderPixels);
            Door doorCard = new Door(this, OnlyCards[randomCard].Xmain, OnlyCards[randomCard].Ymain);
            string key = (doorCard.Xmain + 2).ToString() + "; " + (doorCard.Ymain + 4).ToString();
            Cards[key] = doorCard;
            OnlyCards[randomCard] = doorCard;
            doorCard.DrawCard();
            doorCard.CenterPoint.transform.position = new Vector3(doorCard.CenterPoint.transform.position.x, doorCard.CenterPoint.transform.position.y, doorCard.CenterPoint.transform.position.z + 5);

            int randMagz = random.Next(0, 101);
            if (randMagz <= 50)
            {
                int randomCardShop = randCard();
                OnlyCards[randomCardShop].ClaerHashSet(OnlyCards[randomCardShop].BorderPixels);
                Shop shopCard = new Shop(this, OnlyCards[randomCardShop].Xmain, OnlyCards[randomCardShop].Ymain);
                string keyShop = (shopCard.Xmain + 2).ToString() + "; " + (shopCard.Ymain + 4).ToString();
                Cards[keyShop] = shopCard;
                OnlyCards[randomCardShop] = shopCard;
                shopCard.DrawCard();
                shopCard.CenterPoint.transform.position = new Vector3(shopCard.CenterPoint.transform.position.x, shopCard.CenterPoint.transform.position.y, shopCard.CenterPoint.transform.position.z + 5);
            }
        }
        if (nowLocation != location.Shop) await AnimationRenderTask();
        
        return numOfCard;

    }
    public async Task NextLevl()
    {
        EnableControl = false;
        ClearTextAnimation();
        await ClearTask();
        EnableControl = true;
        OnlyCards.Clear();
        Cards.Clear();
        ChestLins.Clear();
        deletedPixels.Clear();
        ClearText();
        ClaerValumeText();
        ClearColliders();
        ClearCenters(Centers);
        stop = true;
        selectedCard = null;

        switch (nowLocation)
        {
            case location.Dungon:
                AudioManager.PlaySound("Upbeat", 1f, true);
                nowLocation = location.Chest;
                NewBorder(Color.yellow);

                text.DrawText(this, "CHEST", 63, 70, Color.HSVToRGB(60f / 360f, 1f, 0.5f), 3, AllText);
                while (PixelSpawner.StackAnime.Count > 0)
                {
                    await Task.Delay(100);
                }
                await Task.Delay(500);
                ClearTextAnimation();
                while (PixelSpawner.StackAnime.Count > 0)
                {
                    await Task.Delay(100);
                }
                ClearText();
                //Debug.Log(countKillcard);
                int[] myCards = GenerateRandomArray(countKillcard);
                //int[] myCards = {4,2,1,5};
                Card door = new Door(this, 184, 25);                
                AddSingleCard(door);
                door.CenterPoint.transform.position = new Vector3(door.CenterPoint.transform.position.x, door.CenterPoint.transform.position.y, door.CenterPoint.transform.position.z + 5);

                EnableControl = false;
                await NewRender(myCards, "Chest");
                EnableControl = true;                

                break;
            case location.Chest:
                AudioManager.PlaySound("Game_Land", 1f, true);
                countKillcard = 0;
                countDcard = 0;
                dungenLevel++;
                if (dungenLevel > stats.MyStatistics["MaxDungeon"]) stats.MyStatistics["MaxDungeon"] = dungenLevel;

                nowLocation = location.Dungon;
                NewBorder(Color.HSVToRGB(0f, 0f, 0.5f));

                text.DrawText(this, "DUNGEON " + dungenLevel.ToString(), 30, 70, Color.HSVToRGB(0f, 1f, 0.5f), 3, AllText);
                while (PixelSpawner.StackAnime.Count > 0)
                {
                    await Task.Delay(100);
                }
                await Task.Delay(500);
                ClearTextAnimation();
                while (PixelSpawner.StackAnime.Count > 0)
                {
                    await Task.Delay(100);
                }
                ClearText();

                int[] myCardsDungeon = GenerateRandomArray(2, 5, 1, 11);
                //int[] myCards = {10,2,1,5};
                foreach (int i in myCardsDungeon) countDcard += i;

                EnableControl = false;
                await NewRender(myCardsDungeon, "Normal");
                //StartNew();
                EnableControl = true;

                break;
            case location.Shop:
                AudioManager.PlaySound("Game", 1f, true);
                NewBorder(Color.blue);

                text.DrawText(this, "SHOP", 70, 70, Color.HSVToRGB(300f / 360f, 1f, 0.5f), 3, AllText);
                while (PixelSpawner.StackAnime.Count > 0)
                {
                    await Task.Delay(100);
                }
                await Task.Delay(500);
                ClearTextAnimation();
                while (PixelSpawner.StackAnime.Count > 0)
                {
                    await Task.Delay(100);
                }
                ClearText();

                int countOfCardInShop = (int)(countDcard / 2);
                if (countOfCardInShop > 15) countOfCardInShop = 15;
                int[] myCardsShop = Case.GenerateArrayForCase(countOfCardInShop);
                NullCase.NewRender(myCardsShop, "", 70, 70, 0, 12);
                AddSingleCard(new Door(this, 184, 25));

                AddSingleCard(new Sell(this, 8, 90));
                //AddSingleCard(new SellLast(this, 21, 90));
                break;
            case location.Menu:
                AudioManager.PlaySound("neon-gaming", 1f, true);
                text.DrawText(this, "CARD AND DUNGEON", 15, 100, Color.HSVToRGB(240f / 360f, 1f, 0.5f), 2, AllText);
                StartCard startButton = new StartCard(this, 80, 80);
                startButton.AddMenuCard();

                ContinueCard ContinueButton = new ContinueCard(this, 80, 80 - 12);
                ContinueButton.AddMenuCard();

                SettingsCard SettindsButton = new SettingsCard(this, 80, 80 - 12 * 2);
                SettindsButton.AddMenuCard();

                ExitCard ExitButton = new ExitCard(this, 80, 80 - 12 * 3);
                ExitButton.AddMenuCard();
                selectedCard = startButton;
                localX = selectedCard.Xmain + 2;
                localY = selectedCard.Ymain + 4;
                ChekeSelect();
                break;
            case location.Settings:
                text.DrawText(this, "SETTINGS", 61, 100, Color.HSVToRGB(240f / 360f, 1f, 0.5f), 2, AllText);
                text.DrawText(this, "VOLUME", 40, 80, Color.HSVToRGB(60f / 360f, 1f, 0.5f), 1, AllText);
                text.DrawText(this, "Control Mod: ", 40, 65, Color.HSVToRGB(60f / 360f, 1f, 0.5f), 1, AllText);
                for (int i = 80, j = 0; j < volume; j++, i += 5)
                {
                    HashSet<Pixel> polosochka = new HashSet<Pixel>();
                    valumeText.Add(polosochka);
                    text.DrawText(this, "_", i, 80, Color.gray, 1, polosochka);
                }

                VolumeCard plus = new VolumeCard(this, 153, 80);
                plus.AddMenuCard();

                VolumeCard minus = new VolumeCard(this, 166, 80, "", "", false);
                minus.AddMenuCard();

                ToMenuCard ToMenuButton = new ToMenuCard(this, 120, 30);
                ToMenuButton.AddMenuCard(-1);

                StatisticCard StatisticCard = new StatisticCard(this, 30, 30);
                StatisticCard.AddMenuCard(-2);

                MouseControl MouseControlCard = new MouseControl(this, 157, 63);
                MouseControlCard.AddMenuCard();

                KeyboardControl KeyboardControlCard = new KeyboardControl(this, 105, 63);
                KeyboardControlCard.AddMenuCard();

                Editor editor = new Editor(this, 5, 100);
                editor.AddMenuCard();

                selectedCard = ToMenuButton;
                localX = selectedCard.Xmain + 2;
                localY = selectedCard.Ymain + 4;
                ChekeSelect();
                break;
            case location.GameOver:
                AudioManager.PlaySound("system-error", 1f, true);
                if (File.Exists(Application.persistentDataPath + "/saves/data.json")) File.Delete(Application.persistentDataPath + "/saves/data.json");
                coin.Value = 0;

                text.DrawText(this, "GAME OVER", 33, 80, Color.HSVToRGB(0f, 1f, 0.5f), 3, AllText);

                ReStartCard restartButton = new ReStartCard(this, 78, 45, "", "RESTART");
                restartButton.AddMenuCard();

                ToMenuCard ToMenuButtonGameOver = new ToMenuCard(this, 76, 60);
                ToMenuButtonGameOver.AddMenuCard(-1);

                selectedCard = ToMenuButtonGameOver;
                localX = selectedCard.Xmain + 2;
                localY = selectedCard.Ymain + 4;
                ChekeSelect();
                break;
            case location.Ststistics:
                text.DrawText(this, "Count Of Runs", 5, 100, Color.HSVToRGB(60f / 360f, 1f, 0.5f), 1, AllText);
                text.DrawText(this, stats.MyStatistics["CountOfRuns"].ToString(), 90, 100, Color.HSVToRGB(120f / 360f, 1f, 0.5f), 1, AllText);

                text.DrawText(this, "Count Of Kills", 5, 93, Color.HSVToRGB(60f / 360f, 1f, 0.5f), 1, AllText);
                text.DrawText(this, stats.MyStatistics["CountOfKills"].ToString(), 90, 93, Color.HSVToRGB(120f / 360f, 1f, 0.5f), 1, AllText);

                text.DrawText(this, "Count Of PickUp", 5, 86, Color.HSVToRGB(60f / 360f, 1f, 0.5f), 1, AllText);
                text.DrawText(this, stats.MyStatistics["CountOfPickUp"].ToString(), 90, 86, Color.HSVToRGB(120f / 360f, 1f, 0.5f), 1, AllText);

                text.DrawText(this, "Max Dungeon", 5, 79, Color.HSVToRGB(60f / 360f, 1f, 0.5f), 1, AllText);
                text.DrawText(this, stats.MyStatistics["MaxDungeon"].ToString(), 90, 79, Color.HSVToRGB(120f / 360f, 1f, 0.5f), 1, AllText);

                text.DrawText(this, "Time Spent", 5, 72, Color.HSVToRGB(60f / 360f, 1f, 0.5f), 1, AllText);
                text.DrawText(this, stats.MyStatistics["TimeSpent"].ToString() + " min", 90, 72, Color.HSVToRGB(120f / 360f, 1f, 0.5f), 1, AllText);

                ToMenuCard ToMenuButtonStstistics = new ToMenuCard(this, 120, 40);
                ToMenuButtonStstistics.AddMenuCard(-1);

                selectedCard = ToMenuButtonStstistics;
                localX = selectedCard.Xmain + 2;
                localY = selectedCard.Ymain + 4;
                ChekeSelect();
                break;
            case location.LoadSave:
                LoadSave();
                stop = true;
                DrawSave();
                break;
            case location.Editor:
                EditorCard editorCard = new EditorCard(this,95,63);
                MyEditorCard = editorCard;
                editorCard.pixelPrefab = editorPrefab;
                editorCard.DrawBorder(Color.gray);
                Vector3 cameraPoistion = new Vector3(100.5f, 71.5f, -40f);
                SinglAnimation(Camera.gameObject, cameraPoistion, Quaternion.identity, 0.5f);
                editorCard.ClearOrFill();
                editorCard.DrawField();

                MySettings = new EditorSettings();
                MySettings.Init(activPixelColor, polzunok, pereponka, exampleColor);
                StartCoroutine(SetUpSettingsFromMainThread());
              
                AddEditorButton(new Escape(this, 110, 83), 1);
                AddEditorButton(new SaveButton(this, 73, 53), 0);
                AddEditorButton(new LoadButton(this, 96, 53), 0);
                AddEditorButton(new ClearButton(this, 102, 61), 0);
                AddEditorButton(new DefaultBrash(this, 109, 69), 0);
                AddEditorButton(new Filling(this, 115, 69), 0);
                AddEditorButton(new Pipette(this, 121, 69), 0);
                AddEditorButton(new Cancel(this, 92, 81), 0);
                AddEditorButton(new Return(this, 101, 81), 0);
                break;
        }
    }

    public void InputInteractions()
    {
        
        selectDraw = false;
        string key1 = (localX).ToString() + "; " + localY.ToString();
        if (selectedCard != null && Cards.ContainsKey(key1) && selectedCard.Enabled)
        {
            Card originCard = selectedCard;
            int originY = originCard.Ymain;
            if (nowLocation == location.Chest && selectedCard.actionsForCard == actions.Hide) Xinter();
            else
            {
                if (nowLocation is location.Dungon or location.Chest) Inventory.InventoryChecker();
                originCard.Interactions();
                if (ChestLins.ContainsKey(originY) && (Inventory.CardInventory[0].Contains(originCard) || Inventory.CardInventory[1].Contains(originCard)))
                {
                    if (!originCard.locked)
                        ChestLins[originY][1]++;

                    if (ChestLins[originY][1] >= (int)Math.Ceiling((double)ChestLins[originY][0] / 2) && ChestLins[originY][2] == 1)
                    {
                        StartCoroutine(DeleteLine(originY));
                        SelectLast();
                    }
                }
                originCard.locked = false;
            }


        }
    }
    public void InputLock()
    {
        int originY = selectedCard.Ymain;
        if (selectedCard != null && selectedCard.Enabled && nowLocation == location.Chest && ChestLins.ContainsKey(originY) && selectedCard.actionsForCard != actions.Hide)
        {
            if (selectedCard.locked)
            {
                selectedCard.locked = false;
                ChestLins[originY][1]--;
                selectedCard.ClaerHashSet(selectedCard.FramePixels);
                selectedCard.Select(selectColor);
                UpdateFrame();
            }
            else
            {
                selectedCard.locked = true;
                ChestLins[originY][1]++;
                selectedCard.DrawFrame(Color.gray);
                selectedCard.Select(selectColor);
                AudioManager.PlaySound("lock", 1f);
            }


            if (ChestLins[originY][1] >= (int)Math.Ceiling((double)ChestLins[originY][0] / 2) && ChestLins[originY][2] == 1)
            {
                StartCoroutine(DeleteLine(originY));
                SelectLast();
            }
        }
    }
    public void InputSwapActivSlot()
    {
        if (Inventory.ActivSlot == Inventory.CardInventory[1] && Inventory.CardInventory[2].Count > 0) Inventory.SwapActivSlot();
        else if (Inventory.ActivSlot == Inventory.CardInventory[2] && Inventory.CardInventory[1].Count > 0) Inventory.SwapActivSlot();
    }

    public void Xinter()
    {
        int originY = selectedCard.Ymain;
        int counter = 0;
        AudioManager.PlaySound("openLine", 1f);
        foreach (Card cardI in OnlyCards)
        {
            if (cardI.Ymain == originY)
            {
                //cardI.MyAnimator.SetStartInfo();

                cardI.MyAnimator.StartCoroutine(cardI.MyAnimator.ShowAnimation());
                cardI.VisibleRender();
                counter++;
            }
        }
        ChestLins.Add(originY, new List<int>() { counter, 0, 1 });
        ChekeSelect();
    }
    static void ChekeSelect(bool drawSeleck = true)
    {
        string key1 = (localX).ToString() + "; " + localY.ToString();

        if (selectedCard != null)
            selectedCard.UnSelect(unSelectColor);
        for (int i = 0; i < OnlyCards.Count; i++)
        {
            if (OnlyCards[i].enabletToSelect)
            {
                OnlyCards[i].UnSelect(unSelectColor);
            }
        }

        if (Cards.ContainsKey(key1))
        {
            Cards[key1].Select(selectColor, drawSeleck);
        }
    }
    public IEnumerator DeleteLine(int originY)
    {

        List<Coroutine> coroutines = new List<Coroutine>();
        List<Card> cardsToRemove = new List<Card>();

        for (int i = 0; i < OnlyCards.Count; i++)
        {
            if (OnlyCards[i].Ymain == originY && !OnlyCards[i].locked)
            {
                if (!OnlyCards[i].Enabled) continue;
                Coroutine coroutine = OnlyCards[i].MyAnimator.StartCoroutine(OnlyCards[i].MyAnimator.LeavePerformActionsWithDelays());
                coroutines.Add(coroutine);
                cardsToRemove.Add(OnlyCards[i]);
                OnlyCards[i].Enabled = false;
                Destroy(OnlyCards[i].Collider.gameObject); OnlyCards[i].Collider = null;                
            }
        }

        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }

        foreach (var card in cardsToRemove)
        {
            PixelSpawner.KillCard(card);
            OnlyCards.Remove(card);
        }

        if (OnlyCards.Count == 1) FindCard(axes.Up, true, true, true);
        ChestLins[originY][2] = 0;
    }
    public static void KillCard(Card card)
    {
        card.ClaerHashSet(card.ImagePixlels);
        card.ClaerHashSet(card.BorderPixels);
        card.ClaerHashSet(card.FramePixels);
        card.ClaerHashSet(card.ValuePixels);
        card.actionsForCard = actions.Deleted;
        card.select = false;

        string key = (card.Xmain + 2).ToString() + "; " + (card.Ymain + 4).ToString();
        Cards.Remove(key);
        OnlyCards.Remove(card);
    }
    public void SelectTheLine(int originY)
    {
        foreach (Card cardI in OnlyCards)
        {
            if (cardI.Ymain == originY)
            {
                cardI.Enabled = true;
                cardI.borderCondition = condition.InChest;
                cardI.DrawBorder(Color.green, null, false);
            }
        }
        ChekeSelect();
        FindCard(axes.Down, true, true, false);
    }
    public void SelectLast()
    {
        Card lastLineCard = new Card(this, 0, 300);
        foreach (Card cardI in OnlyCards)
        {
            if (cardI.Ymain < lastLineCard.Ymain && cardI.actionsForCard == actions.Hide) lastLineCard = cardI;
        }
        int lastLineY = lastLineCard.Ymain;
        if (lastLineCard.borderCondition != condition.InChest) SelectTheLine(lastLineY);
    }

    public void AddSingleCard(Card singlCard)
    {
        singlCard.actionsForCard = actions.Visible;
        singlCard.DrawCard();
        singlCard.VisibleRender();
        singlCard.Enabled = true;
        string key = (singlCard.Xmain + 2).ToString() + "; " + (singlCard.Ymain + 4).ToString();
        Cards.Add(key, singlCard);
        OnlyCards.Add(singlCard);
    }
    private void AddEditorButton(MenuCard card, int offset)
    {
        card.AddMenuCard(offset);
        card.CenterPoint.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        card.Collider.transform.GetChild(0).localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    IEnumerator SetUpSettingsFromMainThread()
    {      
        yield return null;

        MySettings.SetUpSettings();
    }

    public static void SaveProgres()
    {
        save = new Dictionary<string, object>
                    {
                        {"dungenLevel", dungenLevel},
                        {"volume", volume},
                        {"localX", localX},
                        {"localY", localY},
                        {"nowLocation", nowLocation},
                        {"countDcard", countDcard},
                        {"stop", stop},
                        {"start", start},
                        {"selectedCard", selectedCard},
                        {"Cards", Cards},
                        {"OnlyCards", OnlyCards},
                        {"coin", coin},
                        {"CardInventory", Inventory.CardInventory},
                        {"ChestLins", ChestLins},
                    };

        //GameDataSaver.SaveDictionary(save);
        //string huinia = Save.SerializeCard(save);
        StateManager.SaveState(save);
    }
    public void LoadSave()
    {
        var loadedState = StateManager.LoadState();
        save = loadedState;

        dungenLevel = Convert.ToInt32(save["dungenLevel"]);
        volume = Convert.ToDouble(save["volume"]);
        localX = Convert.ToInt32(save["localX"]);
        localY = Convert.ToInt32(save["localY"]);
        nowLocation = (location)(Convert.ToInt32(save["nowLocation"]));
        countDcard = Convert.ToInt32(save["countDcard"]);
        stop = (bool)(save["stop"]);
        start = (bool)(save["start"]);

        //selectedCard = (Card)(save["selectedCard"]);
        Cards = (Dictionary<string, Card>)(save["Cards"]);
        OnlyCards = (List<Card>)(save["OnlyCards"]);
        coin = (Coin)(save["coin"]);
        Inventory.CardInventory = (List<Card>[])(save["CardInventory"]);
        ChestLins = (Dictionary<int, List<int>>)(save["ChestLins"]);
    }
    public void DrawSave()
    {
        switch (nowLocation)
        {
            case location.Dungon:
                NewBorder(Color.HSVToRGB(0f, 0f, 0.5f));
                AudioManager.PlaySound("Game_Land", 1f, true);
                break;
            case location.Chest:
                NewBorder(Color.yellow);
                AudioManager.PlaySound("Upbeat", 1f, true);
                break;
            case location.Shop:
                NewBorder(Color.blue);
                AudioManager.PlaySound("Game", 1f, true);
                break;
        }

        coin.PixelManager = this;
        foreach (Card cardG in OnlyCards)
        {
            cardG.PixelManager = this;
        }
        foreach (Card cardG in OnlyCards)
        {
            cardG.DrawBorder(Color.gray);

            if (cardG.actionsForCard == actions.Visible || (cardG.actionsForCard == actions.Deleted && nowLocation != location.Chest))
            {
                cardG.DrawCard();
                cardG.VisibleRender();

            }
            if (cardG.locked) cardG.DrawFrame(Color.gray);
            if (cardG.Enabled == true) cardG.EnabledCard();
        }
        foreach (List<Card> list in Inventory.CardInventory)
        {
            foreach (Card card in list) card.PixelManager = this;
        }
        selectedCard = (Card)(save["selectedCard"]);
        if (selectedCard != null) selectedCard.Select(selectColor);

        foreach (var partOfInvent in Inventory.CardInventory)
        {
            foreach (Card card in partOfInvent)
            {
                card.VisibleRender();
            }
        }

        Inventory.RenderPartOfInventory(Inventory.CardInventory[0], 3, 0, 6);
        Inventory.RenderPartOfInventory(Inventory.CardInventory[1], 68, 1, 6);
        Inventory.RenderPartOfInventory(Inventory.CardInventory[2], 133, 2, 6);

        coin.DrowCoin();
        coin.DrowValue();

        Inventory.ActivSlot = (Inventory.CardInventory[1].Count > 0) ? Inventory.CardInventory[1] : Inventory.CardInventory[2];
        if (Inventory.ActivSlot.Count > 0) Inventory.ActivSlot[Inventory.ActivSlot.Count - 1].DrawBorder(Color.red);
    }

    IEnumerator Clear()
    {
        List<Coroutine> coroutines = new List<Coroutine>();
        List<Card> cardsToRemove = new List<Card>();

        foreach (Card cardG in OnlyCards)
        {
            Coroutine coroutine = cardG.MyAnimator.StartCoroutine(cardG.MyAnimator.Leave());
            coroutines.Add(coroutine);
            cardsToRemove.Add(cardG);
        }

        foreach (var coroutine in coroutines)
        {
            yield return coroutine;
        }

        foreach (var card in cardsToRemove)
        {
            card.ClearCard();
        }
        OnlyCards.Clear();
    }
    Task ClearTask()
    {
        var tcs = new TaskCompletionSource<bool>();
        MonoBehaviour host = this;
        host.StartCoroutine(ClearCoroutine(tcs));
        return tcs.Task;
    }       
    IEnumerator ClearCoroutine(TaskCompletionSource<bool> tcs)
    {
        yield return StartCoroutine(Clear());
        tcs.SetResult(true);
    }
    IEnumerator WaitTicks(int ticks)
    {
        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForEndOfFrame();
        }
    }
    Task AnimationRenderTask()
    {
        var tcs = new TaskCompletionSource<bool>();
        MonoBehaviour host = this;

        host.StartCoroutine(AnimationRenderCoroutine(tcs));
        return tcs.Task;
    }
    IEnumerator AnimationRenderCoroutine(TaskCompletionSource<bool> tcs)
    {
        yield return AnimationRender();
        tcs.SetResult(true);
    }
    IEnumerator AnimationRender()
    {
        int index = 0;
        foreach (Card card in OnlyCards)
        {
            yield return StartCoroutine(WaitTicks(6));
            AudioManager.PlaySound("loot", 1f);

            card.MyAnimator.StartCoroutine(card.MyAnimator.PerformActionsWithDelays());

            if (index == OnlyCards.Count - 1)
            {
                StartNew();
                yield return StartCoroutine(WaitTicks(370));
            }
            index++;
        }
    }

    public static string FindCard(axes direction, bool change = true, bool chestMod = false, bool nearMod = false)
    {
        //moveEnable = false;
        double minDistance = 1000;
        Card minCard = NullCard;
        //if (nowLocation == location.Chest) Program.UpdateFrame();


        for (int i = 0; i < OnlyCards.Count; i++)
        {
            double nowDistance = IsDistanceWithinThreshold(localX, localY, (OnlyCards[i].Xmain + 2), (OnlyCards[i].Ymain + 4));
            if (expressions[direction](i) || nearMod)
            {
                if (nowDistance < minDistance && nowDistance > 1)
                {

                    if (OnlyCards[i].Enabled || start || chestMod)
                    {

                        minDistance = nowDistance;
                        minCard = OnlyCards[i];
                    }
                }

            }
        }

        if (change && minCard.Enabled || start)
        {
            localX = minCard.Xmain + 2;
            localY = minCard.Ymain + 4;
            if (change) ChekeSelect();
        }
        start = false;

        return (minCard.Xmain + 2).ToString() + "; " + (minCard.Ymain + 4).ToString();
    }
    public void UpdateFrame()
    {
        for (int i = 0; i < OnlyCards.Count; i++)
        {
            double nowDistance = IsDistanceWithinThreshold(selectedCard.Xmain, selectedCard.Ymain, (OnlyCards[i].Xmain), (OnlyCards[i].Ymain));
            if (nowDistance < 30 && nowDistance > 1)
            {
                //new Pixel(OnlyCards[i].Xmain, OnlyCards[i].Ymain, ConsoleColor.DarkBlue).Drow();
                if (OnlyCards[i].locked) OnlyCards[i].DrawFrame(Color.gray);
            }
        }
    }
    string FindCardY(int value = 1, bool change = true)
    {
        for (int j = 5; j < 100; j++)
        {

            for (int i = 0; i < 100; i++)
            {
                string key1 = (localX + i).ToString() + "; " + (localY - j * value).ToString();
                string key2 = (localX - i).ToString() + "; " + (localY - j * value).ToString();

                if (Cards.ContainsKey(key1))
                {
                    if (change && Cards[key1].Enabled)
                    {
                        localX = localX + i;
                        localY = localY - j * value;
                        ChekeSelect();
                    }

                    return key1;
                }
                if (Cards.ContainsKey(key2))
                {
                    if (change && Cards[key2].Enabled)
                    {
                        localX = localX - i;
                        localY = localY - j * value;
                        ChekeSelect();
                    }

                    return key2;
                }
            }

        }
        return "NullCard";
    }
    string FindCardX(int value = 1, bool change = true)
    {
        string truekey = " ";
        for (int i = 1; i < 100; i++)
        {
            string key1 = (localX + i * value).ToString() + "; " + localY.ToString();


            if (Cards.ContainsKey(key1))
            {
                if (change && (Cards[key1].Enabled || start))
                {
                    start = false;
                    localX = localX + i * value;
                    ChekeSelect();
                }
                truekey = key1;
                return truekey;
            }

        }
        return "NullCard";
    }
    public void SelectNear()
    {
        /*for (int i = 0; i < 4; i++)
        {
            string truekey = FindCard((axes)i, false, true);

            if (Cards.ContainsKey(truekey)) Cards[truekey].EnabledCard();
        }*/

        string truekeyR = FindCardX(1, false);
        string truekeyL = FindCardX(-1, false);
        string truekeyD = FindCardY(-1, false);
        string truekeyU = FindCardY(1, false);

        if (Cards.ContainsKey(truekeyR)) Cards[truekeyR].EnabledCard();
        if (Cards.ContainsKey(truekeyL)) Cards[truekeyL].EnabledCard();
        if (Cards.ContainsKey(truekeyD)) Cards[truekeyD].EnabledCard();
        if (Cards.ContainsKey(truekeyU)) Cards[truekeyU].EnabledCard();

    }
    public void SelectEnable()
    {
        /*for (int i = 0; i < 4; i++)
        {
            string truekey = FindCard((axes)i, false, true);

            if (Cards.ContainsKey(truekey) && !Cards[truekey].Enabled)
            {
                Cards[truekey].DrowBorder(ConsoleColor.DarkGray);
                Cards[truekey].enabletToSelect = true;
            }
        }*/
        string truekeyR = FindCardX(1, false);
        string truekeyL = FindCardX(-1, false);
        string truekeyD = FindCardY(-1, false);
        string truekeyU = FindCardY(1, false);
        Color color = Color.HSVToRGB(0f, 0f, 0.1f);

        if (Cards.ContainsKey(truekeyR) && !Cards[truekeyR].Enabled)
        {
            Cards[truekeyR].DrawBorder(color);
            Cards[truekeyR].enabletToSelect = true;
        }
        if (Cards.ContainsKey(truekeyL) && !Cards[truekeyL].Enabled)
        {
            Cards[truekeyL].DrawBorder(color);
            Cards[truekeyL].enabletToSelect = true;
        }
        if (Cards.ContainsKey(truekeyD) && !Cards[truekeyD].Enabled)
        {
            Cards[truekeyD].DrawBorder(color);
            Cards[truekeyD].enabletToSelect = true;
        }
        if (Cards.ContainsKey(truekeyU) && !Cards[truekeyU].Enabled)
        {
            Cards[truekeyU].DrawBorder(color);
            Cards[truekeyU].enabletToSelect = true;
        }

    }

    public void MovePixelToPosition(Pixel pixel, Vector3 newPosition)
    {
        GameObject PixelObject = pixel.transform.gameObject;
        AnimationManager anime = pixel.GetComponent<AnimationManager>();
        if (anime == null) anime = pixel.AddComponent<AnimationManager>();
        anime.TextMod = true;
        //anime.Mod = MyMod;

        anime.Initialize(PixelObject, newPosition, Quaternion.identity, 0.5f);
        anime.StartInterpolation();
        anime.animationNow = false;
    }
    public void SinglAnimation(GameObject gameObject, Vector3 newPosition, Quaternion newRotation, float duration)
    {
        AnimationManager anime = gameObject.GetComponent<AnimationManager>();
        if (anime == null) anime = gameObject.AddComponent<AnimationManager>();
        anime.ChangeAngle = false;

        anime.Initialize(gameObject, newPosition, newRotation, duration);
        anime.StartInterpolation();
    }

    public static int FindMinIndex(int[] array)
    {
        if (array == null || array.Length == 0)
        {
            throw new ArgumentException("Array must not be empty or null.");
        }

        int minIndex = 0;
        int minValue = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < minValue)
            {
                minIndex = i;
                minValue = array[i];
            }
        }

        return minIndex;
    }
    public static int[] GenerateRandomArray(int countCard)
    {
        int[] array = new int[4];
        int MaxCardsOnlayers = 7;

        int Layers = 3;

        for (int i = 0; i < Layers; i++)
        {
            int countCardNow = random.Next(1, (countCard > MaxCardsOnlayers) ? MaxCardsOnlayers + 1 : countCard + 1);
            array[i] = countCardNow;
            countCard -= countCardNow;
            if (countCard == 0) break;
        }
        if (countCard != 0 && countCard > 7)
        {
            int changeCards = countCard - 7;
            countCard -= changeCards;

            int cardsOnlayers = changeCards / 4;
            int changeCardsOnlayers = changeCards % 4;

            for (int i = 0; i < Layers; i++)
            {
                array[i] += cardsOnlayers;
            }
            array[FindMinIndex(array)] += changeCardsOnlayers;
        }
        array[Layers] = countCard;

        return array;
    }
    public int[] RemoveElementFromArray(int[] array, int indexToRemove)
    {

        int[] newArray = new int[array.Length - 1];
        int newIndex = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (i != indexToRemove)
            {
                newArray[newIndex] = array[i];
                newIndex++;
            }
        }

        return newArray;
    }
    public int[] GenerateRandomArray(int firstL, int lastL, int firsNum, int lastNum)
    {
        System.Random rand = new System.Random();
        int length = rand.Next(firstL, lastL);
        int[] array = new int[length];

        for (int i = 0; i < length; i++)
        {
            array[i] = rand.Next(firsNum, lastNum);
        }

        return array;
    }
    public int[] GenerateRandomArrayForChest(int firstL, int lastL, int firsNum, int lastNum)
    {
        System.Random rand = new System.Random();
        int length = rand.Next(firstL, lastL);
        int[] array = new int[length];

        bool five = false;
        bool one = false;

        for (int i = 0; i < length; i++)
        {
            int randNum = rand.Next(firsNum, lastNum);
            if (!((randNum == 1 && one) || (randNum == 5 && five))) array[i] = randNum;
            else i--;

            if (randNum == 1) one = true;
            if (randNum == 5) five = true;
        }

        return array;
    }
    static double IsDistanceWithinThreshold(double x1, double y1, double x2, double y2, double distanceThreshold = 10)
    {
        double distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        return distance;
    }
    int randCard()
    {
        int rCard = random.Next(0, OnlyCards.Count);
        if (!(OnlyCards[rCard] is Door)) return rCard;
        else return randCard();
    }
}
