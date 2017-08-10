using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace M3U8_Downloader
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 开启httpserver监听
        /// </summary>
        /// <param name="url">监听地址</param>
        /// <param name="fileNamePar">文件名称参数名</param>
        /// <param name="urlPar">下载地址url</param>
        public void RunHttpServer(string url, string fileNamePar , string urlPar)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = "http://localhost:8080/";
            }
            if (string.IsNullOrEmpty(fileNamePar))
            {
                fileNamePar = "file";
            }
            if (string.IsNullOrEmpty(urlPar))
            {
                urlPar = "url";
            }

            using (HttpListener listerner = new HttpListener())
            {
                this.textBox_Info.AppendText($"\r\n已开启httpserver服务器！监听地址：{url}:文件名参数名：{fileNamePar}，地址参数名:{urlPar}\r\n");
                listerner.AuthenticationSchemes = AuthenticationSchemes.Anonymous;//指定身份验证 Anonymous匿名访问
                listerner.Prefixes.Add(url);
                listerner.Start();
                while (this.EnableHttpServer)
                {
                    //等待请求连接
                    //没有请求则GetContext处于阻塞状态
                    HttpListenerContext ctx = listerner.GetContext();
                    ctx.Response.StatusCode = 200;
                    NameValueCollection gb2312Requests;
                    gb2312Requests = HttpUtility.ParseQueryString(ctx.Request.Url.Query, Encoding.UTF8);

                    string fileUrl = gb2312Requests[urlPar];
                    string file = gb2312Requests[fileNamePar];

                    if (fileUrl != null && file != null)
                    {
                        this.Invoke(
                              new Action(delegate ()
                              {
                                  textBox_Adress.Text = WebUtility.UrlDecode(fileUrl);
                                  var name = Utf8ToGB2312(file);
                                  textBox_Name.Text = Utf8ToGB2312(name);
                                  this.BeginDownload();
                                  this.textBox_Info.AppendText($"收到来自http服务器的任务开始下载！！\r\n");
                              })
                          );
                    }


                    //使用Writer输出http响应代码
                    using (StreamWriter writer = new StreamWriter(ctx.Response.OutputStream))
                    {
                        writer.WriteLine("{'code':0}");
                        writer.Close();
                        ctx.Response.Close();
                    }
                    if (DisposeHttpServer)
                    {
                        listerner.Stop();
                    }

                }
            }

            this.textBox_Info.AppendText($"\r\n已关闭httpserver服务器！\r\n");
        }

        public string GB2312ToUtf8(string gb2312String)
        {
            Encoding fromEncoding = Encoding.GetEncoding("gb2312");
            Encoding toEncoding = Encoding.UTF8;
            return EncodingConvert(gb2312String, fromEncoding, toEncoding);
        }

        public string Utf8ToGB2312(string utf8String)
        {
            Encoding fromEncoding = Encoding.UTF8;
            Encoding toEncoding = Encoding.GetEncoding("gb2312");
            return EncodingConvert(utf8String, fromEncoding, toEncoding);
        }
        public string EncodingConvert(string fromString, Encoding fromEncoding, Encoding toEncoding)
        {
            byte[] fromBytes = fromEncoding.GetBytes(fromString);
            byte[] toBytes = Encoding.Convert(fromEncoding, toEncoding, fromBytes);

            string toString = toEncoding.GetString(toBytes);
            return toString;
        }
    }
}
