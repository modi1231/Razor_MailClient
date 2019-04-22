using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Razor_MailClient.Data
{
    /// <summary>
    /// Nuts and bolts of the actions. Reading, deleting, sending, etc.
    /// </summary>
    public class DataAccess
    {
        public async Task<List<Email>> LoadEmailListAsync(int id_folder)
        {
            List<Email> temp = new List<Email>();

            // Test data.  If '0' aka, 'inbox', then show stuff.  Else be empty.
            await Task.Run(() =>
            {
             

                if (id_folder == 0)
                {
                    temp.Add(new Email()
                    {
                        ID = 1,
                        SUBJECT = "Test Subject",
                        FROM = "test@MyTesting.c0m",
                        DATE_RECEIVED = Convert.ToDateTime("01/05/2018")
                    });

                    temp.Add(new Email()
                    {
                        ID = 2,
                        SUBJECT = "Test Subject 2",
                        FROM = "test@MyTesting.c0m",
                        DATE_RECEIVED = Convert.ToDateTime("01/08/2018")
                    });

                    temp.Add(new Email()
                    {
                        ID = 3,
                        SUBJECT = "Test Subject 3",
                        FROM = "test@MyTesting.c0m",
                        DATE_RECEIVED = Convert.ToDateTime("01/10/2018")
                    });
                }
            });

            return temp;
        }

        public async Task<List<Folder>> LoadFolderListAsync(int selected_folder)
        {
            List<Folder> temp = new List<Folder>();

            // test data.  Could be expanded to include sub folders, junk mail, etc.
            // conceptually 0 is 'inbox'
            await Task.Run(() =>
            {
               
                temp.Add(new Folder()
                {
                    ID = 0,
                    NAME = "Inbox"
                });

                temp.Add(new Folder()
                {
                    ID = 1,
                    NAME = "Sent"
                });
 
                if(selected_folder > -1)
                {
                    foreach (Folder i    in temp)
                    {
                        if (i.ID == selected_folder)
                        {
                            i.SELECTED = true;
                            break;
                        }
                    }
                }

            });                      

            return temp;
        }

        public async Task<int> DeleteEmailAsync(int iD)
        {
            // test action 
            await Task.Run(() =>
            {
                // do stuff
            });

            return 0;
        }

        public async Task<Email> ReadEmailAsync(int iD)
        {
            Email temp = null;

            // test data being read.
            await Task.Run(() =>
            {
                temp = new Email()
                {

                    ID = iD,
                    SUBJECT = $"Test Subject - {iD}",
                    FROM = "test@MyTesting.c0m",
                    TO = "me@MyTesting.c0m",
                    BODY = "test body",
                    DATE_RECEIVED = Convert.ToDateTime("01/05/2018")
                };
            });

            return temp;
        }


        public async Task SendEmailAsync(Email email)
        {
            // test action 
            await Task.Run(() =>
            {
                // do stuff
            });
        }
    }
}
