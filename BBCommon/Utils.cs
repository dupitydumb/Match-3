using System;
namespace bonbon
{
    public class Utils
    {
        public static void CollectGC()
        {
#if UNITY_EDITOR
            var used = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
            var heap = UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong();
#endif

            GC.Collect();

#if UNITY_EDITOR
            used = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong();
            heap = UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong();
#endif
        }
    }
}