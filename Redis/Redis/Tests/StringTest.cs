using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Redis.Tests
{
    public class StringTest
    {
        [Fact]
        public async Task TestSet()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();

            await db.StringSetAsync("string_set_key", "string_set_val");

            var result = await db.StringGetAsync("string_set_key");
            Assert.Equal("string_set_val", result);
        }

        [Fact]
        public async Task TestMSet()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();

            await db.StringSetAsync(new KeyValuePair<RedisKey, RedisValue>[] {
                new KeyValuePair<RedisKey, RedisValue>("string_mset_key1","string_mset_val1"),
                new KeyValuePair<RedisKey, RedisValue>("string_mset_key2","string_mset_val2")
            });

            var result = await db.StringGetAsync(new RedisKey[] { "string_mset_key1", "string_mset_key2" });

            Assert.Equal("string_mset_val1", result[0]);
            Assert.Equal("string_mset_val2", result[1]);
        }

        [Fact]
        public async Task TestAppend()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.StringSetAsync("string_append_key", "string_append_val");

            await db.StringAppendAsync("string_append_key", "ue");

            var result = await db.StringGetAsync("string_append_key");
            Assert.Equal("string_append_value", result);
        }

        [Fact]
        public async Task TestIncr()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.StringSetAsync("string_incr_key", 1);

            long result = await db.StringIncrementAsync("string_incr_key");

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task TestDecr()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.StringSetAsync("string_decr_key", 1);

            long result = await db.StringDecrementAsync("string_decr_key");

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task TestGetRange()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.StringSetAsync("string_getrange_key", "string_getrange_val");

            var result = await db.StringGetRangeAsync("string_getrange_key", 7, -5);

            Assert.Equal("getrange", result);
        }

        [Fact]
        public async Task TestSetRange()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.StringSetAsync("string_setrange_key", "string_setrange_val");

            await db.StringSetRangeAsync("string_setrange_key", 7, "test");

            var result = await db.StringGetAsync("string_setrange_key");
            Assert.Equal("string_testange_val", result);
        }
    }
}
