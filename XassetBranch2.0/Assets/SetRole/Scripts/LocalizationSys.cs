using UnityEngine;
using System;

namespace SmipleLocalization
{
    public static class LocalizationSys
    {
        private static LocalizationData localizationDatas;
        public static Action ChangeCurLanguageTypeEvent;//监听语言类型变化事件，在事件触发时设置内容，主要是方便用来游戏运行中测试本地化
        private static LanguageType mCurLanguageType;

        static LocalizationSys()
        {
            InitDatas();
            InitLanguageType();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private static void InitDatas()
        {
            localizationDatas = Resources.Load<LocalizationData>(LocalizationExcelConfig.loadAssetPath);
        }

        /// <summary>
        /// 根据系统语言设置默认语言
        /// </summary>
        private static void InitLanguageType()
        {
            SystemLanguage languageStr = Application.systemLanguage;
            if (languageStr == SystemLanguage.Chinese ||
                languageStr == SystemLanguage.ChineseSimplified)
            {
                CurLanguageType = LanguageType.Chinese;
            }
            else
            {
                CurLanguageType = LanguageType.English;
            }
        }

        /// <summary>
        /// 设置当前语言类型
        /// </summary>
        public static LanguageType CurLanguageType
        {
            set
            {
                if (mCurLanguageType != value)
                {
                    mCurLanguageType = value;
                    ChangeCurLanguageTypeEvent?.Invoke();
                }
            }
        }

        /// <summary>
        /// 对应的语言本地化
        /// </summary>
        /// <param name="textKey">需要本地化的文本</param>
        public static string Localize(TextKey textKey)
        {
            return Localize(textKey, mCurLanguageType);
        }

        /// <summary>
        /// 获取对应的语言本地化
        /// </summary>
        /// <param name="textKey">需要本地化的文本</param>
        /// <param name="languageType">语言类型</param>
        /// <returns></returns>
        private static string Localize(TextKey textKey, LanguageType languageType)
        {
            string value = string.Empty;
            TextLocalization textInfo = localizationDatas.GetTextInfo(textKey);
            if (textInfo != null)
            {
                ItemData typeInfo = textInfo.GetType(languageType);
                //如果没找到对应语言本地化信息，则设置默认中文信息
                if (typeInfo == null)
                {
                    typeInfo = textInfo.GetType(LanguageType.Chinese);
                }
                value = typeInfo.content;
            }
            return value;
        }
    }

    //这里保证与Excel表顺序一致
    public enum LanguageType
    {
        Chinese = 0,//默认语言类型为中文
        English = 1,
        Japanese = 2,
    }

    public enum TextKey
    {
        Face,
        Hair,
        Top,
        Pants,
        Shoes,
        Sex
    }
}
