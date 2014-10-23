﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wwwscan_gui
{
    /// <summary>
    /// txt目录字典类
    /// </summary>
    public class TXTFactory
    {

        public TXTFactory() { }



        /// <summary>
        /// 读取原始字典库
        /// </summary>
        /// <param name="path"></param>
        public List<string> ReadTXT(string path)
        {
            var cgi_list = new List<string>();
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    cgi_list.Add(line);
                }
            }
            return cgi_list;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="filecontent"></param>
        public void WriteTXT(string filepath, string filecontent)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            if (filecontent != null)
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    byte[] data = System.Text.Encoding.Default.GetBytes(filecontent);
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                    fs.Close();
                }
            }
        }



        /// <summary>
        /// 分类
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> PrepareDics(List<string> cgi_list)
        {
            if (cgi_list != null && cgi_list.Count > 0)
            {
                cgi_list = cgi_list.Distinct().ToList();
                var result = new Dictionary<string, List<string>>();
                //***************************************************
                var aspx = (from o in cgi_list
                            where o.Contains(".aspx") || o.Contains(".asp") || o.Contains(".cgi")
                            || o.Contains(".asa") || o.Contains(".ashx")
                            select o);
                result.Add("aspx", aspx.ToList());
                //***************************************************
                var java = (from o in cgi_list
                            where o.Contains(".java") || o.Contains(".jsp") || o.Contains(".cgi")
                            || o.Contains(".do") || o.Contains(".action")
                            select o);
                result.Add("java", java.ToList());
                //***************************************************
                var php = (from o in cgi_list
                           where o.Contains(".php") || o.Contains(".cgi")
                           select o);
                result.Add("php", php.ToList());
                //***************************************************
                var html = (from o in cgi_list
                            where o.Contains(".html") || o.Contains(".htm") || o.Contains(".shtml")
                            select o);
                result.Add("html", html.ToList());
                //***************************************************
                var other = (from o in cgi_list
                             where o.Contains(".txt")
                             || o.Contains(".db") || o.Contains(".rar") || o.Contains(".mdb")
                             || o.Contains(".sql") || o.Contains(".log") || o.Contains(".zip")
                             select o);
                result.Add("other", other.ToList());
                //***************************************************
                var editor = (from o in cgi_list
                              where o.Contains("fckeditor") || o.Contains("ewebeditor") || o.Contains("webedit")
                              || o.Contains("GfEditor") || o.Contains("eWebEditor") || o.Contains("editor")
                              select o);
                result.Add("editor", editor.ToList());
                //***************************************************注意有个endwith
                var dir = (from o in cgi_list
                           where (!o.Contains(".")) || (o.EndsWith("/"))
                           select o);
                result.Add("dir", dir.ToList());
                //***************************************************
                var backup = (from o in cgi_list
                              where o.Contains(".bak") || o.Contains(".tmp") || o.Contains(".test")
                              select o);
                result.Add("backup", backup.ToList());
                //***************************************************
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}