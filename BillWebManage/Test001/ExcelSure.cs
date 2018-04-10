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
            string stateColumnName = "״̬";
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
                    row[stateColumnName] = "�ظ�";
                    more.Add(row);
                    continue;
                }
                row[stateColumnName] = "��ƥ��";
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
                    tempRow[stateColumnName] = "��ƥ��";
                }
                rt.NoRm = count;
                // label5.Text = string.Format("δ�ų�������{0}��", count);
            }
            //rt.NoDo = table.Rows.Count;
        }
        private class FileUploadResult
        {
            public List<string> ColumnDataFields;
            public DataTable Data;
            /// <summary>
            /// ��Ч����
            /// </summary>
            public int All;
            /// <summary>
            /// ��ƥ����
            /// </summary>
            public int Doned;
            /// <summary>
            /// δ�ų���
            /// </summary>
            public int NoDo;
            /// <summary>
            /// �ظ���
            /// </summary>
            public int ReDo;
            /// <summary>
            /// δ�ų���
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
                if (".xlsx".Equals(ext)) // 2007�汾
                    workbook = new XSSFWorkbook(file.InputStream);
                else if (".xls".Equals(ext)) // 2003�汾
                    workbook = new HSSFWorkbook(file.InputStream);
                else
                {
                    throw new Exception("��ǰֵ֧��excel����");
                }

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //���û���ҵ�ָ����sheetName��Ӧ��sheet�����Ի�ȡ��һ��sheet
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
                    int cellCount = firstRow.LastCellNum; //һ�����һ��cell�ı�� ���ܵ�����

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

                    //���һ�еı��
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //û�����ݵ���Ĭ����null��������������

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //ͬ��û�����ݵĵ�Ԫ��Ĭ����null
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