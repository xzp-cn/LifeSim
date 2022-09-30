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

        public void SetTask(Vector2 AllOver)
        {
            if (AllOver.x==0)
            {
                m_Toggle.gameObject.SetActive(false);
            }
            else
            {
                m_Toggle.gameObject.SetActive(true);
                m_Toggle.isOn = AllOver.y!=0;//是否还有收藏品
            }
        }

        public void DisActiveRaycast()
        {
            image_lock.gameObject.SetActive(true);
            image_Btn.gameObject.SetActive(false);
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

