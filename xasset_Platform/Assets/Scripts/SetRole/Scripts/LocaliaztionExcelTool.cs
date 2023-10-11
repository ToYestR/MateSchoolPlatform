using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

namespace SmipleLocalization
{
    public class LocalizationExcelTool
    {
        /// <summary>
        /// 读取表格数据，生成对应的数据
        /// </summary>
        public static List<TextLocalization> CreateDataWithExcel(string filePath)
        {
            List<TextLocalization> localizationTxts = new List<TextLocalization>();
            //获得表格数据
            int row = 0, column = 0;
            DataRowCollection collection = ReadExcel(filePath, ref row, ref column);
            //根据Excel表的定义，第二行开始才是数据
            for (int i = 1; i < row; i++)
            {
                string[] language = new string[column - 1];
                for (int j = 1; j < column; j++)
                {
                    language[j - 1] = collection[i][j].ToString();
                }
                TextLocalization tl = new TextLocalization(collection[i][0].ToString(), language);
                localizationTxts.Add(tl);
            }
            return localizationTxts;
        }

        /// <summary>
        /// 读取Excel文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        private static DataRowCollection ReadExcel(string filePath, ref int row, ref int column)
        {
            //创建文件流
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            //通过文件流读取Excel文件
            IExcelDataReader edReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //将excel表中的数据读取到DataTable中，方便找到对应的数据
            DataSet result = edReader.AsDataSet();
            //Tables[0]下标0表示Excel文件中第一张表的数据
            row = result.Tables[0].Rows.Count;
            column = result.Tables[0].Columns.Count;
            return result.Tables[0].Rows;
        }
    }
}
