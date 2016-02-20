using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Web.Mvc.Html;
using System.Linq;
using System.Linq.Expressions;

namespace SN.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<PhotoGallery> PhotoGallery { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    [Table("Form")]
    public class Form
    {
        [Key]
        public int UserId { get; set; }
        public DateTime BirthDate { get; set; }
        public Sex WhichSex { get; set; }
        public string Photo { get; set; }
        public string Interests { get; set; }
        public bool IsSearching { get; set; }
        public SexPreferences SexPreferences { get; set; }
        public string About { get; set; }
    }

    [Table("Messages")]
    public class Messages
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public bool IsRead { get; set; }
        public System.DateTime Date { get; set; }
        public string Text { get; set; }
    }

    [Table("Likes")]
    public class Likes
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int To { get; set; }
        public int From { get; set; }
        public bool LikeOrDislike { get; set; }
    }

    [Table("PhotoGallery")]
    public class PhotoGallery
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Photo { get; set; }
        public int UserID { get; set; }
        public System.DateTime Date { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }

    public class MessagesModel
    {
        public List<int> ID { get; set; }
        public List<string> From { get; set; }
        public List<string> Photo { get; set; }
        public List<string> To { get; set; }
        public List<string> With { get; set; }
        public List<string> Text { get; set; }
        public List<bool> IsRead { get; set; }
        public List<System.DateTime> Date { get; set; }
    }

    public class SendMessageModel
    {
        [Required]
        [Display(Name = "Message")]
        public string Text { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ExternalLoginData { get; set; }
    }

    public class ConfirmationModel
    {
        //[Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        public string Username {get; set;}
        //[Required(ErrorMessage = "*")]
        [DataType(DataType.Text)]
        [Display(Name = "Confirm your profile")]
        public string Confirmation { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The value \"{0}\" have to contain {2} symbols of more", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password Сonfirmation")]
        [Compare("NewPassword", ErrorMessage = "New Password and Password Confirmation are differ")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public enum Sex { Male, Female };
    public enum SexPreferences { Vaginal, Anal, Oral };
    public enum YesNo {No, Yes};

    public enum Month { January, 
                        February, 
                        March,
                        April,
                        May,
                        June,
                        July,
                        August,
                        September,
                        October,
                        November,
                        December };

    public class FormModel
    {
        //[DataType(DataType.Text)]
        //[Required]
        //[Display(Name = "Birthday")]
        //[RegularExpression(@"(0[1-9]|[12][0-9]|3[01])\.(0[1-9]|1[12])\.(19[0-9][0-9]|2010|200[0-9])", ErrorMessage = "Date is not correct")]
        //public string BirthDate { get; set; }

        public int BirthDay     { get; set; }

        public Month BirthMonth { get; set; }

        public int BirthYear    { get; set; }
        public string Username  { get; set; }
        public Sex WhichSex     { get; set; }
        public string Email     { get; set; }
        [Display(Name = "Photo")]
        public String Photo { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "Interests")]
        public string Interests { get; set; }
        [Display(Name = "In searching")]
        public YesNo IsSearching { get; set; }
        [Display(Name = "Sex preferences")]
        public SexPreferences SexPreferences { get; set; }
        [DataType(DataType.Text)]
        [Display(Name = "About you")]
        public string About { get; set; }
        public List<Likes> Likes { get; set; }
        public int LikesCount { get; set; }
        public int DisLikesCount { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression((@"^\s*(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*\s*$"), ErrorMessage = "E-mail address is not correct")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        public string Confirmation { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The value \"{0}\" have to contain {2} symbols or more.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password confirmation")]
        [Compare("Password", ErrorMessage = "The password and the password confirmation are differ")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }

    public class SearchModel
    {
        public string Username { get; set; }      
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public Sex WhichSex { get; set; }
        public string Text { get; set; }
        [Display(Name = "Filter by sex")]
        public bool FilterBySex { get; set; }
    }

}
