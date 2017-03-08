using CrawlSample.Data;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace CrawlSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd/ HH:mm:ss.fff"));

            ConnectionMultiplexer redis = null;
            try
            {
                redis = ConnectionMultiplexer.Connect("localhost");
                var db = redis.GetDatabase();

                ///// redis cliからの日本語のデータ登録は文字化けしたのでコードから実施
                ////db.SortedSetAdd("Artists", "人間椅子", (double)245346);

                var artistList = new List<Artist>();
                foreach (var data in db.SortedSetRangeByRankWithScores("Artists"))
                {
                    artistList.Add(new Artist((int)data.Score, data.Element.ToString()));
                }

                var taskList = new List<Task>();
                foreach (var artist in artistList)
                {
                    taskList.Add(Task.Run(() =>
                    {
                        Console.WriteLine($"{artist.Name} : {Thread.CurrentThread.ManagedThreadId} : {DateTime.Now.ToString("yyyy/MM/dd/ HH:mm:ss.fff")}");

                        var productList = GetTowerProducts(artist, 1);
                        foreach (var product in productList)
                        {
                            var productsKey = artist.Name + ":Products";
                            if (!db.KeyExists(productsKey) || !db.HashExists(productsKey, product.Title))
                            {
                                // 匿名型はシリアライズできないのでこれはダメ
                                // db.HashSet(productsKey, product.Title, ObjectToByteArray(new { ArtistTowerId = product.ArtistTowerId, ImgSrc = product.ImageSrc, Title = product.Title}));
                                // これだとImgSrcの中に：があるので区切りがわからない。
                                // db.HashSet(productsKey, product.Title, $"{product.ArtistTowerId}:{product.ImageSrc}:{product.Title}");
                                var temp = new Hashtable();
                                temp.Add("ArtistId", product.ArtistTowerId);
                                temp.Add("ImgSrc", product.ImageSrc);
                                temp.Add("Title", product.Title);

                                // 登録するデータをオブジェクトのシリアライズしたものでなくJSON型に変更する
                                db.HashSet(productsKey, product.Title, ObjectToByteArray(temp));
                            }
                        }

                        db.SortedSetAdd("ProductCounts", artist.Name, (double)productList.Count);
                        db.SortedSetAdd("TestUser:CheckedArtists", artist.Name, (double)productList.Count);

                        Console.WriteLine($"{artist.Name} : {Thread.CurrentThread.ManagedThreadId} : {DateTime.Now.ToString("yyyy/MM/dd/ HH:mm:ss.fff")}");
                    }));

                    Thread.Sleep(1000);
                }

                Task.WaitAll(taskList.ToArray());
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"{ex.GetType()} : {ex.Message}");
                foreach (var inner in ex.InnerExceptions)
                {
                    Console.WriteLine($"{inner.GetType()} : {inner.Message}");
                }
            }
            finally
            {
                if (redis != null)
                {
                    redis.Close();
                }
            }

            Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd/ HH:mm:ss.fff"));
        }


        private static object ByteArrayToObject(byte[] arrBytes)
        {
            using (var ms = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                ms.Write(arrBytes, 0, arrBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                return (Object)binForm.Deserialize(ms);
            }
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                return ms.ToArray();
            }
        }


        /// <summary>
        /// TowerRecordのサイトからアーティストの商品情報を取得します。
        /// </summary>
        /// <param name="artist">アーティスト情報</param>
        /// <param name="pageNum">ページ番号></param>
        /// <returns>アーティストの商品リスト</returns>
        private static List<Product> GetTowerProducts(Artist artist, int pageNum, List<Product> productList = null)
        {
            if (productList == null)
            {
                productList= new List<Product>();
            }

            var client = new WebClient();
            string strHtml;
            using (var stream = client.OpenRead(artist.GetTowerUrl(pageNum)))
            using (var reader = new StreamReader(stream))
            {
                strHtml = reader.ReadToEnd();
            }

            if (strHtml != null)
            {
                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(strHtml);

                var productsHtml = htmlDoc.DocumentNode
                                   .SelectNodes(@"//div[@class='discographyTableDivLine01 cfix']")
                                   .Select(a => a);

                var subDoc = new HtmlAgilityPack.HtmlDocument();
                foreach (var product in productsHtml)
                {
                    subDoc.LoadHtml(product.InnerHtml);
                    var imgSrc = subDoc.DocumentNode.SelectSingleNode(@"//div[@class='discographyTableImg01']/p/a/img").GetAttributeValue("src", "");
                    var title = subDoc.DocumentNode.SelectSingleNode(@"//div[@class='discographyTable01In']/p[@class='title']/a").InnerText;

                    productList.Add(new Product(artist.TowerId, imgSrc, WebUtility.HtmlDecode(title)));
                }

                if (productsHtml.Count() == 25)
                {
                    productList = GetTowerProducts(artist, ++pageNum, productList);
                }
            }
            else
            {
                Console.WriteLine("読み込み失敗");
            }

            return productList;
        }
    }
}
