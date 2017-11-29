using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis
{
    public static class TestHelper
    {
        //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("server1:6379,server2:6379");
        public static ConnectionMultiplexer ConnectionMultiplexer = ConnectionMultiplexer.Connect("www.shin820.com");
    }
}
