using com.TubeCube.Web.Models;
using com.TubeCube.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using com.TubeCube.WebSite.Helpers;
using com.TubeCube.WebSite.Models;
using com.TubeCube.Web.Mapping;
using com.TubeCube.Service.Utility;
using System.IO;
using com.TubeCube.Service.AzureProxy;
using com.TubeCube.Data.DBContext;
using System.Web.Hosting;
using com.TubeCube.Web.Controllers;
using NReco.VideoConverter;
using System.Web.Security;
using System.Configuration;
using com.TubeCube.Data.UnitOfWork;
using System.Diagnostics;
using System.Media;
using FineUploader;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using com.TubeCube.Data.GenericRepositories;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace com.TubeCube.WebSite.Helpers
{
    public class Common
    {
        #region Fields

        private static TimeSpan tsTotalDuration = new TimeSpan();
        private static TimeSpan tsProcessedDuration = new TimeSpan();
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly IManageProfileService _manageProfileService;
        private readonly IManageCategoryService _manageCategoryService;
        private readonly IManageImageDetailsService _manageImageDetailsService;
        private readonly IManageFriendlistService _manageFriendlistService;
        private readonly IManageVideoService _manageVideoService;
        private readonly IManageVideoMappingService _manageVideoMappingService;
        private readonly IManageVideoThumbService _manageVideoThumbService;
        private readonly IManageCountryService _manageCountryService;
        private readonly IManageStoreService _manageStoreService;
        private readonly IManageTimelineService _manageTimelineService;
        private readonly IManageOnlineStatusService _manageOnlineStatusService;
        private readonly IManageSettingsService _settingsService;
        private readonly IManageBlockListService _blockListService;
        private readonly IManageFollowService _followService;
        private readonly IManageReviewService _manageReviewService;
        private readonly IManagePremiumService _managePremiumService;
        private readonly IManageSocialMembershipService _manageSocialMembershipService;
        private readonly IManageCartService _manageCartService;
        private readonly IManageOrderService _manageOrderService;
        private readonly IManageSocialService _manageSocialService;
        private readonly IManageMessageService _manageMessageService;
        private readonly IManageNotificationService _manageNotificationService;
        private readonly IManagePayoutSettingService _managePayoutSettingService;
        private readonly IManageAffiliateService _manageAffiliateService;
        private readonly IManageIsModelPayedService _manageIsModelPayedService;
        private readonly IManageBlogTimelineService _manageBlogTimelineService;
        private readonly IManageBlogService _manageBlogService;

        #endregion

        #region Constructors

        public Common()
        {
            _manageProfileService = new ManageProfileService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageCategoryService = new ManageCategoryService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageImageDetailsService = new ManageImageDetailsService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageFriendlistService = new ManageFriendlistService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageVideoService = new ManageVideoService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageVideoMappingService = new ManageVideoMappingService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageVideoThumbService = new ManageVideoThumbService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageCountryService = new ManageCountryService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageStoreService = new ManageStoreService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageTimelineService = new ManageTimelineService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageOnlineStatusService = new ManageOnlineStatusService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _settingsService = new ManageSettingsService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _blockListService = new ManageBlockListService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _followService = new ManageFollowService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageReviewService = new ManageReviewService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _managePremiumService = new ManagePremiumService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageSocialMembershipService = new ManageSocialMembershipService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageCartService = new ManageCartService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageOrderService = new ManageOrderService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageSocialService = new ManageSocialService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageMessageService = new ManageMessageService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageNotificationService = new ManageNotificationService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _managePayoutSettingService = new ManagePayoutSettingService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageAffiliateService = new ManageAffiliateService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageIsModelPayedService = new ManageIsModelPayedService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageBlogTimelineService = new ManageBlogTimelineService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());
            _manageBlogService = new ManageBlogService(new UnitOfWork<com.TubeCube.Data.DBContext.Entities>());

        }

        #endregion

        #region Upload Video Section

        /// <summary>
        /// method to upload video to azure
        /// </summary>
        /// <param name="model">VideoModel</param>
        /// <returns>VideoModel</returns>
        public string[] UploadVideoToAzure(VideoModel model, string userId)
        {
            try
            {
                string[] array = new string[2];

                //string video_URL = string.Empty;
                var user = _manageProfileService.GetAspNetUserById(userId);
                if (!Path.GetExtension(model.PostedFile.FileName).Equals(".mp4"))
                    array = ConvertVideo(model.PostedFile, userId);
                else
                {
                    array[0] = UploadItem(model.PostedFile, (ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"));
                    array[1] = GetVideoLength(model.PostedFile, userId);
                }

                // path for Azure
                var uploadFilePath = HttpContext.Current.Server.MapPath(array[0]);
                model.AssetID = VideoStreamingHelperClass.UploadVideoToAzureMediaServer(uploadFilePath);
                return array;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to Encode video to Azure
        /// </summary>
        /// <param name="uploadedAssetFileId">uploadedAssetFileId</param>
        /// <returns></returns>
        public string EncodeVideoOnAzure(string uploadedAssetFileId)
        {
            try
            {
                return VideoStreamingHelperClass.EncodeVideoOnAzureServer(uploadedAssetFileId);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get Stream video to Azure
        /// </summary>
        /// <param name="uploadedAssetFileId">uploadedAssetFileId</param>
        /// <returns></returns>
        public string StreamVideoOnAzure(string PreparedAssetId)
        {
            try
            {
                return VideoStreamingHelperClass.GetStreamUrlFromAzureServer(PreparedAssetId);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Converts any video to mp4 format
        /// </summary>
        /// <param name="file">file to be conmverted</param>
        /// <returns>path of the file</returns>
        public string[] ConvertVideo(HttpPostedFileBase file, string userId)
        {
            string[] array = new string[2];
            var user = _manageProfileService.GetAspNetUserById(userId);
            var temppath = UploadItem(file, ConfigurationWrapper.TempUrl);
            string VideoUrl = Guid.NewGuid() + Path.GetExtension(file.FileName);
            VideoUrl = Guid.NewGuid() + "." + Format.mp4;
            var finalPath = String.Format("{0}{1}", (ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"), VideoUrl);
            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/")))
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"));
            var ffMpeg = new FFMpegConverter();
            ffMpeg.FFMpegToolPath = HttpContext.Current.Server.MapPath(@"~/images/");
            ffMpeg.ConvertProgress += _event;
            ffMpeg.ConvertMedia(temppath.Remove(0, 1).Insert(0, ".."), finalPath.Remove(0, 1).Insert(0, ".."), Format.mp4);
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(temppath));
            array[0] = finalPath;
            array[1] = tsTotalDuration.ToString();
            return array;
        }

        /// <summary>
        /// Event to calculate the video duration and processed duration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _event(object sender, ConvertProgressEventArgs e)
        {
            if (e != null)
            {
                tsProcessedDuration = e.Processed;
                tsTotalDuration = e.TotalDuration;
                //HttpContext.Current.Session["Progress"] = ((tsTotalDuration.TotalSeconds - tsProcessedDuration.TotalSeconds) / tsTotalDuration.TotalSeconds) * 100;
            }
        }

        /// <summary>
        /// Method to get the length of any video
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetVideoLength(HttpPostedFileBase file, string userId)
        {
            var user = _manageProfileService.GetAspNetUserById(userId);
            var temppath = UploadItem(file, ConfigurationWrapper.TempUrl);
            string VideoUrl = Guid.NewGuid() + Path.GetExtension(file.FileName);
            VideoUrl = Guid.NewGuid() + "." + Format.mp4;
            var finalPath = String.Format("{0}{1}", (ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"), VideoUrl);
            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/")))
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"));
            var ffMpeg = new FFMpegConverter();
            ffMpeg.FFMpegToolPath = HttpContext.Current.Server.MapPath(@"~/images/");
            ffMpeg.ConvertProgress += _event;
            ffMpeg.ConvertMedia(temppath.Remove(0, 1).Insert(0, ".."), finalPath.Remove(0, 1).Insert(0, ".."), Format.mp4);
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(temppath));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(finalPath));

            return tsTotalDuration.ToString();
        }

        /// <summary>
        /// Cuts any video to 9 seconds preview
        /// </summary>
        /// <param name="file">file to be cut</param>
        /// <param name="finalPath">path to which it is to be stored</param>
        /// <returns>final path in which the file is saved</returns>
        public string VideoPreview(HttpPostedFileBase file, string finalPath, string userId)
        {
            var user = _manageProfileService.GetAspNetUserById(userId);
            string VideoUrl = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var ffMpeg = new FFMpegConverter();
            ffMpeg.FFMpegToolPath = HttpContext.Current.Server.MapPath(@"~/images/");
            string previewPath = String.Format("{0}{1}", (ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/"), VideoUrl);
            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/")))
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/"));
            ConvertSettings settings = new ConvertSettings();
            settings.MaxDuration = 9;
            ffMpeg.ConvertMedia(finalPath.Remove(0, 1).Insert(0, ".."), Format.mp4, previewPath.Remove(0, 1).Insert(0, ".."), Format.mp4, settings);
            return previewPath;
        }

        /// <summary>
        /// Cuts any video to 9 seconds preview
        /// </summary>
        /// <param name="previousPath">file to be cut</param>
        /// <param name="min">starting point of preview</param>
        /// <returns></returns>
        public string RegenerateVideoPreview(string previousPath, string min, string userId)
        {
            var user = _manageProfileService.GetAspNetUserById(userId);
            string VideoUrl = Guid.NewGuid() + ".mp4";
            var ffMpeg = new FFMpegConverter();
            ffMpeg.FFMpegToolPath = HttpContext.Current.Server.MapPath(@"~/images/");
            string previewPath = String.Format("{0}{1}", (ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/"), VideoUrl);
            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/")))
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/"));
            ConvertSettings settings = new ConvertSettings();
            settings.MaxDuration = 15;
            settings.Seek = float.Parse(min) * 60;
            ffMpeg.ConvertMedia(previousPath.Insert(0, "../"), Format.mp4, previewPath.Remove(0, 1).Insert(0, ".."), Format.mp4, settings);
            return previewPath;
        }

        #endregion

        #region Bind List Helpers

        /// <summary>
        /// Get video category list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> CategoryList()
        {
            return _manageVideoService.GetVideoCategotries(x => x.IsActive == true).Select
            (x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ID.ToString()
            });
        }

        /// <summary>
        /// Returns the list of all the countries
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> CountryList()
        {
            return _manageCountryService.GetCountries(x => x.IsActive == true).Select
            (x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        }

        /// <summary>
        /// Returns the list of all the active and registered users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> UserList()
        {
            var list = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false).Select
            (x => new SelectListItem
            {
                Text = GetFullName(x.UserId),
                Value = x.UserId
            });

            return list;
        }

        /// <summary>
        /// Returns the list of different size according to the category
        /// </summary>
        /// <param name="categoryId">int category id</param>
        /// <returns>list of sizes</returns>
        public IEnumerable<SelectListItem> GetSizeList(int categoryId)
        {
            if (categoryId == 1 || categoryId == 8 || categoryId == 9 || categoryId == 10 || categoryId == 16 || categoryId == 17 || categoryId == 19)
                return StoreEnum.EmptySizeList;
            else if (categoryId == 12)
                return StoreEnum.ShoeSizeList;
            else if (categoryId == 5)
                return StoreEnum.BraSizeList;
            else if (categoryId == 15)
                return StoreEnum.ToySizeList;
            else
                return StoreEnum.SizeList;
        }

        /// <summary>
        /// Friend list of user
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns>list of friends</returns>
        public List<FriendlistModel> GetFriendList(string userId)
        {
            var list = _manageFriendlistService.GetFriendlist(x => x.UserId == userId && x.IsDeleted == false).Select(x => new FriendlistModel
            {
                FriendId = x.FriendId,
                ImageUrl = GetProfilePic(x.FriendId),
                Name = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), y => y.UserId == x.FriendId).FirstOrDefault().FirstName == "" ? _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), y => y.UserId == x.FriendId).FirstOrDefault().Email.Split('@')[0] : _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), y => y.UserId == x.FriendId).FirstOrDefault().FirstName
            }).ToList();

            var list1 = _manageFriendlistService.GetFriendlist(x => x.FriendId == userId && x.IsDeleted == false).Select(x => new FriendlistModel
            {
                FriendId = x.UserId,
                ImageUrl = GetProfilePic(x.UserId),
                Name = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), y => y.UserId == x.UserId).FirstOrDefault().FirstName == "" ? _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), y => y.UserId == x.UserId).FirstOrDefault().Email.Split('@')[0] : _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), y => y.UserId == x.UserId).FirstOrDefault().FirstName
            }).ToList();

            list.AddRange(list1);

            return list;
        }

        /// <summary>
        /// Returns the list of all the Review Type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> ReviewTypeList()
        {
            return _manageReviewService.Reviewlist(x => x.IsActive == true).Select
            (x => new SelectListItem
            {
                Text = x.Type,
                Value = x.Id.ToString()
            });
        }

        /// <summary>
        /// Returns the list of all the Models
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> ModelList()
        {
            return _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && x.AspNetUser.AspNetUserRoles.Where(y => y.RoleId == "6").Select(y => y.UserId).Contains(x.UserId)).Select
            (x => new SelectListItem
            {
                Text = GetFullName(x.UserId),
                Value = x.UserId
            });
        }

        #endregion

        #region Profile Detail Helpers

        /// <summary>
        /// Method to get the url of profile pic of particular user
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns>string url</returns>
        public string GetProfilePic(string userId)
        {
            try
            {
                var v = _manageImageDetailsService.GetImages(x => x.UserId == userId && x.IsProfilePic == true && x.IsDeleted == false).FirstOrDefault();
                if (v != null)
                {
                    return v.ImageUrl;
                }
                else
                {
                    //var count = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.AspNetUser.AspNetUserRoles.Where(y => y.UserId == userId && y.RoleId == "5").Select(y => y.UserId).Contains(userId)).Count();
                    if (isMember(userId))
                    {
                        return StaticUrl.ProfilePicUrl;
                    }
                    else if (isModel(userId))
                    {
                        return StaticUrl.GirlProfilePicUrl;
                    }
                    else
                    {
                        return StaticUrl.StudioProfilePicUrl;
                    }
                }
            }
            catch
            {
                return StaticUrl.ProfilePicUrl;
            }
        }


        /// <summary>
        /// Returns the image url of the store item if any else default store image
        /// </summary>
        /// <param name="itemId">store item id</param>
        /// <returns>string image url</returns>
        public string GetStoreItemPic(int itemId)
        {
            try
            {
                var v = _manageStoreService.GetStoreImages(x => x.StoreItemId == itemId && x.IsMainImage == true).FirstOrDefault();
                if (v != null)
                {
                    return v.ImageUrl;
                }
                else
                {
                    return StaticUrl.StoreIcon;
                }
            }
            catch
            {
                return StaticUrl.StoreIcon;
            }
        }

        /// <summary>
        /// Method to get user id by username
        /// </summary>
        /// <param name="userName">user name</param>
        /// <returns>user id</returns>
        public string GetUserId(string userName)
        {
            try
            {
                return db.Users.SingleOrDefault(x => x.UserName == userName).Id;
            }
            catch
            {
                return null;
            }
        }

        public string GetUserIdByUserEmail(string userName)
        {
            try
            {
                return db.Users.SingleOrDefault(x => x.UserName.ToLower() == userName.ToLower() ||x.Email.ToLower()==userName.ToLower()).Id;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Method to get username by user id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>user name</returns>
        public string GetUserName(string userId)
        {
            try
            {
                return db.Users.SingleOrDefault(x => x.Id == userId).UserName;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///  Method to get userid by email
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public string GetUserIdByEmail(string emailId)
        {
            try
            {
                return db.Users.SingleOrDefault(x => x.Email == emailId).Id;
            }
            catch
            {
                return null;
            }
        }

        public string GetModelIdByItenId(string itemid)
        {

            int item = Convert.ToInt32(itemid);

            var data = _manageSocialMembershipService.GetList(x => x.Id == item).FirstOrDefault();
            if (data != null)
            {
                return data.ModelId;
            }
            else
            {
                return "";
            }
        }

        public string GetModelIdByAffiliate(string itemid)
        {

            int item = Convert.ToInt32(itemid);

            var data = _manageAffiliateService.GetAffiliatedList(x => x.PremiumId == item).FirstOrDefault();
            if (data != null)
            {
                return data.ModelId;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// returns the email of any user via user id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>string email</returns>
        public string GetEmailByUserId(string userId)
        {
            try
            {
                return db.Users.SingleOrDefault(x => x.Id == userId).Email;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get full name of user
        /// </summary>
        /// <param name="userId">userid of the user</param>
        /// <returns>string full name or email</returns>
        public string GetFullName(string userId)
        {
            try
            {
                var _profile = _manageProfileService.GetProfilesWithoutCheck(x => x.UserId == userId).FirstOrDefault();
                var name = String.IsNullOrEmpty(_profile.FirstName) ? _profile.Email.Split('@')[0] : _profile.FirstName + " " + _profile.LastName;
                return name;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Get birthday of model
        /// </summary>
        /// <param name="userId">userid of the user</param>
        /// <returns>Birthday</returns>
        public DateTime? GetBirthday(string userId)
        {
            try
            {
                var _profile = _manageProfileService.GetProfilesWithoutCheck(x => x.UserId == userId).FirstOrDefault();
                var birthday = _profile.Birthday;
                return birthday;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Ethnicity of model
        /// </summary>
        /// <param name="userId">userid of the user</param>
        /// <returns>Ethnicity</returns>
        public string GetEthnicity(string userId)
        {
            try
            {
                var _profile = _manageProfileService.GetProfilesWithoutCheck(x => x.UserId == userId).FirstOrDefault();
                var ethnicity = _profile.Ethnicity;
                return ethnicity;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Breast Size of model
        /// </summary>
        /// <param name="userId">userid of the user</param>
        /// <returns>Breast Size</returns>
        public string GetBreastSizes(string userId)
        {
            try
            {
                var _profile = _manageProfileService.GetProfilesWithoutCheck(x => x.UserId == userId).FirstOrDefault();
                var breastsize = _profile.BustSize;
                return breastsize;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Breast Type of model
        /// </summary>
        /// <param name="userId">userid of the user</param>
        /// <returns>Breast Type</returns>
        public string GetBreastType(string userId)
        {
            try
            {
                var _profile = _manageProfileService.GetProfilesWithoutCheck(x => x.UserId == userId).FirstOrDefault();
                var breasttype = _profile.BreastType;
                return breasttype;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Method to get country name from id
        /// </summary>
        /// <param name="countryId">string country id</param>
        /// <returns>string country name</returns>
        public string GetCountryName(string countryId)
        {
            try
            {
                int id = countryId.ToInt();
                return _manageCountryService.GetCountries(x => x.Id == id).FirstOrDefault().Name;
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region Check Helpers

        /// <summary>
        /// Method to check image formats
        /// </summary>
        /// <param name="FileName">name of file</param>
        /// <returns>true or false</returns>
        public bool CheckImageFormat(string FileName)
        {
            var ext = Path.GetExtension(FileName).ToLower();
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".png")
                return true;
            else
                return false;
        }

        public bool CheckIdImageFormat(string FileName)
        {
            var ext = Path.GetExtension(FileName).ToLower();
            if (ext == ".jpg" || ext == ".jpeg")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Method to check video formats
        /// </summary>
        /// <param name="FileName">name of file</param>
        /// <returns>true or false</returns>
        public bool CheckVideoFormat(string FileName)
        {
            var ext = Path.GetExtension(FileName).ToLower();
            if (ext == ".mp4" || ext == ".avi" || ext == ".mpeg" || ext == ".wmv" || ext == ".3gp" || ext == ".mov" || ext == ".asf" || ext == ".flv")
                return true;
            else
                return false;
        }

        /// <summary>
        /// check whether two id are friends or not
        /// </summary>
        /// <param name="id">id of friend</param>
        /// <param name="currentUserName">current user name</param>
        /// <returns>bool</returns>
        public bool IsFriend(string id, string currentUserName)
        {
            try
            {
                var currentUser = GetUserId(currentUserName);
                if (id != currentUser)
                {
                    if (_manageFriendlistService.GetFriendlist(x => x.IsDeleted == false && ((x.UserId == id && x.FriendId == currentUser) || (x.UserId == currentUser && x.FriendId == id))).Count() > 0)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// check whether friend request is approved
        /// </summary>
        /// <param name="id">id of friend</param>
        /// <param name="currentUserName">current user name</param>
        /// <returns>bool</returns>
        public bool IsFriendRequestApproved(string id, string currentUserName)
        {
            try
            {
                var currentUser = GetUserId(currentUserName);
                if (id != currentUser)
                {
                    var v = _manageFriendlistService.GetFriendlist(x => x.IsDeleted == false && ((x.UserId == id && x.FriendId == currentUser) || (x.UserId == currentUser && x.FriendId == id))).FirstOrDefault();
                    if (v != null)
                    {
                        if (v.IsApproved.HasValue && v.IsApproved.Value == true)
                            return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the user's profile is liked by the current user
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <param name="currentUserId">string current user id</param>
        /// <returns>true or false</returns>
        public bool? IsLikeByCurrent(string userId, string currentUserId)
        {
            //if (userId == currentUserId)
            //    return null;
            /*else */
            if (_manageProfileService.GetLikes(x => x.UserId == userId && x.FriendId == currentUserId && x.LikeStatus == true).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if user is online
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <param name="currentUserName">string current user id</param>
        /// <returns>bool</returns>
        public bool? IsOnline(string userId, string currentUserName)
        {
            try
            {
                string currentUserId = GetUserId(currentUserName);
                if (userId == currentUserId && !IsFriend(userId, currentUserName))
                    return null;
                else if (_manageOnlineStatusService.GetOnlineStatusList(x => x.UserId == userId).FirstOrDefault().IsOnline.Value)
                    return true;
                else
                    return false;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true if model is online
        /// </summary>
        /// <param name="modelId">model</param>
        /// <returns>bool</returns>
        public bool IsModelOnline(string modelId)
        {
            try
            {
                if (_manageOnlineStatusService.GetOnlineStatusList(x => x.UserId == modelId).FirstOrDefault().IsOnline.Value)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if auto approve video setting is on or off
        /// </summary>
        /// <returns>bool</returns>
        public bool? IsAutoApprove()
        {
            return _settingsService.GetSettings(null).FirstOrDefault().AutoApproveVideo;
        }

        /// <summary>
        /// Checks if auto encode video setting is on or off
        /// </summary>
        /// <returns>bool</returns>
        public bool IsAutoEncode()
        {
            return _settingsService.GetSettings(null).FirstOrDefault().AutoEncodeVideo;
        }


        /// <summary>
        /// Checks whether the friend is blocked by the current user
        /// </summary>
        /// <param name="currentUser">string current username</param>
        /// <param name="user">string friend username</param>
        /// <returns>bool true or false</returns>
        public bool isUserBlocked(string currentUser, string user)
        {
            var currentUserId = GetUserId(currentUser);
            var friendId = GetUserId(user);
            if (friendId == null)
            {
                friendId = user;
            }
            var list1 = _blockListService.GetBlockList(x => x.UserId == currentUserId && x.IsBlocked == true).Select(x => x.FriendId).ToList();
            var list2 = _blockListService.GetBlockList(x => x.FriendId == currentUserId && x.IsBlocked == true).Select(x => x.UserId).ToList();
            list1.AddRange(list2);

            if (list1.Contains(friendId))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if user is following another user or not
        /// </summary>
        /// <param name="currentUser">current user name</param>
        /// <param name="friendId">friend id</param>
        /// <returns></returns>
        public bool isFollowing(string currentUser, string friendId)
        {
            var currentUserId = GetUserId(currentUser);
            var count = _followService.GetList(x => x.UserId == currentUserId && x.FriendId == friendId && x.IsActive == true).Count();
            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Method to check if member has upgraded to premium
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public bool isPremium(string userId)
        {
            if (_managePremiumService.GetList(x => x.UserId == userId && x.Status == true && x.IsActive == true).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Method to check if the user is member or not
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public bool isMember(string userId)
        {
            var count = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.AspNetUser.AspNetUserRoles.Where(y => y.UserId == userId && y.RoleId == "5").Select(y => y.UserId).Contains(x.UserId)).Count();

            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Method to check if the user is verified or not
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public bool isVerified(string userId)
        {
            var count = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.UserId == userId).FirstOrDefault();
            if (count != null)
            {
                return count.verified;
            }
            else
            {
                return false;
            }
        }

        public string GetTwitterUserName(string userId)
        {
            var count = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.UserId == userId).FirstOrDefault();
            if (count != null)
            {
                return count.TwitterUserName;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Method to check if the user is model or not
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public bool isModel(string userId)
        {
            var count = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.AspNetUser.AspNetUserRoles.Where(y => y.UserId == userId && y.RoleId == "6").Select(y => y.UserId).Contains(x.UserId)).Count();

            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Method to check if the user has private profile mode on
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public bool isPrivateProfile(string userId)
        {
            var result = _managePremiumService.GetList(x => x.UserId == userId && x.Status == true && x.IsActive == true).FirstOrDefault();
            if (result != null && result.IsPrivateProfile.HasValue)
                return result.IsPrivateProfile.Value;
            else
                return false;
        }

        /// <summary>
        /// Method to check if the user has private Timeline mode on
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public bool isPrivateTimeline(string userId)
        {
            var result = _managePremiumService.GetList(x => x.UserId == userId && x.Status == true && x.IsActive == true).FirstOrDefault();
            if (result != null && result.IsPrivateTimeline.HasValue)
                return result.IsPrivateTimeline.Value;
            else
                return false;
        }

        /// <summary>
        /// Method to check if the user has protected gallery mode on
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public bool isProtectedGallery(string userId)
        {
            var result = _managePremiumService.GetList(x => x.UserId == userId && x.Status == true && x.IsActive == true).FirstOrDefault();
            if (result != null && result.IsProtectedGallery.HasValue)
                return result.IsProtectedGallery.Value;
            else
                return false;
        }

        /// <summary>
        /// Method to check if member has subscribed to model membership
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public bool isModelMember(string userId, string modelId)
        {
            if (_manageSocialMembershipService.GetList(x => x.CustomerId == userId && x.IsActive == true && x.ModelId == modelId && x.TypeId == (int)SocialMembershipEnum.TypeId.ModelMember).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for personal membership service
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnMembership(string modelId)
        {
            if (_manageSocialService.GetModelMembershipList(x => x.UserId == modelId && x.IsActive == true && (x.M1 != null || x.M3 != null || x.M6 != null || x.M12 != null)).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for personal Fund Me service
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnFundMe(string modelId)
        {
            if (_manageSocialService.GetModelFundMeList(x => x.UserId == modelId && x.IsActive == true).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for skype service
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnSkype(string modelId)
        {
            if (_manageSocialService.GetSkypeList(x => x.UserId == modelId && x.IsActive == true && (x.M15 != null || x.M30 != null || x.M45 != null || x.M60 != null)).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for snapchat service
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnSnapchat(string modelId)
        {
            if (_manageSocialService.GetSnapchatList(x => x.UserId == modelId && x.IsActive == true && (x.M1 != null || x.M3 != null || x.M6 != null || x.M12 != null)).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for kik service
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnKik(string modelId)
        {
            if (_manageSocialService.GetKikList(x => x.UserId == modelId && x.IsActive == true && (x.M1 != null || x.M3 != null || x.M6 != null || x.M12 != null)).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for custom video service
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnCustom(string modelId)
        {
            if (_manageSocialService.GetCustomVideoList(x => x.UserId == modelId && x.IsActive == true && (x.M15 != null || x.M30 != null || x.M45 != null || x.M60 != null)).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for calling service
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnCall(string modelId)
        {
            if (_manageSocialService.GetCallingList(x => x.UserId == modelId && x.IsActive == true && (x.M1 != null || x.M3 != null || x.M6 != null || x.M12 != null)).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for text service
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnText(string modelId)
        {
            if (_manageSocialService.GetTextList(x => x.UserId == modelId && x.IsActive == true && (x.M1 != null || x.M3 != null || x.M6 != null || x.M12 != null)).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if model has saved the settings for audio clip
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>bool</returns>
        public bool isModelOnAudio(string modelId)
        {
            if (_manageSocialService.GetAudioClipList(x => x.UserId == modelId && x.IsActive == true).Count() > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region Count Helpers
        /// <summary>
        /// Return The message count
        /// </summary>
        /// <param name="Senderid">string Sender Id</param>
        /// <param name="CreatedOn">String CreatedOn</param>
        /// <returns>int Message count</returns>
        public int GetMessageCount(string Senderid, string CreatedOn)
        {

            var data = _manageMessageService.GetMessageList(x => x.SenderId == Senderid).ToList();
            int count = data.Where(x => x.CreatedOn.Value.ToString("dd/MM/yyyy") == CreatedOn).Count();
            return count;

        }

        public int GetMessageExchangeCount(string Senderid, string ReceiverId, string CreatedOn)
        {

            var MessageExchangedata = _manageMessageService.GetMessageList(x => (x.SenderId == Senderid && x.ReceiverId == ReceiverId) || (x.SenderId == ReceiverId && x.ReceiverId == Senderid)).ToList();
            int MessageExchangecount = MessageExchangedata.Where(x => x.CreatedOn.Value.ToString("dd/MM/yyyy") == CreatedOn).Count();
            return MessageExchangecount;

        }
        /// <summary>
        /// Returns the total video count
        /// </summary>
        /// <returns>int video count</returns>
        public int GetVideoCount()
        {
            var list = _manageVideoService.GetAllVideos(x => x.IsApproved == true && x.AssetID != "0" && x.IsDeleted == false && (x.LaunchDate == null || x.LaunchDate <= DateTime.Now)).ToViewModel();

            //foreach (var item in list)
            //{
            //    var ModelId = GetVideoModel(item.AssetID);
            //    item.TotalVideos = GetGirlsVideoCount(ModelId);
            //    if (!IsUserActive(GetVideoModel(item.AssetID)))
            //    {
            //        item.IsDeleted = true;
            //    }
            //}

            //return list.Where(x => x.IsDeleted != true && x.TotalVideos > 2).Count();
            return list.Count();

        }

        public decimal? GetAmountSum(string email)
        {
            var TotalAmount = _manageOrderService.GetMemberAmount(x => x.Email == email && x.Status == 1).Sum(x => x.Amount);

            return TotalAmount;

        }

        /// <summary>
        /// Returns the Girls total video count
        /// </summary>
        ///  <param name="id">string user Id</param>
        /// <returns>int video count</returns>
        public int GetGirlsVideoCount(string id)
        {
            var list = _manageVideoMappingService.GetVideoIdList(x => x.UserId == id && x.IsDeleted == false);
            return _manageVideoService.GetAllVideos(x => list.Contains(x.AssetID) && x.IsDeleted == false && x.IsApproved == true && (x.LaunchDate == null || x.LaunchDate <= DateTime.Now)).Count();
           
        }

        /// <summary>
        /// returns the total registerd MGV girl count
        /// </summary>
        /// <returns>int girl count</returns>
        public int GetMGVGirlCount()
        {
            var list = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && x.AspNetUser.AspNetUserRoles.Where(y => y.RoleId == "6").Select(y => y.UserId).Contains(x.UserId)).Select(x => new ProfileModel
            {
                TotalVideos = GetGirlsVideoCount(x.UserId)
            }).ToList();
            return list.Where(x => x.TotalVideos > 2).Count();
        }

        /// <summary>
        /// returns the total registerd MGV studio count
        /// </summary>
        /// <returns>int studio count</returns>
        public int GetMGVStudioCount()
        {
            var list = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && x.AspNetUser.AspNetUserRoles.Where(y => y.RoleId == "7").Select(y => y.UserId).Contains(x.UserId)).Select(x => new ProfileModel
            {
                TotalVideos = GetGirlsVideoCount(x.UserId)
            });
            return list.Where(x => x.TotalVideos > 2).Count();
        }

        /// <summary>
        /// returns the total registerd MGV member count
        /// </summary>
        /// <returns>int studio count</returns>
        public int GetMGVMemberCount()
        {
            return _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && x.AspNetUser.AspNetUserRoles.Where(y => y.RoleId == "5").Select(y => y.UserId).Contains(x.UserId)).Count();
        }

        public int GetBlogCommentsCount(int blogid)
        {
            return _manageBlogTimelineService.GetPostList(x => x.IsActive == true && x.BlogId == blogid).Count();
        }

        /// <summary>
        /// returns the total store items
        /// </summary>
        /// <returns>int store count</returns>
        public int GetStoreItemsCount()
        {
            var list = _manageStoreService.GetStoreItemList(x => x.IsActive == true && x.Quantity > 0);

            foreach (var item in list)
            {
                if (!IsUserActive(item.StoreMappings.Where(x => x.StoreItemId == item.Id).Select(x => x.UserId).FirstOrDefault()))
                {
                    item.IsActive = false;
                }
            }

            return list.Where(x => x.IsActive != false).Count();
        }

        /// <summary>
        /// returns the Girls total store items
        /// </summary>
        ///  <param name="id">string user Id</param>
        /// <returns>int store count</returns>
        public int GetGirlsStoreItemsCount(string id)
        {
            var items = _manageStoreService.GetStoreMappingList(x => x.UserId == id && x.isActive == true).Select(x => x.StoreItemId).ToList();
            return _manageStoreService.GetStoreItemList(x => items.Contains(x.Id) && x.IsActive == true && x.Quantity > 0).Count();
          
        }

        /// <summary>
        /// returns the total store items which are on sale
        /// </summary>
        /// <returns>int store count</returns>
        public int GetSaleItemsCount()
        {
            return _manageStoreService.GetStoreItemList(x => !x.Sale.Equals("0") && !x.Sale.Equals("1") && x.IsActive == true && x.Quantity > 0).Count();
        }

        /// <summary>
        /// total likes for any user profile
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns>total counts</returns>
        public int? GetProfileLikeCount(string userId)
        {
            return _manageProfileService.GetLikes(x => x.UserId == userId && x.LikeStatus == true).Count();
        }

        /// <summary>
        /// total likes for any user profile
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns>total counts</returns>
        public int? GetProfileLikeCountByuser(string userId)
        {
            return _manageProfileService.GetLikes(x => x.FriendId == userId && x.LikeStatus == true).Count();
        }

        /// <summary>
        /// total likes for any user purchase Items
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns>total counts</returns>
        public int? GetPurchaseLikeCount(string userId)
        {
            return _manageCartService.GetPaymentDetails(x => x.OwnerID == userId && x.Status == 1).Sum(x => x.Points);

        }

        /// <summary>
        /// total likes for reviews given by user
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns>total counts</returns>
        public int? GetReviewLikeCount(string userId)
        {
            int Amount = _manageReviewService.UserReview(x => x.UserId == userId && x.IsActive == true).Count();
            return Amount;
        }

        /// <summary>
        /// total likes for any video
        /// </summary>
        /// <param name="videoId">int video id</param>
        /// <returns>total counts</returns>
        public int GetVideoLikeCount(int videoId)
        {
            return _manageVideoService.GetLikes(x => x.VideoId == videoId && x.LikeStatus == true).Count();
        }

        public int GetBlogLikeCount(int blogId)
        {
            return _manageBlogService.GetLikes(x => x.BlogId == blogId && x.LikeStatus == true).Count();
        }

        public int GetModelPurchaseLikeCount(string UserId)
        {
            List<MonthlySaleDetailsModel> model = new List<MonthlySaleDetailsModel>();
            var list = _manageOrderService.ModelPurchaseLikes(UserId).OrderByDescending(x => x.Id);
          
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
           
            int LikeCount = list.Where(x => x.CreatedOn.Value.Year==currentYear && x.CreatedOn.Value.Month == currentMonth).Sum(x => x.MGVLIkes.Value);
            return LikeCount;
        }

        public int GetModelPurchaseCount(string UserId)
        {
            decimal likes= _manageCartService.GetCartItems(x => x.Modelid == UserId && x.Status == 1).Sum(x => x.Price.Value);
            int ModelLikes = Convert.ToInt32(likes);
            return ModelLikes;
        }

        public int GetMGVPremiumLikeCount(string UserId)
        {
            int currentYear = DateTime.Now.Year;
            int CurrentMonth = DateTime.Now.Month;
            int itemtype = Convert.ToInt32(CartEnum.ItemType.Affiliated);
            int[] CartIds = _manageCartService.GetCartItems(x => x.ItemType == itemtype && x.Status == 1).Select(x => x.ItemId.ToInt()).ToArray();
            List<AffiliateModel> Mainlist = new List<AffiliateModel>();
            foreach (var itemn in CartIds)
            {
                var item = _manageAffiliateService.GetAffiliatedList(x => x.PremiumId == itemn && x.IsActive == true && x.ModelId == UserId).FirstOrDefault().ToViewFirstAffiliateModel();
                if (item != null)
                {
                    Mainlist.Add(item);
                }
            }
            decimal MGVPremiumLikes = Mainlist.Where(x => x.CreatedOn.Value.Year == currentYear && x.CreatedOn.Value.Month == CurrentMonth).Sum(x => (x.Amount * 50 / 100));
            int LikeCount = Convert.ToInt32(MGVPremiumLikes);
            return LikeCount;
        }

        /// <summary>
        /// total likes for any store item
        /// </summary>
        /// <param name="itemId">int item id</param>
        /// <returns>total counts</returns>
        public int GetStoreLikeCount(int itemId)
        {
            return _manageStoreService.GetLikes(x => x.ItemId == itemId && x.LikeStatus == true).Count();
        }

        /// <summary>
        /// Returns the count of how many times a video is sold
        /// </summary>
        /// <param name="videoId">video id</param>
        /// <returns>sales count</returns>
        public int GetSoldCount(int videoId)
        {
            return _manageOrderService.GetSoldVideosByVideoId(videoId);
        }

        /// <summary>
        /// Returns the no of free videos available for download
        /// </summary>
        /// <returns>free video count</returns>
        public int GetFreeVideoCount()
        {
            return _manageVideoService.GetAllVideos(x => x.IsDeleted == false && x.AssetID != "0" &&
              x.IsApproved == true &&
              x.MonthlyFreeVideos.Where(y => y.Status == true).Select(y => y.VideoId).Contains(x.ID)).OrderByDescending(x => x.CreatedOn).Count();
        }

        public int GetLiveSkypeShowCount()
        {
            var list = _manageProfileService.GetProfilesWithoutCheck(x => x.IsDeleted == false &&
                 x.isVisibleForSkype &&
                 x.AspNetUser.AspNetUserRoles.Where(y => y.RoleId == "6").Select(y => y.UserId).Contains(x.UserId));

            list = list.Where(x => isModelOnSkype(x.UserId));

            return list.Count();
        }

        /// <summary>
        /// Returns the video rating
        /// </summary>
        /// <param name="videoId">video id</param>
        /// <returns>rating count</returns>
        public decimal GetVideoRatingCount(string videoId)
        {
            return _manageReviewService.GetMaxVideoRatingCount(videoId);
        }
        #endregion

        #region Email Helper

        /// <summary>
        /// Method to get the email content for registration
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string RegisterEmailBody(string userName, string url, string type)
        {
            string path = "~/Content/Template/MemberRegistration.html";

            if (type == "model")
                path = "~/Content/Template/ModelRegistration.html";

            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("{UserName}", userName);
                body = body.Replace("{Url}", url);
                return body;
            }
        }

        /// <summary>
        /// Method for ID verification
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string IDVerification(string userName, string Url)
        {
            string path = "~/Content/Template/IdVerification.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("{ModelName}", userName);
                body = body.Replace("{Url}", Url);
                return body;
            }
        }

        /// <summary>
        /// Method for Age verification
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string AgeVerification(string userName, string Url)
        {
            string path = "~/Content/Template/AgeVerification.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("{UserName}", userName);
                body = body.Replace("{Url}", Url);
                return body;
            }
        }

        /// <summary>
        /// Method for Video sold
        /// </summary>
        /// <param name="ModelName"></param>
        /// <param name="VideoName"></param>
        /// <param name="MemberName"></param>
        /// <param name="Amount"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string VideoSold(string ModelName, string VideoName, string MemberName, decimal? Amount, string Url)
        {
            string path = "~/Content/Template/VideoSold.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string TotalAmount = String.Format("{0:0.00}", Amount);
                string body = reader.ReadToEnd();
                body = body.Replace("{ModelName}", ModelName);
                body = body.Replace("{VideoName}", VideoName);
                body = body.Replace("{MemberName}", MemberName);
                body = body.Replace("{Amount}", TotalAmount);
                body = body.Replace("{url}", Url);
                return body;
            }
        }

        /// <summary>
        /// Method for Video sold
        /// </summary>
        /// <param name="ModelName"></param>
        /// <param name="VideoName"></param>
        /// <param name="MemberName"></param>
        /// <param name="Amount"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string PremiumMembershipSold(string ModelName, string MemberName, string Url)
        {
            string path = "~/Content/Template/PremiumMembership.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("{ModelName}", ModelName);
                body = body.Replace("{MemberName}", MemberName);
                body = body.Replace("{url}", Url);
                return body;
            }
        }

        /// <summary>
        /// Method for Services Sold
        /// </summary>
        /// <param name="ModelName"></param>
        /// <param name="ServiceName"></param>
        /// <param name="MemberName"></param>
        /// <param name="Amount"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string ServicesSold(string ModelName, string ServiceName, string MemberName, decimal Amount, string Url)
        {
            string path = "~/Content/Template/ServicesSold.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string TotalAmount = String.Format("{0:0.00}", Amount);
                string body = reader.ReadToEnd();
                body = body.Replace("{ModelName}", ModelName);
                body = body.Replace("{Services}", ServiceName);
                body = body.Replace("{MemberName}", MemberName);
                body = body.Replace("{Amount}", TotalAmount);
                body = body.Replace("{url}", Url);
                return body;
            }
        }

        /// <summary>
        /// Method for Services Sold
        /// </summary>
        /// <param name="ModelName"></param>
        /// <param name="ServiceName"></param>
        /// <param name="MemberName"></param>
        /// <param name="Amount"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string SkypeRequest(string ModelName, string MemberName, string ModelRate, string Amount, string Url)
        {
            string path = "~/Content/Template/ServicesSold.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                Amount = String.Format("{0:0.00}", Amount);
                string body = reader.ReadToEnd();
                body = body.Replace("{ModelName}", ModelName);
                body = body.Replace("{MemberName}", MemberName);
                body = body.Replace("{Services}", "Skype");
                body = body.Replace("{Amount}", Amount);
                body = body.Replace("{url}", Url);
                return body;
            }
        }

        /// <summary>
        /// Method for Member Messages
        /// </summary>
        /// <param name="ModelName"></param>
        /// <param name="MemberName"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string MemberMessage(string ModelName, string MemberName, string Url)
        {
            string path = "~/Content/Template/MemberMessage.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("{ModelName}", ModelName);
                body = body.Replace("{MemberName}", MemberName);
                body = body.Replace("{url}", Url);
                return body;
            }
        }

        /// <summary>
        /// Method for Timeline
        /// </summary>
        /// <param name="ModelName"></param>
        /// <param name="MemberName"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string Timeline(string ModelName, string MemberName, string Url)
        {
            string path = "~/Content/Template/Timeline.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("{ModelName}", ModelName);
                body = body.Replace("{MemberName}", MemberName);
                body = body.Replace("{url}", Url);
                return body;
            }
        }

        /// <summary>
        /// Method for Friend Request
        /// </summary>
        /// <param name="ModelName"></param>
        /// <param name="MemberName"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string FriendRequest(string ModelName, string MemberName, string Url)
        {
            string path = "~/Content/Template/FriendRequest.html";
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(path)))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("{ModelName}", ModelName);
                body = body.Replace("{MemberName}", MemberName);
                body = body.Replace("{url}", Url);
                return body;
            }
        }

        /// <summary>
        /// /// Method to get the email content for reset password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ResetPasswordEmailBody(string userName, string url)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/Template/ResetPassword.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Url}", url);
            return body;
        }

        /// <summary>
        /// Method to get the email content for order
        /// </summary>
        /// <returns></returns>
        public string OrderEmailBody()
        {
            //_common.SendMail(model.Email, "Order Details", _common.OrderEmailBody());
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/Template/Order.html")))
            {
                body = reader.ReadToEnd();
            }

            string content = "   <tr>" +
                    "<td>{Name}</td>" +
                    "<td><img src={Item} style='height:100px; width:100px;'/></td> " +
                    "<td>{ItemCode}</td>" +
                    "<td>{TransactionId}</td>" +
                    "<td>{TransactionStatus}</td>" +
                    "<td>{Amount}</td>" +
                    "</tr>";

            string finalMessage = "";

            for (int i = 1; i <= 5; i++)
            {
                string msg = content;
                msg = msg.Replace("{Name}", "Bra");
                msg = msg.Replace("{Item}", "http://74.53.186.157/tubecube_7257/Content/Images/Store/ae04ab6e-dab3-4160-bbdc-66e1261f899f.png");
                msg = msg.Replace("{ItemCode}", "I1");
                msg = msg.Replace("{TransactionId}", "123456789");
                msg = msg.Replace("{TransactionStatus}", "Success");
                msg = msg.Replace("{Amount}", "$250");
                finalMessage += msg;
            }

            body = body.Replace("{Items}", finalMessage);
            return body;
        }

        /// <summary>
        /// Method to get the email content for shipping address
        /// </summary>
        /// <returns></returns>
        public string ShippingAddressNotificationBody(string modelName, ShippingAddress address, CartStoreItem item, string itemName)
        {
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/Template/ShippingAddressNotification.html")))
            {
                string body = reader.ReadToEnd();
                string condition = string.Empty;

                if (item != null)
                {
                    if (item.NotePrice || item.PerfumePrice || item.SealedPrice || item.AutographedPrice)
                        condition = "<b>Special Conditions opted by customer:</b> <br />";
                    if (item.NotePrice)
                        condition += "- A sexy note signed with the item.<br />";
                    if (item.PerfumePrice)
                        condition += "- Add perfume to the item. <br />";
                    if (item.SealedPrice)
                        condition += "- Seal the item in a plastic bag. <br />";
                    if (item.AutographedPrice)
                        condition += "- Autographed picture wearing this item. <br />";
                }

                var customerName = address.FirstName;
                if (!string.IsNullOrEmpty(address.LastName))
                    customerName = address.FirstName + " " + address.LastName;
                if (!string.IsNullOrEmpty(condition))
                    body = body.Replace("{Condition}", condition + "<br />");
                else
                    body = body.Replace("{Condition}", "");

                body = body.Replace("{ModelName}", modelName);
                body = body.Replace("{ItemName}", itemName);
                body = body.Replace("{Name}", customerName);
                body = body.Replace("{Email}", address.Email);
                body = body.Replace("{Address}", address.Address);
                body = body.Replace("{City}", address.City);
                body = body.Replace("{Pincode}", address.Postalcode);
                body = body.Replace("{Country}", GetCountryName(address.CountryId.ToString()));
                body = body.Replace("{Number}", address.PhoneNumber);

                return body;
            }
        }

        public string ItemShippedNotificationBody(string Username, string orderId, string itemName)
        {
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/Template/ItemShippedNotification.html")))
            {
                string body = reader.ReadToEnd();
                body = body.Replace("{Username}", Username);
                body = body.Replace("{OrderId}", orderId);
                body = body.Replace("{ItemName}", itemName);
                return body;
            }
        }

        /// <summary>
        /// Method to get the email content for video bug report
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="title"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public string BugReportEmailBody(string userName, string title, string reason)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/Template/BugReport.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Title}", title);
            body = body.Replace("{Reason}", reason);

            return body;
        }

        /// <summary>
        /// Method to send the mail
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Subject"></param>
        /// <param name="body"></param>
        public void SendMail(string To, string Subject, string body)
        {
            MailHelper.SendMailMessage(To, null, null, Subject, body);
        }

        #endregion

        #region Other Helpers

        /// <summary>
        /// Returns the url of flag
        /// </summary>
        /// <param name="id">int country id</param>
        /// <returns>image url of flag</returns>
        public string GetFlagUrl(string id)
        {
            try
            {
                int countryId = id.ToInt();
                var name = _manageCountryService.GetCountries(x => x.Id == countryId).Select(x => x.Value).FirstOrDefault();
                var url = "~/images/flags/" + name + ".png";
                return url;
            }
            catch
            {
                return StaticUrl.ProfilePicUrl;
            }
        }

        /// <summary>
        /// Gives the time difference in custom format (eg: 28 days 5 hours ago.)
        /// </summary>
        /// <param name="dt">created date</param>
        /// <returns>string</returns>
        public string GetTimeDiff(DateTime dt)
        {
            TimeSpan span = DateTime.Now.Subtract(dt);
            string result = span.Days > 0 ? span.Days == 1 ? span.Days.ToString() + " day " : span.Days.ToString() + " days " : "";
            result = span.Hours > 0 ? result + span.Hours.ToString() + " hours " : result;
            result = span.Minutes > 0 ? result + span.Minutes.ToString() + " minutes" : result;
            result = result + " " + "ago";
            result = result == " ago" ? "Just Now" : result;

            return result;
        }

        /// <summary>
        /// Gives the video length in custom format (eg: 1 hr 2 mins.)
        /// </summary>
        /// <param name="span">time span</param>
        /// <returns></returns>
        public string GetVideoLengthFormat(TimeSpan span)
        {
            string result = span.Hours > 0 ? span.Hours == 1 ? span.Hours.ToString() + " hr " : span.Hours.ToString() + " hrs " : "";
            result = span.Minutes > 0 ? result + span.Minutes.ToString() + " mins " : result;
            result = span.Seconds > 0 ? result + span.Seconds.ToString() + " sec" : result;
            return result;
        }

        /// <summary>
        /// Method to get the total minutes of a video
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public string GetVideoLengthMin(TimeSpan span)
        {
            var result = (span.TotalSeconds - 15);
            return result.ToString();
        }

        /// <summary>
        /// returns the user id of user who created the main post 
        /// </summary>
        /// <param name="id">post id</param>
        /// <returns>user id</returns>
        public string GetMainThreadUserId(int id)
        {
            return _manageTimelineService.GetPostList(x => x.Id == id).Select(x => x.UserId).FirstOrDefault();
        }

        /// <summary>
        /// Upload any file in given path
        /// </summary>
        /// <param name="file">file to be saved</param>
        /// <param name="path">path in which the file is to be saved</param>
        /// <returns>complete path in which the file is stored</returns>
        public string UploadItem(HttpPostedFileBase file, string path)
        {
            string itemUrl = string.Empty;
            itemUrl = Guid.NewGuid() + Path.GetExtension(file.FileName);
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
            var final_URL = String.Format("{0}{1}", path, itemUrl);
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(final_URL)))
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(final_URL));
            file.SaveAs(HttpContext.Current.Server.MapPath(final_URL));

            return final_URL;
        }

        /// <summary>
        /// Method to decode the id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string DecodeId(string id, string currentUserName)
        {
            if (String.IsNullOrEmpty(id))
            {
                return GetUserId(currentUserName);
            }
            else
            {
                return Encrypt.Decode(id);
            }
        }

        /// <summary>
        /// Method to decode the id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetId(string id, string currentUserName)
        {
            if (String.IsNullOrEmpty(id))
            {
                return GetUserId(currentUserName);
            }
            else
            {
                return id;
            }
        }

        /// <summary>
        /// returns the category name via category id
        /// </summary>
        /// <param name="id">category id</param>
        /// <returns>category name</returns>
        public string GetCategory(int id)
        {
            var list = CategoryList();

            foreach (var item in list)
            {
                if (item.Value.ToInt() == id)
                    return item.Text;
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the category id via category name
        /// </summary>
        /// <param name="name">category name</param>
        /// <returns>category id</returns>
        public int GetCategoryId(string name)
        {
            var list = CategoryList();

            foreach (var item in list)
            {
                if (item.Text.ToLower().Equals(name.Trim().ToLower()))
                    return item.Value.ToInt();
            }
            return 0;
        }

        /// <summary>
        /// Return password for the gallery
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public string GalleryPassword(string userId)
        {
            var result = _managePremiumService.GetList(x => x.UserId == userId && x.Status == true && x.IsActive == true).FirstOrDefault();
            return result.GalleryPassword;
        }

        /// <summary>
        /// Return password for the library
        /// </summary>
        /// <param name="userId">string user id</param>
        /// <returns></returns>
        public string LibraryPassword(string userId)
        {
            var result = _managePremiumService.GetList(x => x.UserId == userId && x.Status == true && x.IsActive == true).FirstOrDefault();
            return result.LibraryPassword;
        }

        /// <summary>
        /// Returns the model id via video asset id
        /// </summary>
        /// <param name="assetId">asset id</param>
        /// <returns>model id</returns>
        public string GetVideoModel(string assetId)
        {
            return _manageVideoMappingService.GetFullList(x => x.VideoAssetId == assetId).FirstOrDefault().UserId;
        }

        /// <summary>
        /// Returns the asset id of any video by its preview url
        /// </summary>
        /// <param name="previewUrl">preview url</param>
        /// <returns>asset id</returns>
        public string GetVideoAssetId(string previewUrl)
        {
            if (String.IsNullOrEmpty(previewUrl))
                return "javascript:void(0)";

            var res = _manageVideoService.GetVideos(x => x.PreviewUrl == previewUrl).FirstOrDefault();

            if (res != null)
                return res.AssetID;
            else
                return "javascript:void(0)";
        }

        public string GetAssestId(int id)
        {
            var res = _manageVideoService.GetVideos(x => x.ID == id).FirstOrDefault();
            if (res != null)
                return res.AssetID;
            else
                return "javascript:void(0)";
        }

        /// <summary>
        /// Returns the asset id of any video by its preview url
        /// </summary>
        /// <param name="previewUrl">preview url</param>
        /// <returns>asset id</returns>
        public string GetModelId(int videoid)
        {
            string assestid = _manageVideoService.GetVideos(x => x.ID == videoid).FirstOrDefault().AssetID;

            string Userid = _manageVideoMappingService.GetFullList(x => x.VideoAssetId == assestid).FirstOrDefault().UserId;
            return GetUserName(Userid);

        }

        /// <summary>
        /// Method to get the rank of any model
        /// </summary>
        /// <param name="modelId">model id</param>
        /// <returns>int rank</returns>
        public int GetModelRank(string modelId)
        {
            List<string> idList = new List<string>();
            int count = 0, flag = 0;
            //var list = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && x.verified==true && x.MGVGirlNo != 0).Join(_manageProfileService.GetLikes(y => y.LikeStatus == true), x => x.UserId, y => y.UserId, (x, y) => x).GroupBy(y => y.UserId).OrderByDescending(y => y.Count());
            var list = _manageProfileService.GetMissMgvProfilesWithoutCheck(x => x.IsDeleted == false && x.verified == true && x.MGVGirlNo != 0).Select(x => new ProfileModel
            {
                UserId = x.UserId,
                TotalMGVLikes = x.MGVLikes + GetModelPurchaseCount(x.UserId),
                CreatedOn = x.CreatedOn,
                TotalVideos = GetGirlsVideoCount(x.UserId)
        }).OrderBy(x=>x.CreatedOn).OrderByDescending(x => x.TotalMGVLikes).ToList();
            list = list.Where(x => x.TotalVideos > 2).ToList() ;
            foreach (var item in list)
            {
                //idList.Add(item.UserId);
                count++;
                if (item.UserId == modelId)
                {
                    flag = 1;
                    break;
                }
            }

            if (flag == 0)
            {
                var list1 = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && x.MGVGirlNo != 0 && !idList.Contains(x.UserId)).OrderBy(x => x.CreatedOn).Select(x => x.UserId);

                foreach (var item in list1)
                {
                    flag = 1;
                    count++;
                    if (item == modelId)
                    {
                        break;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Method to get the rank of any member
        /// </summary>
        /// <param name="UserId">member id</param>
        /// <returns>int rank</returns>
        public int GetMemberRank(string UserId)
        {
            List<string> idList = new List<string>();
            int count = 0, flag = 0;

            var list = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && (x.AspNetUser.AspNetUserRoles.Any(y => y.RoleId == "5"))).Select(x => new ProfileModel
            {
                UserId = x.UserId,
                TotalMGVLikes = GetProfileLikeCount(x.UserId) + GetPurchaseLikeCount(x.UserId) + GetReviewLikeCount(x.UserId),
                CreatedOn = x.CreatedOn
            }).OrderByDescending(x => x.CreatedOn).OrderByDescending(x => x.TotalMGVLikes).ToList();

            foreach (var item in list)
            {
                idList.Add(item.UserId);
                count++;
                if (item.UserId == UserId)
                {
                    flag = 1;
                    break;
                }
            }

            if (flag == 0)
            {
                var list1 = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && (x.AspNetUser.AspNetUserRoles.Any(y => y.RoleId == "5")) && !idList.Contains(x.UserId)).OrderBy(x => x.CreatedOn).Select(x => x.UserId);

                foreach (var item in list1)
                {
                    flag = 1;
                    count++;
                    if (item == UserId)
                    {
                        break;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Method to get the rank of any member
        /// </summary>
        /// <param name="UserId">member id</param>
        /// <returns>int rank</returns>
        public int GetStudioRank(string UserId)
        {
            List<string> idList = new List<string>();
            int count = 0, flag = 0;

            var list = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && (x.AspNetUser.AspNetUserRoles.Any(y => y.RoleId == "7"))).Select(x => new ProfileModel
            {
                UserId = x.UserId,
                TotalMGVLikes = x.MGVLikes + GetModelPurchaseCount(x.UserId),
                CreatedOn = x.CreatedOn
            }).OrderByDescending(x => x.TotalMGVLikes).ToList();

            foreach (var item in list)
            {
                idList.Add(item.UserId);
                count++;
                if (item.UserId == UserId)
                {
                    flag = 1;
                    break;
                }
            }

            if (flag == 0)
            {
                var list1 = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.IsDeleted == false && (x.AspNetUser.AspNetUserRoles.Any(y => y.RoleId == "7")) && !idList.Contains(x.UserId)).OrderBy(x => x.CreatedOn).Select(x => x.UserId);

                foreach (var item in list1)
                {
                    flag = 1;
                    count++;
                    if (item == UserId)
                    {
                        break;
                    }
                }
            }

            return count;
        }

        #endregion

        #region Rating Helper

        /// <summary>
        /// Method to get the rating for the video
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public decimal GetVideoRating(string itemId)
        {
            decimal rating = 0;
            var list = _manageReviewService.UserReview(x => x.ItemId == itemId);
            if (list != null && list.Count > 0)
            {
                rating = Math.Round(list.Average(x => x.ReviewPoints.Average(y => y.Points)) ?? 0, 2);
            }
            return rating;
        }

        #endregion

        #region Fine Upload

        /// <summary>
        /// Upload any file in given path
        /// </summary>
        /// <param name="file">file to be saved</param>
        /// <param name="path">path in which the file is to be saved</param>
        /// <returns>complete path in which the file is stored</returns>
        public string FineUploadItem(FineUpload file, string path)
        {
            string itemUrl = string.Empty;
            itemUrl = Guid.NewGuid() + Path.GetExtension(file.Filename);
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
            var final_URL = String.Format("{0}{1}", path, itemUrl);
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(final_URL)))
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(final_URL));
            file.SaveAs(HttpContext.Current.Server.MapPath(final_URL));

            return final_URL;
        }

        public string ClikentFineUploadItem(HttpPostedFileBase file, string path)
        {
            string itemUrl = string.Empty;
            itemUrl = Path.GetExtension(file.FileName);
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(path)))
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(path));
            var final_URL = String.Format("{0}{1}", path, itemUrl);
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(final_URL)))
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(final_URL));
            file.SaveAs(HttpContext.Current.Server.MapPath(final_URL));

            return final_URL;
        }

        /// <summary>
        /// Method to get the length of any video
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string FineGetVideoLength(FineUpload file, string userId)
        {

            var user = _manageProfileService.GetAspNetUserById(userId);
            var temppath = FineUploadItem(file, ConfigurationWrapper.TempUrl);
            string VideoUrl = Guid.NewGuid() + Path.GetExtension(file.Filename);
            VideoUrl = Guid.NewGuid() + "." + Format.mp4;
            var finalPath = String.Format("{0}{1}", (ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"), VideoUrl);
            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/")))
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"));
            var ffMpeg = new FFMpegConverter();
            ffMpeg.FFMpegToolPath = HttpContext.Current.Server.MapPath(@"~/images/");
            ffMpeg.ConvertProgress += _event;
            var a = temppath.Remove(0, 1).Insert(0, "..");
            var b = finalPath.Remove(0, 1).Insert(0, "..");
            ffMpeg.ConvertMedia(a, b, Format.mp4);
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(temppath));
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(finalPath));

            return tsTotalDuration.ToString();
        }

        /// <summary>
        /// Converts any video to mp4 format
        /// </summary>
        /// <param name="file">file to be conmverted</param>
        /// <returns>path of the file</returns>
        public string[] FineConvertVideo(string path, string fileName, string userId)
        {
            string[] array = new string[3];
            var user = _manageProfileService.GetAspNetUserById(userId);
            var temppath = path;
            string VideoUrl = Guid.NewGuid() + Path.GetExtension(fileName);
            VideoUrl = Guid.NewGuid() + "." + Format.mp4;
            var finalPath = String.Format("{0}{1}", (ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"), VideoUrl);

            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/")))
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VedioImageUrl + user.uId.ToString() + "/"));

            var ffMpeg = new FFMpegConverter();
            ffMpeg.FFMpegToolPath = HttpContext.Current.Server.MapPath(@"~/images/");
            ffMpeg.ConvertProgress += _event;

            ffMpeg.ConvertMedia(temppath.Remove(0, 1).Insert(0, ".."), finalPath.Remove(0, 1).Insert(0, ".."), Format.mp4);
            System.IO.File.Delete(HttpContext.Current.Server.MapPath(temppath));
            array[0] = finalPath;
            array[1] = tsTotalDuration.ToString();

            return array;
        }

        public string[] FineUploadMP4VideoToAzure(string path, string fileName, string userId)
        {
            try
            {
                string[] array = new string[3];

                // path for Azure
                array[0] = path;
                var uploadFilePath = HttpContext.Current.Server.MapPath(path);


                //return mediainfo.duration;
                array[1] = "00:04:18";
                //array[2] = VideoStreamingHelperClass.UploadVideoToAzureMediaServer(uploadFilePath);

                return array;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// method to upload video to azure
        /// </summary>
        /// <param name="model">VideoModel</param>
        /// <returns>VideoModel</returns>
        public string[] FineUploadVideoToAzure(string path, string fileName, string userId)
        {
            try
            {
                string[] array = new string[3];
                array = FineConvertVideo(path, fileName, userId);

                // path for Azure                
                var uploadFilePath = HttpContext.Current.Server.MapPath(array[0]);
                //array[2] = VideoStreamingHelperClass.UploadVideoToAzureMediaServer(uploadFilePath);

                return array;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Cuts any video to 15 seconds preview
        /// </summary>
        /// <param name="file">file to be cut</param>
        /// <param name="finalPath">path to which it is to be stored</param>
        /// <returns>final path in which the file is saved</returns>
        public string FineVideoPreview(string fileName, string finalPath, string userId)
        {
            var user = _manageProfileService.GetAspNetUserById(userId);
            string VideoUrl = Guid.NewGuid() + Path.GetExtension(fileName);
            var ffMpeg = new FFMpegConverter();
            ffMpeg.FFMpegToolPath = HttpContext.Current.Server.MapPath(@"~/images/");
            string previewPath = String.Format("{0}{1}", (ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/"), VideoUrl);
            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/")))
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/"));
            ConvertSettings settings = new ConvertSettings();
            settings.MaxDuration = 15;
            ffMpeg.ConvertMedia(finalPath.Remove(0, 1).Insert(0, ".."), Format.mp4, previewPath.Remove(0, 1).Insert(0, ".."), Format.mp4, settings);
            return previewPath;
        }

        public string FineOwnVideoPreview(string fileName, string finalPath, string userId)
        {
            var user = _manageProfileService.GetAspNetUserById(userId);
            string VideoUrl = Guid.NewGuid() + Path.GetExtension(fileName);
            var ffMpeg = new FFMpegConverter();
            ffMpeg.FFMpegToolPath = HttpContext.Current.Server.MapPath(@"~/images/");
            string previewPath = String.Format("{0}{1}", (ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/"), VideoUrl);
            if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/")))
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(ConfigurationWrapper.VideoPreviewUrl + user.uId.ToString() + "/"));
            ConvertSettings settings = new ConvertSettings();
            settings.MaxDuration = 30;
            ffMpeg.ConvertMedia(finalPath.Remove(0, 1).Insert(0, ".."), Format.mp4, previewPath.Remove(0, 1).Insert(0, ".."), Format.mp4, settings);
            return previewPath;
        }

        /// <summary>
        /// Creates the thumbnail of the profile image
        /// </summary>
        /// <param name="url">url of image</param>
        /// <param name="fileName">name of image</param>
        /// <param name="userId">userid</param>
        public void CreateProfileThumbnail(string url, string fileName, string userId)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var user = _manageProfileService.GetAspNetUserById(userId);
                string _path = Path.GetDirectoryName(url);
                _path = _path + @"\Thumb\" + user.uId.ToString() + @"\";
                if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(_path)))
                    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(_path));
                Image image = Image.FromFile(HttpContext.Current.Server.MapPath(url + user.uId.ToString() + @"\" + fileName));
                Image thumb = image.GetThumbnailImage(250, 200, () => false, IntPtr.Zero);
                string newPath = HttpContext.Current.Server.MapPath(Path.Combine(_path, fileName));
                thumb.Save(newPath);
            }
        }

        public void CreateQualityProfileThumbnail(string url, string fileName, string userId)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var user = _manageProfileService.GetAspNetUserById(userId);
                string _path = Path.GetDirectoryName(url);
                _path = _path + @"\Thumb\" + user.uId.ToString() + @"\";
                if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(_path)))
                    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(_path));
                System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(url + user.uId.ToString() + @"\" + fileName));
                int newwidthimg = 200;
                float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                int newHeight = 200;
                Bitmap bitMAP1 = new Bitmap(newwidthimg, newHeight);
                Graphics imgGraph = Graphics.FromImage(bitMAP1);
          
                imgGraph.SmoothingMode = SmoothingMode.HighQuality;
                imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imgDimesions = new Rectangle(0, 0, newwidthimg, newHeight);
                imgGraph.DrawImage(image, imgDimesions);
                string newPath = HttpContext.Current.Server.MapPath(Path.Combine(_path, fileName));
                bitMAP1.Save(newPath, ImageFormat.Jpeg);
                bitMAP1.Dispose();
                bitMAP1.Dispose();
                image.Dispose();
               
            }
        }

        public void CreateQualityProfileSmallThumbnail(string url, string fileName, string userId)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var user = _manageProfileService.GetAspNetUserById(userId);
                string _path = Path.GetDirectoryName(url);
                _path = _path + @"\SmallThumb\" + user.uId.ToString() + @"\";
                if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(_path)))
                    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(_path));
                System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(url + user.uId.ToString() + @"\" + fileName));
                int newwidthimg = 200;
                float AspectRatio = (float)image.Size.Width / (float)image.Size.Height;
                int newHeight = 200;
                Bitmap bitMAP1 = new Bitmap(newwidthimg, newHeight);
                Graphics imgGraph = Graphics.FromImage(bitMAP1);

                imgGraph.SmoothingMode = SmoothingMode.HighQuality;
                imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imgDimesions = new Rectangle(0, 0, newwidthimg, newHeight);
                imgGraph.DrawImage(image, imgDimesions);
                string newPath = HttpContext.Current.Server.MapPath(Path.Combine(_path, fileName));
                bitMAP1.Save(newPath, ImageFormat.Jpeg);
                bitMAP1.Dispose();
                bitMAP1.Dispose();
                image.Dispose();

            }
        }

        /// <summary>
        /// Creates the thumbnail of the profile image
        /// </summary>
        /// <param name="url">url of image</param>
        /// <param name="fileName">name of image</param>
        /// <param name="userId">userid</param>
        public void CreateProfileSmallThumbnail(string url, string fileName, string userId)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var user = _manageProfileService.GetAspNetUserById(userId);
                string _path = Path.GetDirectoryName(url);
                _path = _path + @"\SmallThumb\" + user.uId.ToString() + @"\";
                if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(_path)))
                    System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(_path));
                Image image = Image.FromFile(HttpContext.Current.Server.MapPath(url + user.uId.ToString() + @"\" + fileName));
                Image thumb = image.GetThumbnailImage(55, 55, () => false, IntPtr.Zero);
                string newPath = HttpContext.Current.Server.MapPath(Path.Combine(_path, fileName));
                thumb.Save(newPath);
            }
        }

        #endregion

        #region Get IP,Country and State of client accessing Website

        /// <summary>
        /// Gets the clients current WAN IP
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            try
            {
                // var IP = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());


                HttpWebRequest request = WebRequest.Create("http://whatismyip.org/") as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();

                result = Regex.Replace(result, @"<[^>]*(>|$)|&nbsp;|&zwnj;|&raquo;|&laquo;", string.Empty).Trim();
                result = result.Replace("\r", "");
                result = result.Replace("\n", "");
                result = result.Replace("\t", "");

                var startindex = result.IndexOf("Your IP Address:");
                result = result.Substring(startindex, result.Length - startindex);
                int index = result.IndexOf("Spend");
                if (index > 0)
                    result = result.Substring(0, index);

                result = result.Split('(')[0];
                result = result.Split(':')[1];

                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Returns the clients current country and state
        /// </summary>
        /// <returns></returns>
        public string[] GetCountryAndState()
        {
            string result;
            string[] arr = new string[2];
            try
            {
                var ip = /*GetIP()*/ "121.242.47.194";

                //HttpWebRequest request = WebRequest.Create("http://ip-api.com/xml/" + ip) as HttpWebRequest;
                //HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Stream stream = response.GetResponseStream();
                //StreamReader reader = new StreamReader(stream);
                //result = reader.ReadToEnd();


                //var countrystartindex = result.IndexOf("<country><![CDATA[") + 18;
                //var countryendindex = result.IndexOf("]]></country>");
                //arr[0] = result.Substring(countrystartindex, countryendindex - countrystartindex);
                arr[0] = "India";
                //var statestartindex = result.IndexOf("<regionName><![CDATA[") + 21;
                //var stateendindex = result.IndexOf("]]></regionName>");
                //arr[1] = result.Substring(statestartindex, stateendindex - statestartindex);        
                arr[1] = "Uttar Pradesh";
            }
            catch (WebException ex)
            {
                using (var sr = new StreamReader(ex.Response.GetResponseStream()))
                    result = sr.ReadToEnd();
            }
            return arr;
        }

        /// <summary>
        /// Returns the current clients country name via session
        /// </summary>
        /// <returns></returns>
        public string[] GetCurrentCountryName()
        {
            try
            {
                if (String.IsNullOrEmpty(HttpContext.Current.Session["currentCountryAndStateName"].ToString()))
                    HttpContext.Current.Session["currentCountryAndStateName"] = GetCountryAndState();
            }
            catch
            {
                HttpContext.Current.Session["currentCountryAndStateName"] = GetCountryAndState();
            }
            var result = (string[])HttpContext.Current.Session["currentCountryAndStateName"];
            return result;
        }

        /// <summary>
        /// Returns the current userid
        /// </summary>
        /// <returns>userid</returns>
        public string GetCurrentUserIdBySession()
        {
            try
            {
                return HttpContext.Current.Session["currentuserid"].ToString();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Verotel Payment

        /// <summary>
        /// Returns the Payment response from the server
        /// </summary>
        /// <param name="url">payment status url</param>
        /// <returns></returns>
        public string[] GetPaymentStatus(string url)
        {
            HttpWebResponse response = null;

            try
            {
               
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Proxy = null;
              
                response = request.GetResponse() as HttpWebResponse;
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();

                result = result.Replace("\n\n\n", ";");
                result = result.Replace("\n\n", ";");
                result = result.Replace("\n", ";");
                var arr = result.Split(';');
                return arr;
            }
            catch (Exception ex)
            {
                string filePath = @"E:\Error\Log.txt";
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, true))
                {
                    writer.WriteLine("Payment Status:{0}", ex.Message);
                }

                return null;
            }
        }


        #endregion

        public string GetShippingAddress(int shippingId)
        {
            var model = _manageCartService.GetShippingAddressDetails(x => x.Id == shippingId).FirstOrDefault();
            var address = model.Address + ", ";
            address += model.City + "- ";
            address += model.Postalcode + "<br />";
            address += GetCountryName(model.CountryId.ToString()) + "<br >";
            address += model.PhoneNumber + "<br />";
            address += model.Email;
            return address;
        }

        #region Other
        /// <summary>
        /// Check user is active in user and profile table
        /// </summary>
        /// <param name="userId"></param>
        public bool IsUserActive(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = db.Users.SingleOrDefault(x => x.Id == userId);
                var profile = _manageProfileService.GetProfiles(GetCurrentCountryName(), GetCurrentUserIdBySession(), x => x.UserId == userId).FirstOrDefault();
                if (user != null && profile != null)
                {
                    if (user.IsDeleted == false && profile.IsDeleted == false)
                    {
                        return true;
                    }
                    else if (user.IsDeleted == true && profile.IsDeleted == false)
                    {
                        profile.IsDeleted = true;
                        var result = _manageProfileService.EditProfile(profile);
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        #endregion

        public void ExpireMembershipAndSocialMembership()
        {
            var membershipList = _managePremiumService.GetList(x => x.IsActive).ToList();
            foreach (var item in membershipList)
            {
                if (DateTime.Now > item.CreatedOn.AddMonths(item.Months))
                    _managePremiumService.ExpireMembership(item.Id);
            }

            var socialMembershipList = _manageSocialMembershipService.GetList(x => x.IsActive).ToList();
            foreach (var item in socialMembershipList)
            {
                switch (item.PlanTypeId)
                {
                    case 1:
                        if (DateTime.Now > item.CreatedOn.AddMonths(1))
                        {
                            _manageSocialMembershipService.Delete(item.Id);
                        }
                        break;
                    case 2:
                        if (DateTime.Now > item.CreatedOn.AddMonths(3))
                        {
                            _manageSocialMembershipService.Delete(item.Id);
                        }
                        break;
                    case 3:
                        if (DateTime.Now > item.CreatedOn.AddMonths(6))
                        {
                            _manageSocialMembershipService.Delete(item.Id);
                        }
                        break;
                    case 4:
                        if (DateTime.Now > item.CreatedOn.AddMonths(12))
                        {
                            _manageSocialMembershipService.Delete(item.Id);
                        }
                        break;
                    case 5:
                        if (DateTime.Now > item.ModifiedOn.AddMinutes(15))
                        {
                            _manageSocialMembershipService.Delete(item.Id);
                        }
                        break;
                    case 6:
                        if (DateTime.Now > item.ModifiedOn.AddMinutes(30))
                        {
                            _manageSocialMembershipService.Delete(item.Id);
                        }
                        break;
                    case 7:
                        if (DateTime.Now > item.ModifiedOn.AddMinutes(45))
                        {
                            _manageSocialMembershipService.Delete(item.Id);
                        }
                        break;
                    case 8:
                        if (DateTime.Now > item.ModifiedOn.AddMinutes(60))
                        {
                            _manageSocialMembershipService.Delete(item.Id);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public void AddNotification(string UserId, string FriendId, int Type, int? ItemId, decimal? Price)
        {
            NotificationModel model = new NotificationModel();
            int NotificationType = Convert.ToInt32(CustomEnum.Notification.Like);
            if (Type == Convert.ToInt32(CustomEnum.Notification.Like) && _manageNotificationService.GetNotificationList(x => x.UserId == UserId && x.FriendId == FriendId && x.Type == NotificationType).Count() > 0)
            {
                var dmodel = _manageNotificationService.GetNotificationList(x => x.UserId == UserId && x.FriendId == FriendId && x.Type == NotificationType).FirstOrDefault();
                //var dataModel = model.ToUpdateNotificationModel(dmodel);
                _manageNotificationService.EditNotification(dmodel);

            }
            else
            {
                model.UserId = UserId;
                model.FriendId = FriendId;
                model.Type = Type;
                model.ItemId = ItemId;
                model.Price = Price;
                model.IsRead = false;
                _manageNotificationService.AddNotification(model.ToAddNotificationModel());
            }
        }

        public decimal GetAdminNextPayout(string modelId)
        {
            var Payoutmodel = _managePayoutSettingService.GetList(x => x.UserId == modelId).FirstOrDefault();
            var ModelIsPayed = _manageIsModelPayedService.GetList(x => x.UserId == modelId).FirstOrDefault();
           
            string id = null;
            if (!string.IsNullOrEmpty(modelId))
            {
                id = modelId;
            }
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int Day = DateTime.Now.Day;
            int itemtype = Convert.ToInt32(CartEnum.ItemType.Affiliated);
            int[] CartIds = _manageCartService.GetCartItems(x => x.ItemType == itemtype && x.Status == 1).Select(x => x.ItemId.ToInt()).ToArray();
            List<AffiliateModel> Mainlist = new List<AffiliateModel>();
            foreach (var itemn in CartIds)
            {
                var item = _manageAffiliateService.GetAffiliatedList(x => x.PremiumId == itemn && x.IsActive == true && x.ModelId == id).FirstOrDefault().ToViewFirstAffiliateModel();
                if (item != null)
                {
                    Mainlist.Add(item);
                }
            }

            decimal AffiliateAmount;
            if (ModelIsPayed != null)
            {
                AffiliateAmount = Mainlist.Where(x => x.CreatedOn > ModelIsPayed.ModifiedOn).Sum(x=>(x.Amount * 50/100));
            }
            else
            {
                AffiliateAmount = Mainlist.Sum(x => (x.Amount * 50 / 100));
            }
            //if (Day <= 15)
            //{
            //    AffiliateAmount = Mainlist.Where(x => x.CreatedOn.Value.Day <= 15).Sum(x => (x.Amount * 50 / 100));
            //}
            //else
            //{
            //    AffiliateAmount = Mainlist.Where(x => x.CreatedOn.Value.Day > 15).Sum(x => (x.Amount * 50 / 100));
            //}
            decimal DailySales = 0;
            List<DailySaleDetailsModel> model = new List<DailySaleDetailsModel>();
            var allListItem = _manageOrderService.GetDailySaleDetails(id).OrderByDescending(x => x.createdOn);
            if (allListItem != null && allListItem.Count() > 0)
            {
                var startData = allListItem.Last();
                var endData = allListItem.First();
                List<SelectListItem> datelist = new List<SelectListItem>();

                var dtStart = startData.createdOn.Value.Date;
                dtStart = new DateTime(dtStart.Year, dtStart.Month, 1);
                var dtEnd = endData.createdOn.Value.Date;
                int xxx = 0;
                while (dtEnd >= dtStart)
                {
                    string _text = DateTimeFormatInfo.CurrentInfo.GetMonthName(dtEnd.Month) + " " + dtEnd.Year;
                    string _value = (dtEnd.Month).ToString() + (dtEnd.Year).ToString();
                    if (_value.Length == 5)
                    {
                        _value = "0" + _value;
                    }
                    if (xxx == 0)
                    {
                        month = dtEnd.Month;
                        year = dtEnd.Year;
                        datelist.Add(new SelectListItem { Text = _text, Value = _value, Selected = true });
                    }
                    else
                    {
                        datelist.Add(new SelectListItem { Text = _text, Value = _value });
                    }

                    dtEnd = dtEnd.AddMonths(-1);
                    xxx++;
                }

                var list = allListItem.Where(x => x.createdOn.Value.Year == year && x.createdOn.Value.Month == month).ToList();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        model.Add(new DailySaleDetailsModel
                        {
                            Id = item.Id,
                            Item = ((CartEnum.ItemType)System.Enum.GetValues(typeof(CartEnum.ItemType)).GetValue(item.TypeId)).ToString(),
                            createdOn = item.createdOn,
                            CustomerId = item.CustomerId,
                            CustomerName = GetFullName(item.CustomerId),
                            Title = item.Title,
                            Total = (item.TypeId == 5 ? (item.Total * 85 / 100) : (item.Total * 65 / 100)),
                            UserId = item.UserId,
                            SocialId = item.SocialId
                        });
                    }
                }
                int currentDate = DateTime.Now.Day;
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;
                if (ModelIsPayed != null)
                {
                    DailySales = allListItem.Where(x => x.createdOn > ModelIsPayed.ModifiedOn && x.TypeId != 5).Sum(x=>(x.Total*65/100));
                    var CustomDailySales = allListItem.Where(x => x.createdOn > ModelIsPayed.ModifiedOn && x.TypeId == 5).Sum(x=>(x.Total*85/100));
                    DailySales = DailySales + CustomDailySales + AffiliateAmount;
                }
                else
                {
                    DailySales = allListItem.Where(x => x.TypeId != 5).Sum(x => (x.Total * 65 / 100));
                    var CustomDailySales = allListItem.Where(x => x.TypeId == 5).Sum(x => (x.Total * 85 / 100));
                    DailySales = DailySales + CustomDailySales + AffiliateAmount;
                }
                //if (currentDate <= 15)
                //{
                //    DailySales = FirstHalfList.Where(x => x.createdOn.Value.Day <= 15).Sum(x => (x.Total * 65 / 100));
                //    var CustomDailySales = CustomFirstHalfList.Where(x => x.createdOn.Value.Day <= 15).Sum(x => (x.Total * 85 / 100));
                //    DailySales = DailySales + CustomDailySales + AffiliateAmount;
                //}
                //else
                //{
                //    DailySales = FirstHalfList.Where(x => x.createdOn.Value.Day > 15).Sum(x => (x.Total * 65 / 100));
                //    var CustomDailySales = CustomFirstHalfList.Where(x => x.createdOn.Value.Day > 15).Sum(x => (x.Total * 85 / 100));
                //    DailySales = DailySales + CustomDailySales + AffiliateAmount;
                //}
            }
            if (Payoutmodel != null)
            {
                if (Payoutmodel.PaymentType == (int)PaymentEnum.PaymentType.WireTransfer1)
                {
                    DailySales = DailySales - 15;
                }
                if (Payoutmodel.PaymentType == (int)PaymentEnum.PaymentType.WireTransfer2)
                {
                    DailySales = DailySales - 25;
                }
            }
            return DailySales;
        }
        public decimal GetAdminPreviousPayout(string modelId)
        {
            var Payoutmodel = _managePayoutSettingService.GetList(x => x.UserId == modelId).FirstOrDefault();
            string id = null;
            if (!string.IsNullOrEmpty(modelId))
            {
                id = modelId;
            }
            DateTime currentDate = DateTime.Now;
            int days = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            DateTime PreviousDate;
            if (days == 31 && currentDate.Day > 16)
            {
                PreviousDate = DateTime.Now.AddDays(-16);
            }
            else
            {
                PreviousDate = DateTime.Now.AddDays(-15);
            }
            int Previousmonth = PreviousDate.Month;
            int Previousyear = PreviousDate.Year;
            int Previousday = PreviousDate.Day;
            int itemtype = Convert.ToInt32(CartEnum.ItemType.Affiliated);
            int[] CartIds = _manageCartService.GetCartItems(x => x.ItemType == itemtype && x.Status == 1).Select(x => x.ItemId.ToInt()).ToArray();
            List<AffiliateModel> Mainlist = new List<AffiliateModel>();
            foreach (var itemn in CartIds)
            {
                var item = _manageAffiliateService.GetAffiliatedList(x => x.PremiumId == itemn && x.IsActive == true && x.ModelId == id).FirstOrDefault().ToViewFirstAffiliateModel();
                if (item != null)
                {
                    Mainlist.Add(item);
                }
            }

            decimal AffiliateAmount;
            Mainlist = Mainlist.Where(x => x.CreatedOn.Value.Year == Previousyear && x.CreatedOn.Value.Month == Previousmonth).ToList();
            if (Previousday <= 15)
            {
                AffiliateAmount = Mainlist.Where(x => x.CreatedOn.Value.Day <= 15).Sum(x => (x.Amount * 50 / 100));
            }
            else
            {
                AffiliateAmount = Mainlist.Where(x => x.CreatedOn.Value.Day > 15).Sum(x => (x.Amount * 50 / 100));
            }
            decimal DailySales = 0;
            List<DailySaleDetailsModel> model = new List<DailySaleDetailsModel>();
            var allListItem = _manageOrderService.GetDailySaleDetails(id).OrderByDescending(x => x.createdOn);
            if (allListItem != null && allListItem.Count() > 0)
            {
               
                var FirstHalfList = allListItem.Where(x => x.createdOn.Value.Year == Previousyear && x.createdOn.Value.Month == Previousmonth && x.TypeId != 5).ToList();
                var CustomFirstHalfList = allListItem.Where(x => x.createdOn.Value.Year == Previousyear && x.createdOn.Value.Month == Previousmonth && x.TypeId == 5).ToList();
                if (Previousday <= 15)
                {
                    DailySales = FirstHalfList.Where(x => x.createdOn.Value.Day <= 15).Sum(x => (x.Total * 65 / 100));
                    var CustomDailySales = CustomFirstHalfList.Where(x => x.createdOn.Value.Day <= 15).Sum(x => (x.Total * 85 / 100));
                    DailySales = DailySales + CustomDailySales + AffiliateAmount;
                }
                else
                {
                    DailySales = FirstHalfList.Where(x => x.createdOn.Value.Day > 15).Sum(x => (x.Total * 65 / 100));
                    var CustomDailySales = CustomFirstHalfList.Where(x => x.createdOn.Value.Day > 15).Sum(x => (x.Total * 85 / 100));
                    DailySales = DailySales + CustomDailySales + AffiliateAmount;
                }
            }
            if (Payoutmodel != null)
            {
                if (Payoutmodel.PaymentType == (int)PaymentEnum.PaymentType.WireTransfer1)
                {
                    DailySales = DailySales - 15;
                }
                if (Payoutmodel.PaymentType == (int)PaymentEnum.PaymentType.WireTransfer2)
                {
                    DailySales = DailySales - 25;
                }
            }
            return DailySales;
        }

        public string GetPayout(string ModelId)
        {
            var model = _managePayoutSettingService.GetList(x => x.UserId == ModelId).FirstOrDefault();
            string Type = string.Empty;
            if (model != null)
            {
                Type = EnumExtensions.GetDescription((PaymentEnum.PaymentType)model.PaymentType);
            }
            return Type;
        }
        public string GetPayoutAmount(string ModelId)
        {
            var model = _managePayoutSettingService.GetList(x => x.UserId == ModelId).FirstOrDefault();
            string Amount=string.Empty;
            if (model != null)
            {
                Amount =model.Amount;
            }
            return Amount;
        }
        public bool IsPayed(string ModelId)
        {
            var model = _manageIsModelPayedService.GetList(x => x.UserId == ModelId).FirstOrDefault();
            if (model != null)
            {
                return model.IsPayed.HasValue ? model.IsPayed.Value : false;
            }
            else
            {
                return false;
            }
        }

        public decimal GetAdminFilterPayout(string modelId,string year,string month,string half)
        {
            var Payoutmodel = _managePayoutSettingService.GetList(x => x.UserId == modelId).FirstOrDefault();
            var ModelIsPayed = _manageIsModelPayedService.GetList(x => x.UserId == modelId).FirstOrDefault();
            int FilteredYear=0;
            int FilteredMonth=0;
            if (!string.IsNullOrEmpty(year))
            {
                FilteredYear = Convert.ToInt32(year);
            }
            if (!string.IsNullOrEmpty(month))
            {
                FilteredMonth = Convert.ToInt32(month);
            }

            string id = null;
            if (!string.IsNullOrEmpty(modelId))
            {
                id = modelId;
            }
           
            int itemtype = Convert.ToInt32(CartEnum.ItemType.Affiliated);
            int[] CartIds = _manageCartService.GetCartItems(x => x.ItemType == itemtype && x.Status == 1).Select(x => x.ItemId.ToInt()).ToArray();
            List<AffiliateModel> Mainlist = new List<AffiliateModel>();
            foreach (var itemn in CartIds)
            {
                var item = _manageAffiliateService.GetAffiliatedList(x => x.PremiumId == itemn && x.IsActive == true && x.ModelId == id).FirstOrDefault().ToViewFirstAffiliateModel();
                if (item != null)
                {
                    Mainlist.Add(item);
                }
            }

            decimal AffiliateAmount=0;
            if (!string.IsNullOrEmpty(year))
            {
                Mainlist = Mainlist.Where(x => x.CreatedOn.Value.Year == FilteredYear).ToList();
            }
            if (!string.IsNullOrEmpty(month))
            {
                Mainlist = Mainlist.Where(x => x.CreatedOn.Value.Month == FilteredMonth).ToList();
            }
            if (!string.IsNullOrEmpty(half))
            {
                if (half == "1")
                {
                    Mainlist = Mainlist.Where(x => x.CreatedOn.Value.Day<=15).ToList();
                }
                else
                {
                    Mainlist = Mainlist.Where(x => x.CreatedOn.Value.Day > 15).ToList();
                }
            }
            AffiliateAmount = Mainlist.Sum(x => (x.Amount * 50 / 100));
            decimal DailySales = 0;
            decimal CustomDailySales = 0;
            List<DailySaleDetailsModel> model = new List<DailySaleDetailsModel>();
            var allListItem = _manageOrderService.GetDailySaleDetails(id).OrderByDescending(x => x.createdOn);
            if (allListItem != null && allListItem.Count() > 0)
            {
                var DailySalesList = allListItem.Where(x => x.TypeId != 5);
                var CustomDailySalesList = allListItem.Where(x => x.TypeId == 5);
                if (!string.IsNullOrEmpty(year))
                {
                    DailySalesList = allListItem.Where(x => x.createdOn.Value.Year==FilteredYear && x.TypeId != 5);
                    CustomDailySalesList = allListItem.Where(x => x.createdOn.Value.Year==FilteredYear && x.TypeId == 5);                   
                }
                if (!string.IsNullOrEmpty(month))
                {
                    DailySalesList = DailySalesList.Where(x => x.createdOn.Value.Month==FilteredMonth && x.TypeId != 5);
                    CustomDailySalesList = CustomDailySalesList.Where(x => x.createdOn.Value.Month==FilteredMonth && x.TypeId == 5);
                }
                if (!string.IsNullOrEmpty(half))
                {
                    if (half == "1")
                    {
                        DailySalesList = DailySalesList.Where(x => x.createdOn.Value.Day<=15 && x.TypeId != 5);
                        CustomDailySalesList = CustomDailySalesList.Where(x => x.createdOn.Value.Day <= 15 && x.TypeId == 5);
                    }
                    else
                    {
                        DailySalesList = DailySalesList.Where(x => x.createdOn.Value.Day > 15 && x.TypeId != 5);
                        CustomDailySalesList = CustomDailySalesList.Where(x => x.createdOn.Value.Day > 15 && x.TypeId == 5);
                    }
                }
                DailySales = DailySalesList.Sum(x => (x.Total * 65 / 100));
                CustomDailySales = CustomDailySalesList.Sum(x => (x.Total * 85 / 100));
                DailySales = DailySales + CustomDailySales + AffiliateAmount;
            }
            if (Payoutmodel != null)
            {
                if (Payoutmodel.PaymentType == (int)PaymentEnum.PaymentType.WireTransfer1)
                {
                    DailySales = DailySales - 15;
                }
                if (Payoutmodel.PaymentType == (int)PaymentEnum.PaymentType.WireTransfer2)
                {
                    DailySales = DailySales - 25;
                }
            }
            return DailySales;
        }

    }
}
