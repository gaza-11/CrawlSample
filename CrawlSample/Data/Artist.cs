namespace CrawlSample.Data
{
    /// <summary>
    /// Artist情報
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// TowerRecordsのArtistID
        /// </summary>
        public int TowerId { get; set; }

        /// <summary>
        /// Artist名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="towerId">TowerRecordsのArtistID</param>
        /// <param name="name">Artist名</param>
        public Artist(int towerId, string name)
        {
            this.TowerId = towerId;
            this.Name = name;
        }

        /// <summary>
        /// 受け取ったページ番号とインスタンス内のArtistIdを組み合わせてURLを作成して返却します。
        /// </summary>
        /// <param name="pageNum">ページ番号</param>
        /// <returns>アーティストの商品ページ</returns>
        public string GetTowerUrl(int pageNum)
        {
            return $"http://tower.jp/search/item/{this.Name}?recondition=True&artistId={this.TowerId}&page={pageNum}";
        }
    }
}
