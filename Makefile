# all: $(TARGET)
# Removed /unsafe flag in CL, attempting tofix the error.
all:
# Compile the statically referenced DLL.
# We need a two path compilation. Producing the preprocessor output
# prevents the creation of the obj file.
	CL.EXE /Zi /clr /c /DREFDLL /P /FirefAllocManager.pp AllocManager.cpp
	CL.EXE /Zi /clr /c /DREFDLL /ForefAllocManager.obj AllocManager.cpp
	LINK.EXE /LTCG /CLRIMAGETYPE:IJW /DLL /subsystem:console kernel32.lib /delaysign /keyfile:keypair.snk /out:RefLib.dll refAllocManager.obj
	SN.EXE -Ra RefLib.DLL keypair.snk

# Compile the main executable
# We need a two path compilation. Producing the preprocessor output
# prevents the creation of the obj file.
	CL.EXE /Zi /clr /c /P /FiAllocManager.pp AllocManager.cpp
	CL.EXE /Zi /clr /c /FoAllocManager.obj AllocManager.cpp
# Produce the final assembly
	CSC.EXE /t:module /unsafe /addmodule:AllocManager.obj /r:System.dll /r:RefLib.dll Main.cs
	LINK.EXE /LTCG /CLRIMAGETYPE:IJW /entry:Global::Main.Entry /subsystem:console kernel32.lib /delaysign /keyfile:keypair.snk /out:POCMixedAssembly.exe /ASSEMBLYMODULE:Main.netmodule AllocManager.obj Main.netmodule
	SN.EXE -Ra POCMixedAssembly.EXE keypair.snk

# Compile the dynamically referenced assembly.
# Do this AFTER the main executable to ensure no direct reference from
# the executable.
# We need a two path compilation. Producing the preprocessor output
# prevents the creation of the obj file.
	CL.EXE /Zi /clr /c /DDYNREFDLL /P /FidynrefAllocManager.pp AllocManager.cpp
	CL.EXE /Zi /clr /c /DDYNREFDLL /FodynrefAllocManager.obj AllocManager.cpp
	LINK.EXE /LTCG /CLRIMAGETYPE:IJW /DLL /subsystem:console kernel32.lib /delaysign /keyfile:keypair.snk /out:DynRefLib.dll dynrefAllocManager.obj
	SN.EXE -Ra DynRefLib.DLL keypair.snk
