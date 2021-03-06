using System.Collections.Generic;

namespace Hire_Hop_Interface.Interface
{
    public static class Errors
    {
        #region Fields

        public static Dictionary<int, string> errorStrings = new Dictionary<int, string>() {
    {0, "Database error, please contact the HireHop team to report this."},
    {1, "Connection error, please try again. "},
    {2, "User not authorised to perform task."},
    {3, "Missing parameters"},
    {4, "Request made to a job that is closed."},
    {5, "Request made to a project that is closed."},
    {6, "No action taken, could not find record"},
    {7, "Can't find job"},
    {8, "You can't perform this action with the current status."},
    {9, "You are not logged in. Please <u><a href='/logout.php?d=1'>login</a></u> again."},
    {10, "Record not found."},
    {11, "You are not subscribed.<br>You are limited to what you can do unless you buy at least one user <a href='/settings.php?tab=1' target='_blank'>subscription</a>."},
    {12, "Request made to a job that is locked."},
    {13, "Can't accept changes with current status."},
    {14, "Category is not empty. The category must have nothing inside it to be deleted (this includes deleted items)."},
    {15, "Too many autopull items. You are limited to 65"},
    {16, "No items were selected"},
    {17, "The selected stock item to assign the asset to is deleted."},
    {18, "You are limited to adding 50 duplicates at a time"},
    {19, "Duplicate barcode, this barcode is already in use."},
    {20, "No location on map set"},
    {21, "Nothing assigned to upload into."},
    {22, "You have not subscribed to enough storage space to upload this file"},
    {23, "Nothing to upload"},
    {24, "File upload aborted"},
    {25, "No files found to delete"},
    {26, "EMAIL SENT, however:<br>HireHop could not connect to your email server sent box."},
    {27, "EMAIL SENT, however:<br>HireHop was unable to save a copy of the email in your sent box."},
    {28, "Email not found on server."},
    {29, "Deleting this person will delete the company. Are you sure you want to delete?"},
    {30, "Only drafts or awaiting approval can be edited by you."},
    {31, "Only drafts or awaiting approval can be deleted by you."},
    {32, "The invoice needs to be authorised first."},
    {39, "No email accounts have been setup."},
    {40, "Archive not found."},
    {41, "Nothing to archive."},
    {42, "Archive not created as you are limited to 4 archives per job."},
    {43, "Archive not created as you are limited to 30 archives per job."},
    {44, "There are fewer of some items in the archive than have already been checked out.<br><b>Restore aborted!</b>"},
    {50, "Insufficient billed amounts specific to this job (credits will be more than billed)."},
    {51, "Insufficient billed amounts specific to this project (credits will be more than billed)."},
    {52, "Can't edit an authorised invoice or credit note."},
    {60, "Package not found"},
    {61, "Too many items, you have reached the allowed limit."},
    {62, "There are no editable autopulls."},
    {63, "Can't do as requested as there is a locked node nested within the selection."},
    {64, "Not supported on your browser.  Please update your browser."},
    {65, "The items in the clipboard are not for your company."},
    {66, "There are no chargeable items."},
    {67, "Your edit means that there are fewer in the list than have already been checked out."},
    {68, "Reserved assets conflict"},
    {70, "Colour already used"},
    {80, "Document load error"},
    {90, "You have reached the limit on calendar events you can add."},
    {95, "Badly formatted barcode"},
    {100, ":title (:barcode) not available as it is on another <a href='/job.php?id=:job' target='_blank'>job</a>"},
    {101, ":title (:barcode) already checked."},
    {102, "Unrecognised code (:barcode)"},
    {103, "Not enough available; :qty x :title"},
    {104, ":title not in list"},
    {105, "More than required; :qty x :title"},
    {106, "<a href='/modules/stock/equipment.php?asset=:asset_id' target='_blank'>:title (:barcode)</a> is damaged"},
    {107, ":title (:barcode) is not available"},
    {108, ":title is not from this job, but has been added to the pre-prep"},
    {109, "You are returning more :title than was checked out, however it has been added to the pre-prep"},
    {110, ":title is not from this job."},
    {111, ":title is already checked in."},
    {112, ":qty x :title - You are returning more than was checked out"},
    {113, "There were not any no codes out for :title."},
    {114, "Can't connect to multi user sync. mode or connection was lost."},
    {115, "The box must have at least a unique name or an item code"},
    {116, "Unknown barcode."},
    {117, ":title (:barcode) has no required service."},
    {118, ":title (:barcode) has no required PAT test."},
    {119, ":title (:barcode) has no required test."},
    {120, ":barcode is reserved for another job that conflicts with this one."},
    {121, "The following reserved barcodes must be checked out for :title.<br>:txt"},
    {130, "User is already assigned to a resource."},
    {131, "Contact is already assigned to a resource."},
    {160, "Please confirm as changing job dates will causes item priority changes due to clashes with the following jobs:"},
    {161, "Job dates can't be changed as requested as this causes item priority clashes with the following jobs:"},
    {162, "Please confirm as changing the quantity will causes item priority changes due to clashes with the following jobs:"},
    {163, "The quantity can't be changed as requested as this causes item priority clashes with the following jobs:"},
    {200, "Can't cancel a job if there are still some open supplier purchase orders."},
    {201, "Status already assigned."},
    {202, "Parameter error."},
    {300, "Password too short."},
    {301, "Incorrect old password."},
    {302, "Pre-Prep contains extra items not required on this job."},
    {303, "Can't save assets to a virtual item."},
    {304, "Can't edit an asset that is checked out."},
    {305, "All payments and credit notes must be deleted first."},
    {306, "Depot job error"},
    {307, "Depot project error"},
    {308, "Database allocation error"},
    {311, "Nothing to save"},
    {312, "Not a hire stock barcode."},
    {314, "Quantity error."},
    {315, "Number not found."},
    {316, "Not found"},
    {317, "This is an internal hire and can only be edited from the receiving job."},
    {318, "Making space error."},
    {319, "Internal hire depot conflict."},
    {320, "Date order error"},
    {321, "Note field empty"},
    {322, "You can only preprep to an item with an 'on shelf' status."},
    {323, "No tax codes to import."},
    {324, "Not a PDF file."},
    {325, "Only drafts or awaiting approval can be edited."},
    {326, "Purchase Order not found."},
    {327, "Security warning, too many transactions"},
    {329, "Validation error"},
    {330, "You can only preprep items with 1 as the quantity."},
    {331, "Already part of a pre-prep<br><b>(:barcode) :title<b>"},
    {332, ":barcode - Not a code for an asset."},
    {333, "Already a box of a pre-prep<br><b>(:barcode) :title<b>"},
    {334, "Status changed, however you do not have permission to authorise the internal job,"},
    {336, "No tax codes to import."},
    {337, "You do not have permission, contact your administrator."},
    {338, "Bad data"},
    {346, "Names error"},
    {347, "Not valid Time zone"},
    {350, "Your administrator must set an export key first in Settings."},
    {351, "You need more user licenses to have more active users."},
    {360, "New value can't be higher than the old value."},
    {361, "New value can't be lower than the old value."},
    {362, "The new value is higher than the old value. Are you sure you want to save this?"},
    {363, "The new value is lower than the old value. Are you sure you want to save this?"},
    {370, "Payments exceed deposit value."},
    {371, "Payments and credits exceed invoice value."},
    {372, "Payments and credits exceed both invoice and deposit values."},
    {380, "The item is not empty, so it can't be deleted."},
    {381, "You do not have enough mapping credits to perform this action.  You need to subscribe to more mapping credits."},
    {382, "Too many waypoint geolocation requests.  You are limited to a maximum of 15 at a time."},
    {5000, "Can't delete the default price structure."},
    {5001, "Price structure already in use."},
    {5002, "These settings can only be changed by the Administrator."},
    {5003, "To change these settings you need to be in Administrator Mode.<br><br>To switch between Administrator and Normal Mode, click your name on the top right of the screen."},
    {5004, "Feature not supported by your server."}
        };

        #endregion Fields
    }
}