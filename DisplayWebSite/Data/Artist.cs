using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DisplayWebSite.Data
{
    public class Artist
    {
        /// <summary>
        /// 引数なしコンストラクタ
        /// </summary>
        public Artist()
        {
            this.TowerId = 0;
            this.Name = string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="id">TowerId</param>
        /// <param name="name">アーティスト名</param>
        public Artist(int id, string name)
        {
            this.TowerId = id;
            this.Name = name;
        }

        /// <summary>
        /// TowerRecordsのArtistID
        /// </summary
        [DisplayName("タワーレコードのID")]
        public int TowerId { get; set; }

        /// <summary>
        /// Artist名
        /// </summary>
        [DisplayName("アーティスト名")]
        public string Name { get; set; }
    }
}