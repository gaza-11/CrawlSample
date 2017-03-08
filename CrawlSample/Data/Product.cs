using System;

namespace CrawlSample.Data
{
    /// <summary>
    /// 商品情報
    /// </summary>
    [Serializable]
    public class Product
    {
        /// <summary>
        /// TowerRecordsのArtistID
        /// </summary>
        public int ArtistTowerId { get; set; }

        /// <summary>
        /// 商品画像のURL
        /// </summary>
        public  string ImageSrc { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="towerId">TowerRecordsのArtistID</param>
        /// <param name="imageSrc">商品画像のURL</param>
        /// <param name="title">商品名</param>
        public Product(int towerId, string imageSrc, string title)
        {
            this.ArtistTowerId = towerId;
            this.ImageSrc = imageSrc;
            this.Title = title;
        }
    }
}
