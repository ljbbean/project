using Common.Script;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.Part
{
    internal class DataHelper
    {
        public static IHashMapList DataTableToObjectList(DataTable table, int first, int count)
        {
            int rowCount = Math.Min(count, table.Rows.Count - first);
            HashObjectList records = new HashObjectList(rowCount);
            for (int i = 0; i < rowCount; i++)
            {
                records.Add(DataRowToObject(table, table.Rows[first + i]));
            }
            return records;
        }

        /// <summary>
        /// 从DataTable取全部数据以用于返回给客户端做为对象列表
        /// </summary>
        public static IHashMapList DataTableToObjectList(DataTable table)
        {
            return DataTableToObjectList(table, 0, table.Rows.Count);
        }
        public static IHashMap DataRowToObject(DataTable table, DataRow row)
        {
            HashMap record = new HashMap();
            object[] rowData = row.ItemArray;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string name = table.Columns[i].ColumnName;
                record[name] = rowData[i];
            }
            return record;
        }

        /// <summary>
        /// 一行数据返回给客户端做为对象
        /// </summary>
        public static IHashMap DataRowToObject(DataRow row)
        {
            return DataRowToObject(row.Table, row);
        }
    }
}
