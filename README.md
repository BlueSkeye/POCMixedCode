# POCMixedCode
Build and invoke mixed managed/unmanaged .Net assemblies

We develop three scenarios :

1) A single EXE assembly with C#,managed C++ and unmanaged C++ code.
2) A C# EXE invoking a mixed managed/unmanaged C++code. The DLL is referenced by the EXE during the build process.
3) A C# EXE that dynamically load at runtime a mixed managed/unmanaged code C++ DLL using the Assembly.Load method.
