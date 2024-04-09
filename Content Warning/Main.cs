


using DefaultNamespace;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;
using Photon.Realtime;
using static UnityEngine.EventSystems.EventTrigger;
using static Photon.Voice.WebRTCAudioLib;
using ExitGames.Client.Photon;
using Zorro.UI;
using Zorro.Core.CLI;
using static HasSpaceTest;
using Random = UnityEngine.Random;
using System.Collections;

namespace ContentWarningHax
{

    class Main : MonoBehaviour
    {
    

      
        bool esp = true;
        bool itemESP = false;
        bool MaxCharge = false;
        bool takeDamage = false;
        bool GodModeStats = false;
        bool TransformMovement = false;
     
        public string customText = ""; // Custom text input field
        private int currentDay = 0;
        private int currentRun = 0; 
        private int Quotaadd = 0; 
        private int moneyAdded = 0;
        public string customRegion = ""; // String to store custom region input

        public static bool teleportFinderEnabled = false;
        public static Vector3 teleportPosition;
      
        
        public static List<Player> PlayerController = new List<Player>();
        public static List<PropSpawner> PropSpawner = new List<PropSpawner>();
        public static List<ItemInstance> ItemInstance = new List<ItemInstance>();
        public static List<BombItem> BombItem = new List<BombItem>();
        public static List<Room> Room = new List<Room>();
        public static List<DebugUIHandler> DebugUIHandler = new List<DebugUIHandler>();
    
        Photon.Realtime.Player[] otherPlayers = PhotonNetwork.PlayerListOthers;
        private string lobbyInfo = ""; // String to store lobby information
        public HashSet<ItemDataEntry> m_dataEntries = new HashSet<ItemDataEntry>();

        float natNextUpdateTime;
        public float fuze = 5f;
  
     
        public static UnityEngine.Camera cam;
   
        private Rect windowRect = new Rect(0, 0, 400, 400); 
        private int tab = 0; 
        private Color backgroundColor = Color.black; 
        private bool showMenu = true;



        public List<string> monsterNames = new List<string>
{
    "AnglerMimic",     // spawns
    "BarnacleBall",    // spawns
    "BigSlap",         // spawns
    "Bombs",           // spawns
    "Dog",             // spawns
    "Ear",             // spawns
    "EyeGuy",          // spawns
    "Flicker",         // spawns
    "Ghost",           // spawns
    "Jelly",           // spawns
    "Knifo",           // spawns
    "Larva",           // spawns
    "Mouthe",          // spawns
    "Slurper",         // spawns
    "Snatcho",         // spawns
    "Spider",          // spawns
    "Snail",          // spawns
    "Toolkit_Fan",     // spawns
    "Toolkit_Hammer",  // spawns
    "Toolkit_Iron",    // spawns
    "Toolkit_Vaccuum", // spawns
    "Toolkit_Whisk",   // spawns
    "Toolkit_Wisk",    // spawns
    "Weeping",          // spawns
    "Zombe",          // spawns
    "MimicInfiltrator",          // spawns


};


        public static Color TestColor
        {
            get
            {
                return new Color(1f, 0f, 1f, 1f);
            }
        }
      
        int selectedItemIndex = -1;
        
        Vector2 scrollPosition = Vector2.zero;
        Vector2 scrollPosition1 = Vector2.zero;
        Vector2 scrollPositionplayerlist = Vector2.zero;


        void MenuWindow(int windowID)
        {
            GUILayout.BeginHorizontal();

            // Create toggle buttons for each tab
            GUILayout.BeginVertical(GUILayout.Width(100));
            string[] tabNames = { "Main", "Esp", "Items", "Monsters", "Stats", "Random", "Players", "Console", "Console 2" };
            for (int i = 0; i < tabNames.Length; i++)
            {
                if (GUILayout.Toggle(tab == i, tabNames[i], "Button", GUILayout.ExpandWidth(true)))
                {
                    tab = i;
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();

            switch (tab)
            {
                case 0:
                    // Main content
                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    if (GUILayout.Button("Crasher"))
                    {
                        foreach (Player player in PlayerController)
                        {
                            if (!player.IsLocal)
                            {
                                PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                                PhotonNetwork.DestroyPlayerObjects(player.refs.view.Controller);
                            }
                        }
                    }
                    if (GUILayout.Button("Call Revive"))
                    {
                        Player.localPlayer.CallRevive();
                    }
                    if (GUILayout.Button("Set Host"))
                    {
                        PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                    }
                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    if (GUILayout.Button("Dump Items"))
                    {
                        string FilePath = "Items.txt";
                        foreach (var Items in ItemDatabase.Instance.lastLoadedItems)
                        {
                            string ItemInformation = "Name: " + Items.name + "\t | ID: " + Items.id;
                            File.AppendAllText(FilePath, ItemInformation + "\n");
                        }
                    }
                    if (GUILayout.Button("Bot Handler"))
                    {
                        BotHandler.instance.DestroyAll();
                    }

                    if (GUILayout.Button("Kill All"))
                    {
                        for (int i = 0; i < PlayerHandler.instance.playerAlive.Count; i++)
                        {
                            PlayerHandler.instance.playerAlive[i].Die();
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    break;

                case 1:
                    // Esp content
                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    esp = GUILayout.Toggle(esp, "Esp");
                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                    itemESP = GUILayout.Toggle(itemESP, "Items");
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    break;

                case 2:
                    // Items content
                    GUILayout.BeginVertical(GUI.skin.box);
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                    foreach (var item in ItemDatabase.Instance.lastLoadedItems)
                    {
                        if (GUILayout.Button(item.name))
                        {
                            EquipItem(item);
                        }
                    }
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    break;

                case 3:
                    // Monsters content
                    GUILayout.BeginVertical(GUI.skin.box);
                    scrollPosition1 = GUILayout.BeginScrollView(scrollPosition1);
                    foreach (string monsterName in monsterNames)
                    {
                        if (GUILayout.Button(monsterName))
                        {
                            SpawnMonster(monsterName);
                        }
                    }
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    break;

                case 4:
                    // Stats content
                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    MaxCharge = GUILayout.Toggle(MaxCharge, "Max Charge");
                    GodModeStats = GUILayout.Toggle(GodModeStats, "Godmode");
                    takeDamage = GUILayout.Toggle(takeDamage, "Take Damage");
                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                    GUILayout.BeginVertical();
                 
                    TransformMovement = GUILayout.Toggle(TransformMovement, "Trans Move");
                    if (TransformMovement)
                    {
                        GUILayout.Label("LeftControl forward");
                        GUILayout.Label("UpArrow Up");
                        GUILayout.Label("DownArrow Down");
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    break;

                case 5:
                    // Random content
                    GUILayout.BeginVertical(GUI.skin.box);
                    if (GUILayout.Button("Start Money<host>"))
                    {
                        BigNumbers.Instance.StartMoney = int.MaxValue;
                    }
                    if (GUILayout.Button("Join Random Room"))
                    {
                        PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.RandomMatching, TypedLobby.Default, null, null);
                    }

                  

                    GUILayout.EndVertical();
                    break;

                case 6:
                    // Local Players content
                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.Label("Players in Lobby:");
                    lobbyInfo = "";
                    foreach (Photon.Realtime.Player player in otherPlayers)
                    {
                        string playerInfo = string.Format("{0} {1}", player.NickName, player.CustomProperties.ToStringFull());
                        GUILayout.Label(playerInfo);
                        lobbyInfo += playerInfo + "\n";
                    }
                    if (GUILayout.Button("Save Lobby Info"))
                    {
                        SaveLobbyInfo();
                    }
                    GUILayout.EndVertical();
                    break;

                case 7:
                    // Console content
                    GUILayout.BeginVertical(GUI.skin.box);
                    if (GUILayout.Button("Console Show"))
                    {
                        foreach (DebugUIHandler item in DebugUIHandler)
                        {
                            item.Show();
                        }
                    }
                    if (GUILayout.Button("Console Hide"))
                    {
                        foreach (DebugUIHandler item in DebugUIHandler)
                        {
                            item.Hide();
                        }
                    }

                    if (GUILayout.Button("Start Game"))
                    {
                        SurfaceNetworkHandler.Instance.RequestStartGame();
                    }
                    if (GUILayout.Button("Unlock Extractor"))
                    {
                        SurfaceNetworkHandler.UnlockExtractor();
                    }
                    if (GUILayout.Button("Quota Failed"))
                    {
                        SurfaceNetworkHandler.Instance.RPC_QuotaFailed();
                    }
                 
                    GUILayout.EndVertical();
                    break;

                case 8:
                    // Console 2 content
                    GUILayout.BeginVertical(GUI.skin.box);
                    GUILayout.Label("Host Only");
                    GUILayout.Label("Quota:");
                    Quotaadd = int.Parse(GUILayout.TextField(Quotaadd.ToString()));
                    GUILayout.Label("Current Quota:");
                    if (GUILayout.Button("Set Quota"))
                    {
                        SurfaceNetworkHandler.AddQuota(Quotaadd);
                    }
                    GUILayout.Label("Current Day:");
                    currentDay = int.Parse(GUILayout.TextField(currentDay.ToString()));
                    if (GUILayout.Button("Set Current Day"))
                    {
                        SurfaceNetworkHandler.SetCurrentDay(currentDay);
                    }
                    GUILayout.Label("Current Run:");
                    currentRun = int.Parse(GUILayout.TextField(currentRun.ToString()));
                    if (GUILayout.Button("Set Current Run"))
                    {
                        SurfaceNetworkHandler.SetCurrentDay(currentRun);
                    }
                    GUILayout.Label("Current Money:");
                    moneyAdded = int.Parse(GUILayout.TextField(moneyAdded.ToString()));
                    if (GUILayout.Button("Set Current Money"))
                    {
                        SurfaceNetworkHandler.RoomStats.AddMoney(moneyAdded);
                    }
                    GUILayout.EndVertical();
                    break;
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }

        public static void EquipItem(Item item)
        {
            Debug.Log("Spawn item: " + item.name);
            Vector3 debugItemSpawnPos = MainCamera.instance.GetDebugItemSpawnPos();


            Player.localPlayer.RequestCreatePickup(item, new ItemInstanceData(Guid.NewGuid()), debugItemSpawnPos, UnityEngine.Quaternion.identity);
        }
     
        void SaveLobbyInfo()
        {
            string filePath = "LobbyInfo.txt"; 

            try
            {
                // Write lobbyInfo string to a text file
                File.WriteAllText(filePath, lobbyInfo);
                Debug.Log("Lobby info saved to " + filePath);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to save lobby info: " + ex.Message);
            }
        }
        Vector2 CalculatePosition()
        {
            float x = Screen.width / 2 - 50;
            float y = 10;
            float height = 20;
            Vector2 Position = new Vector2(x + (height / 2f) + 3f, y + 1f);
            return Position;
        }

        public static void SpawnMonster(string monster)
        {
            RaycastHit raycastHit = HelperFunctions.LineCheck(MainCamera.instance.transform.position, MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f, HelperFunctions.LayerType.TerrainProp, 0f);
            Vector3 vector = MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 30f;
            if (raycastHit.collider != null)
            {
                vector = raycastHit.point;
            }
            vector = HelperFunctions.GetGroundPos(vector + Vector3.up * 1f, HelperFunctions.LayerType.TerrainProp, 0f);
            PhotonNetwork.Instantiate(monster, vector, UnityEngine.Quaternion.identity, 0, null);
        }
      
        public void OnGUI()
        {
         

            if (showMenu) // Only draw the menu when showMenu is true
            {
                // Set the background color
                GUI.backgroundColor = backgroundColor;

                windowRect = GUI.Window(0, windowRect, MenuWindow, "WoodSDK(Do not Resell or repost From unknowncheats.me)"); // Create the window with title "Menu"
             
                
            }

            string playerInfo2 = string.Format("R: {0} M: {1} Ping: {2}", PhotonNetwork.CloudRegion, PhotonNetwork.MasterClient.NickName, PhotonNetwork.GetPing());
            Vector2 Position = CalculatePosition();

            // Assuming ESPUtils.DrawString method is available and works as intended
            ESPUtils.DrawString(Position, playerInfo2, Color.cyan, true, 20, FontStyle.Bold);



          
            playerEsp();
            ItemEsp();



        }
       
        public void ItemEsp()
        {
            if (itemESP)
            {
                foreach (ItemInstance item in ItemInstance)
                {

                    Vector3 w2s = cam.WorldToScreenPoint(item.transform.position);
                    w2s.y = Screen.height - (w2s.y + 1f);
                    float distance = Vector3.Distance(UnityEngine.Camera.main.transform.position, item.transform.position);
                    int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);
                    if (ESPUtils.IsOnScreen(w2s))
                    {
                        ESPUtils.DrawString(w2s, item.item.displayName, Color.blue, true, fontSize, FontStyle.Bold);
                    }
                }
            }
        }
        public void playerEsp()
        {
            if (esp)
            {

                foreach (Player player in PlayerController)
                {

                    if (player != null)
                    {

                        Vector3 w2s = cam.WorldToScreenPoint(player.HeadPosition());
                        Vector3 enemyBottom = player.HeadPosition();
                        Vector3 enemyTop;
                        enemyTop.x = enemyBottom.x;
                        enemyTop.z = enemyBottom.z;
                        enemyTop.y = enemyBottom.y + 2f;
                        Vector3 worldToScreenBottom = cam.WorldToScreenPoint(enemyBottom);
                        Vector3 worldToScreenTop = cam.WorldToScreenPoint(enemyTop);

                        if (player.IsLocal)
                            return;




                        if (ESPUtils.IsOnScreen(w2s))
                        {

                            float height = Mathf.Abs(worldToScreenTop.y - worldToScreenBottom.y);
                            float x = w2s.x - height * 0.3f;
                            float y = Screen.height - worldToScreenTop.y;


                            Vector2 namePosition = new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 8f);
                            Vector2 hpPosition = new Vector2(x + (height / 2f) + 3f, y + 1f);


                            namePosition -= new Vector2(player.HeadPosition().x - player.HeadPosition().x, 0f);
                            hpPosition -= new Vector2(player.HeadPosition().x - player.HeadPosition().x, 0f);

                            float distance = Vector3.Distance(UnityEngine.Camera.main.transform.position, player.HeadPosition());
                            int fontSize = Mathf.Clamp(Mathf.RoundToInt(12f / distance), 10, 20);



                            if (player.ai)
                            {
                                ESPUtils.DrawString(namePosition, player.name.Replace("(Clone)", ""), Color.yellow, true, fontSize, FontStyle.Bold);
                            }
                            else
                            {
                                if (player.data.dead)
                                {
                                    ESPUtils.DrawString(namePosition, player.refs.view.Controller.ToString(), Color.red, true, fontSize, FontStyle.Bold);
                                }
                                else
                                {
                                    ESPUtils.DrawString(namePosition, player.refs.view.Controller.ToString(), Color.green, true, fontSize, FontStyle.Bold);
                                }

                            }


                        }
                    }

                }
            }
        }


        public void Start()
        {
            // Center the window on the screen
            windowRect.x = (Screen.width - windowRect.width) / 2;
            windowRect.y = (Screen.height - windowRect.height) / 2;


        

        }
       



        public void Update()
        {
          
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                showMenu = !showMenu;
            }
         

            natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 1f)
            {


                PlayerController = Resources.FindObjectsOfTypeAll<Player>().ToList();
                PropSpawner = Resources.FindObjectsOfTypeAll<PropSpawner>().ToList();
                Room = Resources.FindObjectsOfTypeAll<Room>().ToList();
                ItemInstance = Resources.FindObjectsOfTypeAll<ItemInstance>().ToList();
                BombItem = Resources.FindObjectsOfTypeAll<BombItem>().ToList();
                DebugUIHandler = Resources.FindObjectsOfTypeAll<DebugUIHandler>().ToList();
              
                otherPlayers =  PhotonNetwork.PlayerListOthers;
              
              
                natNextUpdateTime = 0f;
            }
            if(MaxCharge)
            {
                PlayerInventory playerInventory;
                Player.localPlayer.TryGetInventory(out playerInventory);
                foreach (InventorySlot inventorySlot in playerInventory.slots)
                {
                    BatteryEntry batteryEntry;
                    if (inventorySlot.ItemInSlot.item != null && inventorySlot.ItemInSlot.data.TryGetEntry<BatteryEntry>(out batteryEntry) && batteryEntry.m_maxCharge > batteryEntry.m_charge)
                    {
                        batteryEntry.AddCharge(100);
                    }
                }
            }
            if (GodModeStats)
            {
                Player.localPlayer.data.remainingOxygen = 99999;
                Player.localPlayer.data.maxOxygen = 99999;
                Player.localPlayer.data.health = 99999;
                Player.localPlayer.data.currentStamina = 99999;
                Player.localPlayer.data.rested = true;

            }
            if (TransformMovement)
            {

                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Player.localPlayer.transform.position += 0.5f * UnityEngine.Camera.main.transform.forward;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Player.localPlayer.transform.position += new Vector3(0f, 5f, 0f);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Player.localPlayer.transform.position += new Vector3(0f, -5f, 0f);
                } 


            }
      

          
            


                cam = UnityEngine.Camera.main;

        }
    }
}

