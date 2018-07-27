using System;
using Carpa.Web.Script;
using Carpa.Web.Ajax;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Text;
using WebMain.Model;

namespace WebMain
{
    public class PlugManager : Page
    {
        private const string tempFileFolder = "tempPlugs";
        private const string saveFileFolder = "data\\plug";
        public override void Initialize()
        {
            base.Initialize();
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                Context["data"] = null;
                return;
            }
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                db.AddParameter("pid", id);
                IHashObject data = db.SelectSingleRow("select picon,pid as id,pname,pversion,pkind,plabel,pshowway,pwindowname,pdes,ppics,pid,pdownpath,pvideo from plugs where pid=@pid");
                string ppics = data.GetValue<string>("ppics");
                string[] array = ppics.Split(',');
                data["ppic1"] = array.Length > 0 ? array[0] : "";
                data["ppic2"] = array.Length > 1 ? array[1] : "";
                data["ppic3"] = array.Length > 2 ? array[2] : "";
                string[] labels = data.GetValue<string>("plabel", ",,").Split(',');
                data["plabel1"] = labels.Length > 0 ? labels[0] : "";
                data["plabel2"] = labels.Length > 1 ? labels[1] : "";
                data["plabel3"] = labels.Length > 2 ? labels[2] : "";

                Context["data"] = data;
            }
        }

        [WebMethod]
        public string SaveDataImage(FileInfoIcon fileInfo)
        {
            HttpPostedFile file = fileInfo.FileIcon;
            return SaveFileToLocal(file, new List<string>() { ".jpg", ".png", ".ico", ".bmp" }, "当前只支持*.jpg,*.png,*.ico,*.bmp格式文件");
        }

        [WebMethod]
        public object SaveDataVideo(FileInfoVideo fileInfo)
        {
            HttpPostedFile file = fileInfo.FileVideo;
            return SaveFileToLocal(file, new List<string>() { ".AVI", ".wma", ".rmvb", ".rm", ".flash", ".mp4", ".mid", "3GP" }, "当前只支持.AVI, .wma, .rmvb,.rm, .flash,.mp4,.mid, 3GP格式文件");
        }

        [WebMethod]
        public object SaveDataZip(FileInfoZip fileInfo)
        {
            HttpPostedFile file = fileInfo.FileZip;
            return SaveFileToLocal(file, new List<string>() { ".zip" }, "当前只支持.zip格式文件");
        }

        [WebMethod]
        public object SavePreviewFile(FileInfoData fileInfo)
        {
            HttpPostedFile file = fileInfo.Filedata;
            return SaveFileToLocal(file, new List<string>() { ".jpg", ".png", ".ico", ".bmp" }, "当前只支持*.jpg,*.png,*.ico,*.bmp格式文件");
        }

        private string SaveFileToLocal(HttpPostedFile file, List<string> exts, string error)
        {
            string ext = Path.GetExtension(file.FileName);
            if (file.ContentLength == 0)
            {
                return "";
            }
            if (!exts.Contains(ext.ToLower()))
            {
                throw new Exception(error);
            }
            string dir = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, tempFileFolder);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string filePath = string.Format("{0}/{1}{2}", dir, Guid.NewGuid().ToString().Replace("-", ""), ext);
            file.SaveAs(filePath);
            return filePath.Substring(AppDomain.CurrentDomain.BaseDirectory.Length);
        }

        public class FileInfoIcon
        {
            public HttpPostedFile FileIcon;
        }

        public class FileInfoVideo
        {
            public HttpPostedFile FileVideo;
        }

        public class FileInfoZip
        {
            public HttpPostedFile FileZip;
        }

        public class FileInfoData
        {
            public HttpPostedFile Filedata;
        }

        [WebMethod]
        public void SavePlugData(Plug plug)
        {
            HashObject hash = new HashObject();
            long fileLength = 0;
            //数据搬移
            plug.PDownpath = MoveFile(plug.PDownpath, out fileLength);
            plug.PTotal = fileLength;
            plug.PVideo = MoveFile(plug.PVideo, out fileLength);
            plug.PIcon = MoveFile(plug.PIcon, out fileLength);
            plug.PPic1 = MoveFile(plug.PPic1, out fileLength);
            plug.PPic2 = MoveFile(plug.PPic2, out fileLength);
            plug.PPic3 = MoveFile(plug.PPic3, out fileLength);
            hash.Add("pdownpath", plug.PDownpath);
            hash.Add("pvideo", plug.PVideo);
            hash.Add("picon", plug.PIcon);
            hash.Add("ppics", plug.PPics);
            hash.Add("pcreatedate", DateTime.Now);

            //数据保存
            using (DbHelper db = AppUtils.CreateDbHelper())
            {
                try
                {
                    db.BeginTransaction();

                    List<string> needToDeleteFiles = DeleteOldData(plug, db, hash);
                    if (plug.PTotal == 0)
                    {
                        plug.PTotal = hash.GetValue<int>("ptotal", 0);
                    }
                    string sql = "insert into plugs (pid,pname,pversion,pkind,plabel,picon,pdes,pvideo,pvideoweb,pcost,plevel,pdownpath,pdownpathweb,ptotal,pext,ppics,pcreatedate,pupdatedate,pshowway,pstorename,pwindowname) values(@pid,@pname,@pversion,@pkind,@plabel,@picon,@pdes,@pvideo,@pvideoweb,@pcost,@plevel,@pdownpath,@pdownpathweb,@ptotal,@pext,@ppics,@pcreatedate,@pupdatedate,@pshowway,@pstorename,@pwindowname)";
                    db.AddParameter("pid", plug.PId);
                    db.AddParameter("pname", plug.PName);
                    db.AddParameter("pversion", plug.PVersion);
                    db.AddParameter("pkind", plug.PKind);
                    db.AddParameter("plabel", plug.PLabel);
                    db.AddParameter("picon", hash.GetValue<string>("picon"));
                    db.AddParameter("pdes", plug.PDes);
                    db.AddParameter("pvideo", hash.GetValue<string>("pvideo"));
                    db.AddParameter("pcost", plug.PCost);
                    db.AddParameter("plevel", plug.PLevel);
                    db.AddParameter("pdownpath", hash.GetValue<string>("pdownpath"));
                    db.AddParameter("ptotal", plug.PTotal);
                    db.AddParameter("pext", plug.PExt);
                    db.AddParameter("ppics", hash.GetValue<string>("ppics"));
                    db.AddParameter("pcreatedate", hash.GetValue<DateTime>("pcreatedate"));
                    db.AddParameter("pupdatedate", plug.PUpdatedate);
                    db.AddParameter("pshowway", plug.PShowway);
                    db.AddParameter("pstorename", plug.PStorename);
                    db.AddParameter("pwindowname", plug.PWindowname);
                    db.AddParameter("pvideoweb", GetWebUri());
                    db.AddParameter("pdownpathweb", GetWebUri());
                    db.ExecuteScalerSQL(sql);
                    foreach (string file in needToDeleteFiles)
                    {
                        File.Delete(file);
                    }
                    db.CommitTransaction();
                }
                catch (Exception e)
                {
                    if (db.HasBegunTransaction)
                    {
                        db.RollbackTransaction();
                    }
                    throw e;
                }

            }
        }
        private string GetWebUri()
        {
            var Reqeuest = this.HttpContext.Request;

            string absoluteUri = Request.Url.AbsoluteUri;
            string pathAndQuery = Request.Url.PathAndQuery;

            return string.Format("{0}{1}/", absoluteUri.Substring(0, absoluteUri.Length - pathAndQuery.Length), Request.ApplicationPath);
        }
        private static List<string> DeleteOldData(Plug plug, DbHelper db, HashObject hash)
        {
            List<string> rtlist = new List<string>();
            if (plug.PId == 0)
            {
                plug.PId = Cuid.NewCuid();
                return rtlist;
            }
            db.AddParameter("pid", plug.PId);
            IHashObject item = db.SelectSingleRow("select pdownpath,pvideo,picon,ppics,pcreatedate,ptotal from plugs where pid=@pid");
            foreach (string key in item.Keys)
            {
                string path = item.GetValue<string>(key);
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }
                if (key == "ppics")
                {
                    foreach (string item1 in path.Split(','))
                    {
                        if (string.IsNullOrEmpty(item1) || item1.StartsWith(saveFileFolder))
                        {
                            continue;
                        }
                        rtlist.Add(string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, item1));
                    }
                }
                else
                {
                    if (hash.GetValue<string>(key) == item.GetValue<string>(key))
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(hash.GetValue<string>(key)))
                    {
                        hash[key] = item[key];
                    }
                    rtlist.Add(string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, item[key]));
                }
            }
            hash["pcreatedate"] = item["pcreatedate"];
            hash["ptotal"] = item["ptotal"];
            db.Delete("plugs", "pid", plug.PId);
            return rtlist;
        }

        private string MoveFile(string filePath, out long fileLength)
        {
            fileLength = 0;
            if (string.IsNullOrEmpty(filePath))
            {
                return "";
            }
            if (filePath.StartsWith(saveFileFolder))
            {
                return filePath;
            }
            if (!filePath.StartsWith(tempFileFolder))
            {
                return "";
            }
            
            string newFilePath = string.Format("{0}{1}\\", AppDomain.CurrentDomain.BaseDirectory, saveFileFolder);
            if (!Directory.Exists(newFilePath))
            {
                Directory.CreateDirectory(newFilePath);
            }
            newFilePath = string.Format("{0}{1}{2}", newFilePath, Guid.NewGuid().ToString().Replace("-", ""), Path.GetExtension(filePath));

            
            filePath = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, filePath);
            FileInfo info = new FileInfo(filePath);
            info.MoveTo(newFilePath);
            fileLength = info.Length;
            File.Delete(filePath);
            return newFilePath.Substring(AppDomain.CurrentDomain.BaseDirectory.Length);
        }
    }
}
