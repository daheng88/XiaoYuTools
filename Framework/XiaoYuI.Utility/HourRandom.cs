using System;
using System.Collections.Generic;
using System.Text;

namespace XiaoYuI.Utility
{
    /// <summary>
    /// 每小时的随机数
    /// </summary>
  public  class HourRandom
    {

      private Random rand = new Random();
      /// <summary>
      /// 一小时的随机数
      /// </summary>
      private const int HOUR = 60;
      /// <summary>
      /// 次数
      /// </summary>
      public int Times { get; set; }

      /// <summary>
      /// 当前值
      /// </summary>
      public int Current { get; set; }

      public HourRandom(int times)
      {
          this.Times = times;
          this.Current =0;
      }

      /// <summary>
      /// 取下一个随机数
      /// </summary>
      /// <param name="current"></param>
      /// <returns></returns>
      public  int  Next()
      {
          if(this.Times<=0)
              this.Times=1;
          int f = HOUR / this.Times;
          int t = this.Current / f + 1;
          if (t >= this.Times)
              t = 0;
           this.Current=rand.Next(f*t,f*(t+1));
           return this.Current;

      }
    }
}
