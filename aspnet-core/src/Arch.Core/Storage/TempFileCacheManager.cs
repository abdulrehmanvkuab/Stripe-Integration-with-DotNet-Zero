﻿using System;
using Abp.Runtime.Caching;

namespace Arch.Storage
{
    public class TempFileCacheManager : ITempFileCacheManager
    {
        private const string TempFileCacheName = "TempFileCacheName";

        private readonly ITypedCache<string, TempFileInfo> _cache;

        public TempFileCacheManager(ICacheManager cacheManager)
        {
            _cache = cacheManager.GetCache<string, TempFileInfo>(TempFileCacheName);
        }

        public void SetFile(string token, byte[] content)
        {
            _cache.Set(token, new TempFileInfo(content), TimeSpan.FromMinutes(60)); // expire time is 1 min by default //Femi increased to 60 because of large files
        }

        public byte[] GetFile(string token)
        {
            var cache = _cache.GetOrDefault(token);
            return cache?.File;
        }

        public void SetFile(string token, TempFileInfo info)
        {
            _cache.Set(token, info, TimeSpan.FromMinutes(60)); // expire time is 1 min by default  //Femi increased to 60 because of large files
        }

        public TempFileInfo GetFileInfo(string token)
        {
            return _cache.GetOrDefault(token);
        }
    }
}