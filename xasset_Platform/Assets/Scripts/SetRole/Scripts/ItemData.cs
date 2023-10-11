using System;
using UnityEngine;

namespace SmipleLocalization
{
    [Serializable]
    public class ItemData
    {
        [SerializeField]
        private string itemName;//语言名称，主要是方便编辑器上查看
        public LanguageType languageType;//语言类型
        public string content;//对应的内容

        //构造函数：初始化数据成员
        public ItemData(LanguageType _languageType, string _content)
        {
            this.languageType = _languageType;
            this.itemName = _languageType.ToString();
            this.content = _content;
        }
    }
}
