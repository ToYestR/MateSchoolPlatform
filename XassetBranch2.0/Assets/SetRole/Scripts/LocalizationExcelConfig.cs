using UnityEngine;

namespace SmipleLocalization
{
    public class LocalizationExcelConfig : MonoBehaviour
    {
        /// <summary>
        /// 存放将Excel表格数据转化CS文件的文件夹路径
        /// </summary>
        public static readonly string assetFolderPath = "Assets/Resources/Datas/";

        /// <summary>
        /// 存放将Excel表格数据转化CS文件的路径
        /// </summary>
        public static readonly string assetPath = string.Format("{0}{1}.asset", assetFolderPath, "LanguageLocalizationData");

        /// <summary>
        /// 加载文件的路径
        /// </summary>
        public static readonly string loadAssetPath = "Datas/LanguageLocalizationData";
    }
}

