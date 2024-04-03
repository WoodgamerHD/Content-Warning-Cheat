


using DefaultNamespace;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
using UnityEngine;
using Steamworks;


namespace ContentWarningHax
{

    class Main : MonoBehaviour
    {
    

      
        bool esp = true;
        bool Botesp = true;
       
        bool FlyTest = false;
        public float NoClipSpeed = 0.1f;

        public static bool teleportFinderEnabled = false;
        public static Vector3 teleportPosition;
      
        
        public static List<Player> PlayerController = new List<Player>();
        public static List<Bot> BotController = new List<Bot>();
        public static List<Room> Room = new List<Room>();
        public static List<AOE> AOE = new List<AOE>();
       
        float natNextUpdateTime;
      
  
        public static UnityEngine.Camera cam;
   
        private Rect windowRect = new Rect(0, 0, 400, 400); 
        private int tab = 0; 
        private Color backgroundColor = Color.black; 
        private bool showMenu = true;
        public List<string> monsterNames = new List<string>
    {
        "Angler",
        "BarnacleBall",
        "BigSlap",
        "Chaser",
        "Drag",
        "Ear",
        "EyeGuy",
        "Fear",
        "Ghost",
        "Jelly",
        "Knifo",
        "Mouth",
        "Skinny",
        "Snactcho",
        "ToolkitBoy",
        "Weeping",
        "Zombie"
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
        void MenuWindow(int windowID)
        {
            GUILayout.BeginHorizontal();

            // Create toggle buttons for each tab
            GUILayout.BeginVertical(GUILayout.Width(100));
            if (GUILayout.Toggle(tab == 0, "Main", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 0;
            }
            if (GUILayout.Toggle(tab == 1, "Esp", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 1;
            }
            if (GUILayout.Toggle(tab == 2, "Items", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 2;
            }
            if (GUILayout.Toggle(tab == 3, "Monsters", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 3;
            }
            if (GUILayout.Toggle(tab == 4, "Steam", "Button", GUILayout.ExpandWidth(true)))
            {
                tab = 4;
            }
            GUILayout.EndVertical();

           

            GUILayout.BeginVertical();


       
            switch (tab)
            {
                case 0:
                 


                    // Content for tab 2
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                
                    if (GUILayout.Button("Crasher"))
                    {
                        foreach (Player player in PlayerController)
                        {

                            if (player.IsLocal == false)
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
                
                 
                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                    if (GUILayout.Button("Dump items"))
                    {


                        string FilePath = "Items.txt";

                        foreach (var Items in ItemDatabase.Instance.lastLoadedItems)
                        {

                            string ItemInformation;
                            ItemInformation = "Name: " + Items.name;
                            ItemInformation += "\t | ID: " + Items.id;
                            File.AppendAllText(FilePath, ItemInformation + "\n");
                        }

                    }

                    if (GUILayout.Button("Max Stats"))
                    {
                            Player.localPlayer.data.remainingOxygen = float.MaxValue;
                            Player.localPlayer.data.maxOxygen = float.MaxValue;
                            Player.localPlayer.data.health = float.MaxValue;
                            Player.localPlayer.data.currentStamina = float.MaxValue;
                            Player.localPlayer.data.rested = true;

                        
                    }
                  
                    if (GUILayout.Button("Kill All"))
                    {
                        for (int i = 0; i < PlayerHandler.instance.players.Count; i++)
                        {
                            PlayerHandler.instance.players[i].Die();
                        }
                    }
                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();


                    break;
                case 1:
                    // Content for tab 2
                    GUILayout.BeginVertical(GUI.skin.box);

                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    esp = GUILayout.Toggle(esp, "Esp");
                 
    


                    GUILayout.EndVertical();

                    GUILayout.Space(10);

                    GUILayout.BeginVertical();
                

                    GUILayout.EndVertical();

                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    break;
                case 2:
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition);

                    for (int i = 0; i < ItemDatabase.Instance.lastLoadedItems.Count; i++)
                    {
                        var item = ItemDatabase.Instance.lastLoadedItems[i];

                        string ItemInformation = item.name;
                        if (GUILayout.Button(ItemInformation))
                        {
                            selectedItemIndex = i;
                        }
                    }
                    GUILayout.EndScrollView();



                    if (selectedItemIndex != -1)
                    {
                      
                        if (GUILayout.Button("Give Item"))
                        {
                            EquipItem(ItemDatabase.Instance.lastLoadedItems[selectedItemIndex]);
                            selectedItemIndex = -1;
                        }
                    }
                    break;
                case 3:
                    foreach (string monsterName in monsterNames)
                    {
                        if (GUILayout.Button(monsterName))
                        {
                            SpawnMonster(monsterName);
                        }
                    }

                    break;
                    case 4:
                    SteamAPICall_t hAPICall = SteamMatchmaking.RequestLobbyList();
                    if (GUILayout.Button("Request Lobby List"))
                    {
                        hAPICall = SteamMatchmaking.RequestLobbyList();
                        Debug.Log("Requested Lobby List");
                    }

                    GUILayout.Label("SteamAPICall_t: " + SteamMatchmaking.RequestLobbyList());

                    if (GUILayout.Button("Random Join"))
                    {
                        MainMenuHandler.Instance.JoinRandom();
                  
                    }
                    if (GUILayout.Button("Set Name"))
                    {
                        PhotonNetwork.NickName = "Артём";
                    }

                    break;
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUI.DragWindow(); // Allow the user to drag the window around
        }
        public static void EquipItem(Item item)
        {
            Debug.Log("Spawn item: " + item.name);
            Vector3 debugItemSpawnPos = MainCamera.instance.GetDebugItemSpawnPos();
            Player.localPlayer.RequestCreatePickup(item, new ItemInstanceData(Guid.NewGuid()), debugItemSpawnPos, UnityEngine.Quaternion.identity);
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


           

            if (esp)
            {
             

                foreach (Player player in PlayerController)
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
                            ESPUtils.DrawString(namePosition, player.name.Replace("(Clone)", ""), Color.red, true, fontSize, FontStyle.Bold);
                        }
                        else
                        {
                            ESPUtils.DrawString(namePosition, player.refs.view.Controller.NickName + "\n" + "HP: " + player.data.health, Color.green, true, fontSize, FontStyle.Bold);
                            ESPUtils.DrawHealth(new Vector2(w2s.x, UnityEngine.Screen.height - w2s.y + 22f), player.data.health, 100f, 0.5f, true);

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
                Room = Resources.FindObjectsOfTypeAll<Room>().ToList();
            
              
                natNextUpdateTime = 0f;
            }


          



            cam = UnityEngine.Camera.main;

        }
    }
}

