using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_MailClient.Data;

/* menu buttons top
-x- list of folders/inbox to the left
-x- content listed in the center
read emails in popup
-x- active folder
-x- active email bolding
-x- reading pane
-x- send email
-x- -- new button
-x- -- send only enabled when new clicked.
-x- enable/disable single email read/send

-- mail client test connect
-- db connect?
-- make pretty  pretty tables https://datatables.net/manual/installation
*/
//
namespace Razor_MailClient.Pages
{
    public class indexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }// no private set b/c we need data back


        public List<Email> _emails { get; set; }
        public List<Folder> _folders { get; set; }

        [BindProperty]
        public Email _readingEmail { get; set; }

        //        public string _activeFolder { get; set; }

        //    [BindProperty]
        public int SELECTED_EMAIL_ID { get; set; }

        //[BindProperty]
        public Folder _active_folder { get; set; }

        public async Task OnGetAsync(int id_email, int id_folder)
        {
            DataAccess data = new DataAccess();

            _emails = await data.LoadEmailListAsync(id_folder);
            //            _folders = await data.LoadFolderListAsync();

            await LoadFoldersAsync(id_folder);

            if (id_email == -1)
            {
                _readingEmail = new Email();
                _readingEmail.ID = -1;

            }
            else if (id_email == 0)
                _readingEmail = new Email();
            else
            {
                _readingEmail = await ReadEmail(id_email);
                SELECTED_EMAIL_ID = id_email;
            }
        }

        public async Task<IActionResult> OnPostReadEmailAsync(int ID, int folder_id)
        {
            DataAccess data = new DataAccess();
            Message = "Email Read";

            Email temp = await ReadEmail(ID);

            return RedirectToPage("/Index", new { id_email = temp.ID, id_folder = folder_id });
        }

        public async Task<IActionResult> OnPostDeleteEmailAsync(int ID)
        {
            DataAccess data = new DataAccess();
            await data.DeleteEmailAsync(ID);

            Message = "Email Deleted";

            return RedirectToPage("/Index");
        }

        private async Task<Email> ReadEmail(int ID)
        {
            DataAccess data = new DataAccess();
            return await data.ReadEmailAsync(ID);
        }

        private async Task LoadFoldersAsync(int selected_folder = 0)
        {
            DataAccess data = new DataAccess();
            _folders = await data.LoadFolderListAsync(selected_folder);

             if (_folders.Count > 0)
            {
                _active_folder = (from x in _folders
                                  where x.SELECTED == true
                                  select x).ToList<Folder>()[0];
            }
        }

        public async Task<IActionResult> OnPostFolderSelectedAsync(int folder_id)
        {

            await Task.Run(() =>
            {
                Message = $"{folder_id} folder selected";
            });

            return RedirectToPage("/Index", new { id_folder = folder_id });
        }

        public IActionResult OnPostNewEmail()
        {
            Message = "New Read";

            return RedirectToPage("/Index", new { id_email = -1 });

        }

        public async Task<IActionResult> OnPostSendEmailAsync()
        {
            Message = "Sending Email.";

            DataAccess data = new DataAccess();
            await data.SendEmailAsync(_readingEmail);

            return RedirectToPage("/Index");

        }
    }
}