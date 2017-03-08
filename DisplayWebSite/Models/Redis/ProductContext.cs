using DisplayWebSite.Data;
using DisplayWebSite.Utilities;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisplayWebSite.Models.Redis
{
    public class ProductContext
    {
        public List<Product> GetAllByArtistName(string artistName)
        {
            ConnectionMultiplexer redis = null;
            try
            {
                redis = ConnectionMultiplexer.Connect("localhost");
                var db = redis.GetDatabase();

                var productList = new List<Product>();
                foreach (var data in db.HashGetAll($"{artistName}:Products"))
                {
                    var temp = (Hashtable)ByteConverter.ByteArrayToObject(data.Value);
                    productList.Add(new Product((int)temp["ArtistId"], temp["ImgSrc"] as string, temp["Title"] as string));
                }

                return productList;
            }
            finally
            {
                if (redis != null)
                {
                    redis.Close();
                }
            }
        }
    }
}