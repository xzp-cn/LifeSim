using GameFramework.DataNode;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.FileSystem;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using StarForce;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Network;
using UnityEngine;
using UnityGameFramework.Runtime;
using FileInfo = GameFramework.FileSystem.FileInfo;
using GameEntry = StarForce.GameEntry;

public class Test : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = Time.timeScale;
        transform.Rotate(new Vector3(0,10,0),Space.Self);
        if (Input.GetKeyDown(KeyCode.A))
        {
            //LoadPrefab();
            //PlayMusic();
            //LoadScene();
            //LoadUI();
            //LoadAsset();

            //Test_SaveSetting();
            //Test_ReadSetting();

            //TestEvents();

            //TestObjectPool();

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //FireEvent();

            //GetPoolObj();

            Localization_Load();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //RecyclePoolObj();

            // LocalizationRead();

        }
        //TestFileSys();

        //TestDownLoad();

        //TestDataNode();

        //TestDataTable();

        //TestFsm();

        //TestSocket();
        TestDataNode();
    }

    #region Entity
    [ContextMenu("Do Something")]
    void DoTest()
    {
        Debug.Log("Do SomeThing");
    }

    /// <summary>
    /// 加载实体
    /// </summary>
    void LoadPrefab()
    {
        IDataTable<DRTest> drTabelTest = GameEntry.DataTable.GetDataTable<DRTest>();
        DRTest drTest = drTabelTest.GetDataRow(111111);
        //GameEntry.Entity.ShowEntity();;
        GameEntry.Entity.ShowEntity<TestEntity>(-6, AssetUtility.GetTestAsset(drTest.AssetName), "Test", new TestEntityData(GameEntry.Entity.GenerateSerialId(), 111111, 10000, CampType.Neutral));
    }

    void PlayMusic()
    {
        GameEntry.Sound.PlayMusic(4);
    }


    void LoadScene()
    {
        GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("Test_w"), Constant.AssetPriority.SceneAsset);
    }

    /// <summary>
    /// 测试UI 
    /// </summary>
    void LoadUI()
    {
        //GameEntry.UI.OpenUIForm(UIFormId.TestUIForm, this);
    }
    
    /// <summary>
    /// 加载普通资源
    /// </summary>
    void LoadAsset()
    {
        string matAsset = "part_test_mat";
        GameEntry.Resource.LoadAsset(AssetUtility.GetMaterialAsset(matAsset), Constant.AssetPriority.FontAsset, new LoadAssetCallbacks(
            (assetName, asset, duration, userData) =>
            {
                //m_LoadedFlag[Utility.Text.Format("Font.{0}", fontName)] = true;
                //UGuiForm.SetMainFont((Font)asset);
                GameEntry.Entity.GetEntity(-6).gameObject.GetComponent<MeshRenderer>().material = (Material)asset;
                Log.Info("Load material '{0}' OK.", matAsset);
            },

            (assetName, status, errorMessage, userData) =>
            {
                Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", matAsset, assetName, errorMessage);
            }));
    }
    #endregion


    #region  数据配置 -数据存到文件中

    void Test_SaveSetting()
    {
        GameEntry.Setting.SetInt("CoinNum", 10);
        Debug.Log("金币数量保存");

    }

    /// <summary>
    /// 读取设置
    /// </summary>
    void Test_ReadSetting()
    {
        int coinNum = GameEntry.Setting.GetInt("CoinNum", 0);
        Debug.Log("金币数量读取  " + coinNum);
    }
    #endregion


    #region 事件
    void TestEvents()
    {
        GameEntry.Event.Subscribe(TestEventsArgs.EventId, ReceiveLoadSucess);
        Debug.Log("注册事件");
    }

    void FireEvent()
    {
        GameEntry.Event.Fire(this, TestEventsArgs.Create("Test"));
        Debug.Log("发送事件 " + GameEntry.Event.Count(TestEventsArgs.EventId));
    }
    void ReceiveLoadSucess(object sender, GameEventArgs args)
    {
        TestEventsArgs testEventsArgs = (TestEventsArgs)args;
        string name = (string)testEventsArgs.UserData;
        Debug.Log(" 接受事件 " + name);
    }
    #endregion

                                                                                                                                                                                                                  
    #region 对象池使用
    /// <summary>
    /// 0.预制体
    /// 1.对象 封装
    /// 2.池对象封装
    ///
    /// </summary>
    public ObjectPoolItem m_HPBarItemTemplate;
    private IObjectPool<TestPoolItem> hpbObjectPool;
    List<ObjectPoolItem> ObjectPoolList = new List<ObjectPoolItem>();

    void TestObjectPool()
    {
        int m_InstancePoolCapacity = 3;
        hpbObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<TestPoolItem>("HPBarItem", m_InstancePoolCapacity);
        ObjectPoolList.Clear();
    }

    void GetPoolObj()
    {
        ObjectPoolItem hpBarItem = null;
        TestPoolItem hpBarItemObject = hpbObjectPool.Spawn();
        if (hpBarItemObject != null)
        {
            hpBarItem = (ObjectPoolItem)hpBarItemObject.Target;
        }
        else
        {
            hpBarItem = Instantiate(m_HPBarItemTemplate);
            Transform _transform = hpBarItem.GetComponent<Transform>();
            _transform.SetParent(transform);
            _transform.localScale = Vector3.one;
            hpbObjectPool.Register(TestPoolItem.Create(hpBarItem), true);
            ObjectPoolList.Add(hpBarItem);
        }
    }

    void RecyclePoolObj()
    {
        hpbObjectPool.Unspawn(ObjectPoolList[0]);
        ObjectPoolList.RemoveAt(0);
    }
    #endregion


    #region 本地化 -多语言设置
    void Localization_Load()
    {
        string dictionaryName = "Default";//不可重复加载,框架已经加载过了
        string dictionaryAssetName = AssetUtility.GetDictionaryAsset(dictionaryName, false);
        GameEntry.Localization.ReadData(dictionaryAssetName, this);
    }

    void LocalizationRead()
    {
        string cancelText = GameEntry.Localization.GetString("Dialog.CancelButton");
        Debug.Log(cancelText);
    }
    #endregion

    #region  文件系统
    private string fullPath;
    void TestFileSys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateFileSys();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            WriteFile();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ReadFileSys();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteFile();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            DesFile();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFile();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SaveFile();
        }
    }
    IFileSystem iFileSystem;
    void CreateFileSys()
    {
        fullPath = Path.Combine(Application.persistentDataPath, "Test.txt");
        if (GameEntry.FileSystem.HasFileSystem(fullPath))
        {
            iFileSystem = GameEntry.FileSystem.GetFileSystem(fullPath);
            Debug.Log("存在文件系统");
        }
        else
        {

            iFileSystem = GameEntry.FileSystem.CreateFileSystem(fullPath, FileSystemAccess.ReadWrite, 16, 256);
            Log.Debug("创建文件");
        }

    }

    void LoadFile()
    {
        fullPath = Path.Combine(Application.persistentDataPath, "Test.txt");
        iFileSystem = GameEntry.FileSystem.LoadFileSystem(fullPath, FileSystemAccess.ReadWrite);//
        FileInfo info = iFileSystem.GetFileInfo("Test.txt");
        Debug.Log(info.Offset);//
    }
    /// <summary>
    ///写入
    /// </summary>
    public string str;


    void WriteFile()
    {
        // string str = "helloworldhelloworld";
        byte[] bytes = Encoding.UTF8.GetBytes(str);

        fullPath = Path.Combine(Application.persistentDataPath, "Test.txt");

        FileInfo info = iFileSystem.GetFileInfo("Test.txt");
        long startIndex = 0;
        bool isuc = iFileSystem.WriteFile("Test.txt", bytes, (int)startIndex, bytes.Length);

        Log.Debug("写入文件 " + isuc);

    }
    /// <summary>
    ///     
    /// </summary>
    void ReadFileSys()
    {
        fullPath = Path.Combine(Application.persistentDataPath, "Test.txt");
        // 读取文件使用的 buffer，用此方式能够复用 buffer 来消除 GCAlloc

        FileInfo fInfo = iFileSystem.GetFileInfo("Test.txt");
        int length = fInfo.Length;
        byte[] buffer = new byte[length];
        int numBytesToRead = length;
        int numBytesRead = 0;
        while (numBytesToRead > 0)
        {
            // Read may return anything from 0 to numBytesToRead.
            int n = iFileSystem.ReadFile("Test.txt", buffer, numBytesRead, numBytesToRead);

            // Break when the end of the file is reached.
            if (n == 0)
                break;

            numBytesRead += n;
            numBytesToRead -= n;
        }

        string _str = Encoding.UTF8.GetString(buffer);
        Log.Debug(_str);
    }

    //从文件系统中删除
    void DeleteFile()
    {
        iFileSystem = GameEntry.FileSystem.GetFileSystem(fullPath);//先加载后读取
        bool isSuc = iFileSystem.DeleteFile("Test.txt");
        Log.Debug("删除文件 " + (isSuc.ToString()));
    }

    /// <summary>
    /// 销毁文件
    /// </summary>
    void DesFile()
    {
        GameEntry.FileSystem.DestroyFileSystem(iFileSystem, false);
        Log.Debug("销毁文件");
    }

    void SaveFile()
    {
        iFileSystem.SaveAsFile("Test.txt", @"C:\Users\Administrator\Desktop\Physics.txt");
    }
    #endregion



    #region 下载

    void TestDownLoad()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DownLoadFile();
        }
        
    }
    void DownLoadFile()
    {
        int downloadID= GameEntry.Download.AddDownload(@"C:\Users\Administrator\Desktop\index.php", "http://127.0.0.1:80/index.php","Test");
        Debug.Log($"下载id={downloadID},下载速度 ={GameEntry.Download.CurrentSpeed} ");
        //
    }
    #endregion


    #region 数据树状存储
    /// <summary>
    /// 设置数据节点，树形层级显示，类似Unity 层级视图
    /// </summary>
    void TestDataNode()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SetDataNode();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            GetDataNode();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SetRootData();
        }

    }
    void SetDataNode()
    {
        //VarString varStr = "123";
        VarDateTime time=new DateTime(2022,6,18,11,01,22);
        GameEntry.DataNode.SetData("A/B1",time);
    }

    void GetDataNode()
    {
        IDataNode ANode = GameEntry.DataNode.GetNode("A");
        VarDateTime varString= GameEntry.DataNode.GetData<VarDateTime>("B1",ANode);
        Debug.Log(varString+"  值类型输出");
    }

    void SetRootData()
    {
        VarString varStr = "123";
        VarTransform _varTransform = transform;
        VarGameObject _varGameObject = gameObject;
        GameEntry.DataNode.SetData(string.Empty,_varGameObject);
    }
    #endregion

    #region  数据表
    /// <summary>
    /// 数据表测试
    /// </summary>
    void TestDataTable()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DataTableRead();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            DataTableAddRow();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            GetAllDataInfo();//
        }
    }

    /// <summary>
    /// 数据表读取
    /// </summary>
    void DataTableRead()
    {
        IDataTable<DRTest> idataDrTests = GameEntry.DataTable.GetDataTable<DRTest>();
        DRTest drTest = idataDrTests.GetDataRow(111111);
        string assetName = drTest.AssetName;
        Log.Debug(assetName);
    }

    /// <summary>
    /// 数据表添加
    /// </summary>
    void DataTableAddRow()
    {
        IDataTable<DRTest> idataDrTests = GameEntry.DataTable.GetDataTable<DRTest>();
        bool isAdded = idataDrTests.AddDataRow("空格\t111113\t注释\tTestAsset\t3",this);
        //Log.Debug(assetName);
        Log.Debug(isAdded);
    }

    /// <summary>
    /// 获取表格每一行数据
    /// </summary>
    void GetAllDataInfo()
    {
        IDataTable<DRTest> idataDrTests= GameEntry.DataTable.GetDataTable<DRTest>();
        DRTest[] drTests= idataDrTests.GetAllDataRows();
        foreach (DRTest drTest in drTests)
        {
            Log.Debug(drTest.AssetName+",   "+drTest.Id+", "+drTest.BackgroundMusicId);
        }
    }

    /// <summary>
    /// 得到最值
    /// </summary>
    void GetMData()
    {
        IDataTable<DRTest> drTest=GameEntry.DataTable.GetDataTable<DRTest>();
        DRTest drMaxRow=drTest.MaxIdDataRow;
      

        Log.Debug(drMaxRow.AssetName+", "+drMaxRow.Id+",    "+drMaxRow.BackgroundMusicId);
        DRTest drMinRow = drTest.MaxIdDataRow;
        Log.Debug(drMinRow.AssetName + ", " + drMinRow.Id + ",    " + drMinRow.BackgroundMusicId);
    }
    #endregion
    
    #region 状态机 -切换状态
    //流程（Fsm：状态机）-流程基类（state:状态）
   
    void TestFsm()
    {
        if (Input.GetKeyDown(KeyCode.C))//创建
        {
            CreateFsm();
        }
       
    }

    private IFsm<ActorOwner> actorFsm;
    void CreateFsm()
    {
        ActorOwner actorOwner = new ActorOwner();
        actorFsm = GameEntry.Fsm.CreateFsm("Fsm", actorOwner, new IdleState(), new MoveState());
        actorFsm.Start<IdleState>();
    }
                
    #endregion

    #region 网络-socket

    void TestSocket()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SocketConnect();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {   
            SendPacket();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ReceivePacket();
        }
    }

    private INetworkChannel iNetworkChannel;
    private CSHeartBeat csHeartBeat = new CSHeartBeat();
    void SocketConnect()
    {
        NetworkChannelHelper helper=new NetworkChannelHelper();
        iNetworkChannel= GameEntry.Network.CreateNetworkChannel("Test", ServiceType.TcpWithSyncReceive, helper);
        iNetworkChannel.Connect(new IPAddress(new byte[]{127,0,0,1 }), 21);
        iNetworkChannel.Send(csHeartBeat);
    }
        
    /// <summary>
    /// 发送数据
    /// </summary>
    void SendPacket()
    {
        iNetworkChannel.Send(csHeartBeat);
    }

    /// <summary>
    /// 接受消息包
    /// </summary>
    void ReceivePacket()
    {
        GameEntry.Event.Subscribe(1, OnNetworkReceived);
        Log.Debug("消息包数量: " + iNetworkChannel.ReceivedPacketCount);
    }
    private void OnNetworkReceived(object sender, BaseEventArgs e)
    {
        CSHeartBeat ne = (CSHeartBeat)e;
        Log.Debug("日志 "+ne.Id);
    }   

    #endregion
}
