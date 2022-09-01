using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;

namespace StarForce
{
    public class PlotItem : MonoBehaviour
    {
        /// <summary>
        /// 剧情id
        /// </summary>
        public int storyId=0;

        public Image image_circle;

        public Image image_textBG;

        public Text storyText;
        // Start is called before the first frame update

        public void Init(int _storyId,string _text)
        {
            if (_storyId == 0)
            {
                Log.Error("Owner is invalid.");
                return;
            }

            storyId = _storyId;

            gameObject.SetActive(true);
            StopAllCoroutines();

            storyText.text = _text;
        }


        public void SetCurrent(bool isCurrent)
        {
            image_textBG.enabled = isCurrent;
        }


        /// <summary>
        /// 当前剧情结束
        /// </summary>
        public void OverStory(bool isOver)
        {
            image_circle.enabled = isOver;
        }


        public void OnBtnClick()
        {
            //流程数据重置
            GameEntry.Event.Fire(this,StoryEventArgs.Create(storyId));
            Log.Debug("切换场景事件 "+storyId);
           
            //切换ui背景
            VarInt32 varId=new VarInt32();
            varId.SetValue(storyId);
            GameEntry.Event.Fire(this,PlotItemCallEventArgs.Create(varId));
        }

        public void PlayUISound(int uiSoundId)
        {
            GameEntry.Sound.PlayUISound(uiSoundId);
        }
        public void Reset()
        {
            StopAllCoroutines();
            storyId = 0;
            gameObject.SetActive(false);
        }
    }
}

