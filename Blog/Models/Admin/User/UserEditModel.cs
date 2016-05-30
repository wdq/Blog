using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Blog.Models.Admin.User
{
    public class UserEditModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string PublicName { get; set; }
        public string Website { get; set; }
        public string Biography { get; set; }
        public Guid? ProfilePicture { get; set; }
        public string Password { get; set; } // todo: Actually secured passwords, not just plain text strings.
        public string AddOrEdit { get; set; }

        public UserEditModel()
        {
            
        }

        public UserEditModel(Blog.User user, string addOrEdit)
        {
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Role = user.Role;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Nickname = user.Nickname;
            PublicName = user.PublicName;
            Website = user.Website;
            Biography = user.Biography;
            ProfilePicture = user.ProfilePicture;
            Password = user.Password;
            AddOrEdit = addOrEdit;

        }

        public static UserEditModel UserEdit(string id)
        {
            BlogDataDataContext database = new BlogDataDataContext();

            if (id.IsNullOrWhiteSpace())
            {
                UserEditModel model = new UserEditModel(new Blog.User(), "Add");
                return model;
            }
            else
            {
                Blog.User user = database.Users.FirstOrDefault(x => x.Id == new Guid(id));
                UserEditModel model = new UserEditModel(user, "Edit");
                return model;
            }
        }

        public static Blog.User UserEditPost(UserEditModel model)
        {
            BlogDataDataContext database = new BlogDataDataContext();
            Blog.User databaseModel = database.Users.FirstOrDefault(x => x.Id == model.Id);

            if (databaseModel != null)
            {
                databaseModel.Username = model.Username;
                databaseModel.Email = model.Email;
                databaseModel.Role = model.Role;
                databaseModel.FirstName = model.FirstName;
                databaseModel.LastName = model.LastName;
                databaseModel.Nickname = model.Nickname;
                databaseModel.PublicName = model.PublicName;
                databaseModel.Website = model.Website;
                databaseModel.Biography = model.Biography;
                databaseModel.ProfilePicture = model.ProfilePicture;
                databaseModel.Password = model.Password;
            }
            else
            {
                databaseModel = new Blog.User();

                databaseModel.Id = Guid.NewGuid();

                databaseModel.Username = model.Username;
                databaseModel.Email = model.Email;
                databaseModel.Role = model.Role;
                databaseModel.FirstName = model.FirstName;
                databaseModel.LastName = model.LastName;
                databaseModel.Nickname = model.Nickname;
                databaseModel.PublicName = model.PublicName;
                databaseModel.Website = model.Website;
                databaseModel.Biography = model.Biography;
                databaseModel.ProfilePicture = model.ProfilePicture;
                databaseModel.Password = model.Password;

                database.Users.InsertOnSubmit(databaseModel);
            }
            database.SubmitChanges();

            return databaseModel;
        }
    }
}