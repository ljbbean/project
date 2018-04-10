﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaoBaoRequest;
using System.IO;
using System.Text;

namespace DataTest
{
    [TestClass]
    public class NetDataHandlerTest
    {
        [TestMethod]
        public void ListDataTransformationTest()
        {
            SearchOrderInfo data;
            using (StreamReader reader = new StreamReader("ListData.txt", Encoding.GetEncoding("gbk")))
            {
                string str = reader.ReadToEnd();
                data = NetDataHandler.ListDataTransformation(str);
            }
            Assert.IsNotNull(data);
        }
    }
}
