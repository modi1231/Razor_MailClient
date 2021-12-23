# Razor_MailClient
ASP.NET Core 2.1 Razor project to highlight how to make a simple mail client.

=================
dreamincode.net tutorial backup ahead of decommissioning

 Posted 22 April 2019 - 01:35 PM 

ASP.NET Core is quite a versatile language and breaks apart common programming architecture into nice, neat, little sections.  Case in point - the ubiquitous web based email client.  On first blush there looks to be many moving parts, but when broken down into basic components and tasks a reusable, single page, project can be done with relative easy.  

[u][b]!! BE AWARE !![/b][/u] this is just the front end for the process.  I may dabble in the code to fetch a list of emails or send an email in a future tutorial.  Thankfully the way this is broken up allows for easy insertion of that code in the future.

[u]Software:[/u]
-- Visual Studios 2017

[u]Concepts:[/u]
-- C#
-- Core 2.1 / Razor pages
-- Entity Framework
-- Test Driven Development
-- Asynchronous
-- GULP
-- NPM

[u]github link:[/u] https://github.com/modi1231/Razor_MailClient


[img]https://i.imgur.com/yYkxchk.jpg[/img]


A good project needs a good plan.  Thinking about data objects I can see an 'email' object as well as a 'folder' object.  The 'email' would need an id, to, from, subject, body, and date.  If I use this object in a collection I will need to visually show the user which is 'selected' so let's add that to the list.

The 'folder' would be simple.. an id, name, and selected boolean.  

Per my usual work flow I will have a single 'data access' class everything taps into.  That sort of decoupling helps in a few ways, but the biggest is I can have my aysnc functions return test data and the rest of the app doesn't care.  For all the ModelView is concerned it could be from a db, a file, or even web calls to a pop email server.  This greatly helps in testing and narrowing focus for future updates.

I am seeing a call to get a list of folders, a call to get a list of emails for said folder, and an area to write emails/see the details of a specific email. 

================================ 

With a general direction let's go about creating the project and get the required scaffolding in place.

I am using my general setup from here:
https://www.dreamincode.net/forums/topic/414490-razor-pages-core-21-gulp-npm-and-basic-project-workflow/

With my index.cshtml plunked into my 'Pages' folder I start with my data classes.

In the created 'Data' folder (from the aforementioned 'general setup' tutorial link) I create a class called 'Email'.

Using the 'Data Annotations' namespace I make sure to give the 'visible' options friendly names.  This means a Razor label can pick up on a name change here without having to hunt it all over the code later.  A nifty single point of entry.

[code]using System;
using System.ComponentModel.DataAnnotations;

namespace Razor_MailClient.Data
{
    /// <summary>
    /// Defining the base components of an 'email'.
    /// Could be expanded to include header, addresses, known names, etc.
    /// </summary>
    public class Email
    {
        public int ID { get; set; }

        [Display(Name = "To")]
        public string TO { get; set; }

        [Display(Name = "From")]
        public string FROM { get; set; }

        [Display(Name = "Subject")]
        public string SUBJECT { get; set; }

        [Display(Name = "Body")]
        public string BODY { get; set; }

        [Display(Name = "Date")]
        public DateTime DATE_RECEIVED { get; set; }

        public bool Selected { get; set; }
    }
}
[/code]

Per the plan I create a 'Folder' class in the same area.  Since the folder names will be their display no need for data annotations here.

[code]namespace Razor_MailClient.Data
{
    /// <summary>
    /// Defining the base components of a 'folder'.
    /// </summary>
    public class Folder
    {
        public int ID { get; set; }

        public string NAME { get; set; }

        public bool SELECTED { get; set; }
    }
}
[/code]

In the index file (under the 'pages' folder).  I like to have a tempdata string called 'Message' to keep me afloat with random debugging messages during development.

Among the items needed will be a list of emails , a list of folders, a way to show which email is being read, which id is selected, and which folder is active.  


[code]        [TempData]
        public string Message { get; set; }// no private set b/c we need data back


        public List<Email> _emails { get; set; }
        public List<Folder> _folders { get; set; }

        [BindProperty] // makes the round trip.
        public Email _readingEmail { get; set; }
        
        public int SELECTED_EMAIL_ID { get; set; }

        public Folder _active_folder { get; set; }
[/code]

TempData - exists until read.
https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-2.2&tabs=visual-studio#tempdata

Bindproperty - data the survives the round trip from the server to the client to the client POSTing it.

It's about here I am thinking what it means to have a 'single page'.  Actions on the page would include clicking on an email (and wanting it pulled up) and clicking on a folder and having that be active.  I figure my page's "on get" would need to be able to take in an email id or folder id.

I also want the data shuffling to be all async so I set that as well.

[code]        public async Task OnGetAsync(int id_email, int id_folder)[/code]

From here there is no real ordered steps in my development except I hit each milestone and went from UI action to OnPost to DataAccess and adjusted things accordingly.  

Example:

When a user clicks 'read this specific email' the on post takes in the email id and current selected folder id (so that stays selected on refresh).  

[code]        public async Task<IActionResult> OnPostReadEmailAsync(int ID, int folder_id)
        {
            DataAccess data = new DataAccess();
            Message = "Email Read";

            Email temp = await ReadEmail(ID);

            return RedirectToPage("/Index", new { id_email = temp.ID, id_folder = folder_id });
        }[/code]

The private function takes an id and asks for an email object.

[code]        private async Task<Email> ReadEmail(int ID)
        {
            DataAccess data = new DataAccess();
            return await data.ReadEmailAsync(ID);
        }[/code]

Go to the DataAccess I have a method stubbed out that returns a filled 'email' data object.  This could easily be converted to a sql call or 

      [code]  public async Task<Email> ReadEmailAsync(int iD)
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
        }[/code]

Things are broken apart into functionality and ultimately that data class is decoupled enough I can change the source and nothing else!  Mmmmm... decouple cake!  Go ahead - smear it all over your face and make a soft moan.  That's the quality goods right there.


Let's see an example of loading the folders.  Folder lists really only need to happen when the page loads (first time or on a get from a post).  So the 'onGetAsync' has a call for:

[code]            await LoadFoldersAsync(id_folder);[/code]


If there is a given folder id (was previously 'selected' on a post or nothing yet so it's 0) call the function.

This asks for a list of folder objects from the data access.  If there is at at least one active folder set that so the html knows to make it fancy.
[code]
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
        }[/code]


In the data access I have another stubbed out function that returns a collection of folder objects.  This could have been picked up from a DB, a pop call, or what ever.. but for testing I fill it out and the rest of the app doesn't know the difference.

    [code]   public async Task<List<Folder>> LoadFolderListAsync(int selected_folder)
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
        }[/code]


The HTML side isn't quite as interesting but there is a nifty use of collections for options for decoration.

Take this line that assigns a css class to the LabelFor.  Straight forward mix up of a basic HTML class selector.

[code]@Html.LabelFor(x => @Model._readingEmail.FROM, new { @class = "badge badge-default" })[/code]


A little further below I wanted to conditionally have a 'disabled' tag to the control, but also still have the class style be assigned.  That third parameter is just a collection and you can, in place, declare a dictionary and fill as needed!  Flipping magic, ya'all!

[code] @Html.TextBoxFor(x => @Model._readingEmail.FROM, new Dictionary<string, object>()
    {{(Model._readingEmail.ID != -1) ? "disabled": "data-notdisabled" , "disabled" },
    { "class","form-control" }})[/code]

Higher up I declare a C# string and fill it with either null or a font weight and assign that variable to a table row's style.  A little bit of mixup but pretty cool for conditional styling.

[code]                            string _style = Model._emails[i].ID == Model.SELECTED_EMAIL_ID ? "font-weight:bolder;" : null;

                            <tr style="@_style">[/code]

Crazy, right?!

For the most part that's all the interesting bits.

Clearly the ID column and fields wouldn't be shown, but for this example - not a big deal.

A quick refresher - remember the Data Annotations mentioned above?  Here's an example of how they are handy.

The label text is pulled from that data annotation (or just the property name if no annotation was provided), and the textbox holds the data.

Don't forget to review what the page's "model" is from above!

[code]                <div class="row">
                    <div class="col-xs-6">
                        @Html.LabelFor(x => @Model._readingEmail.FROM, new { @class = "badge badge-default" })
                    </div>
                    <div class="col-xs-6">
                        @Html.TextBoxFor(x => @Model._readingEmail.FROM, new Dictionary<string, object>()
    {{(Model._readingEmail.ID != -1) ? "disabled": "data-notdisabled" , "disabled" },
    { "class","form-control" }})
                    </div>
                </div>[/code]


[u]Extra reading:[/u]

https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/validation?view=aspnetcore-2.2
https://docs.microsoft.com/en-us/aspnet/web-pages/overview/ui-layouts-and-themes/validating-user-input-in-aspnet-web-pages-sites
