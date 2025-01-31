using BiiGBackend.Models.Entities.StaticItems;
using BiiGBackend.Models.ReqResponses;

namespace BiiGBackend.Models.Extensions
{
    public static class CollectionExtensions
    {
        public static Collection ConvertFromRequest(this CollectionReqResponse response)
        {
            return new Collection()
            {
                CollectionCaption = response.CollectionCaption,
                CollectionImageUrl = response.CollectionImageUrl,
                CollectionName = response.CollectionName,
                CollectionUrl = response.CollectionUrl
            };
        }

        public static CollectionReqResponse ConvertToResponse(this Collection response)
        {
            return new CollectionReqResponse()
            {
                CollectionCaption = response.CollectionCaption,
                CollectionImageUrl = response.CollectionImageUrl,
                CollectionName = response.CollectionName,
                CollectionUrl = response.CollectionUrl
            };
        }


        public static List<CollectionReqResponse> ConvertToResponse(this IEnumerable<Collection> response)
        {
            return response.Select(u => u.ConvertToResponse()).ToList();
        }
    }
}
