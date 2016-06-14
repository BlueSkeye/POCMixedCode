#include <Windows.h>

#ifdef REFDLL
#define PREFIX() Ref
#define DODLLMAIN
#else
#ifdef DYNREFDLL
#define PREFIX() DynRef
#define DODLLMAIN
#else
#define PREFIX()
#endif
#endif
#define PREFIXED(X) PREFIX() ## X

#include "AllocManager.h"

#pragma unmanaged
static int PageSize = 4096;

extern "C"
{
#ifdef DODLLMAIN
	BOOL WINAPI DllMain(_In_ HINSTANCE hinstDLL, _In_ DWORD fdwReason, _In_ LPVOID lpvReserved)
	{
		return TRUE;
	}
#endif

	static unsigned char* __cdecl AllocPage()
	{
		return (unsigned char*)VirtualAlloc(NULL, (SIZE_T)PageSize, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
	}
}

#pragma managed
namespace PREFIXED(Allocation)
{
	System::UIntPtr^ PREFIXED(AllocManager)::Alloc()
	{
		return gcnew System::UIntPtr(AllocPage());
	}
};