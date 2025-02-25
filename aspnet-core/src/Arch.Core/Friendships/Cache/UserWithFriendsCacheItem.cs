﻿using System;
using System.Collections.Generic;

namespace Arch.Friendships.Cache
{
    public class UserWithFriendsCacheItem
    {
        public int? TenantId { get; set; }
        /// <summary>
        /// Is this entity OrganizationalUnit?
        /// </summary>

        public virtual string OrganizationalUnitId { get; set; }

        public long UserId { get; set; }

        public string TenancyName { get; set; }

        public string UserName { get; set; }

        public Guid? ProfilePictureId { get; set; }

        public List<FriendCacheItem> Friends { get; set; }
    }
}