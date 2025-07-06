using Mapster;

namespace ECommerce512.API.Mapping
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Category, CategoryResponse>().Map(des => des.Note, src => src.Description);
        }
    }
}
