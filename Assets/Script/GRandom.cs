using System;
using System.Diagnostics;
using System.Threading;

class GRandom
{
    private static Random random = new Random(GetRandomSeedbyGuid());
    //public Random Random { get { return random; } }

    /// <summary>
    /// ʹ��Guid��������
    /// </summary>
    static int GetRandomSeedbyGuid()
    {
        return new Guid().GetHashCode();
    }

    /// <summary>
    /// ����һ�����ڻ���� 0.0 ��С�� 1.0 �������������
    /// </summary>
    public static double Random()
    {
        return random.NextDouble();
    }
}
