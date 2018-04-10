using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using Carpa.Web.Validation.Validators;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Text;
//using Test001.Login;

namespace Test001
{
    public class ExcelSure : Page
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        [WebMethod]
        public object SaveData(FileInfo fileInfo)
        {
            DataTable table = ExcelToDataTable(fileInfo.File1);
            FileUploadResult rt = new FileUploadResult();
            ReCheckData(table, rt);
            rt.ColumnDataFields = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                rt.ColumnDataFields.Add(column.ColumnName);
            }
            rt.Data = table;
            return rt;
        }
        
        private void ReCheckData(DataTable table, FileUploadResult rt)
        {
            string stateColumnName = "状态";
            table.Columns.Add(stateColumnName);

            StringBuilder builder = new StringBuilder();
            Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
            List<DataRow> more = new List<DataRow>();

            for (var i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                string value = row[0] == DBNull.Value ? "" : row[0].ToString();
                if (string.IsNullOrEmpty(value))
                {
                    table.Rows.RemoveAt(i);
                    i--;
                    continue;
                }
                if (dictionary.ContainsKey(value))
                {
                    row[stateColumnName] = "重复";
                    more.Add(row);
                    continue;
                }
                row[stateColumnName] = "待匹配";
                dictionary.Add(value, row);
                builder.AppendFormat("'{0}',", value);
            }
            rt.All = table.Rows.Count;
            rt.ReDo = more.Count;
            string ids = builder.Length > 0 ? builder.ToString().Substring(0, builder.Length - 1) : "";
            if (string.IsNullOrEmpty(ids))
            {
                return;
            }
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                DataTable hasTable = db.ExecuteSQL(string.Format("select * from bill where scode in ({0})", ids));
                rt.Doned = hasTable.Rows.Count;
                int count = 0;
                for (var i = 0; i < hasTable.Rows.Count; i++)
                {
                    DataRow tRow = hasTable.Rows[i];
                    DataRow tempRow = dictionary[tRow["scode"].ToString()];
                    tempRow[stateColumnName] = "已匹配";
                }
                rt.NoRm = count;
                // label5.Text = string.Format("未排除行数：{0}行", count);
            }
            //rt.NoDo = table.Rows.Count;
        }
        private class FileUploadResult
        {
            public List<string> ColumnDataFields;
            public DataTable Data;
            /// <summary>
            /// 有效行数
            /// </summary>
            public int All;
            /// <summary>
            /// 已匹配数
            /// </summary>
            public int Doned;
            /// <summary>
            /// 未排除数
            /// </summary>
            public int NoDo;
            /// <summary>
            /// 重复数
            /// </summary>
            public int ReDo;
            /// <summary>
            /// 未排除数
            /// </summary>
            public int NoRm;
        }


        private DataTable ExcelToDataTable(HttpPostedFile file, string sheetName = null, bool isFirstRowColumn = true)
        {
            IWorkbook workbook = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                var ext = Path.GetExtension(file.FileName).ToLower();
                if (".xlsx".Equals(ext)) // 2007版本
                    workbook = new XSSFWorkbook(file.InputStream);
                else if (".xls".Equals(ext)) // 2003版本
                    workbook = new HSSFWorkbook(file.InputStream);
                else
                {
                    throw new Exception("当前值支持excel操作");
                }

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        public class FileInfo
        {
            public HttpPostedFile File1;
        }
    }
}