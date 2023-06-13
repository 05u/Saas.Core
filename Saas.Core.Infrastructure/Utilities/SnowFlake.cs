/*
 * 名称 :分布式ID生成算法
 * 描述 :核心思想是：一个long型的ID，使用其中41bit作为毫秒数，10bit作为机器编号，
 *       12bit作为毫秒内序列号。这个算法单机每秒内理论上最多可以生成1000*(2^12)，也就是400W的ID
 */

namespace Saas.Core.Infrastructure.Utilities
{
    /// <summary>
    /// SnowFlake(分布式ID生成算法)
    /// </summary>
    public class SnowFlake
    {
        /// <summary>
        /// 机器ID
        /// </summary>
        private static long _workerId;

        /// <summary>
        /// 唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳
        /// </summary>
        private const long Twepoch = 687888001020L;

        /// <summary>
        /// 机器码字节数。4个字节用来保存机器码
        /// </summary>
        private const int WorkerIdBits = 4;

        /// <summary>
        /// 最大机器ID
        /// </summary>
        public static long MaxWorkerId = -1L ^ -1L << WorkerIdBits;

        /// <summary>
        /// 计数器字节数，10个字节用来保存计数码
        /// </summary>
        private const int SequenceBits = 10;

        /// <summary>
        /// 机器码数据左移位数，就是后面计数器占用的位数
        /// </summary>
        private const int WorkerIdShift = SequenceBits;

        /// <summary>
        /// 时间戳左移动位数就是机器码和计数器总字节数
        /// </summary>
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits;

        /// <summary>
        /// 一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        /// </summary>
        public static long SequenceMask = -1L ^ -1L << SequenceBits;

        /// <summary>
        /// 计数器
        /// </summary>
        private static long _sequence;

        /// <summary>
        /// 时间戳
        /// </summary>
        private long _lastTimestamp = -1L;

        /// <summary>
        /// SnowFlake
        /// </summary>
        private static SnowFlake _sigle;

        /// <summary>
        /// private ctor
        /// </summary>
        /// <param name="workerId">机器ID</param>
        private SnowFlake(long workerId)
        {
            if (workerId > MaxWorkerId || workerId < 0)
                throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0 ", MaxWorkerId));
            _workerId = workerId;
        }

        /// <summary>
        /// 获取一个新的ID
        /// </summary>
        /// <returns></returns>
        public static string NewId()
        {
            if (_sigle == null)
            {
                _sigle = new SnowFlake(4L);//此处4L应该从配置文件里读取当前机器配置
            }

            var id = _sigle.NextId();
            var time = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $"{time}{id}";
        }

        /// <summary>
        /// 生成下一个ID
        /// </summary>
        /// <returns></returns>
        private long NextId()
        {
            lock (this)
            {
                var timestamp = TimeGen();
                if (_lastTimestamp == timestamp)
                {
                    //同一微妙中生成ID
                    _sequence = (_sequence + 1) & SequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                    if (_sequence == 0)
                    {
                        //一微妙内产生的ID计数已达上限，等待下一微妙
                        timestamp = TillNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    //不同微秒生成ID
                    _sequence = 0; //计数清0
                }
                if (timestamp < _lastTimestamp)
                {
                    //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                    throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", _lastTimestamp - timestamp));
                }
                _lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
                var nextId = (timestamp - Twepoch << TimestampLeftShift) | _workerId << WorkerIdShift | _sequence;
                return nextId;
            }
        }

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private static long TillNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp)
            {
                timestamp = TimeGen();
            }
            return timestamp;
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns></returns>
        private static long TimeGen()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
