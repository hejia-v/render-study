using System;
using System.Diagnostics;
using System.Threading;

class GRandom
{
    private static Random random = new Random(GetRandomSeedbyGuid());
    //public Random Random { get { return random; } }

    /// <summary>
    /// 使用Guid生成种子
    /// </summary>
    static int GetRandomSeedbyGuid()
    {
        return new Guid().GetHashCode();
    }

    /// <summary>
    /// 返回一个大于或等于 0.0 且小于 1.0 的随机浮点数。
    /// </summary>
    public static double Random()
    {
        return random.NextDouble();
    }
}
