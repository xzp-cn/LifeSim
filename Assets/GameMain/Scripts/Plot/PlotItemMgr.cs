using System.Collections;
using System.Collections.Generic;
using GameFramework.DataNode;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.ObjectPool;
using GameFramework.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class PlotItemMgr:IUIModule
    {
        [SerializeField]
        private PlotItem m_PlotItemTemplate = null;

        [SerializeField]
        private Transform m_PlotInstanceRoot = null;

        [SerializeField]
        private int m_InstancePoolCapacity = 50;

        private IObjectPool<PlotItemObject> m_PlotItemObjectPool = null;
        private List<PlotItem> m_ActivePlotItems = null;
        private Transform m_CachedTransform = null;

        public int m_StoryId=0;
        public virtual void Init(Transform _par,PlotItem _plotItem)
        {
            if (_par == null)
            {
                Log.Error("You must set HP bar instance root first.");
                return;
            }

            m_PlotInstanceRoot = _par;
            m_PlotItemTemplate = _plotItem;
            m_CachedTransform = m_PlotInstanceRoot.GetComponent<Transform>();
            m_PlotItemObjectPool= GameEntry.ObjectPool.GetObjectPool<PlotItemObject>("PlotItem");
            if (m_PlotItemObjectPool==null)
            {
                m_PlotItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<PlotItemObject>("PlotItem", m_InstancePoolCapacity);
            }
            m_ActivePlotItems = new List<PlotItem>();

        }

        /// <summary>
        /// 显示所有的故事主线
        /// </summary>
        private IDataTable<DRSceneContent> drSceneTable;

        private DRSceneContent[] drSceneContentArray;
        public void InitStoryItems()
        {
            drSceneTable = GameEntry.DataTable.GetDataTable<DRSceneContent>();
            drSceneContentArray = drSceneTable.GetAllDataRows();
            foreach (DRSceneContent drSceneContent in drSceneContentArray)
            {
                PlotItem plotItem= ShowPlotItem(drSceneContent.Id);
                plotItem.Init(drSceneContent.Id,drSceneContent.StorySummary);
            }

        }


        public void FreshStory()
        {
            IDataNode node = GameEntry.DataNode.GetNode("Story");
            if (node == null)
            {
                //重置
                foreach (PlotItem m_activePlot in m_ActivePlotItems)
                {
                    m_activePlot.DisActiveRaycast();
                }
                m_ActivePlotItems[0].ActiveRaycast();
            }
            else
            {
                int id = (VarInt32)node.GetData();
                int startIndex = drSceneContentArray[0].Id;
                for (int i = startIndex, j = 0; i <= id; i++, j++)
                {
                    m_ActivePlotItems[j].ActiveRaycast();
                   
                }
                
            }
        }

        public PlotItem ShowPlotItem(int storyID)
        {
            PlotItem plotItem = GetActivePlotItem(storyID);
            if (plotItem == null)
            {
                plotItem = CreatePlotItem();
                m_ActivePlotItems.Add(plotItem);
            }

            return plotItem;
        }


        /// <summary>
        /// 隐藏 PlotItem
        /// </summary>
        /// <param name="plotItem"></param>
        private void HidePlotItem(PlotItem plotItem)
        {
            plotItem.Reset();
            m_ActivePlotItems.Remove(plotItem);
            m_PlotItemObjectPool.Unspawn(plotItem);

        }

        private PlotItem GetActivePlotItem(int id)
        {

            for (int i = 0; i < m_ActivePlotItems.Count; i++)
            {
                if (m_ActivePlotItems[i].storyId == id)
                {
                    return m_ActivePlotItems[i];
                }
            }

            return null;
        }

        private PlotItem CreatePlotItem()
        {
            PlotItem plotItem = null;
            PlotItemObject plotItemObject = m_PlotItemObjectPool.Spawn();
            if (plotItemObject != null)
            {
                plotItem = (PlotItem)plotItemObject.Target;
            }
            else
            {
                plotItem = Object.Instantiate(m_PlotItemTemplate);
                Transform transform = plotItem.GetComponent<Transform>();
                transform.SetParent(m_PlotInstanceRoot);
                transform.localScale = Vector3.one;
                m_PlotItemObjectPool.Register(PlotItemObject.Create(plotItem), true);
            }

            return plotItem;
        }

        //周期管理
        public virtual void OnOpen()
        {
            m_PlotInstanceRoot.gameObject.SetActive(true);

            foreach (PlotItem _plotItem in m_ActivePlotItems)
            {
                VarVector2 vector2= GameEntry.DataNode.GetData<VarVector2>("StoryPower/" + _plotItem.storyId);
                _plotItem.SetTask(vector2);
            }
            FreshStory();
        }
        public virtual void Update()
        {
        }
        public  void OnClose(bool isShutdown, object userData)
        {
            m_PlotInstanceRoot.gameObject.SetActive(isShutdown);
        }

        public void OnRecycle()
        {
           
        }
    }
}

