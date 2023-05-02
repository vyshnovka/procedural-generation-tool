using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

#if DEBUG

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).RunAll(new DebugInProcessConfig());

#else

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

#endif
