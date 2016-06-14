#pragma once

namespace PREFIXED(Allocation)
{
	public ref class PREFIXED(AllocManager)
	{
	public:
		static System::UIntPtr^ Alloc();
	};
}
