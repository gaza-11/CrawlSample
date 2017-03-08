using DisplayWebSite.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DisplayWebSite.Models.Redis
{
    public class ArtistContext
    {
        /// <summary>
        /// アーティスト一覧の取得
        /// </summary>
        /// <returns></returns>
        public List<Artist> GetAll()
        {
            ConnectionMultiplexer redis = null;
            try
            {
                redis = ConnectionMultiplexer.Connect("localhost");
                var db = redis.GetDatabase();

                var artistList = new List<Artist>();
                foreach (var data in db.SortedSetRangeByRankWithScores("Artists"))
                {
                    artistList.Add(new Artist((int)data.Score, data.Element.ToString()));
                }

                return artistList;
            }
            finally
            {
                if (redis != null)
                {
                    redis.Close();
                }
            }
        }

        /// <summary>
        /// アーティスト一覧とお気に入りアーティストへの追加処理
        /// </summary>
        /// <param name="artist">アーティスト情報</param>
        /// <param name="userName">ユーザ名</param>
        internal void Add(Artist artist, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                // TODO：元の画面にエラー情報を返す方法はどうする？
                return;
            }

            ConnectionMultiplexer redis = null;
            try
            {
                redis = ConnectionMultiplexer.Connect("localhost");
                var db = redis.GetDatabase();

                // アーティスト一覧とお気に入りアーティストへの登録
                db.SortedSetAdd("Artists", artist.Name, (double)artist.TowerId);
                db.SortedSetAdd($"{userName}:CheckedArtists", artist.Name, (double)0);
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