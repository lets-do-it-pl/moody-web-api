using System;
namespace LetsDoIt.Moody.Application
{


	public interface ICategoryService
	{
		public ICategoryService()
		{ }

		void Update(int Id, string Name, int Order, byte[] Image);

	}
}