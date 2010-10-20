﻿using System;
using Newtonsoft.Json.Linq;

namespace XBMC.JsonRpc
{
    public class XbmcVideoPlaylist : XbmcMediaPlaylist<XbmcVideo>
    {
        #region Constructor

        internal XbmcVideoPlaylist(JsonRpcClient client)
            : base("VideoPlaylist", client)
        {
            this.fields = new string[] { "title", "artist", "genre", "year", "rating", 
                                         "director", "trailer", "tagline", "plot", "plotoutline",
                                         "originaltitle", "lastplayed", "showtitle", "firstaired", "duration",
                                         "season", "episode", "runtime", "playcount", "writer",
                                         "studio", "mpaa", "premiered", "album" };
        }

        #endregion

        #region JSON RPC Calls

        #endregion

        #region Overrides of XbmcMediaPlaylist<XbmcVideo>

        public override XbmcVideo GetCurrentItem(params string[] fields)
        {
            JObject query = this.getItems(fields, -1, -1);
            if (query == null || query["current"] == null || query["items"] == null)
            {
                return null;
            }

            int current = (int)query["current"];
            JArray items = (JArray)query["items"];
            if (current < 0 || items == null || current > items.Count)
            {
                return null;
            }

            return XbmcVideo.FromJson((JObject)items[current]);
        }

        public override XbmcPlaylist<XbmcVideo> GetItems(params string[] fields)
        {
            return this.GetItems(-1, -1, fields);
        }

        public override XbmcPlaylist<XbmcVideo> GetItems(int start, int end, params string[] fields)
        {
            JObject query = this.getItems(fields, start, end);
            if (query == null || query["items"] == null)
            {
                return null;
            }

            XbmcPlaylist<XbmcVideo> playlist = XbmcPlaylist<XbmcVideo>.FromJson(query);
            foreach (JObject item in (JArray)query["items"])
            {
                playlist.Add(XbmcVideo.FromJson(item));
            }

            return playlist;
        }

        #endregion
    }
}