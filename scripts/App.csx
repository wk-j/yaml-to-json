#! "netcoreapp2.0"
#r "nuget:NetStandard.Library,2.0"
#r "nuget:BenchmarkDotNet,0.10.12"
#r "nuget:System.Memory,4.4.0-preview1-25305-02"

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class Test {

    int[] values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

    [Benchmark]
    public void Take() {
        var a = values.Skip(5).Count();
    }

    [Benchmark]
    public void Slice() {
        var span = new Span<int>(values, 0);
        var a = span.Slice(5, span.Length - 5).Length;
    }
}

BenchmarkRunner.Run<Test>();
