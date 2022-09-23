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

        //public Image image_circle;

        //public Image image_textBG;

        public Image image_Btn;

        public Text storyText;
        public Text m_StoryOrderText;

        public Image image_lock;

        public Toggle m_Toggle;
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
            m_StoryOrderText.text = (storyId%10000+1).ToString();
        }

        public void SetTask(bool AllOver)
        {
            m_Toggle.isOn = AllOver;
        }

        /// <summary>
        /// 当前剧情结束
        /// </summary>
        public void OverStory(bool isOver)
        {
            ActiveRaycast();
        }

        public void ActiveRaycast()
        {
            image_lock.gameObject.SetActive(false);
            image_Btn.gameObject.SetActive(true);
        }


        public void OnBtnClick()
        {

            GameEntry.DataNode.SetData("Story", new VarInt32() { Value = storyId });

            GameEntry.Event.Fire(this, StoryEventArgs.Create(storyId));

            return;
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

