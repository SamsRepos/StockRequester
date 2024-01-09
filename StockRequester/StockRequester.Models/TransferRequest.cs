using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockRequester.Models
{
    public class TransferRequest
    {
        [Key] public int Id { get; set; }

        //
        // Company:
        //

        // non-nullable id for cascade delete
        [Required] public int CompanyId { get; set; }

        [ValidateNever]
        [InverseProperty(nameof(Company.TransferRequests))]
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        //
        // Created By User:
        //
        
        public string? CreatedByUserId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(CreatedByUserId))]
        public ApplicationUser? CreatedByUser { get; set; }

        //
        // Edited By Users:
        //

        public string? EditedByUsersIdsBlob {  get; set; }

        private static List<string> BlobToIdsList(string? blob)
        {
            if (blob is null) return new List<string>();
            return JsonConvert.DeserializeObject<List<string>>(blob);
        }

        private static string IdsListToBlob(List<string> ids)
        {
            return JsonConvert.SerializeObject(ids);
        }

        public List<string> GetEditedByUsersIds()
        {
            return BlobToIdsList(EditedByUsersIdsBlob);
        }

        public void AddEditedByUserId(string userId)
        {
            List<string> userIds = GetEditedByUsersIds();
            
            if(userIds.Contains(userId)) return;

            userIds.Add(userId);

            EditedByUsersIdsBlob = IdsListToBlob(userIds);
        }


        //
        // Status Changed By Users:
        //

        public string? StatusChangedByUsersIdsBlob { get; set; }

        public List<string> GetStatusChangedByUsersIds()
        {
            return BlobToIdsList(StatusChangedByUsersIdsBlob);
        }

        public void AddStatusChangedByUserId(string userId)
        {
            List<string> userIds = GetStatusChangedByUsersIds();

            if (userIds.Contains(userId)) return;

            userIds.Add(userId);

            StatusChangedByUsersIdsBlob = IdsListToBlob(userIds);
        }

        //
        // Item:
        //

        [ValidateNever]
        [Required] 
        public string ItemBlob { get; set; }

        public Item GetItem()
        {
            return Item.BlobToItem(ItemBlob);
        }

        public void SetItem(Item item)
        {
            ItemBlob = item.ToBlob();
        }

        [Required]
        [Range(1, 10000)]
        public int Quantity { get; set; }
        

        //
        // Destination Location (where the request comes from):
        //

        [Required]
        [Display(Name = "Destination Location")]
        public int? DestinationLocationId { get; set; }

        [ValidateNever]
        [InverseProperty(nameof(Location.TransferRequestsToThisDestination))]
        [ForeignKey(nameof(DestinationLocationId))]
        public Location DestinationLocation { get; set; }

        //
        // Origin Location:
        //

        [Required]
        [Display(Name = "Origin Location")]
        public int? OriginLocationId { get; set; }

        [ValidateNever]
        [InverseProperty(nameof(Location.TransferRequestsFromThisOrigin))]
        [ForeignKey(nameof(OriginLocationId))]
        public Location OriginLocation { get; set; }

        //
        // Request Status:
        //

        [Required]
        [Display(Name = "Request Status")]
        public int StatusId { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(StatusId))]
        public RequestStatus Status { get; set; }

        //
        //
        //

        // - The ability to query/flag up requests that are taking a while

        // - Estimated date?, by Origin

        [DisplayName("Miscellaneous Notes")]
        public string? MiscNotes { get; set; }

        [Required]
        public bool Archived { get; set; }
    }
}
