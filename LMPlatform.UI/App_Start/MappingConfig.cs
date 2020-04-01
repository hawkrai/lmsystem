using AutoMapper;
using LMPlatform.Models;
using LMPlatform.UI.MappingModels;

namespace LMPlatform.UI
{
	public class MappingConfig
	{
		public static void Initialize()
		{
			Mapper.Initialize(config =>
			{
				config.CreateMap<Student, StudentSimpleModel>();
			});
		}
	}
}