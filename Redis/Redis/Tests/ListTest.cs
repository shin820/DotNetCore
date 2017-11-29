using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Redis.Tests
{
    public class ListTest
    {
        [Fact]
        public async Task TestLPush()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();

            await db.KeyDeleteAsync("list_lpush_key");
            await db.ListLeftPushAsync("list_lpush_key", "val1");
            await db.ListLeftPushAsync("list_lpush_key", "val2");

            var result = await db.ListRangeAsync("list_lpush_key", 0, -1);
            Assert.Equal("val2", result[0]);
            Assert.Equal("val1", result[1]);
        }

        [Fact]
        public async Task TestRPush()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();

            await db.KeyDeleteAsync("list_rpush_key");
            await db.ListRightPushAsync("list_rpush_key", "val1");
            await db.ListRightPushAsync("list_rpush_key", "val2");

            var result = await db.ListRangeAsync("list_rpush_key", 0, -1);
            Assert.Equal("val1", result[0]);
            Assert.Equal("val2", result[1]);
        }

        [Fact]
        public async Task TestLRem()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync("list_lrem_key");
            await db.ListRightPushAsync("list_lrem_key", "val1");
            await db.ListRightPushAsync("list_lrem_key", "val2");
            await db.ListRightPushAsync("list_lrem_key", "val1");
            await db.ListRightPushAsync("list_lrem_key", "val2");

            await db.ListRemoveAsync("list_lrem_key", "val2", 1); // 从左往右删除1个匹配值
            await db.ListRemoveAsync("list_lrem_key", "val1", -1);// 从右往左删除1个匹配值

            var result = await db.ListRangeAsync("list_lrem_key", 0, -1);
            Assert.Equal("val1", result[0]);
            Assert.Equal("val2", result[1]);
        }

        [Fact]
        public async Task TestLTrim()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync("list_ltrim_key");
            await db.ListRightPushAsync("list_ltrim_key", "val1");
            await db.ListRightPushAsync("list_ltrim_key", "val2");
            await db.ListRightPushAsync("list_ltrim_key", "val3");
            await db.ListRightPushAsync("list_ltrim_key", "val4");

            await db.ListTrimAsync("list_ltrim_key", 1, 2);

            var result = await db.ListRangeAsync("list_ltrim_key", 0, -1);
            Assert.Equal(2, result.Length);
            Assert.Equal("val2", result[0]);
            Assert.Equal("val3", result[1]);
        }

        [Fact]
        public async Task TestLLen()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync("list_llen_key");
            await db.ListRightPushAsync("list_llen_key", "val1");
            await db.ListRightPushAsync("list_llen_key", "val2");

            var result = await db.ListLengthAsync("list_llen_key");

            Assert.Equal(2, result);
        }


        [Fact]
        public async Task TestLInsert()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync("list_linsert_key");
            await db.ListRightPushAsync("list_linsert_key", "val1");
            await db.ListRightPushAsync("list_linsert_key", "val3");

            await db.ListInsertBeforeAsync("list_linsert_key", "val3", "val2");
            await db.ListInsertAfterAsync("list_linsert_key", "val3", "val4");

            var result = await db.ListRangeAsync("list_linsert_key", 0, -1);

            Assert.Equal(4, result.Length);
            Assert.Equal("val1", result[0]);
            Assert.Equal("val2", result[1]);
            Assert.Equal("val3", result[2]);
            Assert.Equal("val4", result[3]);
        }

        [Fact]
        public async Task TestRPopLPush()
        {
            IDatabase db = TestHelper.ConnectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync("list_rpoplpush_l");
            await db.ListRightPushAsync("list_rpoplpush_l", "val2");
            await db.KeyDeleteAsync("list_rpoplpush_r");
            await db.ListRightPushAsync("list_rpoplpush_r", "val1");

            await db.ListRightPopLeftPushAsync("list_rpoplpush_r", "list_rpoplpush_l");

            var result1 = await db.ListRangeAsync("list_rpoplpush_l", 0, -1);
            var result2 = await db.ListRangeAsync("list_rpoplpush_r", 0, -1);
            Assert.Equal(2, result1.Length);
            Assert.Empty(result2);
            Assert.Equal("val1", result1[0]);
            Assert.Equal("val2", result1[1]);
        }
    }
}
