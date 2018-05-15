using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Threading;

using HtmlAgilityPack;
using Newtonsoft.Json;

using GroupControl.BLL;
using GroupControl.Model;
using GroupControl.Common;
using System.Text.RegularExpressions;
using GroupControl.Model;

namespace CrawPhoneNumberSection
{
    class Program
    {

        private static string url = "http://www.guisd.com/";

        private static IList<AreaInfo> areaList= SingleHepler<AreaInfoBLL>.Instance.GetList(null);
        static void Main(string[] args)
        {
            try
            {

                var list = new List<Test>() {
                    new Test() { name = "2", nickName = "2" },
                    new Test() { name = "1", nickName = "1" },
                    new Test() { name = "3", nickName = "3" },
                    new Test() { name = "3", nickName = "acv" }
                };

                Console.WriteLine(JsonConvert.SerializeObject(list));

                list =list.OrderBy(o => {

                    var defaluteNum = 0;

                    int.TryParse(o.nickName, out defaluteNum);

                    return defaluteNum;


                }).ToList();

                Console.WriteLine(JsonConvert.SerializeObject(list));

                Console.Read();

                //var str = "123";

                //Console.WriteLine(str.GetType().ToString());

                //if (str.GetType().Equals(typeof(string)))
                //{
                //    Console.WriteLine("this is type string");
                //}

                //var data = 23.00;

                //Console.WriteLine(data.GetType().ToString());


                //Task.Factory.StartNew(() =>
                //{
                //    var returnUrlList =GetNumberType(string.Format("{0}hd/", url)).Result;
                //    returnUrlList.ToList().ForEach( o =>
                //    {

                //        Console.WriteLine(o);
                //       // Thread.Sleep(1000);
                //        var numberSectionList =GetNumberSection(string.Format("{0}{1}", url, o)).Result;

                //        Console.WriteLine(JsonConvert.SerializeObject(numberSectionList));

                //    });
                //});

                Console.Read();

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 获取手机号类型以及号码段的Url
        /// </summary>
        /// <returns></returns>
        public static async Task<IList<string>> GetAreaInfo(string url)
        {
            Lazy<List<string>> urlList = new Lazy<List<string>>();
            var returnData = await GetCrawContentWithUrl(url);

            return await Task.Factory.StartNew<IList<string>>(() =>
            {
                ///解析抓取的内容
                var doc = new HtmlDocument();
                doc.LoadHtml(returnData);
                var nodeList = doc.DocumentNode.SelectNodes("//div[@class='wrap h_list']/dl[@class='hao_list'][1]/dd/a");

                if (nodeList != null && nodeList.Count > 0)
                {
                    var initCode = 1000;
                    foreach (var item in nodeList)
                    {
                        var text = Regex.Replace(item.InnerText, @"\d", "").Trim();

                        urlList.Value.Add(text);

                        Task.Factory.StartNew((code) =>
                        {
                            SingleHepler<AreaInfoBLL>.Instance.Insert(new AreaInfo() { Code = code.ToString(), Name = text });
                        }, initCode);

                        initCode++;
                    }
                }

                return urlList.Value;
            });
        }

        /// <summary>
        /// 获取手机号类型以及号码段的Url
        /// </summary>
        /// <returns></returns>
        public static async Task<IList<string>> GetNumberType(string url)
        {
            Lazy<List<string>> urlList = new Lazy<List<string>>();
            var returnData = await GetCrawContentWithUrl(url);

            return await Task.Factory.StartNew<IList<string>>(() =>
            {
                ///解析抓取的内容
                var doc = new HtmlDocument();
                doc.LoadHtml(returnData);
                var nodeList = doc.DocumentNode.SelectNodes("//div[@class='wrap h_list']/dl[@class='hao_list']/dd/a");
                if (nodeList != null && nodeList.Count > 0)
                {
                    nodeList.ToList().ForEach(o =>
                    {
                        if (o.HasAttributes)
                        {
                            o.Attributes.ToList().ForEach(q =>
                            {

                                if (q.Name.Equals("href"))
                                {
                                    urlList.Value.Add(q.Value);
                                }
                            });
                        }
                    });
                }

                return urlList.Value;
            });
        }


        /// <summary>
        /// 抓取手机区段
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<IList<string>> GetNumberSection(string url)
        {
            var returModelList = SingleHepler<AreaToPhoneNumberBLL>.Instance.GetList(new AreaToPhoneNumberViewModel() { LinkUrl=url});

            if (returModelList!=null && returModelList.Count>0)
            {
                return default(List<string>);
            }

            var returnData = await GetCrawContentWithUrl(url);
            Lazy<List<string>> urlList = new Lazy<List<string>>();
            Lazy<List<AreaToPhoneNumber>> modelList = new Lazy<List<AreaToPhoneNumber>>();

            return await Task.Factory.StartNew<IList<string>>(() =>
            {
                ///解析抓取的内容
                var doc = new HtmlDocument();
                doc.LoadHtml(returnData);
                var type = EnumPhoneNumberType.CMCC;
                var areaCode = string.Empty;
                var node = doc.DocumentNode.SelectSingleNode("//div[@class='wrap h_list']/dl[2]/dt");
                if (node != null)
                {
                    var text = node.InnerText;
                    if (text.Contains("联通"))
                    {
                        type= EnumPhoneNumberType.CUCC;
                    }
                    else if (text.Contains("移动"))
                    {
                        type = EnumPhoneNumberType.CMCC;
                    }
                    else if (text.Contains("电信"))
                    {
                        type = EnumPhoneNumberType.CTCC;
                    }

                   var returnModel=areaList.Where(o => text.Contains(o.Name.Trim())).FirstOrDefault();

                    if (returnModel!=null)
                    {
                        areaCode = returnModel.Code;
                    }

                }
                var nodeList = doc.DocumentNode.SelectNodes("//div[@class='wrap h_list']/dl[2]/dd/a");
                if (nodeList != null && nodeList.Count > 0)
                {
                    nodeList.ToList().ForEach(o =>
                    {
                        urlList.Value.Add(o.InnerText);
                        modelList.Value.Add(new AreaToPhoneNumber() {LinkUrl= url, NumberSection = o.InnerText, AreaCode = areaCode, Type = (int)type });
                    });
                }

                Console.WriteLine("开始保存数据到数据库");

                Task.Factory.StartNew(() =>
                {
                    SingleHepler<AreaToPhoneNumberBLL>.Instance.BatchInsert(modelList.Value);
                });

                return urlList.Value;
            });
        }

        /// <summary>
        /// 异步获取抓取的内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetCrawContentWithUrl(string url)
        {
            WebRequest request = WebRequest.Create(url);
            var returnResponse = await request.GetResponseAsync();
            Stream dataStream = returnResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding("UTF-8"));

            return await reader.ReadToEndAsync();
        }
    }

    public class Test {
         
       public  string name { get; set; }

       public string nickName { get; set; }

    }
}
