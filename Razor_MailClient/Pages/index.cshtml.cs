using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_MailClient.Data;

namespace Razor_MailClient.Pages
{
    public class indexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }// no private set b/c we need data back


        public List<Email> _emails { get; set; }
        public List<Folder> _folders { get; set; }

        [BindProperty] // makes the round trip.
        public Email _readingEmail { get; set; }
        
        public int SELECTED_EMAIL_ID { get; set; }

        public Folder _active_folder { get; set; }

        /// <summary>
        /// Single reusable page, but with some post actions information must be given to itself.  Emails that are active, folders, etc.  Could be expaned with Ajax.
        /// </summary>
        /// <param name="id_email"></param>
        /// <param name="id_folder"></param>
        /// <returns></returns>
        public async Task OnGetAsync(int id_email, int id_folder)
        {
            DataAccess data = new DataAccess();

            _emails = await data.LoadEmailListAsync(id_folder);

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

        /// <summary>
        /// What occurs when an email is being read.  
        /// </summary>
        /// <param name="ID">email id</param>
        /// <param name="folder_id">current selected folder</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostReadEmailAsync(int ID, int folder_id)
        {
            DataAccess data = new DataAccess();
            Message = "Email Read";

            Email temp = await ReadEmail(ID);

            return RedirectToPage("/Index", new { id_email = temp.ID, id_folder = folder_id });
        }


        /// <summary>
        /// Delete a specific email.
        /// </summary>
        /// <param name="ID">email id</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteEmailAsync(int ID)
        {
            DataAccess data = new DataAccess();
            await data.DeleteEmailAsync(ID);

            Message = "Email Deleted";

            return RedirectToPage("/Index");
        }

        /// <summary>
        /// Show the specific email in the details body.
        /// </summary>
        /// <param name="ID">email id</param>
        /// <returns></returns>
        private async Task<Email> ReadEmail(int ID)
        {
            DataAccess data = new DataAccess();
            return await data.ReadEmailAsync(ID);
        }
        /// <summary>
        /// Obtain a list of the folders.. could be expanded with spam, sub folders, etc.
        /// </summary>
        /// <param name="selected_folder"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Send the new 'selected folder' to the form's on get.  
        /// -- Could be subplanted with ajax.
        /// </summary>
        /// <param name="folder_id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostFolderSelectedAsync(int folder_id)
        {

            await Task.Run(() =>
            {
                Message = $"{folder_id} folder selected";
            });

            return RedirectToPage("/Index", new { id_folder = folder_id });
        }

        /// <summary>
        /// Vehicle to send the id to the form's 'on get'
        /// -- Could be subplanted with ajax.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostNewEmail()
        {
            Message = "New Read";

            return RedirectToPage("/Index", new { id_email = -1 });
        }

        /// <summary>
        /// Take the user information typed and 'send' it.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSendEmailAsync()
        {
    
            Message = "Sending Email.";

            DataAccess data = new DataAccess();
            await data.SendEmailAsync(_readingEmail);

            return RedirectToPage("/Index");

        }
    }
}