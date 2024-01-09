using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockRequester.Models.ViewModels
{
    public class TrTableOptions
    {
        public bool ItemNameVisible { get; set; }
        public bool ItemDescriptionVisible { get; set; }
        public bool QuantityVisible { get; set; }
        public bool OriginLocationVisible { get; set; }
        public bool DestinationLocationVisible { get; set; }
        public bool StatusVisible { get; set; }
        public bool MiscNotesVisible { get; set; }

        public bool ActionButtonsVisible {  get; set; }

        private bool _viewInfoButtonVisible;
        public bool ViewInfoButtonVisible
        {
            get
            {
                return _viewInfoButtonVisible && ActionButtonsVisible;
            }
            set
            {
                _viewInfoButtonVisible = value;
            }
        }

        private bool _editDetailsButtonVisible;
        public bool EditDetailsButtonVisible 
        {
            get
            {
                return _editDetailsButtonVisible && ActionButtonsVisible;
            } 
            set
            {
                _editDetailsButtonVisible = value;
            }
        }

        private bool _updateStatusButtonVisible;
        public bool UpdateStatusButtonVisible 
        {
            get
            {
                return _updateStatusButtonVisible && ActionButtonsVisible;
            }
            set
            {
                _updateStatusButtonVisible = value;
            }
        }

        private bool _archiveButtonVisible;
        public bool ArchiveButtonVisble 
        {
            get
            {
                return _archiveButtonVisible && ActionButtonsVisible;
            }
            set
            {
                _archiveButtonVisible = value;
            }
        }

        public int? BackLocationId {  get; set; }

        public bool IsArchivedView {  get; set; }

        public static TrTableOptions ForCompanyView(bool isArchivedView = false)
        {
            return new TrTableOptions()
            {
                ItemNameVisible            = true,
                ItemDescriptionVisible     = false,
                QuantityVisible            = true,
                OriginLocationVisible      = true,
                DestinationLocationVisible = true,
                StatusVisible              = true,
                MiscNotesVisible           = false,

                ActionButtonsVisible = true,

                ViewInfoButtonVisible     = true,
                EditDetailsButtonVisible  = true,
                UpdateStatusButtonVisible = true,
                ArchiveButtonVisble       = true,

                BackLocationId = null,

                IsArchivedView = isArchivedView
            };
        }
            
        public static TrTableOptions ForUpdateStatusView(bool isArchivedView = false)
        {
            return new TrTableOptions()
            {
                ItemNameVisible             = true,
                ItemDescriptionVisible      = true,
                QuantityVisible             = true,
                OriginLocationVisible       = true,
                DestinationLocationVisible  = true,
                StatusVisible               = false,
                MiscNotesVisible            = true,

                ActionButtonsVisible = false,

                ViewInfoButtonVisible     = false,
                EditDetailsButtonVisible  = false,
                UpdateStatusButtonVisible = false,
                ArchiveButtonVisble       = false,

                BackLocationId = null,

                IsArchivedView = isArchivedView
            };
        }

        public static TrTableOptions ForLocationView(int backLocationId, bool isArchivedView = false)
        {
            return new TrTableOptions()
            {
                ItemNameVisible            = true,
                ItemDescriptionVisible     = false,
                QuantityVisible            = true,
                OriginLocationVisible      = true,
                DestinationLocationVisible = true,
                StatusVisible              = true,
                MiscNotesVisible           = false,

                ActionButtonsVisible = true,

                ViewInfoButtonVisible     = true,
                EditDetailsButtonVisible  = true,
                UpdateStatusButtonVisible = true,
                ArchiveButtonVisble       = true,

                BackLocationId = backLocationId,

                IsArchivedView = false
            };
        }
    }
}
